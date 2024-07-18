using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMS.Models
{
	public class Role
	{
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MinLength(1), MaxLength(20)]
        public  string Name { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
