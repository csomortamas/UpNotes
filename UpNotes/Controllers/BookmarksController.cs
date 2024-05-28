using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UpNotes.Data;
using UpNotes.Entities;

namespace UpNotes.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BookmarksController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public BookmarksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		// ADD BOOKMARK
		[HttpPost("{noteId}")]
		public async Task<IActionResult> AddBookmarkAsync(int noteId)
		{
			var user = await _userManager.GetUserAsync(User);

			if (user == null)
			{
				return Unauthorized();
			}

			var bookmark = new Bookmark
			{
				UserId = user.Id,
				NoteId = noteId
			};

			_context.Bookmarks.Add(bookmark);
			await _context.SaveChangesAsync();

			return Ok();
		}

		// REMOVE BOOKMARK
		[HttpDelete("{noteId}")]
		public async Task<IActionResult> RemoveBookmarkAsync(int noteId)
		{
			var user = await _userManager.GetUserAsync(User);

			if (user == null)
			{
				return Unauthorized();
			}

			var bookmark = _context.Bookmarks.FirstOrDefault(b => b.NoteId == noteId && b.UserId == user.Id);

			if (bookmark == null)
			{
				return NotFound();
			}

			_context.Bookmarks.Remove(bookmark);
			await _context.SaveChangesAsync();

			return Ok();
		}

		// GET BOOKMARK FOR NOTE BY CURRENT USER
		[HttpGet("{noteId}")]
		public async Task<IActionResult> GetBookmarkAsync(int noteId)
		{
			var user = await _userManager.GetUserAsync(User);

			if (user == null)
			{
				return Unauthorized();
			}

			var bookmark = _context.Bookmarks.FirstOrDefault(b => b.NoteId == noteId && b.UserId == user.Id);

			if (bookmark == null)
			{
				return Ok(false);
			}

			return Ok(true);
		}
	}
}
