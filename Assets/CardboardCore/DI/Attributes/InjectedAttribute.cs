using System;

namespace CardboardCore.DI
{
    /// <summary>
    /// Add this attribute to a class to make it injectable
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class InjectableAttribute : Attribute
    {
        public Type Layer { get; set; } = typeof(InjectionLayer);
        public bool Singleton { get; set; }
    }
}