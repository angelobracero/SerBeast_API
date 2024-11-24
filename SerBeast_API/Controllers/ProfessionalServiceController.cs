using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SerBeast_API.Data;
using SerBeast_API.Model;
using SerBeast_API.Model.Dto;

namespace SerBeast_API.Controllers
{
    [Route("api/ProfessionalService")]
    [ApiController]
    public class ProfessionalServiceController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ApiResponse _response;

        public ProfessionalServiceController(ApplicationDbContext db)
        {
            _db = db;
            _response = new ApiResponse();
        }


        // GET: api/ProfessionalService 
        [HttpGet]
        public async Task<IActionResult> GetProfessionalServices()
        {
            var professionalServices = await _db.ProfessionalServices.ToListAsync();
            _response.Result = professionalServices;
            _response.StatusCode = System.Net.HttpStatusCode.OK;
            return Ok(_response);
        }


        // GET: api/ProfessionalService/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProfessionalServiceById(int id)
        {
            var service = await _db.ProfessionalServices.FindAsync(id);
            if (service == null)
            {
                return NotFound("Service not found.");
            }

            _response.Result = service;
            _response.StatusCode = System.Net.HttpStatusCode.OK;
            return Ok(_response);
        }


        // GET: api/ProfessionalService/professional/{professionalId}
        [HttpGet("professional/{professionalId}")]
        public async Task<IActionResult> GetServicesByProfessionalId(string professionalId)
        {
            var services = await _db.ProfessionalServices
                                    .Where(s => s.ProfessionalId == professionalId)
                                    .Include(ps => ps.Service)       
                                    .Include(ps => ps.Service.Category) 
                                    .Select(ps => new
                                    {
                                        ProfessionalServiceId = ps.ProfessionalServiceId,
                                        Price = ps.Price,
                                        Description = ps.Description,
                                        ServiceName = ps.Service.Title,
                                        CategoryName = ps.Service.Category.Name
                                    })
                                    .ToListAsync();

            if (services == null || !services.Any())
            {
                return NotFound("No services found for the specified Professional ID.");
            }

            _response.Result = services;
            _response.StatusCode = System.Net.HttpStatusCode.OK;
            return Ok(_response);
        }


        // POST: api/ProfessionalService
        [HttpPost]
        public async Task<IActionResult> CreateProfessionalService([FromForm] ProfessionalServiceCreateDTO professionalServiceCreateDTO, IFormFile image)
        {
            // Error handling: Check if the service details are provided
            if (professionalServiceCreateDTO == null)
            {
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Service details are required.");
                return BadRequest(_response);  // Return 400 Bad Request if details are missing
            }

            // Error handling: Check model validity
            if (!ModelState.IsValid)
            {
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Invalid model state.");
                return BadRequest(ModelState);  // Return 400 Bad Request if the model is invalid
            }

            // Process the image if provided
            if (image != null)
            {
                var imageDirectory = Path.Combine("Uploads/images/services");
                if (!Directory.Exists(imageDirectory))
                {
                    Directory.CreateDirectory(imageDirectory); // Create the directory if it doesn't exist
                }

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var imagePath = Path.Combine(imageDirectory, uniqueFileName);

                try
                {
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);  
                    }
                }
                catch (Exception ex)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                    _response.ErrorMessages.Add("Error while saving image: " + ex.Message);
                    return StatusCode(500, _response);  
                }

                professionalServiceCreateDTO.Image = uniqueFileName; 
            }

            // Create the ProfessionalService object
            var professionalService = new ProfessionalService
            {
                CategoryId = professionalServiceCreateDTO.CategoryId,
                ServiceId = professionalServiceCreateDTO.ServiceId,
                ProfessionalId = professionalServiceCreateDTO.ProfessionalId,
                Price = professionalServiceCreateDTO.Price,
                Description = professionalServiceCreateDTO.Description,
                Image = professionalServiceCreateDTO.Image
            };

            try
            {
                _db.ProfessionalServices.Add(professionalService);  
                await _db.SaveChangesAsync();  // Save the changes

                _response.StatusCode = System.Net.HttpStatusCode.Created;
                _response.ErrorMessages.Add("Professional service created successfully.");
                _response.Result = professionalService;

                return CreatedAtAction(nameof(GetProfessionalServiceById), new { id = professionalService.ProfessionalServiceId }, _response);  // Return 201 Created with the resource
            }
            catch (Exception ex)
            {
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("Error while creating the service: " + ex.Message);
                return StatusCode(500, _response);  // Return 500 Internal Server Error if the save fails
            }
        }




        // PUT: api/ProfessionalService/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateService(int id, [FromForm] ProfessionalServiceUpdateDTO professionalServiceUpdateDTO, IFormFile? image)
        {

            // Validate the model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Find the existing service
            var existingProService = await _db.ProfessionalServices.FindAsync(id);
            if (existingProService == null)
            {
                return NotFound("Service not found.");
            }

            // Process the image if provided
            if (image != null)
            {
                var imageDirectory = Path.Combine("Uploads/images/services");
                if (!Directory.Exists(imageDirectory))
                {
                    Directory.CreateDirectory(imageDirectory); 
                }

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var imagePath = Path.Combine(imageDirectory, uniqueFileName);

                try
                {
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    // Delete the old image if it exists
                    if (!string.IsNullOrEmpty(existingProService.Image))
                    {
                        var oldImagePath = Path.Combine(imageDirectory, existingProService.Image);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Update the image path in the model
                    existingProService.Image = uniqueFileName;
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error while saving image: " + ex.Message);
                }
            }

            // Update other fields only if the DTO values are not null
            if (professionalServiceUpdateDTO.ServiceId.HasValue)
            {
                existingProService.ServiceId = professionalServiceUpdateDTO.ServiceId.Value;
            }

            if (professionalServiceUpdateDTO.Price.HasValue)
            {
                existingProService.Price = professionalServiceUpdateDTO.Price.Value;
            }

            if (!string.IsNullOrEmpty(professionalServiceUpdateDTO.Description))
            {
                existingProService.Description = professionalServiceUpdateDTO.Description;
            }

            try
            {
                await _db.SaveChangesAsync(); 
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating the service. Please try again.");
            }

            return NoContent(); // Indicate successful update
        }


        // DELETE: api/ProfessionalService/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _db.ProfessionalServices.FindAsync(id);
            if (service == null)
            {
                return NotFound("Service not found.");
            }

            try
            {
                _db.ProfessionalServices.Remove(service);
                await _db.SaveChangesAsync();
            }
            catch (Exception)
            {
                // Log the exception (ex) as needed for debugging
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting service.");
            }

            return NoContent();
        }



    }
}
