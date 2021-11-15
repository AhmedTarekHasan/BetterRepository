using System;
using BetterRepository.Entities;

namespace BetterRepository.Models
{
	public interface IQueryRepository
	{
		IQueryResult<Entity> GetAll();
		Entity Get(int id);
		IQueryResult<Entity> Get(int pageSize, int pageIndex);
		IQueryResult<Entity> Get(Func<Entity, bool> predicate);
		IQueryResult<Entity> Get(Func<Entity, bool> predicate, int pageSize, int pageIndex);
	}

	public interface IQueryRepository<out TEntity> : IQueryRepository where TEntity : Entity
	{
		new IQueryResult<TEntity> GetAll();
		new TEntity Get(int id);
		new IQueryResult<TEntity> Get(int pageSize, int pageIndex);
		IQueryResult<TEntity> Get(Func<TEntity, bool> predicate);
		IQueryResult<TEntity> Get(Func<TEntity, bool> predicate, int pageSize, int pageIndex);

		abstract IQueryResult<Entity> IQueryRepository.Get(Func<Entity, bool> predicate);
		abstract IQueryResult<Entity> IQueryRepository.Get(Func<Entity, bool> predicate, int pageSize, int pageIndex);
	}

	public abstract class QueryRepository<TEntity> : IQueryRepository<TEntity> where TEntity : Entity
	{
		public abstract IQueryResult<TEntity> GetAll();

		public abstract IQueryResult<TEntity> Get(int pageSize, int pageIndex);
		
		public abstract TEntity Get(int id);

		public abstract IQueryResult<TEntity> Get(Func<TEntity, bool> predicate);

		public abstract IQueryResult<TEntity> Get(Func<TEntity, bool> predicate, int pageSize, int pageIndex);


		IQueryResult<Entity> IQueryRepository.GetAll()
		{
			return GetAll();
		}

		Entity IQueryRepository.Get(int id)
		{
			return Get(id);
		}

		IQueryResult<Entity> IQueryRepository.Get(int pageSize, int pageIndex)
		{
			return Get(pageSize, pageIndex);
		}

		IQueryResult<Entity> IQueryRepository.Get(Func<Entity, bool> predicate)
		{
			var passedInType = predicate.Method.GetParameters()[0].ParameterType;

			if (typeof(TEntity).IsAssignableFrom(passedInType))
			{
				return Get(predicate);
			}

			throw new ArgumentException(
				$"The type \"{passedInType}\" does not match the type \"{typeof(TEntity)}\"");
		}

		IQueryResult<Entity> IQueryRepository.Get(Func<Entity, bool> predicate, int pageSize, int pageIndex)
		{
			var passedInType = predicate.Method.GetParameters()[0].ParameterType;

			if (typeof(TEntity).IsAssignableFrom(passedInType))
			{
				return Get(predicate, pageSize, pageIndex);
			}

			throw new ArgumentException(
				$"The type \"{passedInType}\" does not match the type \"{typeof(TEntity)}\"");
		}
	}
}
