using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMS.Models
{
	public class User
	{
        [Required]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		public string Email { get; set; }
		[Required]
		[MaxLength(100)]
		public string Password { get; set; }
		[Required]
		[MaxLength(50)]
		public string Name { get; set; }
		[Required]
		[MaxLength(50)]
		public string Surname { get; set; }
		[Required]
		public bool Approved { get; set; }
		[Column(TypeName = "text")]
		public string MembershipReason { get; set; }
		public DateTime MembershipRequestDate { get; set; }
		public DateTime MembershipApprovalDate { get; set; }
		[Required]
		public int RoleId { get; set; }
		[Required]
		public Role Role { get; set; }
		public ICollection<UserData> UserData { get; set; } = new List<UserData>();
		public ICollection<Payment> Payments { get; set; } = new List<Payment>();
	}
}
