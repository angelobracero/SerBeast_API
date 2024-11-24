using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SerBeast_API.Data;
using SerBeast_API.Model;
using SerBeast_API.Model.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SerBeast_API.Controllers
{
    [Route("api/Category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ApiResponse _response;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
            _response = new ApiResponse();
        }

        // POST: api/Category
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest("Category details are required.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newCategory = new Category
            {
                Name = category.Name,
            };

            _db.Categories.Add(newCategory);
            await _db.SaveChangesAsync();

            _response.Result = newCategory;
            _response.StatusCode = HttpStatusCode.Created;

            return CreatedAtAction(nameof(GetCategoryById), new { id = newCategory.Id }, _response);
        }

        // GET: api/Category/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            _response.Result = category;
            _response.StatusCode = HttpStatusCode.OK;

            return Ok(_response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await _db.Categories
                    .Include(c => c.Services)
                    .ToListAsync();

                var categoryDtos = categories.Select(c => new CategoryGetDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Services = c.Services.Select(s => new ServiceGetDTO
                    {
                        Id = s.Id,
                        Title = s.Title,
                        Description = s.Description,
                        ImageUrl = s.ImageUrl
                    }).ToList()
                }).ToList();

                _response.Result = categoryDtos; 
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
                _response.StatusCode = HttpStatusCode.InternalServerError;

                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }



        // PUT: api/Category/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category category)
        {
            if (category == null || id != category.Id)
            {
                return BadRequest("Category details are required.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCategory = await _db.Categories.FindAsync(id);
            if (existingCategory == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            existingCategory.Name = category.Name;
            // Update other properties if necessary

            await _db.SaveChangesAsync();

            _response.Result = existingCategory;
            _response.StatusCode = HttpStatusCode.OK;

            return Ok(_response);
        }
    }
}
