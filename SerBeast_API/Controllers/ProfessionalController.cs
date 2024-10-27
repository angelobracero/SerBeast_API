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
                        var professionalDTO = new ProfessionalsGetDTO
                        {
                            Id = pro.Id,
                            FirstName = pro.FirstName,
                            MiddleInitial = pro.MiddleInitial,
                            LastName = pro.LastName,
                            Barangay = pro.Barangay,
                            Description = pro.Description,
                            Rating = pro.Rating,
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
                var applicationUser = await _db.ApplicationUsers.FindAsync(id);

                if (applicationUser is null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages.Add("Professional not found.");
                    return NotFound(_response);
                }

                _response.Result = applicationUser;
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
