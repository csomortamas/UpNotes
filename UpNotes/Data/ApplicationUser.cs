using Microsoft.AspNetCore.Identity;
using UpNotes.Entities;

namespace UpNotes.Data
{
	// Add profile data for application users by adding properties to the ApplicationUser class
	public class ApplicationUser : IdentityUser
	{
		public List<Rating> Ratings { get; set; }
		public List<Like> Likes { get; set; }
		public List<Comment> Comments { get; set; }
		public List<Note> Notes { get; set; }
	}

}
