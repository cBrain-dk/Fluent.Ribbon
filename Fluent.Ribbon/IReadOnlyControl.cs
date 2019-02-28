using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluent
{
    public interface IReadOnlyControl
    {
        /// <summary>
        /// Gets or sets IsReadOnly for the element
        /// </summary>
        bool IsReadOnly { get; set; }
    }
}
