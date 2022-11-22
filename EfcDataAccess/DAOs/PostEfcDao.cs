using Application.DaoInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using SharedDomain.DTOs;
using SharedDomain.Models;

namespace EfcDataAccess.DAOs;

public class PostEfcDao : IPostDao
{
    private readonly SpredditContext context;

    public PostEfcDao(SpredditContext context)
    {
        this.context = context;
    }
    public async Task<Post> CreateAsync(Post post)
    {
        await context.Posts.AddAsync(post);
        await context.SaveChangesAsync();
        return post;
    }

    public async Task<Post?> GetByIdAsync(int id)
    {
        var result = await context.Posts
            //also create the users
            .Include(posts => posts.User)
            .Include(posts => posts.Votes)
            .FirstOrDefaultAsync(p => p.Id == id);
        return result;
    }

    public async Task<IEnumerable<Post>> GetByParameterAsync(PostFilterDTO dto)
    {
        IEnumerable<Post> res = await context.Posts
            .Include(posts => posts.Votes)
            .Include(post => post.User)
            .ToListAsync();
        if (dto.Title is not null)
            res = res.Where(post => post.Title.Contains(dto.Title, StringComparison.OrdinalIgnoreCase));
        if (dto.Username is not null)
            res = res.Where(post => post.CreatedBy.ToLower().Equals(dto.Username.ToLower()));

        
        return res;
    }

    public async Task<IEnumerable<Post>> GetByUserAsync(string username)
    {
        IEnumerable<Post> res = await context.Posts.Include(posts=>posts.User).Include(posts => posts.Votes).ToListAsync();

        res = res.Where(post => post.User.Username.ToLower().Equals(username.ToLower()));

        return res;
    }
}