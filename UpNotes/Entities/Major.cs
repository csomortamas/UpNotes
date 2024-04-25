namespace UpNotes.Entities
{
	public class Major
	{
		public int Id { get; set; }
		public required string Name { get; set; }
		public int SemesterCount { get; set; }
		public List<Subject> Subjects { get; set; }
	}
}
