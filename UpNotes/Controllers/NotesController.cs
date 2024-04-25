using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpNotes.Client.Dtos;
using UpNotes.Data;
using UpNotes.Entities;

namespace UpNotes.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class NotesController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public NotesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		// GET FULL NOTES
		[HttpGet]
		public IActionResult GetAllNotes()
		{
			var noteDtos = _context.Notes
				.Select(n => new ListItemNoteDto(
					n.Id,
					n.Title,
					n.Content,
					n.UploadDate,
					n.UserId,
					new SubjectDto(n.Subject.Id, n.Subject.Name, n.Subject.SemesterNumber, n.Subject.MajorId),
					new MajorDto(n.Subject.Major.Id, n.Subject.Major.Name, n.Subject.Major.SemesterCount),
					_context.Ratings.Where(r => r.NoteId == n.Id)
									.Select(r => (double)r.Value)
									.DefaultIfEmpty()
									.Average()
				));

			return Ok(noteDtos);
		}

		// GET FULL NOTES
		[HttpGet("page/{page}")]
		public IActionResult GetNotesPaged(
			[FromRoute] int page,
			[FromQuery] string? searchTerm,
			[FromQuery] int? subjectId,
			[FromQuery] int? majorId)
		{
			var noteDtos = _context.Notes
				.Where(n =>
					(searchTerm == null || n.Title.Contains(searchTerm))
					&& (subjectId == null || n.SubjectId == subjectId)
					&& (majorId == null || n.Subject.MajorId == majorId))
				.Skip((page - 1) * 5)
				.Take(5)
				.Select(n => new ListItemNoteDto(
					n.Id,
					n.Title,
					n.Content,
					n.UploadDate,
					n.UserId,
					new SubjectDto(n.Subject.Id, n.Subject.Name, n.Subject.SemesterNumber, n.Subject.MajorId),
					new MajorDto(n.Subject.Major.Id, n.Subject.Major.Name, n.Subject.Major.SemesterCount),
					_context.Ratings.Where(r => r.NoteId == n.Id)
									.Select(r => (double)r.Value)
									.DefaultIfEmpty()
									.Average()
			));

			return Ok(noteDtos);
		}

		// GET PAGE NUMBER
		[HttpGet("page-count")]
		public IActionResult GetPageNumber(
			[FromQuery] string? searchTerm,
			[FromQuery] int? subjectId,
			[FromQuery] int? majorId)
        {
			var noteCount = _context.Notes
				.Where(n => 
					(searchTerm == null || n.Title.Contains(searchTerm))
					&& (subjectId == null || n.SubjectId == subjectId)
					&& (majorId == null || n.Subject.MajorId == majorId))
				.Count();
			var pageCount = noteCount / 5 + (noteCount % 5 == 0 ? 0 : 1);

			return Ok(pageCount);
		}


		// GET FULL NOTE BY ID
		[HttpGet("full/{id}")]
		public IActionResult GetFullNoteById([FromRoute] int id)
		{
			var note = _context.Notes
				.Include(n => n.Subject)
				.ThenInclude(s => s.Major)
				.SingleOrDefault(n => n.Id == id);

			if (note == null)
			{
				return NotFound();
			}

			var noteDto = new ListItemNoteDto(
					note.Id,
					note.Title,
					note.Content,
					note.UploadDate,
					note.UserId,
					new SubjectDto(note.Subject.Id, note.Subject.Name, note.Subject.SemesterNumber, note.Subject.MajorId),
					new MajorDto(note.Subject.Major.Id, note.Subject.Major.Name, note.Subject.Major.SemesterCount),
					_context.Ratings.Where(r => r.NoteId == note.Id)
									.Select(r => (double)r.Value)
									.DefaultIfEmpty()
									.Average()
					);

			return Ok(noteDto);
		}

		// GET FULL NOTES BY USER ID
		[HttpGet("user/{userId}")]
		public IActionResult GetFullNotesByUserId([FromRoute] string userId)
		{
			var noteDtos = _context.Notes
				.Where(n => n.UserId == userId)
				.Select(n => new ListItemNoteDto(
					n.Id,
					n.Title,
					n.Content,
					n.UploadDate,
					n.UserId,
					new SubjectDto(n.Subject.Id, n.Subject.Name, n.Subject.SemesterNumber, n.Subject.MajorId),
					new MajorDto(n.Subject.Major.Id, n.Subject.Major.Name, n.Subject.Major.SemesterCount),
					_context.Ratings.Where(r => r.NoteId == n.Id)
					.Select(r => (double)r.Value)
					.DefaultIfEmpty()
					.Average()
					));

			return Ok(noteDtos);
		}


		// GET NOTE BY ID
		[HttpGet("{id}")]
		public IActionResult GetNoteById([FromRoute] int id)
		{
			var note = _context.Notes.SingleOrDefault(n => n.Id == id);

			if (note == null)
			{
				return NotFound();
			}

			var noteDto = new NoteDto(note.Id, note.Title, note.Content, note.UploadDate, note.UserId, note.SubjectId);

			return Ok(noteDto);
		}

		// UPDATE NOTE
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateNoteAsync([FromRoute] int id, [FromBody] InsertNoteDto insertNoteDto)
		{
			var user = await _userManager.GetUserAsync(User);
			var note = _context.Notes.SingleOrDefault(n => n.Id == id);

			if (user.Id != note.UserId)
			{
				return Forbid();
			}

			if (note == null)
			{
				return NotFound();
			}

			note.Title = insertNoteDto.Title;
			note.Content = insertNoteDto.Content;
			note.SubjectId = insertNoteDto.SubjectId;

			_context.SaveChanges();

			return Ok();
		}

		// INSERT NOTE
		[HttpPost]
		public async Task<IActionResult> InsertNoteAsync([FromBody] InsertNoteDto insertNoteDto)
		{
			var user = await _userManager.GetUserAsync(User);

			var note = new Note
			{
				Title = insertNoteDto.Title,
				Content = insertNoteDto.Content,
				UploadDate = DateTime.Now,
				UserId = user.Id,
				SubjectId = insertNoteDto.SubjectId
			};

			_context.Notes.Add(note);
			_context.SaveChanges();

			return Ok();
		}

		// DELETE NOTE
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteNoteAsync([FromRoute] int id)
		{
			var user = await _userManager.GetUserAsync(User);
			var note = _context.Notes.SingleOrDefault(n => n.Id == id);

			if (user.Id != note.UserId)
			{
				return Forbid();
			}

			if (note == null)
			{
				return NotFound();
			}

			_context.Notes.Remove(note);
			_context.SaveChanges();

			return Ok();
		}
	}
}
