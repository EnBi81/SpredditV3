using Application.DaoInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SharedDomain.Models;

namespace EfcDataAccess.DAOs;

public class CommentEfcDao : ICommentDao
{
    private readonly SpredditContext context;

    public CommentEfcDao(SpredditContext context)
    {
        this.context = context;
    }
    public async Task<Comment> CreateAsync(Comment comment)
    {
        EntityEntry<Comment> newComment = await context.Comments
            .AddAsync(comment);
        await context.SaveChangesAsync();
        return newComment.Entity;
    }

    public async Task<IEnumerable<Comment>> GetAllByPostId(int id)
    {
        var post = await context.Posts.Include(post => post.Comments).FirstOrDefaultAsync(post => post.Id == id);
        if (post == null)
            return new List<Comment>();

        return post.Comments;
    }
}