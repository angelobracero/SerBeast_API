using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SerBeast_API.Data;
using SerBeast_API.Model;
using SerBeast_API.Model.Dto;
using System.Net;

namespace SerBeast_API.Controllers
{
    [Route("api/Service")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ApiResponse _response;

        public ServiceController(ApplicationDbContext db)
        {
            _db = db;
            _response = new ApiResponse();
        }

        [HttpGet(Name = "GetAllServices")]
        public async Task<IActionResult> GetAllServices()
        {
            var services = await _db.Services
                .Select(s => new ServiceGetDTO
                {
                    Id = s.Id,
                    Title = s.Title,
                    ProfessionalServices = s.ProfessionalServices
                        .Select(ps => new ServiceProfessionalServiceDTO
                        {
                            ProfessionalServiceId = ps.ProfessionalServiceId,
                            Price = ps.Price,
                            ProfessionalId = ps.ProfessionalId,
                            ProfessionalName = $"{ps.Professional.FirstName} {ps.Professional.LastName}",
                            Barangay = ps.Professional.Barangay,
                            Description = ps.Professional.Description,
                            Rating = (decimal)ps.Professional.Rating
                        }).ToList()
                })
                .ToListAsync();

            if (services is null || !services.Any())
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                return NotFound(_response);
            }

            _response.Result = services;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }


        [HttpGet("{id:int}", Name = "GetService")]
        public async Task<IActionResult> GetService(int id)
        {
            if (id <= 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

            var service = await _db.Services
                .Where(s => s.Id == id)
                .Select(s => new ServiceGetDTO
                {
                    Id = s.Id,
                    Title = s.Title,
                    ProfessionalServices = s.ProfessionalServices
                        .Select(ps => new ServiceProfessionalServiceDTO
                        {
                            ProfessionalServiceId = ps.ProfessionalServiceId,
                            Price = ps.Price,
                            ProfessionalId = ps.ProfessionalId,
                            ProfessionalName = $"{ps.Professional.FirstName} {ps.Professional.LastName}",
                            Barangay = ps.Professional.Barangay,
                            Description = ps.Professional.Description,
                            Rating = (decimal)ps.Professional.Rating,
                            PhoneNumber = ps.Professional.PhoneNumber
                        }).ToList()
                })
                .FirstOrDefaultAsync();

            if (service is null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                return NotFound(_response);
            }

            _response.Result = service;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }



        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateService([FromForm] ServiceCreateDTO serviceCreateDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                if (serviceCreateDTO.File == null || serviceCreateDTO.File.Length == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { "File is required." };
                    return BadRequest(_response);
                }

                string uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "images", "services");
                Directory.CreateDirectory(uploadsPath);
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(serviceCreateDTO.File.FileName);
                string filePath = Path.Combine(uploadsPath, fileName);


                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await serviceCreateDTO.File.CopyToAsync(fileStream); 
                }

                // Create a new service object
                var serviceToCreate = new Service()
                {
                    Title = serviceCreateDTO.Title,
                    Description = serviceCreateDTO.Description,
                    CategoryId = serviceCreateDTO.CategoryId,
                    ImageUrl = fileName 
                };

                await _db.Services.AddAsync(serviceToCreate);
                await _db.SaveChangesAsync();

                _response.Result = serviceToCreate;
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetService", new { id = serviceToCreate.Id }, _response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };

                // Log the inner exception details
                if (ex.InnerException != null)
                {
                    _response.ErrorMessages.Add(ex.InnerException.Message);
                }

                return StatusCode(500, _response);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse>> UpdateService(int id, [FromForm] ServiceUpdateDTO serviceUpdateDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                    return BadRequest(_response);
                }

                if (serviceUpdateDTO == null || id != serviceUpdateDTO.Id)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { "Invalid service data." };
                    return BadRequest(_response);
                }

                var serviceFromDb = await _db.Services.FindAsync(id);

                if (serviceFromDb == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { "Service not found." };
                    return NotFound(_response); 
                }

                serviceFromDb.Title = serviceUpdateDTO.Title;
                serviceFromDb.Description = serviceUpdateDTO.Description;
                serviceFromDb.CategoryId = serviceUpdateDTO.CategoryId;

                if (serviceUpdateDTO.File != null && serviceUpdateDTO.File.Length > 0)
                {
                    string uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "images", "services");
                    string oldImagePath = Path.Combine(uploadsPath, serviceFromDb.ImageUrl);

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }

                    string newFileName = Guid.NewGuid() + Path.GetExtension(serviceUpdateDTO.File.FileName);
                    string newFilePath = Path.Combine(uploadsPath, newFileName);

                    using (var fileStream = new FileStream(newFilePath, FileMode.Create))
                    {
                        await serviceUpdateDTO.File.CopyToAsync(fileStream);
                    }

                    serviceFromDb.ImageUrl = newFileName;
                }

                _db.Services.Update(serviceFromDb);
                await _db.SaveChangesAsync();

                _response.Result = serviceFromDb;
                _response.StatusCode = HttpStatusCode.OK; 
                return Ok(_response); 
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };

                if (ex.InnerException != null)
                {
                    _response.ErrorMessages.Add(ex.InnerException.Message);
                }

                return StatusCode(500, _response);
            }
        }

        [HttpDelete("{id:int}")] 
        public async Task<ActionResult<ApiResponse>> DeleteService(int id)
        {
            try
            {
                if (id <= 0) 
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { "Invalid service ID." };
                    return BadRequest(_response);
                }

                Service serviceFromDb = await _db.Services.FindAsync(id);

                if (serviceFromDb == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { "Service not found." };
                    return NotFound(_response);
                }

                string uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "images", "services");

                if (!string.IsNullOrEmpty(serviceFromDb.ImageUrl)) 
                {
                    string oldImagePath = Path.Combine(uploadsPath, serviceFromDb.ImageUrl);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                _db.Services.Remove(serviceFromDb);
                await _db.SaveChangesAsync();

                _response.Result = serviceFromDb; 
                _response.StatusCode = HttpStatusCode.NoContent; 
                return NoContent(); 
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };

                if (ex.InnerException != null)
                {
                    _response.ErrorMessages.Add(ex.InnerException.Message);
                }

                return StatusCode(500, _response);
            }
        }
    }
}
