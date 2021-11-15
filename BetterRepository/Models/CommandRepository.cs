using System;
using System.Collections.Generic;
using System.Linq;
using BetterRepository.Entities;

namespace BetterRepository.Models
{
	public interface ICommandRepository
	{
		int Add(Entity entity);
		IEnumerable<int> Add(IEnumerable<Entity> entities);
		void Update(Entity entity);
		void Update(IEnumerable<Entity> entities);
		IAddOrUpdateDescriptor AddOrUpdate(Entity entity);
		IEnumerable<IAddOrUpdateDescriptor> AddOrUpdate(IEnumerable<Entity> entities);
		bool Delete(int id);
		bool Delete(Entity entity);
		IDictionary<int, bool> Delete(IEnumerable<Entity> entities);
	}

	public interface ICommandRepository<in TEntity> : ICommandRepository where TEntity : Entity
	{
		int Add(TEntity entity);
		IEnumerable<int> Add(IEnumerable<TEntity> entities);
		void Update(TEntity entity);
		void Update(IEnumerable<TEntity> entities);
		IAddOrUpdateDescriptor AddOrUpdate(TEntity entity);
		IEnumerable<IAddOrUpdateDescriptor> AddOrUpdate(IEnumerable<TEntity> entities);
		bool Delete(TEntity entity);
		IDictionary<int, bool> Delete(IEnumerable<TEntity> entities);

		abstract int ICommandRepository.Add(Entity entity);
		abstract IEnumerable<int> ICommandRepository.Add(IEnumerable<Entity> entities);
		abstract void ICommandRepository.Update(Entity entity);
		abstract void ICommandRepository.Update(IEnumerable<Entity> entities);
		abstract IAddOrUpdateDescriptor ICommandRepository.AddOrUpdate(Entity entity);
		abstract IEnumerable<IAddOrUpdateDescriptor> ICommandRepository.AddOrUpdate(IEnumerable<Entity> entities);
		abstract bool ICommandRepository.Delete(Entity entity);
		abstract IDictionary<int, bool> ICommandRepository.Delete(IEnumerable<Entity> entities);
	}

	public abstract class CommandRepository<TEntity> : ICommandRepository<TEntity> where TEntity : Entity
	{
		public abstract bool Delete(int id);
		public abstract int Add(TEntity entity);
		public abstract IEnumerable<int> Add(IEnumerable<TEntity> entities);
		public abstract void Update(TEntity entity);
		public abstract void Update(IEnumerable<TEntity> entities);
		public abstract IAddOrUpdateDescriptor AddOrUpdate(TEntity entity);
		public abstract IEnumerable<IAddOrUpdateDescriptor> AddOrUpdate(IEnumerable<TEntity> entities);
		public abstract bool Delete(TEntity entity);
		public abstract IDictionary<int, bool> Delete(IEnumerable<TEntity> entities);


		int ICommandRepository.Add(Entity entity)
		{
			if (entity.GetType() == typeof(TEntity))
			{
				return Add(entity as TEntity);
			}

			throw new ArgumentException(
				$"The type \"{entity.GetType()}\" does not match the type \"{typeof(TEntity)}\"");
		}

		IEnumerable<int> ICommandRepository.Add(IEnumerable<Entity> entities)
		{
			if (entities is IEnumerable<TEntity>)
			{
				return Add(entities.Select(e => e as TEntity));
			}

			throw new ArgumentException(
				$"The type \"{entities.GetType()}\" does not match the type \"{typeof(IEnumerable<TEntity>)}\"");
		}

		void ICommandRepository.Update(Entity entity)
		{
			if (entity.GetType() == typeof(TEntity))
			{
				Update(entity as TEntity);
			}
			else
			{
				throw new ArgumentException(
					$"The type \"{entity.GetType()}\" does not match the type \"{typeof(TEntity)}\"");
			}
		}

		void ICommandRepository.Update(IEnumerable<Entity> entities)
		{
			if (entities is IEnumerable<TEntity>)
			{
				Update(entities.Select(e => e as TEntity));
			}
			else
			{
				throw new ArgumentException(
					$"The type \"{entities.GetType()}\" does not match the type \"{typeof(IEnumerable<TEntity>)}\"");
			}
		}

		IAddOrUpdateDescriptor ICommandRepository.AddOrUpdate(Entity entity)
		{
			if (entity.GetType() == typeof(TEntity))
			{
				return AddOrUpdate(entity as TEntity);
			}

			throw new ArgumentException(
				$"The type \"{entity.GetType()}\" does not match the type \"{typeof(TEntity)}\"");
		}

		IEnumerable<IAddOrUpdateDescriptor> ICommandRepository.AddOrUpdate(IEnumerable<Entity> entities)
		{
			if (entities is IEnumerable<TEntity>)
			{
				return AddOrUpdate(entities.Select(e => e as TEntity));
			}

			throw new ArgumentException(
				$"The type \"{entities.GetType()}\" does not match the type \"{typeof(IEnumerable<TEntity>)}\"");
		}

		bool ICommandRepository.Delete(Entity entity)
		{
			if (entity.GetType() == typeof(TEntity))
			{
				return Delete(entity as TEntity);
			}

			throw new ArgumentException(
				$"The type \"{entity.GetType()}\" does not match the type \"{typeof(TEntity)}\"");
		}

		IDictionary<int, bool> ICommandRepository.Delete(IEnumerable<Entity> entities)
		{
			if (entities is IEnumerable<TEntity>)
			{
				return Delete(entities.Select(e => e as TEntity));
			}

			throw new ArgumentException(
				$"The type \"{entities.GetType()}\" does not match the type \"{typeof(IEnumerable<TEntity>)}\"");
		}
	}
}
