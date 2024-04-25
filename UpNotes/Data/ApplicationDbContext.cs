using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UpNotes.Entities;

namespace UpNotes.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options) { }

		public DbSet<Major> Majors { get; set; }
		public DbSet<Subject> Subjects { get; set; }
		public DbSet<Note> Notes { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<Rating> Ratings { get; set; }
		public DbSet<Like> Likes { get; set; }
	}
}
