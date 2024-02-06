namespace BookStoreApp.Api.Models.Author
{
	public class AuthorReadOnlyDto: BaseDto
	{
        // Used for read operations
        // get the Id from BaseDto

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }
    }
}
