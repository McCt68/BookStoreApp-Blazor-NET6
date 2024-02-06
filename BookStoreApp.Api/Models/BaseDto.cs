namespace BookStoreApp.Api.Models
{
	// Can not instantiate this abstract class.
	// Its only made so other classess can inherit from it and use the Id property
	public abstract class BaseDto
	{
		public int Id { get; set; }
	}
}
