using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BetterRepository.Entities;
using BetterRepository.Models;

namespace BetterRepository.Repositories
{
	internal static class EmployeePersistence
	{
		public static readonly List<Employee> Employees = new();

		static EmployeePersistence()
		{
			Reset();
		}

		public static void Reset()
		{
			Employees.Clear();
			Employees.Add(new Employee(0, "Ahmed"));
			Employees.Add(new Employee(1, "Tarek"));
			Employees.Add(new Employee(2, "Patrick"));
			Employees.Add(new Employee(3, "Mohamed"));
			Employees.Add(new Employee(4, "Sara"));
			Employees.Add(new Employee(5, "Ali"));
		}
	}

	public class EmployeeQueryRepository : QueryRepository<Employee>
	{
		private static int MaxResultsCountPerPage = 5;

		public override IQueryResult<Employee> GetAll()
		{
			return Get(emp => true, null, null);
		}

		public override Employee Get(int id)
		{
			return EmployeePersistence.Employees.FirstOrDefault(e => e.Id == id);
		}

		public override IQueryResult<Employee> Get(int pageSize, int pageIndex)
		{
			return Get(emp => true, pageSize, pageIndex);
		}

		public override IQueryResult<Employee> GetByExpression(Expression<Func<Employee, bool>> predicate)
		{
			return Get(predicate, null, null);
		}

		public override IQueryResult<Employee> GetByExpression(Expression<Func<Employee, bool>> predicate, int pageSize, int pageIndex)
		{
			return Get(predicate, pageSize, pageIndex);
		}

		private static IQueryResult<Employee> Get(Expression<Func<Employee, bool>> predicate, int? pageSize, int? pageIndex)
		{
			var filteredItems = 
				predicate != null ? 
					EmployeePersistence.Employees.AsQueryable().Where(predicate).ToList() : 
					EmployeePersistence.Employees;

			var finalPageSize = Math.Min(MaxResultsCountPerPage, filteredItems.Count);
			var finalPageIndex = 0;

			if (pageSize != null)
			{
				if (pageSize <= MaxResultsCountPerPage)
				{
					finalPageSize = pageSize.Value;
					finalPageIndex = pageIndex ?? 0;
				}
				else
				{
					finalPageSize = MaxResultsCountPerPage;

					if (pageIndex != null)
					{
						var oldPagingDescriptor = filteredItems.Page(pageSize.Value);
						var oldPageBoundries = oldPagingDescriptor.PagesBoundries[pageIndex.Value];
						var targetedItemZeroIndex = oldPageBoundries.FirstItemZeroIndex;

						var newPagingDescriptor = filteredItems.Page(finalPageSize);
						
						finalPageIndex =
							newPagingDescriptor
								.PagesBoundries
								.ToList()
								.FindIndex(i => i.FirstItemZeroIndex <= targetedItemZeroIndex && i.LastItemZeroIndex >= targetedItemZeroIndex);
					}
				}
			}

			var pagingDescriptor = filteredItems.Page(finalPageSize);
			var pageBoundries = pagingDescriptor.PagesBoundries[finalPageIndex];
			var from = pageBoundries.FirstItemZeroIndex;
			var to = pageBoundries.LastItemZeroIndex;

			return new QueryResult<Employee>(pagingDescriptor, finalPageIndex, filteredItems.Skip(from).Take(to - from + 1));
		}
	}

	public class EmployeeCommandRepository : CommandRepository<Employee>
	{
		public override int Add(Employee entity)
		{
			var newEmployee = new Employee(entity, EmployeePersistence.Employees.Count);
			EmployeePersistence.Employees.Add(newEmployee);
			return newEmployee.Id;
		}

		public override IEnumerable<int> Add(IEnumerable<Employee> entities)
		{
			return entities.Select(Add).ToList();
		}

		public override void Update(Employee entity)
		{
			var foundIndex = EmployeePersistence.Employees.FindIndex(e => e.Id == entity.Id);

			if (foundIndex == -1)
			{
				throw new InvalidOperationException($"Employee with Id \"{entity.Id}\" does not exist.");
			}

			var foundEmployee = EmployeePersistence.Employees[foundIndex];
			var newEmployee = new Employee(entity, foundEmployee.Id);
			EmployeePersistence.Employees.RemoveAt(foundIndex);
			EmployeePersistence.Employees.Insert(foundIndex, newEmployee);
		}

		public override void Update(IEnumerable<Employee> entities)
		{
			foreach (var employee in entities)
			{
				Update(employee);
			}
		}

		public override IAddOrUpdateDescriptor AddOrUpdate(Employee entity)
		{
			var foundIndex = EmployeePersistence.Employees.FindIndex(e => e.Id == entity.Id);

			if (foundIndex != -1)
			{
				Update(entity);
				return new AddOrUpdateDescriptor(Models.AddOrUpdate.Update, entity.Id);
			}
			else
			{
				return new AddOrUpdateDescriptor(Models.AddOrUpdate.Add, Add(entity));
			}
		}

		public override IEnumerable<IAddOrUpdateDescriptor> AddOrUpdate(IEnumerable<Employee> entities)
		{
			return entities.Select(AddOrUpdate).ToList();
		}

		public override bool Delete(int id)
		{
			var foundIndex = EmployeePersistence.Employees.FindIndex(e => e.Id == id);

			if (foundIndex == -1) return false;

			EmployeePersistence.Employees.RemoveAt(foundIndex);
			return true;
		}

		public override bool Delete(Employee entity)
		{
			return Delete(entity.Id);
		}

		public override IDictionary<int, bool> Delete(IEnumerable<Employee> entities)
		{
			return entities
			       .ToDictionary(emp => emp.Id, Delete);
		}
	}
}
