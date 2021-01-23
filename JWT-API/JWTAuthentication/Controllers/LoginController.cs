using JWTAuthentication.Model;
using JWTAuthentication.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration _configuration;

        public LoginController(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            AuthRepository _auth = new AuthRepository(userManager, _configuration);


            var user = await userManager.FindByEmailAsync(model.Email);
            var result = await userManager.CheckPasswordAsync(user, model.Password);
            try
            {
                if (user != null && result)
                {
                    return Ok(new { token = _auth.GenerateJWT(user).Result, status = "Success" });
                }
                else
                {
                    return NotFound(new { token = "", status = "Fail", message = "User doesnot exist!" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = "Fail", message = ex.Message });
            }

        }
    }
}
