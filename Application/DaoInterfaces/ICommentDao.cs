using SharedDomain.Models;

namespace Application.DaoInterfaces;

public interface ICommentDao
{
    Task<Comment> CreateAsync(Comment comment);
    Task<IEnumerable<Comment>> GetAllByPostId(int id);
}