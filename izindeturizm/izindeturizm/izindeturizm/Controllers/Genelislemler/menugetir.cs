using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using izindeturizm.Models;

namespace izindeturizm.Controllers.Genelislemler
{
    public class menugetir
    {
        public static string menuduzelt(int id, List<Menu> model)
        {
            string menu = "";
            foreach (var item in model.Where(x => x.ustid == id))
            {
                menu = menu + "<li onmousemove='menu_ac("+item.id+");' onmouseout='menu_kapa("+item.id+");'><a href=\"" + "/1/" + item.id +"/"+ item.adi.routeduzelt() + ".html" + "\">" + item.adi + "</a>  ";
                menu = menu + " <div id='menu_"+item.id+ "' class='menu-acilir' style='display:none;'><ul>"+menuduzelt(item.id,model)+" </ul></div>";
            }
            return menu + "</li>";
        }
    }
}