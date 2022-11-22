using System.Security.Claims;
using System.Text.Json;
using HttpClients.ClientInterfaces;
using SharedDomain.DTOs;
using SharedDomain.Models;

namespace BlazorWASM.Auth.HTTP;

public class JwtAuthService : IAuthService
{
    private readonly IUserService userService;
    
    public JwtAuthService(IUserService userService)
    {
        this.userService = userService;
    }
    
    public static string? Jwt { get; private set; } = "";
    
    public async Task LoginAsync(string username, string password)
    {
        LoginDTO user = new LoginDTO(username, password);

        
        string token = await userService.LoginAsync(user);
        Jwt = token;
        
        
        ClaimsPrincipal principal = CreateClaimsPrincipal();
        OnAuthStateChanged?.Invoke(principal);
    }
    
    public Task LogoutAsync()
    {
        Jwt = null;
        ClaimsPrincipal principal = new();
        OnAuthStateChanged.Invoke(principal);
        return Task.CompletedTask;
    }

    public async Task RegisterAsync(User user)
    {
        await userService.RegisterAsync(user);
    }
    
    private static ClaimsPrincipal CreateClaimsPrincipal()
    {
        if (string.IsNullOrEmpty(Jwt))
        {
            return new ClaimsPrincipal();
        }

        IEnumerable<Claim> claims = ParseClaimsFromJwt(Jwt);

        Console.WriteLine("Claims: ");
    
        ClaimsIdentity identity = new(claims, "jwt");

        ClaimsPrincipal principal = new(identity);
        return principal;
    }

    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        string payload = jwt.Split('.')[1];
        byte[] jsonBytes = ParseBase64WithoutPadding(payload);
        Dictionary<string, object>? keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        return keyValuePairs!.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!));
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2:
                base64 += "==";
                break;
            case 3:
                base64 += "=";
                break;
        }

        return Convert.FromBase64String(base64);
    }

    
    public Task<ClaimsPrincipal> GetAuthAsync()
    {
        ClaimsPrincipal principal = CreateClaimsPrincipal();
        return Task.FromResult(principal);
    }

    // Method that will fire an event whenever the authentication state changes
    // upon a log in or log out
    public Action<ClaimsPrincipal> OnAuthStateChanged { get; set; }
}