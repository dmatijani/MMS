using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMS.Models
{
	public class UserData
	{
		[Required]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		[MinLength(1), MaxLength(30)]
		public string Name { get; set; }
		[Required]
		[MinLength(1), MaxLength(80)]
		public string Value { get; set; }
		[Required]
		public int UserId { get; set; }
		[Required]
		public User User { get; set; }
    }
}
