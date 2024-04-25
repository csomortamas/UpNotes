using UpNotes.Data;

namespace UpNotes.Entities
{
	public class Rating
	{
		public int Id { get; set; }
		public int Value { get; set; }
		public string? UserId { get; set; }
		public ApplicationUser User { get; set; }

		public int NoteId { get; set; }
		public Note Note { get; set; }
	}
}
