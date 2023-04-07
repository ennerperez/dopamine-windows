using System;
using Amphetamine.Data.Interfaces;

namespace Amphetamine.Data.Abstractions
{
	public abstract class Entity : Entity<long>
	{
		public override long Id { get; set; }
	}

	public abstract class Entity<TKey> : IEntity<TKey> where TKey : struct, IEquatable<TKey>, IComparable<TKey>
	{
		public virtual TKey Id { get; set; }

		public abstract override string ToString();

		public override bool Equals(object obj)
		{
			return obj != null && obj is Entity<TKey> && Id.Equals(((Entity<TKey>)obj).Id);
		}

		protected bool Equals(Entity<TKey> other)
		{
			return Id.Equals(other.Id);
		}

		public override int GetHashCode()
		{
			// ReSharper disable once NonReadonlyMemberInGetHashCode
			return Id.GetHashCode();
		}
	}
}
