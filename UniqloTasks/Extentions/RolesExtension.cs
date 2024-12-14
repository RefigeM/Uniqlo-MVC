using UniqloTasks.Views.Account.Enums;

namespace UniqloTasks.Extentions
{
	public static class RolesExtension
	{
		public static string GetRole(this Roles role) 
		{
			return role switch
			{
				Roles.Admin => nameof(Roles.Admin),
				Roles.User => nameof(Roles.User),
				Roles.Moderator => nameof(Roles.Moderator)

			};
		}
	}
}
