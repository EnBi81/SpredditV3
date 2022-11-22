using Application.DaoInterfaces;
using Application.LogicIntrefaces;
using SharedDomain.DTOs;
using SharedDomain.Models;

namespace Application.Logic;

public class UserLogic : IUserLogic
{
    private IUserDao dao;
    
    public UserLogic(IUserDao dao)
    {
        this.dao = dao;
    }
    
    public async Task<UserToSendDTO> CreateUser(UserCreationDto userDto)
    {
        User? existing = await dao.GetByUsernameAsync(userDto.Username);

        if (existing != null)
        {
            throw new Exception($" Username: {userDto.Username} already exists");
        }
        
        ValidateData(userDto);
        var user = new User()
        {
            Password = userDto.Password,
            Username = userDto.Username,
            DateOfBirth = userDto.DateOfBirth
        };
        return await dao.CreateUser(user);
    }

    public async Task<UserToSendDTO> LogIn(string username, string password)
    {
        User? user = await dao.GetByUsernameAsync(username);
        if (user == null)
        {
            throw new Exception($"The user {username} does not exist");
        }

        if (!user.Password.Equals(password))
        {
            throw new Exception("Incorrect password");
        }

        UserToSendDTO userToSendDto = new UserToSendDTO(user.Username);

        return userToSendDto;
    }

    private static void ValidateData(UserCreationDto user)
    {
        string username = user.Username;

        if (username.Length > 20 || username.Length < 5)
        {
            throw new Exception("Username must have more than 5 characters and less than 21");
        }

        DateTime dateOfBirth = user.DateOfBirth;
        int age = (int)((DateTime.Now - dateOfBirth).TotalDays / 365.242199);
        
        if (age < 14)
        {
            throw new Exception("Age is less than 14");
        }
    }
}