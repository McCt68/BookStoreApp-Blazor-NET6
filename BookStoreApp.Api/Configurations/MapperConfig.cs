using AutoMapper;
using BookStoreApp.Api.Data;
using BookStoreApp.Api.Models.Author;
using BookStoreApp.Api.Models.Book;

namespace BookStoreApp.Api.Configurations
{
	public class MapperConfig: Profile
	{
        public MapperConfig()
        {
            // Kinda create an Author from a AuthorCreateDto object, and reverse ??
            CreateMap<AuthorCreateDto, Author>().ReverseMap();
            CreateMap<AuthorReadOnlyDto, Author>().ReverseMap();
            CreateMap<AuthorUpdateDto, Author>().ReverseMap();

            CreateMap<BookCreateDto, Book>().ReverseMap();
			CreateMap<BookUpdateDto, Book>().ReverseMap();

			// Map a book object to a BookReadOnlyDto object and kinda use an inner join -
			// So I join the Auhtor First and LastName from the book object to the BookReadOnly Object's -
			// AuthorName Property
			CreateMap<Book, BookReadOnlyDto>()
				.ForMember(q => q.AuthorName, d => d.MapFrom(map => $"{map.Author.FirstName} {map.Author.LastName}"))
				.ReverseMap();

			CreateMap<Book, BookDetailsDto>()
				.ForMember(q => q.AuthorName, d => d.MapFrom(map => $"{map.Author.FirstName} {map.Author.LastName}"))
				.ReverseMap();

		}
    }
}
