using System;
using System.Linq.Expressions;
using BetterRepository.Entities;

namespace BetterRepository.Models
{
	public interface IQueryRepository
	{
		IQueryResult<Entity> GetAll();
		Entity Get(int id);
		IQueryResult<Entity> Get(int pageSize, int pageIndex);
		//IQueryResult<Entity> Get(Expression<Func<Entity, bool>> predicate);
		//IQueryResult<Entity> Get(Expression<Func<Entity, bool>> predicate, int pageSize, int pageIndex);
	}

	public interface IQueryRepository<TEntity> : IQueryRepository where TEntity : Entity
	{
		new IQueryResult<TEntity> GetAll();
		new TEntity Get(int id);
		new IQueryResult<TEntity> Get(int pageSize, int pageIndex);
		IQueryResult<TEntity> GetByExpression(Expression<Func<TEntity, bool>> predicate);
		IQueryResult<TEntity> GetByExpression(Expression<Func<TEntity, bool>> predicate, int pageSize, int pageIndex);

		//abstract IQueryResult<Entity> IQueryRepository.Get(Expression<Func<Entity, bool>> predicate);
		//abstract IQueryResult<Entity> IQueryRepository.Get(Expression<Func<Entity, bool>> predicate, int pageSize, int pageIndex);
	}

	public abstract class QueryRepository<TEntity> : IQueryRepository<TEntity> where TEntity : Entity
	{
		public abstract IQueryResult<TEntity> GetAll();

		public abstract IQueryResult<TEntity> Get(int pageSize, int pageIndex);
		
		public abstract TEntity Get(int id);

		public abstract IQueryResult<TEntity> GetByExpression(Expression<Func<TEntity, bool>> predicate);

		public abstract IQueryResult<TEntity> GetByExpression(Expression<Func<TEntity, bool>> predicate, int pageSize, int pageIndex);


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

		/*IQueryResult<Entity> IQueryRepository.Get(Expression<Func<Entity, bool>> predicate)
		{
			var passedInType = predicate.Parameters[0].Type;

			if (typeof(TEntity).IsAssignableFrom(passedInType))
			{
				return GetByExpression(predicate);
			}

			throw new ArgumentException(
				$"The type \"{passedInType}\" does not match the type \"{typeof(TEntity)}\"");
		}*/

		/*IQueryResult<Entity> IQueryRepository.Get(Expression<Func<Entity, bool>> predicate, int pageSize, int pageIndex)
		{
			var passedInType = predicate.Parameters[0].Type;

			if (typeof(TEntity).IsAssignableFrom(passedInType))
			{
				return GetByExpression(predicate, pageSize, pageIndex);
			}

			throw new ArgumentException(
				$"The type \"{passedInType}\" does not match the type \"{typeof(TEntity)}\"");
		}*/
	}
}
