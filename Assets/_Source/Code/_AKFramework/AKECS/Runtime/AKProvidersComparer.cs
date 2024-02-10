using System.Collections.Generic;

namespace _Source.Code._AKFramework.AKECS.Runtime
{
    public class AKProvidersComparer : IEqualityComparer<IAKComponent>
    {
        public bool Equals(IAKComponent x, IAKComponent y)
        {
            return x.GetType() == y.GetType();
        }

        public int GetHashCode(IAKComponent obj)
        {
            return obj.GetType().GetHashCode();
        }
    }
}