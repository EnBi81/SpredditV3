using Application.DaoInterfaces;
using Application.LogicIntrefaces;
using SharedDomain.DTOs;
using SharedDomain.Models;

namespace Application.Logic;

public class PostLogic : IPostLogic
{
    private IPostDao postDao;
    private IUserDao userDao;

    public PostLogic(IPostDao postDao, IUserDao userDao)
    {
        this.postDao = postDao;
        this.userDao = userDao;
    }


    public async Task<Post> CreateAsync(PostCreationDTO dto)
    {
        //Searching if the user exists 
        User? user = await userDao.GetByUsernameAsync(dto.Username);
        if (user == null)
        {
            throw new Exception($"User with username: {dto.Username} not found.");
        }
        
        //A new post is created
        Post post = new Post(dto.Title, dto.Body, user);
        
        //Validating the new post
        ValidatePost(post);
        
        //Handing over to the DAO, which return the finalized object
        Post created = await postDao.CreateAsync(post);
        return created;
    }

    public async Task<IEnumerable<Post>> GetByParameterAsync(PostFilterDTO dto)
    {
        return await postDao.GetByParameterAsync(dto);
    }

    public async Task<IEnumerable<Post>> GetByUserAsync(string username)
    {
        return await postDao.GetByUserAsync(username);
    }

    public async Task<Post?> GetByIdAsync(int id)
    {
        return await postDao.GetByIdAsync(id);
    }


    private static void ValidatePost(Post post)
    {
        if (string.IsNullOrEmpty(post.Title))
        {
            throw new Exception("The title can not be empty");
        }

        if (string.IsNullOrEmpty(post.Body))
        {
            throw new Exception("The body of the post can not be empty");
        }
    }
}