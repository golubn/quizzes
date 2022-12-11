using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz.Models
{
    public class Game
    {
        [Key]
        public int UserId { get; set; }

        public string GameName { get; set; }
        public int Points { get; set; }

        [Required]
        public User User { get; set; }
    }
}
