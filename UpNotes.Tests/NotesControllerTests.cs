using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using UpNotes.Client.Dtos;
using UpNotes.Controllers;
using UpNotes.Data;
using UpNotes.Entities;

namespace UpNotes.Tests
{
	public class NotesControllerTests
	{
		private readonly DbContextOptions<ApplicationDbContext> _options;
		private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
		private readonly ApplicationDbContext _context;


		public NotesControllerTests()
		{
			_options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(databaseName: "UpNotesTestDb")
				.Options;

			_mockUserManager = new Mock<UserManager<ApplicationUser>>(
				new Mock<IUserStore<ApplicationUser>>().Object,
				null, null, null, null, null, null, null, null);

			_context = new ApplicationDbContext(_options);

			// Add dummy major
			var MajorComputerScience = new Major { Id = 1, Name = "Computer Science", SemesterCount = 8 };
			_context.Majors.Add(MajorComputerScience);

			// Add dummy subjects
			var SubjectMath = new Subject { Id = 1, Name = "Mathematics", SemesterNumber = 1, MajorId = 1, Major = MajorComputerScience};
			_context.Subjects.Add(SubjectMath);

			var Rating1 = new Rating { Id = 1, Value = 5, NoteId = 1, UserId = "abc" };
			var Rating2 = new Rating { Id = 2, Value = 4, NoteId = 2, UserId = "bcd" };
			_context.Ratings.Add(Rating1);
			_context.Ratings.Add(Rating2);

			// Add dummy notes
			_context.Notes.Add(new Note { Id = 1, Title = "Test Title", Content = "Test Content", UploadDate = DateTime.Now, UserId = "abc", Subject = SubjectMath });
			_context.Notes.Add(new Note { Id = 2, Title = "Test Title2", Content = "Test Content2", UploadDate = DateTime.Now, UserId = "bcd", Subject = SubjectMath });

			_context.SaveChanges();
		}

		[Fact]
		public void GetAllNotes_Returns_The_Correct_Ammount_Of_Notes()
		{
			// Arrange
			var controller = new NotesController(_context, _mockUserManager.Object);

			// Act
			var result = controller.GetAllNotes();

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result);
			var noteDtos = Assert.IsType<List<ListItemNoteDto>>(okResult.Value);
			Assert.Equal(2, noteDtos.Count);
		}
	}
}