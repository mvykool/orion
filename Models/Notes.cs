namespace orion.Models
{
    public class Notes
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public int UserId { get; set; } // Foreign key referencing User model
        public User User { get; set; } // Navigation property
    }
}
