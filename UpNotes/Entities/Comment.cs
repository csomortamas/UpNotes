using UpNotes.Data;

namespace UpNotes.Entities
{
	public class Comment
	{
		public int Id { get; set; }
		public required string Content { get; set; }
		public DateTime CommentDate { get; set; }
		public string? UserId { get; set; }
		public ApplicationUser User { get; set; }
		public int NoteId { get; set; }
		public Note Note { get; set; }
		public List<Like> Likes { get; set; }
	}
}
