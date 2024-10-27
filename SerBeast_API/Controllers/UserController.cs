using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SerBeast_API.Data;
using SerBeast_API.Model;
using SerBeast_API.Model.Dto;
using SerBeast_API.Utility;
using System.Net;

namespace SerBeast_API.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ApiResponse _response;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _response = new ApiResponse();
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var allUsers = await _db.ApplicationUsers.ToListAsync();
                var customerUsers = new List<ApplicationUser>();

                foreach (var user in allUsers)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains(UserRoles.Role_Customer))
                    {
                        customerUsers.Add(user);
                    }

                }

                _response.Result = customerUsers;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add("An error occurred while retrieving users.");
                _response.ErrorMessages.Add(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
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
                var applicationUser = await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == id);

                if (applicationUser is null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages.Add("User not found.");
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
                _response.ErrorMessages.Add("An error occurred while retrieving the user.");
                _response.ErrorMessages.Add(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserUpdateDTO updateUserDTO)
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
                var userFromDb = await _db.ApplicationUsers.FindAsync(id);
                string normalizedEmail = updateUserDTO.Email.ToLower();

                if (userFromDb == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages.Add("User not found.");
                    return NotFound(_response);
                }

                userFromDb.Email = normalizedEmail;
                userFromDb.UserName = normalizedEmail;
                userFromDb.NormalizedEmail = normalizedEmail.ToUpper();
                userFromDb.NormalizedUserName = normalizedEmail.ToUpper();
                userFromDb.FirstName = updateUserDTO.FirstName;
                userFromDb.MiddleInitial = updateUserDTO.MiddleInitial;
                userFromDb.LastName = updateUserDTO.LastName;
                userFromDb.PhoneNumber = updateUserDTO.PhoneNumber;

                _db.ApplicationUsers.Update(userFromDb);
                await _db.SaveChangesAsync();

                _response.Result = userFromDb;
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
        public async Task<IActionResult> DeleteUser(string id)
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
                var user = await _db.ApplicationUsers.FindAsync(id);

                if (user == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages.Add("User not found.");
                    return NotFound(_response);
                }

                _db.ApplicationUsers.Remove(user);
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
