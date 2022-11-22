using System.Security.Claims;
using System.Text;
using System.Text.Json;
using HttpClients.ClientInterfaces;
using SharedDomain.DTOs;
using SharedDomain.Models;

namespace HttpClients.Implementations;

public class UserHttpClient : IUserService
{
    private readonly HttpClient client;

    public UserHttpClient(HttpClient client)
    {
        this.client = client;
    }

    
    public async Task<string> LoginAsync(LoginDTO dto)
    {
        string userAsJson = JsonSerializer.Serialize(dto);
        StringContent content = new(userAsJson, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync("/User/login", content);
        string responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(responseContent);
        }

        // return the token
        return responseContent;
    }

    public async Task RegisterAsync(User user)
    {
        string userAsJson = JsonSerializer.Serialize(user);
        StringContent content = new StringContent(userAsJson, Encoding.UTF8, "application/json");
        
        HttpResponseMessage response = await client.PostAsync("/User/register", content);
        string responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine(responseContent);
            throw new Exception(responseContent);
        }
    }
}