using Microsoft.AspNetCore.Identity;
using UpNotes.Client.Dtos;
using Microsoft.AspNetCore.Mvc;
using UpNotes.Data;
using UpNotes.Entities;

namespace UpNotes.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CommentsController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public CommentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		// GET ALL COMMENTS FOR A NOTE
		[HttpGet("{noteId}")]
		public async Task<IActionResult> GetCommentsForNoteIdAsync(int noteId)
		{
			var user = await _userManager.GetUserAsync(User);

			var comments = _context.Comments.Where(c => c.NoteId == noteId)
				.Select(c => new ListItemCommentDto(
					c.Id,
					c.Content,
					c.CommentDate,
					_context.Likes.Where(l => l.CommentId == c.Id && l.Type == "Like").Count(),
					_context.Likes.Where(l => l.CommentId == c.Id && l.Type == "Dislike").Count(),
					c.UserId,
					user != null ? _context.Likes.Where(l => l.UserId == user.Id && l.CommentId == c.Id).Select(l => l.Type).FirstOrDefault() : null	
				));

			return Ok(comments);
		}

		// INSERT COMMENT
		[HttpPost("{noteId}")]
		public async Task<IActionResult> InsertCommentAsync(InsertCommentDto insertCommentDto)
		{
			var user = await _userManager.GetUserAsync(User);

			if (user == null)
			{
				return Unauthorized();
			}

			var comment = new Comment
			{
				Content = insertCommentDto.Content,
				UserId = user.Id,
				NoteId = insertCommentDto.NoteId,
				CommentDate = DateTime.Now
			};

			_context.Comments.Add(comment);
			_context.SaveChanges();

			return Ok();
		}

		// DELETE COMMENT
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCommentAsync([FromRoute] int id)
		{
			var user = await _userManager.GetUserAsync(User);
			var comment = _context.Comments.SingleOrDefault(c => c.Id == id);

			if (user.Id != comment.UserId)
			{
				return Forbid();
			}

			if (comment == null)
			{
				return NotFound();
			}

			_context.Comments.Remove(comment);
			_context.SaveChanges();

			return Ok();
		}
	}
}
