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
            // Kinda create and Author from a AuthorCreateDto object, and reverse ??
            CreateMap<AuthorCreateDto, Author>().ReverseMap();

            CreateMap<AuthorReadOnlyDto, Author>().ReverseMap();

            CreateMap<AuthorUpdateDto, Author>().ReverseMap();

            CreateMap<BookCreateDto, Book>().ReverseMap();

        }
    }
}
