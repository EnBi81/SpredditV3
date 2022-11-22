using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using HttpClients.ClientInterfaces;
using SharedDomain.DTOs;
using SharedDomain.Models;

namespace HttpClients.Implementations;

public class PostHttpClient : IPostService
{
    private readonly HttpClient client;

    public PostHttpClient(HttpClient client)
    {
        this.client = client;
    }
    
    
    public async Task<IEnumerable<Post>> GetAsync(PostFilterDTO? filters = null)
    {
        string uri = "/post";

        // process filters
        if (filters is not null)
        {
            // this will hold the search params
            var searchParams = new List<string>();

            if (filters.Title != null)
            {
                string title = HttpUtility.UrlEncode(filters.Title);
                searchParams.Add("title=" + title);
            }
            
            if (filters.Username != null)
            {
                string username = HttpUtility.UrlEncode(filters.Username);
                searchParams.Add("username=" + username);
            }

            if (searchParams.Any())
            {
                uri += "?" + string.Join("&", searchParams);
            }
        }
        
        
        HttpResponseMessage response = await client.GetAsync(uri);
        string result = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(result);
        }

        var posts = JsonSerializer.Deserialize<IEnumerable<Post>>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;

        Console.WriteLine("WWW");
        foreach (var post in posts)
        {
            foreach (var propertyInfo in post.GetType().GetProperties())
            {
                Console.WriteLine(propertyInfo.Name + ": " + propertyInfo.GetValue(post));
            }
        }
        
        return posts;
    }
    
    public async Task<IEnumerable<Post>> GetAllPostsAsync()
    {
        string uri = "/post";
        HttpResponseMessage response = await client.GetAsync(uri);
        string result = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(result);
        }
        
        IEnumerable<Post> posts = JsonSerializer.Deserialize<IEnumerable<Post>>(result, new JsonSerializerOptions()
        {
            MaxDepth = 10,
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.Preserve,
        })!;

        return posts;
    }

    public async Task<IEnumerable<Post>> GetPostsByFiltering(string? username, string? titleContains)
    {
        string query = ConstructQuery(username, titleContains);
        
        HttpResponseMessage response = await client.GetAsync("/post" + query );
        string result = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(result);
        }

        IEnumerable<Post> posts = JsonSerializer.Deserialize<IEnumerable<Post>>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
        
        
        
        
        return posts;
    }

    public async Task<Post> GetByIdAsync(int id)
    {
        HttpResponseMessage response = await client.GetAsync($"/post/{id}");
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        Post post = JsonSerializer.Deserialize<Post>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;

        Console.WriteLine("received post: " + JsonSerializer.Serialize(post));

        return post;
    }

    public async Task<Post> CreateAsync(PostCreationDTO dto)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("/post", dto);
        string result = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(result);
        }
        var post = JsonSerializer.Deserialize<Post>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
        
        return post;
    }

    public async Task<int> GetNumberOfUpVotes(int id)
    {
        HttpResponseMessage response = await client.GetAsync($"/post/{id}/upvotes");
        string result = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(result);
        }
        var number = JsonSerializer.Deserialize<int>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
        
        return number;
    }

    public async Task<int> GetNumberOfDownVotes(int id)
    {
        HttpResponseMessage response = await client.GetAsync($"/post/{id}/downvotes");
        string result = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(result);
        }
        var number = JsonSerializer.Deserialize<int>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
        
        return number;
    }

    public  async Task<IEnumerable<Comment>?> GetAllComments(int id)
    {
        HttpResponseMessage response = await client.GetAsync($"/comment/{id}");
        string result = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(result);
        }
        var comments = JsonSerializer.Deserialize<IEnumerable<Comment>>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
        
        return comments;
    }

    public async Task<Comment> CommentPost(CommentToSendDTO dto)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("/comment",dto);
        string result = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(result);
        }
        var comment = JsonSerializer.Deserialize<Comment>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
        
        return comment;
    }

    public async Task UpVote(int id, VoteDTO dto)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync($"/post/{id}/upvote",dto);
        string result = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("StatusCode = " + response.StatusCode);
            throw new Exception(response.StatusCode + "" + result);
        }
        
    }

    public async Task DownVote(int id, VoteDTO dto)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync($"/post/{id}/downvote",dto);
        string result = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(result);
        }
    }

    private static string ConstructQuery(string? username, string? titleContains)
    {
        string query = "";
        
        if(!string.IsNullOrEmpty(username))
        {
            query += $"?username={username}";
        }

        if (!string.IsNullOrEmpty(titleContains))
        {
            query += string.IsNullOrEmpty(query) ? "?" : "&";
            query += $"title={titleContains}";
        }

        return query;
    }
}