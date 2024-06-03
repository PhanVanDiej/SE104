using System.Net.Mail;

namespace QuanLyHocSinh.Resources
{
    public class EmailCheck
    {
        public static bool Validate(string Email)
        {
            if (!string.IsNullOrEmpty(Email))
            {
                var trimmedEmail = Email.Trim();

                // Check if email ends with a dot
                if (trimmedEmail.EndsWith("."))
                {
                    return false;
                }

                try
                {
                    var addr = new MailAddress(trimmedEmail);
                    return addr.Address == trimmedEmail;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
    }
}
