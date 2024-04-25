namespace UpNotes.Client.Dtos
{
	public record NoteDto(int Id, string Title, string Content, DateTime UploadDate, string UserId, int SubjectId);
	public record ListItemNoteDto(int Id, string Title, string Content, DateTime UploadDate, string UserId, SubjectDto Subject, MajorDto Major, double Rating);
	public record InsertNoteDto(string Title, string Content, int SubjectId);

	public class MutableNoteDto
	{
		public string? Title { get; set; }
		public string? Content { get; set; }
		public int SubjectId { get; set; }

		public MutableNoteDto(NoteDto noteDto)
		{
			Title = noteDto.Title;
			Content = noteDto.Content;
			SubjectId = noteDto.SubjectId;
		}

		public MutableNoteDto() { }
	}

	public class MutableListItemNoteDto
	{
		public int Id { get; set; }
		public string? Title { get; set; }
		public string? Content { get; set; }
		public string? UserId { get; set; }
		public SubjectDto? Subject { get; set; }
		public MajorDto? Major { get; set; }

		public MutableListItemNoteDto(ListItemNoteDto listItemNoteDto)
		{
			Title = listItemNoteDto.Title;
			Content = listItemNoteDto.Content;
			UserId = listItemNoteDto.UserId;
			Subject = listItemNoteDto.Subject;
			Major = listItemNoteDto.Major;
		}

		public MutableListItemNoteDto() { }
	}	
}
