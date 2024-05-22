using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UpNotes.Client.Dtos;
using UpNotes.Data;

namespace UpNotes.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MajorsController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public MajorsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		// GET ALL MAJORS
		[HttpGet]
		public IActionResult GetAllMajors()
		{
			var majorsDto = _context.Majors.Select(m => new MajorDto(m.Id, m.Name, m.SemesterCount)).ToList();

			return Ok(majorsDto);
		}

		// GET A MAJOR BY ID
		[HttpGet("{id}")]
		public IActionResult GetMajorById(int id)
		{
			var major = _context.Majors.SingleOrDefault(m => m.Id == id);

			if (major == null)
			{
				return NotFound();
			}

			var majorDto = new MajorDto(major.Id, major.Name, major.SemesterCount);

			return Ok(majorDto);
		}
	}
}
