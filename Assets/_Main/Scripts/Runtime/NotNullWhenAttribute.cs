#nullable enable
#pragma warning disable CS1591
#pragma warning disable CS0436
namespace System.Diagnostics.CodeAnalysis
{
    sealed class NotNullWhenAttribute : Attribute
    {
        public NotNullWhenAttribute(bool returnValue)
        {
            ReturnValue = returnValue;
        }

        public bool ReturnValue { get; }
    }
}
