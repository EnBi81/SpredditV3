using System.Security.Claims;
using SharedDomain.DTOs;
using SharedDomain.Models;

namespace HttpClients.ClientInterfaces;

public interface IUserService
{
    public Task<string> LoginAsync(LoginDTO dto);
    public Task RegisterAsync(User user);
}