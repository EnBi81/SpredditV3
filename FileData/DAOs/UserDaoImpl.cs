using Application.DaoInterfaces;
using SharedDomain.DTOs;
using SharedDomain.Models;

namespace FileData.DAOs;

public class UserDaoImpl : IUserDao
{
    private FileContext context;

    public UserDaoImpl(FileContext context)
    {
        this.context = context;
    }
    public Task<UserToSendDTO> CreateUser(User user)
    {
        context.Users.Add(user);
        context.SaveChange();

        UserToSendDTO userToSend = new UserToSendDTO(user.Username);
        return Task.FromResult(userToSend);
    }

    public Task<User?> GetByUsernameAsync(string username)
    {
        User? user = context.Users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        
        return Task.FromResult(user);
    }
    
}