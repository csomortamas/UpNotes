namespace UpNotes.Client.Dtos
{
	public record SubjectDto(int Id, string Name, int SemesterNumber, int MajorId);
	public record InsertSubjectDto(string Name, int SemesterNumber, int MajorId);

	public class MutableSubjectDto
	{
		public string? Name { get; set; }
		public int SemesterNumber { get; set; }
		public int MajorId { get; set; }

		public MutableSubjectDto(SubjectDto subjectDto)
		{
			Name = subjectDto.Name;
			SemesterNumber = subjectDto.SemesterNumber;
			MajorId = subjectDto.MajorId;
		}

		public MutableSubjectDto() {}
	}
}
