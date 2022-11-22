namespace SharedDomain.DTOs;

public class UserToSendDTO
{
    public string Username { get; }

    public UserToSendDTO(string username)
    {
        Username = username;
    }
}