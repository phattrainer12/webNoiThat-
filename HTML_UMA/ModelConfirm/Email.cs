using HTML_UMA.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Net.Mail;

namespace HTML_UMA.ModelConfirm
{
    public class Email
    {
        
        public static string Send(int id)
        {
            DB_UMAEntities db = new DB_UMAEntities();
            try
            {
                var obj = db.OrderDetails.SingleOrDefault(x => x.ID == id);
                var inf = db.InformationMails.SingleOrDefault(x => x.active == true);
                //Configuring webMail class to send emails  
                //gmail smtp serverz 
                WebMail.SmtpServer = inf.SmtpServer;
                //gmail port to send emails  
                WebMail.SmtpPort = int.Parse(inf.SmtpPort);
                WebMail.SmtpUseDefaultCredentials = true;
                //sending emails with secure protocol  
                WebMail.EnableSsl = true;
                //EmailId used to send emails from application  
                WebMail.UserName = inf.UserName;
                WebMail.Password = inf.Password;

                //Sender email address.  
                WebMail.From = inf.FromEmail;

                //Send email  
                WebMail.Send(
                    to: obj.detail_PayEmail,
                    subject: "Xác nhận đơn hàng: " + obj.detail_ID,
                    body: "Đơn hàng của bạn đã được chúng tối ghi nhận với mã hóa đơn là : " + obj.detail_ID + ". Tổng số tiền cần thanh toán là: " + obj.detail_Totalmoney + "VNĐ, mọi thắc mắc vui lòng liên hệ qua số điện thoại hỗ trợ: 0905 717879 - 0931 993179. Trân trọng!"
                    );
                return "Gửi Email thành công";

            }
            catch
            {
                return "Gửi thất bại, vui lòng kiểm tra lại Config Email của bạn!";
            }
                
        }
    }
}