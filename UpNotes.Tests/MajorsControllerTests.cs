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
    public class MajorsControllerTests
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
            var MajorComputerScience = new Major { Id = 1, Name = "Computer Science", SemesterCount = 5 };
            context.Majors.Add(MajorComputerScience);

            context.SaveChanges();

            return (context, mockUserManager);
        }

        [Fact]
        public void GetAllMajors_Returns_The_Correct_Ammount_Of_Majors()
        {
            // Arrange
            var (context, mockUserManager) = InitializeContext();
            var controller = new MajorsController(context, mockUserManager.Object);

            // Act
            var result = controller.GetAllMajors();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var majors = Assert.IsType<List<MajorDto>>(okResult.Value);
            Assert.Single(majors);
            
        }

        [Fact]
        public void GetMajorById_Returns_The_Correct_Major()
        {
            // Arrange
            var (context, mockUserManager) = InitializeContext();
            var controller = new MajorsController(context, mockUserManager.Object);

            // Act
            var result = controller.GetMajorById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var major = Assert.IsType<MajorDto>(okResult.Value);
            Assert.Equal(1, major.Id);
            Assert.Equal("Computer Science", major.Name);
            Assert.Equal(5, major.SemesterCount);
        }
    }
}
