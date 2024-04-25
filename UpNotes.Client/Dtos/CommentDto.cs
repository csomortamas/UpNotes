namespace UpNotes.Client.Dtos
{
	public record CommentDto(int Id, string Content, string UserId, int NoteId);
	public record InsertCommentDto(string Content, int NoteId);
	public record ListItemCommentDto(int Id, string Content, DateTime CommentDate, int LikeCount, int DislikeCount, string UserId, string? LikeByLoggedInUser);
}
