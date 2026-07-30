using System.Collections.Generic;
using System.Linq;

namespace Shared.Domain.Abstractions.Primitives
{
    public abstract class ValueObject<T>
        where T : ValueObject<T>
    {
        protected abstract IEnumerable<object> GetAttributesToIncludeInEqualityCheck();

        public override bool Equals(object obj)
        {
            return Equals((T)obj!);
        }

        public virtual bool Equals(T other)
        {
            if (other is null)
            {
                return false;
            }
            return GetAttributesToIncludeInEqualityCheck()
                .SequenceEqual(other.GetAttributesToIncludeInEqualityCheck());
        }

        public static bool operator ==(ValueObject<T> left, ValueObject<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ValueObject<T> left, ValueObject<T> right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            foreach (var obj in GetAttributesToIncludeInEqualityCheck())
                hash = hash * 31 + (obj == null ? 0 : obj.GetHashCode());

            return hash;
        }
    }
}
