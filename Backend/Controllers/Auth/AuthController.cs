using Burgermania.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Burgermania.Options;
using Microsoft.Extensions.Options;
using Microsoft.CodeAnalysis.Scripting;
using Burgermania.Services;
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly BurgerDbContext _context; // Assuming you have a DbContext
    private readonly ITokenService _tokenService;

    public AuthController(BurgerDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService; // Get the value of JwtSettings
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.MobileNumber) || string.IsNullOrEmpty(request.Password))
        {
            return BadRequest("Invalid login request");
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.MobileNumber == request.MobileNumber);

        if (user == null )
        {
            return Unauthorized();
        }




        var jwtToken = _tokenService.GetToken(request.MobileNumber);
        return Ok(new { token = jwtToken, user });
    }
}

    public class LoginRequest
{
    public string MobileNumber { get; set; }
    public string Password { get; set; }
    
}
