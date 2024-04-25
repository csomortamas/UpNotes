namespace UpNotes.Entities
{
	public class Subject
	{
		public int Id { get; set; }
		public required string Name { get; set; }
		public int SemesterNumber { get; set; }
		public int MajorId { get; set; }
		public Major Major { get; set; }
		public List<Note> Notes { get; set; }
	}
}
