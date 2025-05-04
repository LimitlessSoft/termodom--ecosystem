namespace TD.Web.Common.Contracts.Helpers;

public static class EmailHelpers
{
	public static bool IsEmailValid(string email)
	{
		if (string.IsNullOrWhiteSpace(email))
			return false;

		try
		{
			var addr = new System.Net.Mail.MailAddress(email);
			return addr.Address == email;
		}
		catch
		{
			return false;
		}
	}
}
