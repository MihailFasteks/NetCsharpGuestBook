namespace NetCsharpGuestBook.Models
{
    public class Users
    {

        public Users()
        {
            this.Messages = new HashSet<Messages>();
        }
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password {  get; set; }
        public string? Salt { get; set; }
        public ICollection<Messages> Messages { get; set; }
    }
}
