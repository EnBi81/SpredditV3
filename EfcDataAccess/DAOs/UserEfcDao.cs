using Application.DaoInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SharedDomain.DTOs;
using SharedDomain.Models;

namespace EfcDataAccess.DAOs;

public class UserEfcDao : IUserDao
{
    private readonly SpredditContext context;

    public UserEfcDao(SpredditContext context)
    {
        this.context = context;
    }
    public async Task<UserToSendDTO> CreateUser(User user)
    {
        EntityEntry<User> newUser = await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        UserToSendDTO userToSendDto = new UserToSendDTO(newUser.Entity.Username);
        return userToSendDto;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        User? existing = await context.Users.FirstOrDefaultAsync(u =>
            u.Username.ToLower().Equals(username.ToLower()));
        return existing;
    }
}