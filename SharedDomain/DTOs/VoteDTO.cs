namespace SharedDomain.DTOs;

/// <summary>
/// Data transfer object for upvoting/downvoting.
/// </summary>
public class VoteDTO
{
    /// <summary>
    /// Username who vote on the post.
    /// </summary>
    public string? Username { get; set; }
}