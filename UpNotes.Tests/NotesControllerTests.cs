using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using UpNotes.Client.Dtos;
using UpNotes.Controllers;
using UpNotes.Data;
using UpNotes.Entities;
using Xunit;

namespace UpNotes.Tests
{
    public class NotesControllerTests
    {
        private DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        private (ApplicationDbContext context, Mock<UserManager<ApplicationUser>> mockUserManager) InitializeContext()
        {
            var options = CreateNewContextOptions();

            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
                null, null, null, null, null, null, null, null);

            var context = new ApplicationDbContext(options);

            // Add dummy data
            var MajorComputerScience = new Major { Id = 1, Name = "Computer Science", SemesterCount = 8 };
            context.Majors.Add(MajorComputerScience);

            var SubjectMath = new Subject { Id = 1, Name = "Mathematics", SemesterNumber = 1, MajorId = 1, Major = MajorComputerScience };
            context.Subjects.Add(SubjectMath);

            var Rating1 = new Rating { Id = 1, Value = 5, NoteId = 1, UserId = "abc" };
            var Rating2 = new Rating { Id = 2, Value = 4, NoteId = 2, UserId = "bcd" };
            context.Ratings.Add(Rating1);
            context.Ratings.Add(Rating2);

            context.Notes.Add(new Note { Id = 1, Title = "Test Title", Content = "Test Content", UploadDate = DateTime.Now, UserId = "abc", Subject = SubjectMath });
            context.Notes.Add(new Note { Id = 2, Title = "Test Title2", Content = "Test Content2", UploadDate = DateTime.Now, UserId = "bcd", Subject = SubjectMath });

            context.SaveChanges();

            return (context, mockUserManager);
        }

        [Fact]
        public void GetAllNotes_Returns_The_Correct_Ammount_Of_Notes()
        {
            // Arrange
            var (context, mockUserManager) = InitializeContext();
            var controller = new NotesController(context, mockUserManager.Object);

            // Act
            var result = controller.GetAllNotes();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var noteDtos = Assert.IsType<List<ListItemNoteDto>>(okResult.Value);
            Assert.Equal(2, noteDtos.Count);
        }

        [Fact]
        public void GetNoteById_Returns_The_Correct_Note()
        {
            // Arrange
            var (context, mockUserManager) = InitializeContext();
            var controller = new NotesController(context, mockUserManager.Object);

            // Act
            var result = controller.GetNoteById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var noteDto = Assert.IsType<NoteDto>(okResult.Value);
            Assert.Equal(1, noteDto.Id);
            Assert.Equal("Test Title", noteDto.Title);
            Assert.Equal("Test Content", noteDto.Content);
        }

        [Fact]
        public void GetNotesPaged_Returns_The_Correct_Ammount_Of_Notes()
        {
            // Arrange
            var (context, mockUserManager) = InitializeContext();
            var controller = new NotesController(context, mockUserManager.Object);

            // Act
            var result = controller.GetNotesPaged(1, null, null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var noteDtos = Assert.IsType<List<ListItemNoteDto>>(okResult.Value);
            Assert.Equal(2, noteDtos.Count);
        }

        [Fact]
        public void GetNotesPaged_Returns_The_Correct_Ammount_Of_Notes_With_Filter()
        {
            // Arrange
            var (context, mockUserManager) = InitializeContext();
            var controller = new NotesController(context, mockUserManager.Object);

            // Act
            var result = controller.GetNotesPaged(1, "Test Title2", null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var noteDtos = Assert.IsType<List<ListItemNoteDto>>(okResult.Value);
            Assert.Single(noteDtos);
        }
    }
}
