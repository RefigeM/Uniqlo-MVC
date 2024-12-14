using System.ComponentModel.DataAnnotations;

namespace UniqloTasks.ViewModels.Auths
{
	public class RegisterVM
	{
		[Required, MaxLength(64)]
		public string Fullname { get; set; }

		[Required, MaxLength(128),DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[Required, MaxLength(64)]
		public string Username { get; set; }

		[Required, MaxLength(64), DataType(DataType.Password)]
		public string Password { get; set; }

		[Required, MaxLength(64),DataType(DataType.Password),Compare(nameof(Password))]
		public string RePassword { get; set; }
	}
}
