using UpNotes.Data;

namespace UpNotes.Entities
{
	public class Bookmark
	{
		public int Id { get; set; }
		public int NoteId { get; set; }
		public Note Note { get; set; }
		public string UserId { get; set; }
		public ApplicationUser User { get; set; }
	}
}
