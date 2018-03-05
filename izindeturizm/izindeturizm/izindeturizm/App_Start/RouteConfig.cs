using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace izindeturizm
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            

            //KULLANICI TARAFINDAKİ URL LER
            #region kullanicilar
            routes.MapRoute(
              name: "iletisim",
              url: "iletisim-hakkimizda.html",
              defaults: new { controller = "Kullanicilar", action = "Iletisim_hakkimizda" }
          );

            routes.MapRoute(
              name: "menudangetir",
              url: "{sayfa}/{menuid}/{menu}.html",
              defaults: new { controller = "Kullanicilar", action = "menudanturgetir", sayfa = 1, menu = "", menuid = 1 }
          );
           // routes.MapRoute(
           //    name: "turrezervasyon",
           //    url: "rezarvasyon/{baslik}/{turid}.html",
           //    defaults: new { controller = "Kullanicilar", action = "TurRezervasyon", turid = 0, baslik = "" }
           //);
            routes.MapRoute(
                name: "turdetay",
                url: "secilen/{id}/detay/{baslik}/{nere}.html",
                defaults: new { controller = "Kullanicilar", action = "TurDetay", id = 0, baslik = "",nere="" }
            );
            routes.MapRoute(
                name: "kanasayfa",
                url: "anasayfa",
                defaults: new { controller = "Kullanicilar", action = "AnaSayfa" }
            );
            routes.MapRoute(
                name: "tumturlar",
                url: "{sayfa}/tum-turlar.html",
                defaults: new { controller = "Kullanicilar", action = "tumturlar", sayfa = 1 }
            );
            routes.MapRoute(
               name: "tumfirsatlar",
               url: "{sayfa}/tum-firsatlar.html",
               defaults: new { controller = "Kullanicilar", action = "tumfirsatlar", sayfa = 1 }
           );
            routes.MapRoute(
               name: "turrezarvasyon",
               url: "secilen/{baslik}/{id}/{nere}/rezarvasyon.html",
               defaults: new { controller = "Kullanicilar", action = "TurRezarvasyon", id=0}
           );

            #endregion

            //YÖNETİM PANELİNDEKİ URL LER

            #region yonetim yurunkisibilgileri

            routes.MapRoute(
             name: "yuruntalepformu",
             url: "{urun_id}/Urun-talep-formu",
             defaults: new { controller = "Yonetim", action = "YTalep_formlari", urun_id = 0 }
         );

            routes.MapRoute(
             name: "yurunkisibilgileri",
             url: "{iletisim_id}/Urun-rezarvasyon-iletisim-kisi-bilgileri",
             defaults: new { controller = "Yonetim", action = "YIletisim_Kisi_Bilgileri", urun_id = 0 }
         );
            routes.MapRoute(
             name: "yuruniletisimbilgileri",
             url: "{urun_id}/Urun-rezarvasyon-iletisim-bilgileri",
             defaults: new { controller = "Yonetim", action = "YIletisim_Bilgileri" ,urun_id=0 }
         );

            routes.MapRoute(
              name: "yyoneticibilgileri",
              url: "yonetici-bilgileri",
              defaults: new { controller = "Yonetim", action = "YYonetici_Bilgileri" }
          );

            routes.MapRoute(
              name: "ygiris",
              url: "admin-giris",
              defaults: new { controller = "Yonetim", action = "YGiris" }
          );

            routes.MapRoute(
               name: "ycikis",
               url: "Yonetim/Cikis",
               defaults: new { controller = "Yonetim", action = "YCikis" }
           );
            routes.MapRoute(
               name: "yanasayfa",
               url: "Yonetim/AnaSayfa",
               defaults: new { controller = "Yonetim", action = "Yanasayfa" }
           );
            routes.MapRoute(
               name: "ytumtransferler",
               url: "tum-transferler/{sayfa}",
               defaults: new { controller = "Yonetim", action = "YTumTransferler", sayfa = 1 }
           );
            routes.MapRoute(
              name: "ytransfersil",
              url: "transfer-sil/{id}",
              defaults: new { controller = "Yonetim", action = "YTransfersil", id = 0 }
          );
            routes.MapRoute(
               name: "ykalkissil",
               url: "kalkis-sil/{id}",
               defaults: new { controller = "Yonetim", action = "YKalkisSil", id = 0 }
           );
            routes.MapRoute(
               name: "yvarissil",
               url: "varis-sil/{id}",
               defaults: new { controller = "Yonetim", action = "YVarisSil", id = 0 }
           );
            routes.MapRoute(
               name: "yfiyatsil",
               url: "fiyat-sil/{id}",
               defaults: new { controller = "Yonetim", action = "YFiyatSil", id = 0 }
           );
            routes.MapRoute(
               name: "ytransfer",
               url: "transferler",
               defaults: new { controller = "Yonetim", action = "YTransfer" }
           );
            routes.MapRoute(
                name: "ymenusil",
                url: "menu-sil/{id}",
                defaults: new { controller = "Yonetim", action = "YMenuSil", id = 0 }
            );
            routes.MapRoute(
                name: "ymenu",
                url: "menu-islemleri",
                defaults: new { controller = "Yonetim", action = "YMenu" }
            );
            routes.MapRoute(
                name: "ytursil",
                url: "tur-sil/{id}",
                defaults: new { controller = "Yonetim", action = "YTurSil", id = 0 }
            );
            routes.MapRoute(
                name: "yturekle",
                url: "yeni-tur-ekle",
                defaults: new { controller = "Yonetim", action = "YTurlar" }
            );
            routes.MapRoute(
                name: "yturguncelle",
                url: "tur-guncelle/{id}",
                defaults: new { controller = "Yonetim", action = "YTurlar", id = 0 }
            );
            routes.MapRoute(
                name: "ytumturlar",
                url: "tum-turlar/{sayfa}",
                defaults: new { controller = "Yonetim", action = "YTumTurlar", sayfa = 1 }
            );
            routes.MapRoute(
                name: "ygaleri",
                url: "tum-galeri-islemleri/{sayfa}",
                defaults: new { controller = "Yonetim", action = "galeri", sayfa = 1 }
            );
            routes.MapRoute(
                name: "ygalerisil",
                url: "tum-galeri-sil/{id}",
                defaults: new { controller = "Yonetim", action = "galerisil", id = 0 }
            );
            routes.MapRoute(
               name: "yturtarihleri",
               url: "tur-tarihler/{turid}",
               defaults: new { controller = "Yonetim", action = "YTarihler", turid = 0 }
           );
            routes.MapRoute(
               name: "ytarihsil",
               url: "tur-tarih-sil/{id}",
               defaults: new { controller = "Yonetim", action = "YTarihSil", id = 0 }
           );
            routes.MapRoute(
               name: "yturotelekle",
               url: "tur-otel-tarih-ekle/{turid}",
               defaults: new { controller = "Yonetim", action = "YTurOtelEkle", turid = 0 }
           );

            #endregion




            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Kullanicilar", action = "AnaSayfa", id = UrlParameter.Optional }
           );


        }
    }
}
