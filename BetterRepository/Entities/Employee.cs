using System;
using System.Text;

namespace BetterRepository.Entities
{
	public class Employee : Entity
	{
		public int Id { get; }
		public string Name { get; }

		public Employee(int id, string name)
		{
			Id = id;
			Name = name;
		}

		public Employee(Employee other, int id)
		{
			if (other == null) throw new ArgumentException("Other cannot be null");

			Id = id;
			Name = other.Name;
		}

		// This code is added for demonstration purposes only.
		public override string ToString()
		{
			var builder = new StringBuilder();
			builder.AppendLine($"Id: {Id}, Name: {Name}");
			return builder.ToString();
		}
	}
}