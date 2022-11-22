using Application.LogicIntrefaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedDomain.DTOs;
using SharedDomain.Models;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class PostController : ControllerBase
{
    private readonly IPostLogic postLogic;
    private readonly IVoteLogic voteLogic;

    public PostController(IPostLogic logic, IVoteLogic voteLogic)
    {
        postLogic = logic;
        this.voteLogic = voteLogic;
    }

    [HttpPost]
    public async Task<ActionResult<Post>> CreateAsync(PostCreationDTO dto)
    {
        try
        {
            Post post = await postLogic.CreateAsync(dto);
            return Created($"/post/{post.Id}", post);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Post>>> GetAll([FromQuery]string? title, [FromQuery]string? username)
    {
        var filterParams = new PostFilterDTO
        {
            Title = title,
            Username = username
        };
        
        var posts = await postLogic.GetByParameterAsync(filterParams);
        return Ok(posts);
    }

    [HttpPost("{id:int}/upvote")]
    public async Task<IActionResult> UpVotePost([FromRoute]int id, [FromBody] VoteDTO voteDto)
    {
        try
        {
            await voteLogic.UpVote(id, voteDto);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
    
   
    [HttpPost("{id:int}/downvote")]
    public async Task<IActionResult> DownVotePost([FromRoute]int id, [FromBody] VoteDTO voteDto)
    {
        try
        {
            await voteLogic.DownVote(id, voteDto);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet("{username}")]
    public async Task<ActionResult<Post>> GetByUsernameAsync([FromRoute] string username)
    {
        try
        {
            var posts = await postLogic.GetByUserAsync(username);
            return Ok(posts);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

   
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Post>> GetById([FromRoute] int id)
    {
        try
        {
            Post? post = await postLogic.GetByIdAsync(id);
            return Ok(post);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpGet("{id:int}/upvotes")]
    public async Task<ActionResult<int>> GetAllUpVote([FromRoute] int id)
    {
        try
        {
            int number = await voteLogic.GetNumberOfUpVote(id);
            return Created($"/upVotes", number);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }   
    }
    
    [HttpGet("{id:int}/downvotes")]
    public async Task<ActionResult<int>> GetAllDownVote([FromRoute] int id)
    {
        try
        {
            int number = await voteLogic.GetNumberOgDownVote(id);
            return Created($"/upVotes", number);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }   
    }
    
    
}