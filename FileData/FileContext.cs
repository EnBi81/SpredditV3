using System.Text.Json;
using SharedDomain.Models;

namespace FileData;

public class FileContext
{
    private const string filePath = "data.json";
    private DataContainer? dataContainer;

    public ICollection<User> Users
    {
        get
        {
            LoadData();
            return dataContainer!.Users!;
        }
    }

    public ICollection<Post> Posts
    {
        get
        {
            LoadData();
            return dataContainer!.Posts!;
        }
    }

    public ICollection<Comment> Comments
    {
        get
        {
            LoadData();
            return dataContainer!.Comments!;
        }
    }

    private void LoadData()
    {
        if (dataContainer != null) return;
        
        if(!File.Exists(filePath))
        {
            dataContainer = new()
            {
                Users = new List<User>(),
                Posts = new List<Post>(),
                Comments = new List<Comment>()
            };
            
            return;
        }

        string content = File.ReadAllText(filePath);
        dataContainer = JsonSerializer.Deserialize<DataContainer>(content);
    }

    public void SaveChange()
    {
        string serialized = JsonSerializer.Serialize(dataContainer, new JsonSerializerOptions()
        {
            WriteIndented = true
        });
        File.WriteAllText(filePath, serialized);
        dataContainer = null;
    }
}