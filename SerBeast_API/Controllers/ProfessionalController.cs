using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SerBeast_API.Data;
using SerBeast_API.Model;
using SerBeast_API.Model.Dto;
using SerBeast_API.Utility;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace SerBeast_API.Controllers
{
    [Route("api/Professional")]
    [ApiController]
    public class ProfessionalController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ApiResponse _response;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfessionalController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _response = new ApiResponse();
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfessionals()
        {
            try
            {
                var allUsers = await _db.ApplicationUsers.ToListAsync();
                var professionals = new List<ProfessionalsGetDTO>();

                foreach (var pro in allUsers)
                {
                    var roles = await _userManager.GetRolesAsync(pro);
                    if (roles.Contains(UserRoles.Role_Professional))
                    {
                        // Fetch the professional services for the current professional
                        var professionalServices = await _db.ProfessionalServices
                                                             .Where(ps => ps.ProfessionalId == pro.Id)
                                                             .Include(ps => ps.Service) // Include related Service details
                                                             .ToListAsync();

                        // Map the professional's services to the DTO
                        var professionalServicesDTO = professionalServices.Select(ps => new ProfessionalServiceGetDTO
                        {
                            ProfessionalServiceId = ps.ProfessionalServiceId,
                            ServiceId = ps.ServiceId,
                            Price = ps.Price,
                            Description = ps.Service.Description,
                            CategoryId = ps.Service.CategoryId,
                            ServiceName = ps.Service.Title 
                        }).ToList();

                        // Map the professional to the DTO
                        var professionalDTO = new ProfessionalsGetDTO
                        {
                            Id = pro.Id,
                            FirstName = pro.FirstName,
                            MiddleInitial = pro.MiddleInitial,
                            LastName = pro.LastName,
                            Barangay = pro.Barangay,
                            Description = pro.Description,
                            Rating = pro.Rating,
                            PhoneNumber = pro.PhoneNumber,
                            ProfessionalServices = professionalServicesDTO
                        };

                        professionals.Add(professionalDTO);
                    }
                }

                _response.Result = professionals;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("An error occurred while retrieving professionals.");
                _response.ErrorMessages.Add(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }




        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfessional(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Professional ID is required.");
                return BadRequest(_response);
            }

            try
            {
                // Retrieve the professional user by ID
                var professional = await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == id);
                if (professional == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages.Add("Professional not found.");
                    return NotFound(_response);
                }

                // Check if the user has the 'Professional' role
                var roles = await _userManager.GetRolesAsync(professional);
                if (!roles.Contains(UserRoles.Role_Professional))
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages.Add("User is not a professional.");
                    return NotFound(_response);
                }

                // Fetch the professional's services
                var professionalServices = await _db.ProfessionalServices
                                                     .Where(ps => ps.ProfessionalId == id)
                                                     .Include(ps => ps.Service) // Include related service details
                                                     .ThenInclude(s => s.Category) // Include related category details
                                                     .ToListAsync();

                // Map the professional's services to a DTO
                var professionalServicesDTO = professionalServices.Select(ps => new ProfessionalServiceGetDTO
                {
                    ProfessionalServiceId = ps.ProfessionalServiceId,
                    ServiceId = ps.ServiceId,
                    Price = ps.Price,
                    Description = ps.Service.Description,
                    CategoryId = ps.Service.CategoryId,
                    ServiceName = ps.Service.Title,
                    CategoryName = ps.Service.Category.Name
                }).ToList();

                // Map the professional details to a DTO
                var professionalDTO = new ProfessionalsGetDTO
                {
                    Id = professional.Id,
                    FirstName = professional.FirstName,
                    MiddleInitial = professional.MiddleInitial,
                    LastName = professional.LastName,
                    Barangay = professional.Barangay,
                    Description = professional.Description,
                    Rating = professional.Rating,
                    Email = professional.Email,
                    PhoneNumber = professional.PhoneNumber,
                    ProfessionalServices = professionalServicesDTO
                };

                _response.Result = professionalDTO;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("An error occurred while retrieving the professional.");
                _response.ErrorMessages.Add(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpGet("professionalInfo/{id}")]
        public async Task<IActionResult> GetProfessionalInfo(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Professional ID is required.");
                return BadRequest(_response);
            }

            try
            {
                // Retrieve the professional user by ID
                var professional = await _db.ApplicationUsers
                    .Where(u => u.Id == id)
                    .Select(u => new ProfessionalInfoDTO
                    {
                        Id = u.Id,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Email = u.Email,
                        PhoneNumber = u.PhoneNumber,
                        HouseLotBlockNumber = u.HouseLotBlockNumber,
                        Street = u.Street,
                        Barangay = u.Barangay,
                        Birthday = u.Birthday,
                        Description = u.Description,
                        ProfileImageUrl = u.ProfileImageUrl
                    })
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (professional == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages.Add("Professional not found.");
                    return NotFound(_response);
                }
                _response.Result = professional;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("An error occurred while retrieving the professional.");
                _response.ErrorMessages.Add(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }


            [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfessional(string id, [FromBody] ProfessionalUpdateDTO updateProfessionalDTO)
        {
            if (string.IsNullOrEmpty(id))
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("User ID is required.");
                return BadRequest(_response);
            }

            try
            {
                var professionalFromDb = await _db.ApplicationUsers.FindAsync(id);
                string normalizedEmail = updateProfessionalDTO.Email.ToLower();

                if (professionalFromDb == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages.Add("User not found.");
                    return NotFound(_response);
                }

                // Map updated properties from DTO
                professionalFromDb.Email = normalizedEmail;
                professionalFromDb.UserName = normalizedEmail;
                professionalFromDb.NormalizedEmail = normalizedEmail.ToUpper();
                professionalFromDb.NormalizedUserName = normalizedEmail.ToUpper();
                professionalFromDb.FirstName = updateProfessionalDTO.FirstName;
                professionalFromDb.MiddleInitial = updateProfessionalDTO.MiddleInitial;
                professionalFromDb.LastName = updateProfessionalDTO.LastName;
                professionalFromDb.PhoneNumber = updateProfessionalDTO.PhoneNumber;
                professionalFromDb.Description = updateProfessionalDTO.Description;
                professionalFromDb.PhoneNumber = updateProfessionalDTO.PhoneNumber;
                professionalFromDb.Barangay = updateProfessionalDTO.Barangay;

                _db.ApplicationUsers.Update(professionalFromDb);
                await _db.SaveChangesAsync();

                _response.Result = professionalFromDb;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("An error occurred while updating the user.");
                _response.ErrorMessages.Add(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfessional(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Professional ID is required.");
                return BadRequest(_response);
            }

            try
            {
                var professional = await _db.ApplicationUsers.FindAsync(id);

                if (professional == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages.Add("User not found.");
                    return NotFound(_response);
                }

                _db.ApplicationUsers.Remove(professional);
                await _db.SaveChangesAsync();

                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("An error occurred while deleting the user.");
                _response.ErrorMessages.Add(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }
    }
}
