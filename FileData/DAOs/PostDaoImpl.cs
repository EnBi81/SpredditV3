using Application.DaoInterfaces;
using SharedDomain.DTOs;
using SharedDomain.Models;

namespace FileData.DAOs;

public class PostDaoImpl : IPostDao
{
    private readonly FileContext context;

    public PostDaoImpl(FileContext context)
    {
        this.context = context;
    }
    public Task<Post> CreateAsync(Post post)
    {
        int postId = 1;
        
        if (context.Posts.Any())
        {
            //Looking through all post id s and finding the biggest one
            postId = context.Posts.Max(t => t.Id);
            postId++;
        }

        post.Id = postId;

        context.Posts.Add(post);
        context.SaveChange();

        return Task.FromResult(post);
    }
    public Task<Post?> GetByIdAsync(int id)
    {
        Post? post = context.Posts.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(post);
    }

    public Task<IEnumerable<Post>> GetByParameterAsync(PostFilterDTO dto)
    {
        string? title = dto.Title;
        string? username = dto.Username;
        
        IEnumerable<Post> posts = context.Posts;

        if (title is not null)
            posts = posts.Where(p => p.Title.Contains(title, StringComparison.OrdinalIgnoreCase));

        if (username is not null)
            posts = posts.Where(p => p.User.Username == username);
        
        return Task.FromResult(posts);
    }

    public Task<IEnumerable<Post>> GetByUserAsync(string username)
    {
        IEnumerable<Post> posts = context.Posts;

        posts = posts.Where(p => p.User.Username == username);

        return Task.FromResult(posts);
    }
}