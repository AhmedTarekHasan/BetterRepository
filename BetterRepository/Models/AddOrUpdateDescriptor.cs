namespace BetterRepository.Models
{
	public enum AddOrUpdate
	{
		Add,
		Update
	}

	public interface IAddOrUpdateDescriptor
	{
		AddOrUpdate ActionType { get; }
		int Id { get; }
	}

	public class AddOrUpdateDescriptor : IAddOrUpdateDescriptor
	{
		public AddOrUpdate ActionType { get; }
		public int Id { get; }

		public AddOrUpdateDescriptor(AddOrUpdate actionType, int id)
		{
			ActionType = actionType;
			Id = id;
		}
	}
}
