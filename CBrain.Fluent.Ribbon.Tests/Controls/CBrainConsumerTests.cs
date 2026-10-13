namespace CBrain.Consumer.Probes;

using System;
using System.Threading;
using System.Windows.Markup;
using NUnit.Framework;

[TestFixture]
[Apartment(ApartmentState.STA)]
public class CBrainConsumerTests
{
    [TestCase("RibbonComboBox", typeof(Fluent.CBrain.RibbonComboBox))]
    [TestCase("RibbonSplitButton", typeof(Fluent.CBrain.RibbonSplitButton))]
    public void Parse_FluentCBrainNamespace_CreatesControlInFluentAssembly(string controlName, Type expectedType)
    {
        // Arrange
        var xaml = $"<native:{controlName} xmlns:native='clr-namespace:Fluent.CBrain;assembly=Fluent' />";

        // Act
        var control = XamlReader.Parse(xaml);

        // Assert
        Assert.That(control, Is.TypeOf(expectedType));
        Assert.That(control, Is.InstanceOf<Fluent.IRibbonControl>());
        Assert.That(control.GetType().Assembly, Is.SameAs(typeof(Fluent.Ribbon).Assembly));
    }
}
