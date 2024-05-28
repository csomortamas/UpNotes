using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UpNotes.Client.Dtos;
using UpNotes.Data;
using UpNotes.Entities;

namespace UpNotes.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LikesController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public LikesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		[HttpGet("{commentId}")]
		public async Task<IActionResult> GetUserLikeOrDislikeAsync(int commentId)
		{
			var user = await _userManager.GetUserAsync(User);

			if (user == null)
			{
				return Unauthorized();
			}

			var like = _context.Likes.FirstOrDefault(l => l.CommentId == commentId && l.UserId == user.Id);

			if (like == null)
			{
				return Ok("None");
			}

			return Ok(like.Type);
		}

		// ADD LIKE OR DISLIKE TO COMMENT
		[HttpPost]
		public async Task<IActionResult> AddLikeOrDislikeAsync(AddLikeOrDislikeDto addLikeOrDislikeDto)
		{
			var user = await _userManager.GetUserAsync(User);

			if (user == null)
			{
				return Unauthorized();
			}

			var like = _context.Likes.FirstOrDefault(l => l.CommentId == addLikeOrDislikeDto.CommentId && l.UserId == user.Id);
			if (like != null)
			{
				like.Type = addLikeOrDislikeDto.Type;
			}
			else
			{
				like = new Like
				{
					CommentId = addLikeOrDislikeDto.CommentId,
					UserId = user.Id,
					Type = addLikeOrDislikeDto.Type,
				};

				_context.Likes.Add(like);
			}

			_context.SaveChanges();

			return Ok();
		}

		// DELETE LIKE OR DISLIKE
		[HttpDelete("{commentId}")]
		public async Task<IActionResult> DeleteLikeOrDislikeAsync(int commentId)
		{
			var user = await _userManager.GetUserAsync(User);

			if (user == null)
			{
				return Unauthorized();
			}

			var like = _context.Likes.FirstOrDefault(l => l.CommentId == commentId && l.UserId == user.Id);

			if (like == null)
			{
				return NotFound();
			}

			_context.Likes.Remove(like);
			_context.SaveChanges();

			return Ok();
		}
	}
}
