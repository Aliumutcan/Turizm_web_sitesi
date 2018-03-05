using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace izindeturizm.Controllers.Genelislemler
{
    public static class routeurlduzenle
    {
        public static string routeduzelt(this string url)
        {
            string temp = url;
            temp = temp.ToLower();
            temp = temp.Replace('/', '-');
            temp = temp.Replace('ö', 'o');
            temp = temp.Replace('ü', 'u');
            temp = temp.Replace('ç', 'c');
            temp = temp.Replace('=', '-');
            temp = temp.Replace('%', '-');
            temp = temp.Replace('&', '-');
            temp = temp.Replace('\'', '-');
            temp = temp.Replace(' ', '-');
            return temp;

        }
    }
}