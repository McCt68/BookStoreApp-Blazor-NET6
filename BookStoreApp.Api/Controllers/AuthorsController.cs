using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.Api.Data;
using BookStoreApp.Api.Models.Author;
using AutoMapper;
using BookStoreApp.Api.Static;
using Microsoft.AspNetCore.Authorization;

// Every method inside a controller is called an Action

namespace BookStoreApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthorsController : ControllerBase
    {
        private readonly BookStoreDbContext _context;
		private readonly IMapper mapper;
		private readonly ILogger<AuthorsController> logger;

		// context and autoMapper provided with DI. The Di is defined in Program.cs
		public AuthorsController(BookStoreDbContext context, IMapper mapper, ILogger<AuthorsController> logger)
        {
            _context = context;
			this.mapper = mapper;
			this.logger = logger;
		}

        // GET: api/Authors
        // Basically return SELECT * FROM Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorReadOnlyDto>>> GetAuthors()
        {
            // logger.LogInformation($"Request to {nameof(GetAuthors)}");
            try
            {
				var authors = await _context.Authors.ToListAsync();
				var authorDtos = mapper.Map<IEnumerable<AuthorReadOnlyDto>>(authors);
				return Ok(authorDtos); // return to calling client 
			}
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error Performing GET in {nameof(GetAuthors)}");
                // return StatusCode(500, "There was an error completing your request. Please try again later");
                return StatusCode(500, Messages.Error500Message);
            }                     
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorReadOnlyDto>> GetAuthorById(int id)
        {
            try
            {
				var author = await _context.Authors.FindAsync(id);

				if (author == null)
				{
                    logger.LogWarning(id, $"Record with id:{nameof(GetAuthorById)} Not found - ID: {id}");
					return NotFound();
				}
				var authorDto = mapper.Map<AuthorReadOnlyDto>(author);
				return Ok(authorDto);
			}catch(Exception ex)
            {
				logger.LogError(ex, $"Error Performing GET in {nameof(GetAuthors)}");
				// return StatusCode(500, "There was an error completing your request. Please try again later");
				return StatusCode(500, Messages.Error500Message);
			}          
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> PutAuthor(int id, AuthorUpdateDto authorDto)
        {
            if (id != authorDto.Id)
            {
                logger.LogWarning($"Update Id invalid in {nameof(PutAuthor)} - Id:{id}");
                return BadRequest(); // 400
            }

            var author = await _context.Authors.FindAsync(id);
			if (author == null)
			{
                logger.LogWarning($"{nameof(Author)} record not found in {nameof(PutAuthor)} - ID {id}");
				return NotFound();
			}

			mapper.Map(authorDto, author);
            _context.Entry(author).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await AuthorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    logger.LogError(ex, $"Error performing GET in {nameof(PutAuthor)}");
                    return StatusCode(500, Messages.Error500Message);
                }
            }

            return NoContent(); // We don't get anything back like when we request some data 204
        }

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize (Roles = "Administrator")]
        public async Task<ActionResult<AuthorCreateDto>> PostAuthor(AuthorCreateDto authorDto)
        {
            try
            {
				// sorta convert authorDto into an Author object
				var author = mapper.Map<Author>(authorDto); 
				await _context.Authors.AddAsync(author);
				await _context.SaveChangesAsync();

				return CreatedAtAction(nameof(GetAuthorById), new { id = author.Id }, author); // 201
			}
            catch (Exception ex)
            {
				logger.LogError(ex, $"Error Performing POST in {nameof(PostAuthor)}", authorDto);				
				return StatusCode(500, Messages.Error500Message);
            }                             
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        [Authorize (Roles = "Administrator")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            try
            {
				var author = await _context.Authors.FindAsync(id);
				if (author == null)
				{
                    logger.LogWarning($"{nameof(Author)} record not found in {nameof(DeleteAuthor)} - ID: {id}");
                    return NotFound();
				}

				_context.Authors.Remove(author);
				await _context.SaveChangesAsync();

				return NoContent();
            }
            catch(Exception ex) 
            {
                logger.LogError(ex, $"Error performing DELETE in {nameof(DeleteAuthor)}");
                return StatusCode(500, Messages.Error500Message);
            }            
            
        }

        private async Task<bool> AuthorExists(int id)
        {
            // Original auto generated code
            // return await (_context.Authors?.AnyAsync(e => e.Id == id));.GetValueOrDefault();

            // Code from video 18 20 minutes in
            return await (_context.Authors.AnyAsync(e => e.Id == id));// .GetValueOrDefault();
        }
    }
}
