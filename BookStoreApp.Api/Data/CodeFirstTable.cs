namespace BookStoreApp.Api.Data
{

    // This is not actually used in the final app !

    // Showing how to do a codefirst aproch instead of scafolding from an existing database video 16 - min 13.44
    public class CodeFirstTable
    {
        public int Id { get; set; }
        public string ProductName { get; set; }

        public decimal Price { get; set; }

        // So after adding the properties. i need to manually add them to the BookStoreDbContext class
        // And do migration in nuget packet manager
    }
}
