using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using izindeturizm.Models;
using PagedList;
using PagedList.Mvc;
using izindeturizm.Controllers.Genelislemler;
using izindeturizm.ServiceReference1;
using Newtonsoft.Json;

namespace izindeturizm.Controllers.Kullanicilar
{
    public class KullanicilarController : Controller
    {
        // GET: Kullanicilar
        Model1 db = new Model1();

        public ActionResult kullaniciBilgilendirme()
        {
            string[] mesaj = new string[2];
            if (Session["mesaj"] != null)
            {
                mesaj = (string[])Session["mesaj"];
                ViewBag.mesaj = mesaj[0];
                ViewBag.mesajderecesi = mesaj[1];
                Session.Remove("mesaj");
            }
            return View();
        }

        public ActionResult AnaSayfa()
        {
            List<Urunler> values = new List<Urunler>();
            var gelen= db.Urunler.Where(x => x.acik_kapali == true).ToList().ToPagedList(1, 3);
            values.AddRange(gelen);
            if (gelen.Count % 3 == 1)
                values.AddRange(Saltur_Urunleri("Yurt İçi Turlar", 1, 5));
            else if (gelen.Count % 3 == 2)
                values.AddRange(Saltur_Urunleri("Yurt İçi Turlar", 1, 4));
            else
                values.AddRange(Saltur_Urunleri("Yurt İçi Turlar", 1, 3));

            

            return View(values);
        }

        private List<Menu> menularigetir()
        {
            return db.Menu.ToList();
        }

        public ActionResult paritalmenu()
        {
            return View(menularigetir());
        }

        public ActionResult Parital_Urunler()
        {
            return View();
        }

        public ActionResult Paritalgaleri()
        {
            List<Galeri> galerigetir = db.Galeri.Where(x => x.nereicin == "galeri").ToList();
            return View(galerigetir);
        }

        public ActionResult tumturlar(int sayfa)
        {
            List<Urunler> urunler = new List<Urunler>();
            var menu_idleri = db.Menu.Where(x => x.adi.ToLower().IndexOf("yurt içi")>=0).Take(1).FirstOrDefault();

            while (menu_idleri != null)
            {
                var gelen = db.Urunler.Where(x => x.menu_ID == menu_idleri.id && x.acik_kapali == true).ToList().ToPagedList(sayfa, 6);
                urunler.AddRange(gelen);
                if (gelen.Count % 3 == 1)
                    urunler.AddRange(Saltur_Urunleri(menu_idleri.adi, sayfa, 8));
                else if (gelen.Count % 3 == 2)
                    urunler.AddRange(Saltur_Urunleri(menu_idleri.adi, sayfa, 7));
                else
                    urunler.AddRange(Saltur_Urunleri(menu_idleri.adi, sayfa, 6));

                menu_idleri = db.Menu.Where(x => x.id == menu_idleri.ustid).Take(1).FirstOrDefault();
            }

            if (urunler.Count <= 0)
                return RedirectToAction("AnaSayfa");
            else
                return View(urunler);

        }

        public ActionResult menudanturgetir(int sayfa, int menuid, string menu)
        {
            sayfa = sayfa > 0 ? sayfa : 1;
            List<Urunler> urunler = new List<Urunler>();
            var menu_idleri = db.Menu.Where(x => x.id == menuid).Take(1).FirstOrDefault();
            while(menu_idleri!=null)
            {
                var gelen = db.Urunler.Where(x => x.menu_ID == menu_idleri.id && x.acik_kapali == true).ToList().ToPagedList(sayfa, 6);
                urunler.AddRange(gelen);
                if (gelen.Count%3==1)
                    urunler.AddRange(Saltur_Urunleri(menu_idleri.adi, sayfa,8));
                else if (gelen.Count%3==2)
                    urunler.AddRange(Saltur_Urunleri(menu_idleri.adi, sayfa, 7));
                else
                    urunler.AddRange(Saltur_Urunleri(menu_idleri.adi, sayfa, 6));
                menu_idleri = db.Menu.Where(x => x.id == menu_idleri.ustid).Take(1).FirstOrDefault();
            }
            if (urunler.Count <= 0 && sayfa>1)
                return RedirectToAction("menudanturgetir", new { sayfa = sayfa - 1, menuid = menuid, menu = menu });
            else
                return View(urunler);
        }
        
        public ActionResult TurAra(string aranacak, DateTime gidis , DateTime donus )
        {
            HttpCookie dene=new HttpCookie("bulunanlar",Json(Saltur_TurAra(aranacak, gidis, donus)).ToString());
            HttpCookie bulunanlar = new HttpCookie("bulunanalar");
            var model = JsonConvert.DeserializeObject<List<Urunler>>(Response.Cookies["bulunanlar"].Value);
            return View();
        }

        [HttpGet]
        public ActionResult TurDetay(int id, string nere)
        {
            if (nere == "saltur")
                return View(Saltur_Detay(id));
            if (id > 0 && id < int.MaxValue)
            {
                try
                {
                    Urunler tur = db.Urunler.Where(x => x.id == id && x.acik_kapali == true).Take(1).FirstOrDefault();
                    if (tur != null)
                        return View(tur);
                }
                catch (Exception) {return RedirectToAction("AnaSayfa"); }
            }
            return RedirectToAction("AnaSayfa");
        }

        [HttpGet]
        public ActionResult TalepFormu()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TalepFormu(int id, Urun_Talep_Formu talepformu)
        {
            if (talepformu.tel.Trim().Length > 0 && talepformu.adsoyad.Trim().Length > 0 && id > 0)
            {
                try
                {
                    talepformu.id = 0;
                    talepformu.urun_ID = id;
                    db.Urun_Talep_Formu.Add(talepformu);
                    db.SaveChanges();
                    string[] mesaj = new string[2];
                    mesaj[0] = "Talep başarılı bir şekilde iletildi.";
                    mesaj[1] = "0";
                    Session["mesaj"] = mesaj;
                }
                catch (Exception)
                {
                    string[] mesaj = new string[2];
                    mesaj[0] = "Talep iletimi başarısız";
                    mesaj[1] = "2";
                    Session["mesaj"] = mesaj;
                }
            }
            else
            {
                string[] mesaj = new string[2];
                mesaj[0] = "Talep de eksik bilgi var";
                mesaj[1] = "1";
                Session["mesaj"] = mesaj;
            }


            return RedirectToAction("TurDetay", new { id = id, nere = "izinde", baslik = "Basarili" });
        }
        [HttpGet]
        public ActionResult TurRezarvasyon(int id, string nere)
        {
            if (nere=="saltur")
            {
                return RedirectToAction("Saltur_Rezarvasyon",new {id=id });
            }
            if (id > 0 && id < int.MaxValue && nere=="izinde")
            {
                Session["secilen_urun"] = id;
                return View(db.Urunler.Where(x => x.id == id).Take(1).FirstOrDefault());
            }
            else
                return RedirectToAction("AnaSayfa");
        }
        [HttpPost]
        public ActionResult TurRezarvasyon(Urun_Rezervasyon_Iletisim_Bilgileri iletisim, List<Urun_Rezervasyon_Kisi_Bilgileri> turrez)
        {
            int urun_ID = 0;
            string[] mesaj = new string[2];
            try
            {
                urun_ID = (int)Session["secilen_urun"];
                if (iletisim != null && iletisim.tel.Trim().Length > 0 && turrez != null && turrez.Count > 0 && urun_ID > 0)
                {
                    iletisim.tarih = DateTime.Now;
                    iletisim.odendimi = false;
                    db.Urun_Rezervasyon_Iletisim_Bilgileri.Add(iletisim);
                    db.SaveChanges();
                    foreach (var item in turrez)
                    {
                        item.iletisim_bilgileri_ID = iletisim.id;
                        db.Urun_Rezervasyon_Kisi_Bilgileri.Add(item);
                    }
                    db.SaveChanges();
                    mesaj[0] = "Rezervasyonunuz başarılı bir şekilde alındı.";
                    mesaj[1] = "0";
                    Session["mesaj"] = mesaj;
                    return RedirectToAction("turdetay", new { id = urun_ID,nere="izinde", baslik = "Basarili" });
                }
                mesaj[0] = "Eksik bilgiler var kontrol edip tekrar deneyin";
                mesaj[1] = "1";
                Session["mesaj"] = mesaj;
                return RedirectToAction("turrezarvasyon", new { id = urun_ID, nere = "izinde", baslik = "tekrar-dene" });
            }
            catch (Exception)
            {
                mesaj[0] = "Hata! İşleminiz başarısız.";
                mesaj[1] = "2";
                Session["mesaj"] = mesaj;
                return RedirectToAction("turrezarvasyon", new { id = urun_ID, nere = "izinde", baslik = "Rezervasyon-Basarisiz" });
            }


        }
        [HttpGet]
        public ActionResult Kredi_Karti_Bilgileri()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Kredi_Karti_Bilgileri(int iletisim_ID, int urun_ID)
        {
            return View();
        }
        [HttpGet]
        public ActionResult FirsatDetay(int id, string nere)
        {
            return View();
        }

        public ActionResult Transfer(Transfer yeni)
        {
            if (yeni != null && yeni.adsoyad.Trim().Length > 0 && yeni.nerden > 0 && yeni.nereye > 0 && yeni.t_not != null && yeni.tamadres != null && yeni.tarih >= DateTime.Now.Date && yeni.tel.Trim().Length > 8)
            {
                try
                {
                    yeni.durum = "acik";
                    db.Transfer.Add(yeni);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    string[] mesaj = new string[2];
                    mesaj[0] = "Bir hata oluştu sonra tekrar deneyin.";
                    mesaj[1] = "2";
                    Session["mesaj"] = mesaj;
                    throw;
                }
            }
            else
            {
                string[] mesaj = new string[2];
                mesaj[0] = "Eksik bilgi girildi.";
                mesaj[1] = "1";
                Session["mesaj"] = mesaj;
            }


            return RedirectToAction("AnaSayfa");
        }
        [HttpGet]
        public ActionResult Iletisim_hakkimizda()
        {
            ViewData["hakkimizda"] = db.Database.ExecuteSqlCommand("select hakkimizda from Admin ");
            return View();
        }

        [HttpPost]
        public ActionResult Iletisim_hakkimizda(Iletisim iletisim)
        {
            if (iletisim.adisoyadi.Trim().Length > 2 && iletisim.mesaj.Trim().Length > 30)
            {
                iletisim.tarih = DateTime.Now;
                iletisim.goruldu = false;
                db.Iletisim.Add(iletisim);
                db.SaveChanges();
            }
            ViewData["hakkimizda"] = db.Database.ExecuteSqlCommand("select hakkimizda from Admin ");
            return View();
        }


        #region saltur servis
        public ActionResult Saltur_Rezarvasyon(int id)
        {
            List<salturturlistesi> liste = Saltur_Urun(id);
            
            if (liste.Count>0)
            {
                ViewData["saltur"] = liste;
            }
            else
            {
                return RedirectToAction("AnaSayfa");
            }
            return View();
        }


        /// <summary>
        /// girilen kategoriye göre verileri ceker
        /// Eğer kategoride hiçbir urun yoksa rasgele karışık ceker
        /// </summary>
        /// <param name="kategori"></param>
        /// <returns></returns>
        public IPagedList<Urunler> Saltur_Urunleri(string kategori, int sayfa, int adet)
        {
            List<Urunler> urunler = new List<Urunler>();
            SalturWebServiceSoapClient veriler = new SalturWebServiceSoapClient();
            var liste = veriler.GetEnYakinTarihliTurlar(kategori, "BZLzeruGZz23HAfUqLmjySQT", "123456").Where(x=>x.SatisDurum==true).ToPagedList(sayfa, adet);
            for (int i = 0; i < liste.Count; i++)
            {
                urunler.Add(new Urunler()
                {
                    baslik = liste[i].TurAdi,
                    fiyat = liste[i].TurFiyati,
                    icerik = liste[i].TurProgrami,
                    id = liste[i].turid,
                    kacgun_kacgece = liste[i].TurGunGece,
                    para_birimi = liste[i].TurParaBirimi
                    
                    
                });
                urunler[i].Galeri.Add(new Galeri() { link = liste[i].TurAnasayfaResmi, nereicin = "tur", resimyol = liste[i].TurAnasayfaResmi.Replace("http://","https://"), aciklama = "saltur" });

            }
            return urunler.ToPagedList(1, adet);
        }
        public List<Urunler> Saltur_TurAra(string aranacak,DateTime gidis,DateTime donus)
        {
            List<Urunler> urunler = new List<Urunler>();
            SalturWebServiceSoapClient veriler = new SalturWebServiceSoapClient();
            List<salturturlistesi> liste = new List<salturturlistesi>();
            foreach (var item in aranacak.Split(' '))
            {
                
                if (gidis>DateTime.Now && donus>DateTime.Now)
                {
                    liste.AddRange(veriler.GetEnYakinTarihliTurlar("Yurt İci Turlar", "BZLzeruGZz23HAfUqLmjySQT", "123456").Where(x => x.TurAdi.ToLower().IndexOf(item) >= 0 && x.turbaslangictarihi>=gidis && x.turbitistarihi<=donus && x.SatisDurum==true));
                    liste.AddRange(veriler.GetEnYakinTarihliTurlar("Yurt Dışı Turlar", "BZLzeruGZz23HAfUqLmjySQT", "123456").Where(x => x.TurAdi.ToLower().IndexOf(item) >= 0 && x.turbaslangictarihi >= gidis && x.turbitistarihi <= donus && x.SatisDurum == true));
                    liste.AddRange(veriler.GetEnYakinTarihliTurlar("Gemi Turlar", "BZLzeruGZz23HAfUqLmjySQT", "123456").Where(x => x.TurAdi.ToLower().IndexOf(item) >= 0 && x.turbaslangictarihi >= gidis && x.turbitistarihi <= donus && x.SatisDurum == true));
                }
                else if (gidis>DateTime.Now)
                {
                    liste.AddRange(veriler.GetEnYakinTarihliTurlar("Yurt İci Turlar", "BZLzeruGZz23HAfUqLmjySQT", "123456").Where(x => x.TurAdi.ToLower().IndexOf(item) >= 0 && x.turbaslangictarihi >= gidis && x.SatisDurum == true));
                    liste.AddRange(veriler.GetEnYakinTarihliTurlar("Yurt Dışı Turlar", "BZLzeruGZz23HAfUqLmjySQT", "123456").Where(x => x.TurAdi.ToLower().IndexOf(item) >= 0 && x.turbaslangictarihi >= gidis && x.SatisDurum == true));
                    liste.AddRange(veriler.GetEnYakinTarihliTurlar("Gemi Turlar", "BZLzeruGZz23HAfUqLmjySQT", "123456").Where(x => x.TurAdi.ToLower().IndexOf(item) >= 0 && x.turbaslangictarihi >= gidis && x.SatisDurum == true));
                }
                else if (donus>DateTime.Now)
                {
                    liste.AddRange(veriler.GetEnYakinTarihliTurlar("Yurt İci Turlar", "BZLzeruGZz23HAfUqLmjySQT", "123456").Where(x => x.TurAdi.ToLower().IndexOf(item) >= 0 && x.turbitistarihi <= donus && x.SatisDurum == true));
                    liste.AddRange(veriler.GetEnYakinTarihliTurlar("Yurt Dışı Turlar", "BZLzeruGZz23HAfUqLmjySQT", "123456").Where(x => x.TurAdi.ToLower().IndexOf(item) >= 0 && x.turbitistarihi <= donus && x.SatisDurum == true));
                    liste.AddRange(veriler.GetEnYakinTarihliTurlar("Gemi Turlar", "BZLzeruGZz23HAfUqLmjySQT", "123456").Where(x => x.TurAdi.ToLower().IndexOf(item) >= 0 && x.turbitistarihi <= donus && x.SatisDurum == true));
                }
            }
            for (int i = 0; i < liste.Count; i++)
            {
                urunler.Add(new Urunler()
                {
                    baslik = liste[i].TurAdi,
                    fiyat = liste[i].TurFiyati,
                    icerik = liste[i].TurProgrami,
                    id = liste[i].turid,
                    kacgun_kacgece = liste[i].TurGunGece,
                    para_birimi = liste[i].TurParaBirimi

                });
                urunler[i].Galeri.Add(new Galeri() { link = liste[i].TurAnasayfaResmi, nereicin = "tur", resimyol = liste[i].TurAnasayfaResmi.Replace("http://", "https://"), aciklama = "saltur" });

            }

            return urunler;
        }
        public List<TurGelismisUcret> Saltur_Urun_Tarihleri(int id)
        {
            List<Urunler> urunler = new List<Urunler>();
            SalturWebServiceSoapClient veriler = new SalturWebServiceSoapClient();

            var liste = veriler.GetTurDetayi(id, "BZLzeruGZz23HAfUqLmjySQT", "123456");

            List<TurGelismisUcret> tarihfiyar = new List<TurGelismisUcret>();
            foreach (var item in liste)
            {
                tarihfiyar.AddRange(item.turfiyatlari.Where(x => x.TurUcretSubID == id));

            }
            return tarihfiyar;



        }
        public List<salturturlistesi> Saltur_Urun(int id)
        {
            SalturWebServiceSoapClient veriler = new SalturWebServiceSoapClient();
            List<salturturlistesi> liste= veriler.GetTurDetayi(id, "BZLzeruGZz23HAfUqLmjySQT", "123456");
            for (int i = 0; i < liste.Count; i++)
            {
                if (liste[i].TurParaBirimi == "$")
                {
                    liste[i].TurFiyati = (Convert.ToDouble(liste[i].TurFiyati) * liste[i].Dolar).ToString();
                    if(liste[i].BebekFiyati!=null)
                        liste[i].BebekFiyati = (Convert.ToDouble(liste[i].BebekFiyati) * liste[i].Dolar).ToString();
                    else
                        liste[i].BebekFiyati = (50 * liste[i].Dolar).ToString();
                    if (liste[i].CocukFiyati != null)
                        liste[i].CocukFiyati = (Convert.ToDouble(liste[i].CocukFiyati) * liste[i].Dolar).ToString();
                    else
                        liste[i].CocukFiyati = ((Convert.ToDouble(liste[i].TurFiyati) - ((Convert.ToDouble(liste[i].TurFiyati) * 15) / 100))).ToString();
                }                    
                else if (liste[i].TurParaBirimi == "€")
                {
                    liste[i].TurFiyati = (Convert.ToDouble(liste[i].TurFiyati) * liste[i].Euro).ToString();
                    if (liste[i].BebekFiyati != null)
                        liste[i].BebekFiyati = (Convert.ToDouble(liste[i].BebekFiyati) * liste[i].Euro).ToString();
                    else
                        liste[i].BebekFiyati = (50 * liste[i].Euro).ToString();

                    if(liste[i].CocukFiyati != null)
                        liste[i].CocukFiyati = (Convert.ToDouble(liste[i].CocukFiyati) * liste[i].Euro).ToString();
                    else
                        liste[i].CocukFiyati = ((Convert.ToDouble(liste[i].TurFiyati)-((Convert.ToDouble(liste[i].TurFiyati) * 15)/100))).ToString();
                }
                    
            }
            try
            {
                for (int i = 0; i < liste[0].turfiyatlari.Count(); i++)
                {
                    if (liste[0].TurParaBirimi == "$")
                        liste[0].turfiyatlari[i].Ucret = (Convert.ToDouble(liste[0].turfiyatlari[i].Ucret) * liste[0].Dolar).ToString();
                    else if (liste[0].TurParaBirimi == "€")
                        liste[0].turfiyatlari[i].Ucret = (Convert.ToDouble(liste[0].turfiyatlari[i].Ucret) * liste[0].Euro).ToString();
                }
            }
            catch (Exception)
            {
            }
            
            return liste; 
        }
        public Urunler Saltur_Detay(int id)
        {
            if (id > 0 && id < int.MaxValue)
            {
                try
                {
                    SalturWebServiceSoapClient veri = new SalturWebServiceSoapClient();
                    var detay = veri.GetTurDetayi(id, "BZLzeruGZz23HAfUqLmjySQT", "123456");
                    Urunler urun = new Urunler();
                    urun.id = detay[0].turid;
                    urun.baslik = detay[0].TurAdi;
                    urun.icerik = detay[0].TurProgrami;
                    urun.kacgun_kacgece = detay[0].TurGunGece;
                    urun.para_birimi = detay[0].TurParaBirimi;
                    urun.fiyat = detay[0].TurFiyati;
                    urun.talep_formu = false;
                    urun.Urun_Tarihleri.Add(new Urun_Tarihleri() { cikis_tarihi = detay[0].turbaslangictarihi, donus_tarihi = detay[0].turbitistarihi });

                    var galeri = veri.GetTurResimleri(id, "BZLzeruGZz23HAfUqLmjySQT", "123456");
                    foreach (var item in galeri)
                    {
                        urun.Galeri.Add(new Galeri() { aciklama = "saltur", resimyol = item.SalturTurResim.Replace("http://", "https://") });
                    }
                    return urun;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
                return null;
            
        }
        #endregion
    }
}