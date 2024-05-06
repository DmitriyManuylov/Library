using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        public string Phone { get; set; }

        public string Email { get; set; }

    }
}