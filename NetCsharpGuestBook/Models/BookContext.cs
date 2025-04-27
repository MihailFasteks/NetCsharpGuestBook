using Microsoft.EntityFrameworkCore;

namespace NetCsharpGuestBook.Models
{
    public class BookContext:DbContext
    {
        
            public BookContext(DbContextOptions<BookContext> options)
               : base(options)
            {
                Database.EnsureCreated();
            }
            public DbSet<Messages> Messages { get; set; }
            public DbSet<Users> Users { get; set; }
        
    }
}
