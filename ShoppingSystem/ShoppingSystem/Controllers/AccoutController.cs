using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ShoppingSystem.DTO;
using ShoppingSystem.Models;
using System.Web.Http.Cors;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ShoppingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(origins: "http://localhost:4200/", headers: "*", methods: "*")]
    public class AccoutController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;

        public Context _context { get; }
        public AccoutController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config, Context systemEntity)
        {
            _userManager = userManager;
            _config = config;
            _context = systemEntity;
           _roleManager = roleManager;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<Result>> RegisterAsync([FromForm] RegisterDTO registerDTO)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser newUsr = new ApplicationUser();

                newUsr.Email = registerDTO.Email;
                newUsr.UserName = registerDTO.UserName;

                IdentityResult result = await _userManager.CreateAsync(newUsr, registerDTO.Password);
                Result result1 = new Result();

                if (result.Succeeded)
                {
                    User usr = new User();

                    usr.ApplicationUserID = newUsr.Id;
                    usr.ProfilePicture = ImagesHelper.UploadImg(registerDTO.Image, "images");

                    var addToRoleResult = await _userManager.AddToRoleAsync(newUsr, "Customer");
                    if (!addToRoleResult.Succeeded)
                    {
                        // Handle the error if adding to the role failed
                        // You can return an error response or take other actions as needed
                        return BadRequest("Failed to assign the 'Admin' role.");
                    }

                    _context.Users.Add(usr);
                    _context.SaveChanges();
                    result1.Message = "sucess";
                    result1.IsPass = true;
                    result1.Data = usr.ApplicationUser.UserName;
                    return Ok(result1);
                }
                else
                {
                    result1.Message = "the register failed";
                    result1.IsPass = false;
                    return BadRequest(result1);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpPost]
        public async Task<ActionResult> AddRole(string role)
        {

            var roleExist = await _roleManager.RoleExistsAsync(role);
            if (!roleExist)
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
                return Ok("Role is Created");
            }

            return Ok("Is Already Exist");
        }

        [HttpPost("Login")]
        public async Task<ActionResult<Result>> LoginAsync(LoginDTO loginDTO)
        {

            if (ModelState.IsValid)
            {
                ApplicationUser Usr = await _userManager.FindByNameAsync(loginDTO.UserName);
                if (Usr != null && await _userManager.CheckPasswordAsync(Usr, loginDTO.Password))
                {
                    IList<string> roles = await _userManager.GetRolesAsync(Usr);

                    List<Claim> myClaims = new List<Claim>();

                    myClaims.Add(new Claim(ClaimTypes.NameIdentifier, Usr.Id));
                    myClaims.Add(new Claim(ClaimTypes.Name, Usr.UserName));
                    myClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));


                    foreach (var role in roles)
                    {
                        myClaims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    var authSecritKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecuriytKey"]));

                    SigningCredentials credentials =
                        new SigningCredentials(authSecritKey, SecurityAlgorithms.HmacSha256);



                    JwtSecurityToken jtw = new JwtSecurityToken
                        (
                            issuer: "JWT:ValidIssuer",
                            audience: "JWT:ValidAudience",
                            expires: DateTime.Now.AddHours(1),
                            claims: myClaims,
                            signingCredentials: credentials
                        );

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(jtw),
                        expiration = jtw.ValidTo,
                        message = "sucesss"
                    });

                }
                Result result = new Result();
                result.Message = "failed";
                return BadRequest(result);
            }

            return BadRequest(ModelState);
        }


        private async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecuriytKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:ValidIssuer"],
                audience: _config["Jwt:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
