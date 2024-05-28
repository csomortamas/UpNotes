using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UpNotes.Client.Dtos;
using UpNotes.Data;
using UpNotes.Entities;

namespace UpNotes.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    public class SubjectsController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public SubjectsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		// GET ALL SUBJECTS
		[HttpGet]
		public IActionResult GetAllSubjects()
		{
			var subjectDtos = _context.Subjects.OrderBy(s => s.SemesterNumber).Select(s => new SubjectDto(s.Id, s.Name, s.SemesterNumber, s.MajorId));

			return Ok(subjectDtos);
		}

		// GET SUBJECTS BY MAJOR ID
		[HttpGet("major/{majorId}")]
		public IActionResult GetSubjectsByMajorId(int majorId)
		{
			var subjects = _context.Subjects.Where(s => s.MajorId == majorId).OrderBy(s => s.SemesterNumber);

			if (subjects == null)
			{
				return NotFound();
			}

			var subjectsDto = subjects.Select(s => new SubjectDto(s.Id, s.Name, s.SemesterNumber, s.MajorId));

			return Ok(subjectsDto);
		}

		// GET A SUBJECT BY ID
		[HttpGet("{id}")]
		public IActionResult GetSubjectById(int id)
		{
			var subject = _context.Subjects.SingleOrDefault(s => s.Id == id);

			if (subject == null)
			{
				return NotFound();
			}

			var subjectDto = new SubjectDto(subject.Id, subject.Name, subject.SemesterNumber, subject.MajorId);

			return Ok(subjectDto);
		}

		// DELETE SUBJECT
		[HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public IActionResult DeleteSubject(int id)
		{
			var subject = _context.Subjects.SingleOrDefault(s => s.Id == id);

			if (subject == null)
			{
				return NotFound();
			}

			_context.Subjects.Remove(subject);
			_context.SaveChanges();

			return NoContent();
		}

		// UPDATE SUBJECT
		[HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public IActionResult UpdateSubject(int id, [FromBody] InsertSubjectDto insertSubjectDto)
		{
			var subject = _context.Subjects.SingleOrDefault(s => s.Id == id);

			if (subject == null)
			{
				return NotFound();
			}

			subject.Name = insertSubjectDto.Name;
			subject.SemesterNumber = insertSubjectDto.SemesterNumber;
			subject.MajorId = insertSubjectDto.MajorId;

			_context.SaveChanges();

			return Ok();
		}

		// INSERT SUBJECT
		[HttpPost]
        [Authorize(Roles = "ADMIN")]
        public IActionResult InsertSubject([FromBody] InsertSubjectDto insertSubjectDto)
		{
			var subject = new Subject
			{
				Name = insertSubjectDto.Name,
				SemesterNumber = insertSubjectDto.SemesterNumber,
				MajorId = insertSubjectDto.MajorId
			};

			_context.Subjects.Add(subject);
			_context.SaveChanges();

			return Ok();
		}
	}
}
