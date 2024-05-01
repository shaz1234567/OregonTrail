

using System;
using OregonTrailDotNet.Event;

namespace OregonTrailDotNet.Module.Director
{
    
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class DirectorEventAttribute : Attribute
    {
     
        public DirectorEventAttribute(
            EventCategory eventCategory,
            EventExecution eventExecutionType = EventExecution.RandomOrManual)
        {
            EventCategory = eventCategory;
            EventExecutionType = eventExecutionType;
        }

     
        public EventCategory EventCategory { get; }

       
        public EventExecution EventExecutionType { get; }
    }
}