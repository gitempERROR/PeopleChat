namespace PeopleChatAPI.Dto
{
    public record MessageDto
    {
        public int Id { get; set; }

        public int SenderId { get; set; }

        public int ReceaverId { get; set; }

        public string MessageContent { get; set; } = null!;
    }
}
