namespace BookStoreApp.Api.Models.User
{
	public class AuthenticationRepsonse
	{
        public string UserId { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }        
    }
}
