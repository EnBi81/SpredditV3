using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.LogicIntrefaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SharedDomain.DTOs;
using SharedDomain.Models;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace WebAPI.Controllers;

/**
 * Class responsible for handling incoming HTTP requests and send reponse back to the   
 */

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private IUserLogic logic;
    private readonly IConfiguration config;

    public UserController(IConfiguration config, IUserLogic logic)
    {
        this.config = config;
        this.logic = logic;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserToSendDTO>> CreateAsync(UserCreationDto user)
    {
        try
        {
            UserToSendDTO userToSend = await logic.CreateUser(user);
            string token = GenerateJwt(userToSend);

            return Ok(token);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserToSendDTO>> LoginAsync( LoginDTO userToLogin)
    {
        try
        {
            UserToSendDTO userToSendDto = await logic.LogIn(userToLogin.Username, userToLogin.Password);
            string token = GenerateJwt(userToSendDto);
            
            return Ok(token);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
    
    private List<Claim> GenerateClaims(UserToSendDTO user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, config["Jwt:Subject"]),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim("Username", user.Username)
        };
        return claims.ToList();
    }
    
    private string GenerateJwt(UserToSendDTO user)
    {
        List<Claim> claims = GenerateClaims(user);
    
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        SigningCredentials signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
    
        JwtHeader header = new JwtHeader(signIn);
    
        JwtPayload payload = new JwtPayload(
            config["Jwt:Issuer"],
            config["Jwt:Audience"],
            claims, 
            null,
            DateTime.UtcNow.AddMinutes(60));
    
        JwtSecurityToken token = new JwtSecurityToken(header, payload);
    
        string serializedToken = new JwtSecurityTokenHandler().WriteToken(token);
        return serializedToken;
    }
}