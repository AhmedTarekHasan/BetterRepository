using System.Collections.Generic;
using System.Text;
using BetterRepository.Entities;

namespace BetterRepository.Models
{
	public interface IQueryResult
	{
		PagingDescriptor PagingDescriptor { get; }
		int ActualPageZeroIndex { get; }
		IEnumerable<Entity> Results { get; }
	}

	public interface IQueryResult<out TEntity> : IQueryResult where TEntity : Entity
	{
		new IEnumerable<TEntity> Results { get; }
	}

	public class QueryResult<TEntity> : IQueryResult<TEntity> where TEntity : Entity
	{
		public QueryResult(
			PagingDescriptor pagingDescriptor,
			int actualPageZeroIndex,
			IEnumerable<TEntity> results)
		{
			PagingDescriptor = pagingDescriptor;
			ActualPageZeroIndex = actualPageZeroIndex;
			Results = results;
		}

		public PagingDescriptor PagingDescriptor { get; }

		public int ActualPageZeroIndex { get; }

		public IEnumerable<TEntity> Results { get; }

		IEnumerable<Entity> IQueryResult.Results => Results;


		// This code is added for demonstration purposes only.
		public override string ToString()
		{
			var builder = new StringBuilder();
			builder.AppendLine($"ActualPageZeroIndex: {ActualPageZeroIndex}");
			builder.AppendLine("");
			builder.AppendLine($"PagingDescriptor:");
			builder.AppendLine($"----------------");
			builder.AppendLine(PagingDescriptor.ToString());
			builder.AppendLine($"Results:");
			builder.AppendLine($"----------------");

			foreach (var entity in Results)
			{
				builder.Append(entity.ToString());
			}

			return builder.ToString();
		}
	}
}