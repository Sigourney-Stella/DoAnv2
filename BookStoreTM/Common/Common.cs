using System.Net.Mail;
using System.Net;

namespace BookStoreTM.Common
{
    public class Common
    {
       // private static string password = ConfigurationManager.AppSettings["PasswordEmail"];
       // private static string Email = ConfigurationManager.AppSettings["Email"];
        public static bool SendMail(string name, string subject, string content,
            string toMail)
        {
            bool rs = false;
            try
            {
                MailMessage message = new MailMessage();
                var smtp = new SmtpClient();
                {
                    smtp.Host = "smtp.gmail.com"; //host name
                    smtp.Port = 587; //port number
                    smtp.EnableSsl = true; //whether your smtp server requires SSL
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;

                    smtp.UseDefaultCredentials = false;
                    
                }
                message.To.Add(toMail);
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = content;
                smtp.Send(message);
                rs = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                rs = false;
            }
            return rs;
        }
        public static string FormatNumber(object value, int decimalCount = 2, string decimalSeparator = ".", string thousandsSeparator = ",")
        {
            bool isNumber = IsNumeric(value);
            decimal GT = 0;
            if (isNumber)
            {
                GT = Convert.ToDecimal(value);
            }

            // Chuyển đổi số thành chuỗi dạng số có dấu phẩy phân cách hàng nghìn
            string str = string.Format("{0:#,0}", GT);

            // Kiểm tra và thêm số sau dấu phẩy nếu cần
            if (decimalCount > 0)
            {
                string decimalPart = "";
                for (int i = 0; i < decimalCount; i++)
                {
                    decimalPart += "#";
                }
                if (!string.IsNullOrEmpty(decimalPart))
                {
                    str += decimalSeparator + decimalPart;
                }
            }

            // Thay thế ký tự phân cách hàng nghìn
            str = str.Replace(thousandsSeparator, decimalSeparator);

            return str;
        }


        private static bool IsNumeric(object value)
        {
            return value is sbyte
                       || value is byte
                       || value is short
                       || value is ushort
                       || value is int
                       || value is uint
                       || value is long
                       || value is ulong
                       || value is float
                       || value is double
                       || value is decimal;
        }
    }
}
