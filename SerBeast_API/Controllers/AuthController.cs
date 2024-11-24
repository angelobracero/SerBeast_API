using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SerBeast_API.Data;
using SerBeast_API.Model;
using SerBeast_API.Model.Dto;
using SerBeast_API.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace SerBeast_API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ApiResponse _response;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly string _secretKey;

        public AuthController(ApplicationDbContext db, IConfiguration configuration,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _response = new ApiResponse();
            _roleManager = roleManager;
            _userManager = userManager;
        }

        private async Task<IActionResult> RegisterUser(RegisterRequestDTO model, string role)
        {
            string normalizedEmail = model.Email.ToLower();

            var existingUser = await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.Email == normalizedEmail);
            if (existingUser != null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Email already exists");
                return BadRequest(_response);
            }

            var newUser = new ApplicationUser
            {
                Email = normalizedEmail,
                UserName = normalizedEmail,
                NormalizedEmail = normalizedEmail.ToUpper(),
                NormalizedUserName = normalizedEmail.ToUpper(),
                FirstName = model.FirstName,
                MiddleInitial = model.MiddleInitial,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Description = model.Description,
                HouseLotBlockNumber = model.HouseLotBlockNumber,
                Street = model.Street,
                Barangay = model.Barangay
            };

            try
            {
                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync(role))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(role));
                    }

                    await _userManager.AddToRoleAsync(newUser, role);

                    _response.StatusCode = HttpStatusCode.Created;
                    _response.IsSuccess = true;
                    return Ok(_response);
                }
                else
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.AddRange(result.Errors.Select(e => e.Description));
                    return BadRequest(_response);
                }
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error while registering: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }


        [HttpPost("register/professional")]
        public Task<IActionResult> RegisterProfessional([FromBody] RegisterRequestDTO model)
        {
            return RegisterUser(model, UserRoles.Role_Professional);
        }

        [HttpPost("register/customer")]
        public Task<IActionResult> RegisterCustomer([FromBody] RegisterRequestDTO model)
        {
            return RegisterUser(model, UserRoles.Role_Customer);
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            string normalizedEmail = model.Email.ToLower();
            var userFromDb = await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.Email == normalizedEmail);

            if (userFromDb == null || !(await _userManager.CheckPasswordAsync(userFromDb, model.Password)))
            {
                _response.Result = new LoginRequestDTO();
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username or password is incorrect");
                return BadRequest(_response);
            }

            var roles = await _userManager.GetRolesAsync(userFromDb);
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.ASCII.GetBytes(_secretKey);

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("firstname", userFromDb.FirstName ?? ""),
                    new Claim("middleinitial", userFromDb.MiddleInitial ?? ""),
                    new Claim("lastname", userFromDb.LastName ?? ""),
                    new Claim("id", userFromDb.Id),
                    new Claim(ClaimTypes.Email, userFromDb.Email),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault() ?? "User")
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            LoginResponseDTO loginResponse = new()
            {
                Email = userFromDb.Email,
                Token = tokenHandler.WriteToken(token)
            };

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = loginResponse;
            return Ok(_response);
        }

       

        [HttpPost("register/fakecustomer")]
        public async Task<IActionResult> RegisterFakeCustomer()
        {
            var fakeCustomer = FakeDataGenerator.GenerateFakeCustomer();
            return await RegisterUser(fakeCustomer, UserRoles.Role_Customer);
        }

        [HttpPost("register/fakecustomers/{count}")]
        public async Task<IActionResult> RegisterFakeCustomers(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var fakeCustomer = FakeDataGenerator.GenerateFakeCustomer();
                await RegisterUser(fakeCustomer, UserRoles.Role_Customer);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = $"{count} fake customers created successfully.";
            return Ok(_response);
        }


        [HttpPost("register/fakeprofessional")]
        public async Task<IActionResult> RegisterFakeProfessional()
        {
            var fakeProfessional = FakeDataGenerator.GenerateFakeCustomer();
            return await RegisterUser(fakeProfessional, UserRoles.Role_Professional);
        }

        [HttpPost("register/fakeprofessional/{count}")]
        public async Task<IActionResult> RegisterFakeProfessional(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var fakeProfessional = FakeDataGenerator.GenerateFakeCustomer();
                await RegisterUser(fakeProfessional, UserRoles.Role_Professional);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = $"{count} fake professional created successfully.";
            return Ok(_response);
        }










    }
}
