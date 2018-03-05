using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Helpers;

namespace izindeturizm.Controllers.Genelislemler
{
    public class iletisim_islemleri
    {
        public bool Email_Gonder(string mesaj,string mailadresi,string mailsifre,string kime)
        {
            
            try
            {
                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.UserName = mailadresi;
                WebMail.Password = mailsifre;
                WebMail.EnableSsl = true;
                WebMail.SmtpPort = 587;
                WebMail.Send(kime, "İzinde Turizm İletişim Mesajı", mesaj, mailadresi);
                return true;
            }
            catch {
                return false;
            }
        }
    }
}