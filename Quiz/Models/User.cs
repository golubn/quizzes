using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        public string UserName { get; set; }

        public int? Score { get; set; }

        public string Password { get; set; }

        public List<Game> Games { get; set; }
    }
}
