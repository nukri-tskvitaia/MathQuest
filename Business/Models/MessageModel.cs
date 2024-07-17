namespace Business.Models
{
    public class MessageModel
    {
        public string SenderId { get; set; } = string.Empty;

        public string ReceiverId { get; set; } = string.Empty;

        public string Text { get; set; } = string.Empty;

        public DateTime? SentDate { get; set; }
    }
}