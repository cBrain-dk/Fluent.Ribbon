namespace Fluent
{
    // From https://gist.github.com/weitzhandler/65ac9113e31d12e697cb58cd92601091
    // Licensed to the .NET Foundation under one or more agreements.
    // The .NET Foundation licenses this file to you under the MIT license.
    // See the LICENSE file in the project root for more information.

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Data;

    /// <summary>
    /// WPF specific implementation of a Observable collection that supports performing ranged additions and removals.
    /// </summary>
    /// <typeparam name="T">Type of the elements in the collection</typeparam>
    public class ObservableVisualRangeCollection<T> : ObservableRangeCollection<T>
    {
        private DeferredEventsCollection internalDeferredEvents;

        /// <summary>
        /// Base instantiation
        /// </summary>
        public ObservableVisualRangeCollection()
        {
        }

        /// <summary>
        /// Instantiation from collection
        /// </summary>
        /// <param name="collection">Collection to start from</param>
        public ObservableVisualRangeCollection(IReadOnlyCollection<T> collection) 
            : base(collection)
        {
        }

        /// <summary>
        /// Instantiation from list
        /// </summary>
        /// <param name="list">List to start from</param>
        public ObservableVisualRangeCollection(List<T> list) 
            : base(list)
        {
        }

        /// <summary>
        /// Raise CollectionChanged event to any listeners.
        /// Properties/methods modifying this ObservableCollection will raise
        /// a collection changed event through this virtual method.
        /// </summary>
        /// <remarks>
        /// When overriding this method, either call its base implementation
        /// or call <see cref="ObservableCollection{T}.BlockReentrancy"/> to guard against reentrant collection changes.
        /// </remarks>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            var deferredEvents = (ICollection<NotifyCollectionChangedEventArgs>)typeof(ObservableRangeCollection<T>).GetField("internalDeferredEvents", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(this);
            if (deferredEvents != null)
            {
                deferredEvents.Add(e);
                return;
            }

            foreach (var handler in this.GetHandlers())
            {
                if (this.IsRange(e) && handler.Target is CollectionView cv)
                {
                    cv.Refresh();
                }
                else
                {
                    handler(this, e);
                }
            }
        }

        /// <summary>
        /// Provides a new list for defered events
        /// </summary>
        /// <returns></returns>
        protected override IDisposable DeferEvents() => new DeferredEventsCollection(this);

        private bool IsRange(NotifyCollectionChangedEventArgs e) => e.NewItems?.Count > 1 || e.OldItems?.Count > 1;

        private IEnumerable<NotifyCollectionChangedEventHandler> GetHandlers()
        {
            var info = typeof(ObservableCollection<T>).GetField(nameof(this.CollectionChanged), BindingFlags.Instance | BindingFlags.NonPublic);
            var @event = (MulticastDelegate)info.GetValue(this);
            return @event?.GetInvocationList()
              .Cast<NotifyCollectionChangedEventHandler>()
              .Distinct()
              ?? Enumerable.Empty<NotifyCollectionChangedEventHandler>();
        }

        internal class DeferredEventsCollection : List<NotifyCollectionChangedEventArgs>, IDisposable
        {
            private readonly ObservableVisualRangeCollection<T> internalCollection;

            public DeferredEventsCollection(ObservableVisualRangeCollection<T> collection)
            {
                Debug.Assert(collection != null, "cannot instantiate on no collection");
                Debug.Assert(collection.internalDeferredEvents == null, "cannot instantiate on collection without event management");
                this.internalCollection = collection;
                this.internalCollection.internalDeferredEvents = this;
            }

            public void Dispose()
            {
                this.internalCollection.internalDeferredEvents = null;

                var handlers = this.internalCollection
                  .GetHandlers()
                  .ToLookup(h => h.Target is CollectionView);

                foreach (var handler in handlers[false])
                {
                    foreach (var e in this)
                    {
                        handler(this.internalCollection, e);
                    }
                }

                foreach (var cv in handlers[true]
                  .Select(h => h.Target)
                  .Cast<CollectionView>()
                  .Distinct())
                {
                    cv.Refresh();
                }
            }
        }
    }
}