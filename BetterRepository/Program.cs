using System;
using BetterRepository.Entities;
using BetterRepository.Models;
using BetterRepository.Repositories;

namespace BetterRepository
{
	internal class Program
	{
		private static readonly EmployeeQueryRepository m_EmployeeQueryRepository = new();
		private static readonly EmployeeCommandRepository m_EmployeeCommandRepository = new();

		static void Main(string[] args)
		{
			DemonstratingBasicOperation();
			DemonstratingWrongCastingChecks();
			DemonstratingDealingWithAbstractions();
			Console.ReadLine();
		}

		public static void DemonstratingBasicOperation()
		{
			Console.WriteLine("Started DemonstratingBasicOperation");

			var result = m_EmployeeQueryRepository.GetAll();
			Console.WriteLine(result);

			/*
			ActualPageZeroIndex: 0

			PagingDescriptor:
			----------------
			ActualPageSize: 5
			NumberOfPages: 2

			PagesBoundries:
			----------------
			FirstItemZeroIndex: 0, LastItemZeroIndex: 4
			FirstItemZeroIndex: 5, LastItemZeroIndex: 5

			Results:
			----------------
			Id: 0, Name: Ahmed
			Id: 1, Name: Tarek
			Id: 2, Name: Patrick
			Id: 3, Name: Mohamed
			Id: 4, Name: Sara
			*/


			result = m_EmployeeQueryRepository.Get(result.PagingDescriptor.ActualPageSize, result.ActualPageZeroIndex + 1);
			Console.WriteLine(result);

			/*
			ActualPageZeroIndex: 1

			PagingDescriptor:
			----------------
			ActualPageSize: 5
			NumberOfPages: 2

			PagesBoundries:
			----------------
			FirstItemZeroIndex: 0, LastItemZeroIndex: 4
			FirstItemZeroIndex: 5, LastItemZeroIndex: 5

			Results:
			----------------
			Id: 5, Name: Ali
			*/


			result = m_EmployeeQueryRepository.Get(6, 0);
			Console.WriteLine(result);

			/*
			ActualPageZeroIndex: 0

			PagingDescriptor:
			----------------
			ActualPageSize: 5
			NumberOfPages: 2

			PagesBoundries:
			----------------
			FirstItemZeroIndex: 0, LastItemZeroIndex: 4
			FirstItemZeroIndex: 5, LastItemZeroIndex: 5

			Results:
			----------------
			Id: 0, Name: Ahmed
			Id: 1, Name: Tarek
			Id: 2, Name: Patrick
			Id: 3, Name: Mohamed
			Id: 4, Name: Sara
			*/


			var tarek = m_EmployeeQueryRepository.Get(2);
			Console.WriteLine(tarek);

			/*
			Id: 1, Name: Tarek
			*/


			result = m_EmployeeQueryRepository.GetByExpression(emp => emp.Name.ToLower().Contains("t"));
			Console.WriteLine(result);

			/*
			ActualPageZeroIndex: 0

			PagingDescriptor:
			----------------
			ActualPageSize: 2
			NumberOfPages: 1

			PagesBoundries:
			----------------
			FirstItemZeroIndex: 0, LastItemZeroIndex: 1

			Results:
			----------------
			Id: 1, Name: Tarek
			Id: 2, Name: Patrick
			*/


			var erikId = m_EmployeeCommandRepository.Add(new Employee(0, "Erik"));
			Console.WriteLine(erikId);

			/*
			6
			*/


			var added = m_EmployeeCommandRepository.Add(new Employee[]
			{
				new Employee(0, "Hasan"),
				new Employee(0, "Mai"),
				new Employee(0, "John")
			});

			Console.WriteLine("");
			Console.WriteLine(String.Join("\r\n", added));

			/*
			7
			8
			9
			*/


			m_EmployeeCommandRepository.Update(new Employee(1, "Tarek - Updated"));
			var tarekUpdated = m_EmployeeQueryRepository.Get(1);
			Console.WriteLine("");
			Console.WriteLine(tarekUpdated);

			/*
			Id: 1, Name: Tarek - Updated
			*/


			m_EmployeeCommandRepository.AddOrUpdate(new Employee(1, "Tarek - Updated - Updated"));
			var tarekUpdatedUpdated = m_EmployeeQueryRepository.Get(1);
			Console.WriteLine("");
			Console.WriteLine(tarekUpdatedUpdated);

			/*
			Id: 1, Name: Tarek - Updated - Updated
			*/


			var deletedTarek = m_EmployeeCommandRepository.Delete(1);
			Console.WriteLine("");
			Console.WriteLine(deletedTarek);

			/*
			True
			*/


			var checkTarek = m_EmployeeQueryRepository.Get(1);
			Console.WriteLine("");
			Console.WriteLine(checkTarek != null);

			/*
			False
			*/

			Console.WriteLine("Finished DemonstratingBasicOperation");
		}

		public static void DemonstratingWrongCastingChecks()
		{
			Console.WriteLine("");
			Console.WriteLine("");
			Console.WriteLine("Started DemonstratingWrongCastingChecks");

			var queryRepository = m_EmployeeQueryRepository as IQueryRepository;
			var commandRepository = m_EmployeeCommandRepository as ICommandRepository;

			try
			{
				commandRepository.Add(new Student());
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);

				/*
				System.ArgumentException: 'The type "BetterRepository.Student" does not match the type 
				"BetterRepository.Entities.Employee"'
				*/
			}

			try
			{
				commandRepository.Add(new Student[]
				{
					new Student(),
					new Student()
				});
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);

				/*
				System.ArgumentException: The type "BetterRepository.Student[]" does not match the type 
				"System.Collections.Generic.IEnumerable`1[BetterRepository.Entities.Employee]"
				*/
			}

			try
			{
				bool FilterStudents(object obj)
				{
					return true;
				}

				// Compiler would not allow it as Func<Entity, bool> predicate is contravariant
				// This means that for the predicate parameter, you can only provide Func<Entity, bool>
				// or Func<Parent of Entity, bool>. In our case, the only available parent of Entity is
				// the Object class.
				//queryRepository.Get(FilterStudents);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			Console.WriteLine("Finished DemonstratingWrongCastingChecks");
		}

		public static void DemonstratingDealingWithAbstractions()
		{
			Console.WriteLine("");
			Console.WriteLine("");
			Console.WriteLine("Started DemonstratingDealingWithAbstractions");

			// Resetting the employees collection
			EmployeePersistence.Reset();

			var queryRepository = m_EmployeeQueryRepository as IQueryRepository;
			var commandRepository = m_EmployeeCommandRepository as ICommandRepository;

			// Getting first two Employees when we actually don't know their type
			// and we don't care about their type
			var firstTwoItems = queryRepository.Get(2, 0);
			Console.WriteLine(firstTwoItems);

			/*
			Id: 0, Name: Ahmed
			Id: 1, Name: Tarek
			*/

			// Now we are deleting the first two items blindly when we don't know their type
			// and we don't care about their type
			commandRepository.Delete(firstTwoItems.Results);

			// Now we get the first two Employees again to check if it worked
			firstTwoItems = queryRepository.Get(2, 0);
			Console.WriteLine(firstTwoItems);

			/*
			Id: 2, Name: Patrick
			Id: 3, Name: Mohamed
			*/

			Console.WriteLine("Finished DemonstratingDealingWithAbstractions");
		}
	}

	public class Student : Entity
	{

	}
}
