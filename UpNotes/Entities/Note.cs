using UpNotes.Data;

namespace UpNotes.Entities
{
	public class Note
	{
		public int Id { get; set; }
		public required string Title { get; set; }
		public required string Content { get; set; }
		public DateTime UploadDate { get; set; }
		public string? UserId { get; set; }
		public ApplicationUser? User { get; set; }

		public int SubjectId { get; set; }
		public Subject Subject { get; set; }
		public List<Comment> Comments { get; set; }
		public List<Rating> Ratings { get; set; }
	}
}
