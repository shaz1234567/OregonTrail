

using System;
using System.Collections.Generic;
using WolfCurses;

namespace OregonTrailDotNet.Entity
{
   
    public interface IEntity : IComparer<IEntity>, IComparable<IEntity>, IEquatable<IEntity>, IEqualityComparer<IEntity>,
        ITick
    {
       
        string Name { get; }
    }
}