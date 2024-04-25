namespace UpNotes.Client.Dtos
{
	public record MajorDto(int Id, string Name, int SemesterCount);

	public class MutableMajorDto
	{
		public string? Name { get; set; }
		public int SemesterCount { get; set; }

		public MutableMajorDto(MajorDto majorDto)
		{
			Name = majorDto.Name;
			SemesterCount = majorDto.SemesterCount;
		}

		public MutableMajorDto() { }
	}
}
