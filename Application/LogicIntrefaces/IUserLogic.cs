using SharedDomain.DTOs;
using SharedDomain.Models;

namespace Application.LogicIntrefaces;

public interface IUserLogic
{
    public Task<UserToSendDTO> CreateUser(UserCreationDto userDto);
    public Task<UserToSendDTO> LogIn(string username, string password);
}