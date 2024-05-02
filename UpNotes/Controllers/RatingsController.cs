using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UpNotes.Client.Dtos;
using UpNotes.Data;
using UpNotes.Entities;
using UpNotes.Migrations;

namespace UpNotes.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RatingsController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public RatingsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		// GET RATING FOR A NOTE BY USER
		[HttpGet("{noteId}")]
		public async Task<IActionResult> GetRating(int noteId)
		{
			var user = await _userManager.GetUserAsync(User);
			Console.WriteLine(user);

			if (user == null)
			{
				return Unauthorized();
			}

			var rating = _context.Ratings.FirstOrDefault(r => r.NoteId == noteId && r.UserId == user.Id);

			if (rating == null)
			{
				return Ok(0);
			}

			return Ok(rating.Value);
		}

		// ADD RATING TO NOTE
		[HttpPost]
		public async Task<IActionResult> AddRating(AddRatingDto addRatingDto)
		{
			var user = await _userManager.GetUserAsync(User);
			var rating = _context.Ratings.FirstOrDefault(r => r.NoteId == addRatingDto.NoteId && r.UserId == user.Id);

			if (rating != null)
			{
				rating.Value = addRatingDto.Value;
			}
			else
			{
				rating = new Rating
				{
					NoteId = addRatingDto.NoteId,
					UserId = user.Id,
					Value = addRatingDto.Value
				};

				_context.Ratings.Add(rating);
			}

			_context.SaveChanges();

			return Ok();
		}

		// DELETE RATING
		[HttpDelete("{noteId}")]
		public async Task<IActionResult> DeleteRating(int noteId)
		{
			var user = await _userManager.GetUserAsync(User);
			var rating = _context.Ratings.FirstOrDefault(r => r.NoteId == noteId && r.UserId == user.Id);

			if (rating == null)
			{
				return NotFound();
			}

			_context.Ratings.Remove(rating);
			_context.SaveChanges();

			return Ok();
		}
	}
}
