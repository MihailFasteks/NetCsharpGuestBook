namespace NetCsharpGuestBook.Models
{
    public class Messages
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public int UserId {  get; set; }
        public Users User {  get; set; }
    }
}
