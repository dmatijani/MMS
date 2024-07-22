using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MMS.Models
{
	public class Payment
	{
		[Required]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		[Column(TypeName = "date")]
		public DateTime Date { get; set; }
		[Required]
		public int UserId { get; set; }
		[Required]
		public User User { get; set; }
		[NotMapped]
		public int Year
		{
			get
			{
				return Date.Year;
			}
		}
		[NotMapped]
		public DateTime DateUntil
		{
			get
			{
				return Date.AddYears(1);
			}
		}
	}
}
