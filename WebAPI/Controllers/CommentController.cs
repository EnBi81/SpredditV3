using Application.LogicIntrefaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedDomain.DTOs;
using SharedDomain.Models;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentController : ControllerBase
{
    private readonly ICommentLogic commentLogic;

    public CommentController(ICommentLogic commentLogic)
    {
        this.commentLogic = commentLogic;
    }

    [HttpPost]
    public async Task<ActionResult<Comment>> CreateAsync(CommentToSendDTO commentToCreate)
    {
        try
        {
            Comment comment = await commentLogic.CreateAsync(commentToCreate);
            return Created($"/comments", comment);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<IEnumerable<Comment>>> GetAll([FromRoute] int id)
    {
        try
        {
            IEnumerable<Comment> comments = await commentLogic.GetAllByPostId(id);
            return Ok(comments);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }   
    }
 }