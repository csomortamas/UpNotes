namespace UpNotes.Client.Dtos
{
	public record AddLikeOrDislikeDto(int CommentId, string Type);
	public record LikesOnCommentDto(int CommentId, int LikeCount, int DislikeCount);
}
