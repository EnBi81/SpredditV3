using Application.DaoInterfaces;
using Application.LogicIntrefaces;
using SharedDomain.DTOs;
using SharedDomain.Models;

namespace Application.Logic;

public class CommentLogic : ICommentLogic
{
    private readonly ICommentDao commentDao;
    private readonly IPostDao postDao;
    private readonly IUserDao userDao;

    public CommentLogic(ICommentDao commentDao, IPostDao postDao, IUserDao userDao)
    {
        this.commentDao = commentDao;
        this.postDao = postDao;
        this.userDao = userDao;
    }
    
    public async Task<Comment> CreateAsync(CommentToSendDTO comment)
    {
        User? user = await userDao.GetByUsernameAsync(comment.Username);

        if (user == null)
        {
            throw new Exception($"User with username: {comment.Username} doesn't exist");
        }

        Post? post = await postDao.GetByIdAsync(comment.PostId);

        if (post == null)
        {
            throw new Exception($"Post with id: {comment.PostId} doesn't exist");
        }

        Comment commentToCreate = new Comment()
        {
            Username = comment.Username,
            Body = comment.Comment
        };
        
        Comment created = await commentDao.CreateAsync(commentToCreate);
        return created;
    }

    public async Task<IEnumerable<Comment>> GetAllByPostId(int id)
    {
        return await commentDao.GetAllByPostId(id);
    }
}