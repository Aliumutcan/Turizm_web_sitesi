using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using izindeturizm.Models;
using izindeturizm.Controllers.Genelislemler;
using System.Data;
using PagedList;
using System.Web.Helpers;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace izindeturizm.Controllers.Yonetim
{
    public class YonetimController : Controller
    {
        // GET: Yonetim

        Model1 db = new Model1();

        [HttpGet]
        public ActionResult YGiris()
        {
            return View();
        }

        [HttpPost]
        public ActionResult YGiris(Admin giris)
        {
            if (giris == null || giris.sifre == null || giris.kullaniciadi == null)
                return View();
            try
            {
                giris.sifre = md5.MD5eDonustur(giris.sifre);
                Admin admin = db.Admin.Where(x => x.sifre == giris.sifre && x.kullaniciadi==giris.kullaniciadi).Take(1).FirstOrDefault();
                if (admin != null)
                {
                    Session["admin"] = admin;
                    return RedirectToAction("Yanasayfa");
                }
                else
                {

                    Session["mesaj"] = "Şifre veya kullanıcı adınız yanlış";
                    Session["mesajderecesi"] = 1;
                }
            }
            catch (Exception)
            {
                Session["mesaj"] = "Şifre veya kullanıcı adınız yanlış";
                Session["mesajderecesi"] = 1;
            }
            return View();
        }
        [admingiris]
        public ActionResult YCikis()
        {
            if (Session["admin"] != null)
            {
                Session.Remove("admin");
            }

            return RedirectToAction("YGiris");
        }
        [HttpGet]
        [admingiris]
        public ActionResult YYonetici_Bilgileri()
        {
            return View(db.Admin.Take(1).FirstOrDefault());
        }
        [HttpPost]
        [ValidateInput(false)]
        [admingiris]
        public ActionResult YYonetici_Bilgileri(Admin admin, string sifre2 = "")
        {
            Admin guncelle = db.Admin.Take(1).FirstOrDefault();
            if (guncelle != null && sifre2 == admin.sifre && admin.kullaniciadi.Trim().Length > 5 && admin.is_tel.Trim().Length > 9)
            {
                guncelle.adres = admin.adres;
                guncelle.adsoyad = admin.adsoyad;
                guncelle.cep_tel = admin.cep_tel;
                guncelle.email = admin.email;
                guncelle.Hakkimizda = admin.Hakkimizda;
                guncelle.is_tel = admin.is_tel;
                guncelle.kullaniciadi = admin.kullaniciadi;
                guncelle.sifre = md5.MD5eDonustur(sifre2);
                db.SaveChanges();
            }
            else if (guncelle != null && sifre2 == "" && admin.sifre == null && admin.kullaniciadi.Trim().Length > 5 && admin.is_tel.Trim().Length > 9)
            {
                guncelle.adres = admin.adres;
                guncelle.adsoyad = admin.adsoyad;
                guncelle.cep_tel = admin.cep_tel;
                guncelle.email = admin.email;
                guncelle.Hakkimizda = admin.Hakkimizda;
                guncelle.is_tel = admin.is_tel;
                guncelle.kullaniciadi = admin.kullaniciadi;
                db.SaveChanges();
            }
            return RedirectToRoute("ycikis");
        }
        [admingiris]
        public ActionResult Yanasayfa()
        {
            ViewBag.tur_sayisi = db.Urunler.Where(x => x.urun_sinifi == 0).Count();
            ViewBag.otel_sayisi = db.Urunler.Where(x => x.urun_sinifi == 1).Count();
            ViewBag.otobus_sayisi = db.Urunler.Where(x => x.urun_sinifi == 2).Count();
            ViewBag.ucak_sayisi = db.Urunler.Where(x => x.urun_sinifi == 3).Count();
            ViewBag.tren_sayisi = db.Urunler.Where(x => x.urun_sinifi == 4).Count();
            ViewBag.gemi_sayisi = db.Urunler.Where(x => x.urun_sinifi == 5).Count();
            ViewBag.transfer_sayisi = db.Transfer.Count();
            ViewData["iletisim"] = db.Iletisim.Where(x => x.goruldu == false).ToList();
            ViewData["tkalkis"] = kalkisgetir();
            ViewData["tvaris"] = varisgetir();
            ViewData["transfer"] = db.Transfer.Where(x => x.durum == "acik").ToList();

            return View();
        }
        public ActionResult YMesaj()
        {
            if (Session["mesaj"] != null && Session["mesajderecesi"] != null)
            {
                ViewBag.mesaj = Session["mesaj"];
                ViewBag.mesajderecesi = Session["mesajderecesi"];
                Session.Remove("mesaj");
                Session.Remove("mesajderecesi");
            }
            return View();
        }

        #region tur işlemleri

        [HttpGet]
        [admingiris]
        public ActionResult YTurlar(int id = 0)
        {
            ViewData["menuler"] = munularigetir();
            if (id != 0 && id > 0)
            {
                try
                {
                    Urunler tur = db.Urunler.Where(x => x.id == id).Take(1).FirstOrDefault();
                    if (tur != null)
                        return View(tur);
                    else
                    {

                        Session["mesaj"] = "Bu id'e ait bilgi bulunamadı.";
                        Session["mesajderecesi"] = 1;
                        return View();
                    }
                }
                catch
                {
                    Session["mesaj"] = "İstenmeyen bir hata oluştu.";
                    Session["mesajderecesi"] = 2;
                    return View();
                }
            }

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [admingiris]
        public ActionResult YTurlar(Urunler turlar, List<HttpPostedFileBase> resimler, string etiketler)
        {
            ViewData["menuler"] = munularigetir();
            if (turlar.baslik == null || turlar.fiyat == null || turlar.icerik == null || turlar.ing_baslik == null || turlar.ing_icerik == null)
            {
                Session["mesaj"] = "Eksik veya yanlış giriş yaptınız. Lütfen kontrol edip tekrar deneyiniz.";
                Session["mesajderecesi"] = 1;
                return View();
            }
            if (turlar.id > 0 && turlar.baslik.Trim().Length > 0 && turlar.ing_baslik.Trim().Length > 0 && turlar.icerik.Trim().Length > 0 && turlar.ing_icerik.Trim().Length > 0 && turlar.fiyat.Trim().Length > 0 && turlar.menu_ID > 0)
            {
                try
                {
                    if (resimler.Count() > 0 && resimler[0] != null)
                    {
                        if (Galeriturekle(resimler, turlar.id) == 0)
                        {
                            Session["mesaj"] = "Sistem kaynaklı bir hataya raslandı. Resim kaydedilemedi. Daha büyük veya başka format deneyin.";
                            Session["mesajderecesi"] = 2;
                            return View();
                        }
                        else
                        {
                            Urunler guncelle = db.Urunler.Where(x => x.id == turlar.id).Take(1).FirstOrDefault();
                            guncelle.baslik = turlar.baslik;
                            guncelle.fiyat = turlar.fiyat;
                            guncelle.icerik = turlar.icerik;
                            guncelle.ing_baslik = turlar.ing_baslik;
                            guncelle.ing_icerik = turlar.ing_icerik;
                            guncelle.menu_ID = turlar.menu_ID;
                            guncelle.tarih = DateTime.Now;
                            guncelle.urun_sinifi = turlar.urun_sinifi;
                            guncelle.talep_formu = turlar.talep_formu;
                            List<Etiketler> varolan = guncelle.Etiketler.ToList();
                            for (int i = 0; i < guncelle.Etiketler.Count; i++)
                                guncelle.Etiketler.Remove(varolan[i]);

                            string[] gelenetiketler = etiketler.Split(',');
                            var etiketkontrol = db.Etiketler.ToList();

                            for (int i = 0; i < gelenetiketler.Count(); i++)
                            {
                                if (etiketkontrol != null && etiketkontrol.Count > 0 && etiketkontrol.Where(x => x.etiket_adi == gelenetiketler[i]).Count() <= 0)
                                    guncelle.Etiketler.Add(new Etiketler() { etiket_adi = gelenetiketler[i] });
                                else if (etiketkontrol != null && etiketkontrol.Count > 0 && etiketkontrol.Where(x => x.etiket_adi == gelenetiketler[i]).Count() > 0)
                                    guncelle.Etiketler.Add(etiketkontrol.Where(x => x.etiket_adi == gelenetiketler[i]).Take(1).FirstOrDefault());
                            }
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        Urunler guncelle = db.Urunler.Where(x => x.id == turlar.id).Take(1).FirstOrDefault();
                        guncelle.baslik = turlar.baslik;
                        guncelle.fiyat = turlar.fiyat;
                        guncelle.icerik = turlar.icerik;
                        guncelle.ing_baslik = turlar.ing_baslik;
                        guncelle.ing_icerik = turlar.ing_icerik;
                        guncelle.menu_ID = turlar.menu_ID;
                        guncelle.tarih = DateTime.Now;
                        guncelle.urun_sinifi = turlar.urun_sinifi;
                        guncelle.talep_formu = turlar.talep_formu;
                        List<Etiketler> varolan = guncelle.Etiketler.ToList();
                        for (int i = 0; i < guncelle.Etiketler.Count; i++)
                            guncelle.Etiketler.Remove(varolan[i]);
                        string[] gelenetiketler = etiketler.Split(',');
                        var etiketkontrol = db.Etiketler.ToList();

                        for (int i = 0; i < gelenetiketler.Count(); i++)
                        {
                            if (etiketkontrol != null && etiketkontrol.Count > 0 && etiketkontrol.Where(x => x.etiket_adi == gelenetiketler[i]).Count() <= 0)
                                guncelle.Etiketler.Add(new Etiketler() { etiket_adi = gelenetiketler[i] });
                            else if (etiketkontrol != null && etiketkontrol.Count > 0 && etiketkontrol.Where(x => x.etiket_adi == gelenetiketler[i]).Count() > 0)
                                guncelle.Etiketler.Add(etiketkontrol.Where(x => x.etiket_adi == gelenetiketler[i]).Take(1).FirstOrDefault());
                        }
                        db.SaveChanges();
                    }
                }
                catch (Exception)
                {
                    Session["mesaj"] = "Sistem kaynaklı bir hataya raslandı daha sonra tekrar deneyin.";
                    Session["mesajderecesi"] = 2;
                    return View();
                }

                return RedirectToRoute("yturtarihleri", new { turid = turlar.id });
            }
            else if (turlar.baslik.Trim().Length > 0 && turlar.ing_baslik.Trim().Length > 0 && turlar.icerik.Trim().Length > 0 && turlar.ing_icerik.Trim().Length > 0 && turlar.fiyat.Trim().Length > 0 && resimler.Count() > 0 && turlar.menu_ID > 0 && resimler[0] != null)
            {
                try
                {

                    turlar.acik_kapali = false;
                    turlar.tarih = DateTime.Now;


                    db.Urunler.Add(turlar);
                    db.SaveChanges();
                    string[] gelenetiketler = etiketler.Split(',');
                    var etiketkontrol = db.Etiketler.ToList();

                    for (int i = 0; i < gelenetiketler.Count(); i++)
                    {
                        if (etiketkontrol != null && etiketkontrol.Count > 0 && etiketkontrol.Where(x => x.etiket_adi == gelenetiketler[i]).Count() <= 0)
                            turlar.Etiketler.Add(new Etiketler() { etiket_adi = gelenetiketler[i] });
                        else if (etiketkontrol != null && etiketkontrol.Count > 0 && etiketkontrol.Where(x => x.etiket_adi == gelenetiketler[i]).Count() > 0)
                            turlar.Etiketler.Add(etiketkontrol.Where(x => x.etiket_adi == gelenetiketler[i]).Take(1).FirstOrDefault());
                    }
                    if (Galeriturekle(resimler, turlar.id) == 0)
                    {
                        db.Urunler.Remove(turlar);
                        db.SaveChanges();
                        Session["mesaj"] = "Sistem kaynaklı bir hataya raslandı. Resim kaydedilemedi. Daha büyük veya başka format deneyin.";
                        Session["mesajderecesi"] = 2;
                        return View();
                    }
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    Session["mesaj"] = "Sistem kaynaklı bir hataya raslandı daha sonra tekrar deneyin.";
                    Session["mesajderecesi"] = 2;
                    return View();
                }
                return RedirectToRoute("yturtarihleri", new { turid = turlar.id });

            }
            else
            {
                Session["mesaj"] = "Eksik veya yanlış giriş yaptınız. Lütfen kontrol edip tekrar deneyiniz.";
                Session["mesajderecesi"] = 1;
                return View();
            }
        }
        [HttpGet]
        [admingiris]
        public ActionResult YTarihler(int turid)
        {
            ViewBag.turid = turid;
            Urunler urunler = db.Urunler.Where(x => x.id == turid).Take(1).FirstOrDefault();
            return View(urunler.Urun_Tarihleri.ToList());
        }
        [HttpPost]
        [admingiris]
        public ActionResult YTarihler(Urun_Tarihleri tarih, int turid, string parabirimi)
        {
            if (tarih != null && turid > 0 && tarih.cikis_tarihi != null && tarih.donus_tarihi != null && tarih.cikis_tarihi >= DateTime.Now.Date && tarih.donus_tarihi > DateTime.Now.Date)
            {
                try
                {
                    Urunler urun = db.Urunler.Where(x => x.id == turid).Take(1).FirstOrDefault();
                    ViewBag.urun_sinifi = urun.urun_sinifi;
                    if (urun.urun_sinifi == 1 || urun.urun_sinifi == 2 || urun.urun_sinifi == 3 || urun.urun_sinifi == 4 || urun.urun_sinifi == 5)
                    {
                        urun.acik_kapali = true;
                        urun.para_birimi = parabirimi;
                        tarih.urun_ID = turid;
                        db.Urun_Tarihleri.Add(tarih);
                        db.SaveChanges();

                        return RedirectToRoute("yanasayfa");
                    }
                    tarih.urun_ID = turid;
                    db.Urun_Tarihleri.Add(tarih);
                    db.SaveChanges();

                }
                catch (Exception)
                {
                    Session["mesaj"] = "Ekleme işleminiz başarısız";
                    Session["mesajderecesi"] = 2;
                }
            }
            else
            {
                Session["mesaj"] = "Eksik bilgi. Ekleme işleminiz başarısız";
                Session["mesajderecesi"] = 1;
            }
            ViewBag.turid = turid;
            return View(db.Urun_Tarihleri.Where(x => x.urun_ID == turid).ToList());
        }
        [admingiris]
        public ActionResult YTarihSil(int id)
        {
            int turid = 0;

            if (id > 0 && int.MaxValue > id)
            {
                var tarihler = db.Urun_Tarihleri.Where(x => x.id == id).Take(1).FirstOrDefault();
                
                turid = (int)tarihler.urun_ID;
                if (tarihler != null) db.Urun_Tarihleri.Remove(tarihler);
                db.SaveChanges();
                try
                {

                }
                catch (Exception)
                {
                    Session["mesaj"] = "Silme işleminiz başarısız";
                    Session["mesajderecesi"] = 2;
                }
            }

            return RedirectToRoute("yturtarihleri", new { turid = turid });
        }
        /*
        [HttpGet]
        [admingiris]
        public ActionResult YTurOtelEkle(int turid)
        {
            var konaklama_tarih = db.Urun_Konaklama_Tarih.Where(x => x.Urun_Tarihleri.urun_ID == turid && x.Urun_Konaklama_Bilgisi.urun_ID == turid).ToList();

            ViewData["tarihler"] = db.Urun_Tarihleri.Where(x => x.urun_ID == turid).ToList();
            return View(konaklama_tarih);
        }
        */
        //[HttpPost]
        //[admingiris]
        /*public ActionResult YTurOtelEkle(int turid, List<int> tarihidler, Urun_Konaklama_Bilgisi konaklama_bilgisi)
        {
            if (tarihidler != null && tarihidler.Count > 0 && konaklama_bilgisi.adi.Trim().Length > 0 && turid > 0)
            {
                Urun_Konaklama_Bilgisi kontrol = db.Urun_Konaklama_Bilgisi.Where(x => x.adi.ToUpper() == konaklama_bilgisi.adi.Trim().ToUpper()).Take(1).FirstOrDefault();
                if (kontrol == null)
                {
                    konaklama_bilgisi.adi = konaklama_bilgisi.adi.ToUpper();
                    konaklama_bilgisi.urun_ID = turid;
                    db.Urun_Konaklama_Bilgisi.Add(konaklama_bilgisi);
                    db.SaveChanges();
                    foreach (var item in tarihidler)
                    {
                        Urun_Konaklama_Tarih konaklama_tarih = new Urun_Konaklama_Tarih();
                        konaklama_tarih.konaklama_id = konaklama_bilgisi.id;
                        konaklama_tarih.tarih_id = item;
                        db.Urun_Konaklama_Tarih.Add(konaklama_tarih);
                    }
                    db.SaveChanges();
                }
            }
            else
            {
                Session["mesaj"] = "Eksik Bilgi Girişi Yaptınız";
                Session["mesajderecesi"] = 1;
            }

            var konaklama_tarih2 = db.Urun_Konaklama_Tarih.Where(x => x.Urun_Tarihleri.urun_ID == turid && x.Urun_Konaklama_Bilgisi.urun_ID == turid).ToList();
            ViewData["tarihler"] = db.Urun_Tarihleri.Where(x => x.urun_ID == turid).ToList();

            return View(konaklama_tarih2);
        }
        */
        /*
        [HttpPost]
        [admingiris]
        public ActionResult Yotelsil(List<int> konaklama_tarik_id, int turid)
        {
            if (konaklama_tarik_id.Count > 0)
            {
                foreach (var item in konaklama_tarik_id)
                    db.Urun_Konaklama_Tarih.Remove(db.Urun_Konaklama_Tarih.Where(x => x.id == item).Take(1).FirstOrDefault());
                db.SaveChanges();
            }
            return RedirectToRoute("yturotelekle", new { turid = turid });
        }
        */
        [HttpGet]
        [admingiris]
        public ActionResult YTumTurlar(int sayfa)
        {
            if (sayfa > 0)
            {
                try
                {
                    IEnumerable<Urunler> tumturlar = db.Urunler.Where(x => x.acik_kapali == true).ToList().ToPagedList(sayfa, 10);
                    if (tumturlar == null)
                    {
                        Session["mesaj"] = "Hiç bir kayıt bulunamadı.";
                        Session["mesajderecesi"] = 1;
                        return View(tumturlar);
                    }
                    return View(tumturlar);
                }
                catch (Exception)
                {
                    Session["mesaj"] = "İstenmeyen bir hata oluştu.";
                    Session["mesajderecesi"] = 2;
                    return View();
                }
            }
            else
            {
                Session["mesaj"] = "Urlde bir yanlışlık var";
                Session["mesajderecesi"] = 1;
                return View();
            }

        }
        [HttpPost]
        [admingiris]
        public ActionResult YTumTurlar()
        {
            return View();
        }

        [admingiris]
        public ActionResult YTurSil(int id)
        {
            if (id > 0)
            {
                try
                {
                    if (galeridensil(id, "turlar") == 1)
                    {
                        Session["mesaj"] = "Silme işlemi sırasında istenmeyen bir hata oluştu silme işlemi yarım kalmış olabilir.";
                        Session["mesajderecesi"] = 2;
                    }
                    else
                    {
                        Urunler kapat = db.Urunler.Where(x => x.id == id).Take(1).FirstOrDefault();
                        kapat.acik_kapali = false;
                        db.SaveChanges();
                        Session["mesaj"] = "İşleminiz başarılı";
                        Session["mesajderecesi"] = 0;
                    }
                }
                catch (Exception)
                {
                    Session["mesaj"] = "İstenmeyen bir hata oluştu.";
                    Session["mesajderecesi"] = 2;
                }
            }
            else
            {
                Session["mesaj"] = "Belirlenemeyen bir hata oluştu.";
                Session["mesajderecesi"] = 1;
            }
            return RedirectToAction("YTumTurlar", new { sayfa = 1 });
        }
        [admingiris]

        /// <summary>
        /// neyin idsi tur veya menu parametrelerini yazın
        /// </summary>
        /// <param name="id"></param>
        /// <param name="neyinidsi"></param>
        /// <returns></returns>
        private int tursil(int id, string neyinidsi)
        {
            if (id > 0 && neyinidsi != "")
            {
                try
                {
                    if (neyinidsi == "tur")
                    {
                        Urunler kapat = db.Urunler.Where(x => x.id == id).Take(1).FirstOrDefault();
                        kapat.acik_kapali = false;

                        if (galeridensil(kapat.id, "turlar") == 1)
                        {
                            db.SaveChanges();
                            Session["mesaj"] = "İşleminiz başarıyla gerçekleştirildi.";
                            Session["mesajderecesi"] = 0;
                        }
                        else
                        {
                            Session["mesaj"] = "İstenmeyen bir hata oluştu.";
                            Session["mesajderecesi"] = 2;
                            return 1;
                        }

                    }
                    else if (neyinidsi == "menu")
                    {
                        List<Urunler> toplusil = db.Urunler.Where(x => x.menu_ID == id).ToList();
                        foreach (var item in toplusil)
                        {

                            if (galeridensil(item.id, "turlar") == 1)
                            {
                                item.acik_kapali = false;

                                Session["mesaj"] = "İşleminiz başarıyla gerçekleştirildi.";
                                Session["mesajderecesi"] = 0;
                            }
                            else
                            {
                                Session["mesaj"] = "İstenmeyen bir hata oluştu.";
                                Session["mesajderecesi"] = 2;
                                return 1;
                            }

                        }
                        db.SaveChanges();
                    }
                }
                catch (Exception)
                {
                    Session["mesaj"] = "İstenmeyen bir hata oluştu.";
                    Session["mesajderecesi"] = 2;
                    return 1;
                }
            }
            else
            {
                Session["mesaj"] = "Belirlenemeyen bir hata oluştu.";
                Session["mesajderecesi"] = 1;
                return 1;
            }
            return 0;
        }

        [HttpGet]
        [admingiris]
        public ActionResult YIletisim_Bilgileri(int urun_id)
        {
            if (urun_id > 0)
            {
                List<Urun_Rezervasyon_Iletisim_Bilgileri> iletisim_bilgileri = db.Urun_Rezervasyon_Iletisim_Bilgileri.Where(x => x.urun_ID == urun_id).ToList();
                return View(iletisim_bilgileri);
            }
            return View();
        }
        [admingiris]
        public ActionResult YIletisim_Bilgileri_sil(int iletisim_bilgileri_id)
        {
            if (iletisim_bilgileri_id > 0)
            {
                Urun_Rezervasyon_Iletisim_Bilgileri iletisim_bilgileri = db.Urun_Rezervasyon_Iletisim_Bilgileri.Where(x => x.id == iletisim_bilgileri_id).Take(1).FirstOrDefault();
                iletisim_bilgileri.acik_kapali = false;
                db.SaveChanges();
                return RedirectToRoute("yuruniletisimbilgileri", new { urun_id = iletisim_bilgileri.urun_ID });
            }
            return RedirectToAction("yanasayfa");
        }
        [admingiris]
        public ActionResult YIletisim_Kisi_Bilgileri(int iletisim_id)
        {
            if (iletisim_id > 0)
            {
                return View(db.Urun_Rezervasyon_Kisi_Bilgileri.Where(x => x.iletisim_bilgileri_ID == iletisim_id).ToList());
            }
            return null;
        }

        public ActionResult YTalep_formlari(int urun_id)
        {
            if (urun_id > 0 && urun_id < int.MaxValue)
            {
                List<Urun_Talep_Formu> talep_formu = db.Urun_Talep_Formu.Where(x => x.urun_ID == urun_id).ToList();
                return View(talep_formu);
            }
            return View();

        }
        [admingiris]
        public void GridExportToExcel(int urun_id)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[8] {
                        new DataColumn("Adı"),
                        new DataColumn("Soyadı"),
                        new DataColumn("Doğum Tarihi"),
                        new DataColumn("Tc Kimlik No"),
                        new DataColumn("Pasaport No"),
                        new DataColumn("Adres"),
                        new DataColumn("Eposta"),
                        new DataColumn("Telefon No")

            });
            List<Urun_Rezervasyon_Iletisim_Bilgileri> iletisim_bilgileri = db.Urun_Rezervasyon_Iletisim_Bilgileri.Where(x => x.urun_ID == urun_id).ToList();
            if (iletisim_bilgileri.Count > 0)
            {
                foreach (var iletisim in iletisim_bilgileri)
                {
                    foreach (var kisi in db.Urun_Rezervasyon_Kisi_Bilgileri.Where(x => x.iletisim_bilgileri_ID == iletisim.id).ToList())
                        dt.Rows.Add(kisi.adi, kisi.soyadi, kisi.dogum_tarihi, kisi.tckimlik, kisi.pasaportno, iletisim.email, iletisim.tel, iletisim.adres);

                }
            }


            string dosyaAdi = db.Urunler.Where(x => x.id == urun_id).Select(y => y.baslik).Take(1).FirstOrDefault() + " Rezarvasyon Gerçekleştirenler";
            var grid = new GridView();
            grid.DataSource = dt;
            grid.DataBind();

            //Response.ClearContent();
            //Response.Charset = "utf-8";
            //Response.AddHeader("content-disposition", "attachment; filename=" + dosyaAdi + ".xls");

            //Response.ContentType = "application/excel";
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter htw = new HtmlTextWriter(sw);

            //grid.RenderControl(htw);

            //Response.Write(sw.ToString());
            //Response.End();



            Response.ClearContent();
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}.xls", dosyaAdi));
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Write(sw.ToString());
            Response.End();
        }
        #endregion

        #region menü işlemleri


        [HttpGet]
        [admingiris]
        public ActionResult YMenu()
        {
            try
            {
                return View(munularigetir());
            }
            catch (Exception)
            {
                Session["mesaj"] = "Bu id'e ait bilgi bulunamadı.";
                Session["mesajderecesi"] = 1;
                return RedirectToAction("YAnasayfa");
            }
        }

        private List<izindeturizm.Models.Menu> munularigetir()
        {
            List<izindeturizm.Models.Menu> menular = db.Menu.ToList();
            return menular;
        }
        [admingiris]
        [HttpPost]
        public ActionResult YMenu(izindeturizm.Models.Menu yenimenu)
        {
            if (yenimenu != null && yenimenu.adi.Trim().Length > 0 && yenimenu.id > 0)
            {
                try
                {
                    if (db.Menu.Where(x => x.adi == yenimenu.adi || x.ingadi == yenimenu.ingadi || x.ustid == yenimenu.id).Count() > 0)
                    {
                        Session["mesaj"] = "Ekleme işlmemi yapılamaz.Alt tarafta kendini gösteremez.";
                        Session["mesajderecesi"] = 1;
                    }
                    else
                    {
                        izindeturizm.Models.Menu guncelle = db.Menu.Where(x => x.id == yenimenu.id).Take(1).FirstOrDefault();
                        guncelle.adi = yenimenu.adi;
                        guncelle.ingadi = yenimenu.ingadi;
                        guncelle.ustid = yenimenu.ustid;
                        db.SaveChanges();
                        Session["mesaj"] = "Guncelleme işlmemi başarılı.";
                        Session["mesajderecesi"] = 0;
                    }

                }
                catch (Exception)
                {
                    Session["mesaj"] = "Guncelleme işlmemi yapılamadı.";
                    Session["mesajderecesi"] = 2;
                }
            }
            else if (yenimenu != null && yenimenu.adi.Trim().Length > 0)
            {

                try
                {
                    if (db.Menu.Where(x => x.adi == yenimenu.adi || x.ingadi == yenimenu.ingadi || x.ustid == yenimenu.id).Count() > 0)
                    {
                        Session["mesaj"] = "Ekleme işlmemi yapılamaz.Böyle bir kayıt zaten var";
                        Session["mesajderecesi"] = 1;
                    }
                    else
                    {

                        if (yenimenu.ustid == -1)
                        {
                            var kontrol = db.Menu.Where(x => x.ustid > 0).ToList();
                            if (kontrol.Count() > 7)
                            {
                                Session["mesaj"] = "Üst menu sınırını gectiniz. Daha fazla üst menu ekleyemesiniz. Alt menu ekleyin.";
                                Session["mesajderecesi"] = 1;
                            }
                            else
                                db.Menu.Add(yenimenu);
                        }
                        else
                            db.Menu.Add(yenimenu);
                        db.SaveChanges();
                    }

                }
                catch (Exception)
                {
                    Session["mesaj"] = "Ekleme işlmemi yapılamadı.";
                    Session["mesajderecesi"] = 2;
                }
            }
            else
            {
                Session["mesaj"] = "Eksik bilgi girişi yaptınız.";
                Session["mesajderecesi"] = 2;
            }


            return View(munularigetir());
        }
        [admingiris]
        public ActionResult YMenuSil(int id)
        {
            try
            {
                if (id > 0)
                {
                    List<izindeturizm.Models.Menu> sil = db.Menu.Where(x => x.id == id || x.ustid == id).ToList();

                    foreach (var item in sil)
                    {
                        db.Menu.Remove(item);
                        if (tursil(item.id, "menu") == 1)
                        {
                            Session["mesaj"] = "Silme işlemi sırasında istenmeyen bir hata oluştu silme işlemi yarım kalmış olabilir.";
                            Session["mesajderecesi"] = 2;
                        }
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                Session["mesaj"] = "Silme işlemi başarısız.";
                Session["mesajderecesi"] = 2;
            }
            return RedirectToAction("YMenu");
        }
        #endregion

        #region Transfer işlemleri
        [admingiris]
        public ActionResult Ytumtransferler(int sayfa)
        {
            ViewData["tkalkis"] = kalkisgetir();
            ViewData["tvaris"] = varisgetir();
            IEnumerable<Transfer> transfer = db.Transfer.ToList().ToPagedList(sayfa, 20);
            return View(transfer);
        }
        [admingiris]
        public ActionResult YTransfersil(int id)
        {
            if (id > 0)
            {
                try
                {
                    Transfer sil = db.Transfer.Where(x => x.id == id).Take(1).FirstOrDefault();
                    if (sil != null)
                    {
                        db.Transfer.Remove(sil);
                        db.SaveChanges();
                    }
                }
                catch (Exception)
                {

                }
            }
            return RedirectToRoute("ytumtransferler", new { sayfa = 1 });
        }
        [admingiris]
        public ActionResult YTDurum(int id, string durum)
        {
            if (id > 0 && durum == "acik")
            {
                try
                {
                    Transfer guncelle = db.Transfer.Where(x => x.id == id).Take(1).FirstOrDefault();
                    if (guncelle != null)
                    {
                        guncelle.durum = "acik";
                        db.SaveChanges();
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else if (id > 0 && durum == "beklemede")
            {
                try
                {
                    Transfer guncelle = db.Transfer.Where(x => x.id == id).Take(1).FirstOrDefault();
                    if (guncelle != null)
                    {
                        guncelle.durum = "beklemede";
                        db.SaveChanges();
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else if (id > 0 && durum == "kapalı")
            {
                try
                {
                    Transfer guncelle = db.Transfer.Where(x => x.id == id).Take(1).FirstOrDefault();
                    if (guncelle != null)
                    {
                        guncelle.durum = "kapalı";
                        db.SaveChanges();
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return RedirectToRoute("ytumtransferler", new { sayfa = 1 });
        }
        [HttpGet]
        [admingiris]
        public ActionResult YTransfer()
        {
            ViewData["kalkis"] = kalkisgetir();
            ViewData["varis"] = varisgetir();
            ViewData["fiyat"] = TFiyatgetir();
            return View();
        }
        [admingiris]
        private List<TKalkis> kalkisgetir()
        {
            try
            {
                return db.TKalkis.ToList();
            }
            catch (Exception)
            {
                Session["mesaj"] = "istenmeyen bir hata oluştu.";
                Session["mesajderecesi"] = 2;
                return null;
            }

        }
        [admingiris]
        private List<TVaris> varisgetir()
        {
            try
            {
                return db.TVaris.ToList();
            }
            catch (Exception)
            {
                Session["mesaj"] = "istenmeyen bir hata oluştu.";
                Session["mesajderecesi"] = 2;
                return null;
            }
        }
        [admingiris]
        private List<TFiyat> TFiyatgetir()
        {
            try
            {
                return db.TFiyat.ToList();
            }
            catch (Exception)
            {
                Session["mesaj"] = "istenmeyen bir hata oluştu.";
                Session["mesajderecesi"] = 2;
                return null;
            }
        }
        [admingiris]
        public ActionResult YFiyat(TFiyat yenifiyat)
        {
            if (yenifiyat != null && yenifiyat.id == 0 && yenifiyat.kalkisID > 0 && yenifiyat.varisID > 0 && yenifiyat.fiyat.Length > 0)
            {
                try
                {
                    db.Database.ExecuteSqlCommand(
                        "insert into TFiyat (kalkisID,varisID,fiyat) values(@param1,@param2,@param3)",
                        new[] {
                        new SqlParameter("param1", yenifiyat.kalkisID),
                        new SqlParameter("param2",yenifiyat.varisID),
                        new SqlParameter("param3",yenifiyat.fiyat)
                    }
                        );
                    //db.TFiyat.Add(yenifiyat);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    Session["mesaj"] = "Kaydedilemedi.";
                    Session["mesajderecesi"] = 2;
                }
                return RedirectToAction("YTransfer");
            }
            else if (yenifiyat != null && yenifiyat.id > 0 && yenifiyat.kalkisID > 0 && yenifiyat.varisID > 0 && yenifiyat.fiyat.Length > 0)
            {
                try
                {
                    TFiyat fiyat = db.TFiyat.Where(x => x.id == yenifiyat.id).Take(1).FirstOrDefault();
                    if (db.TFiyat.Where(x => x.kalkisID == yenifiyat.kalkisID && x.varisID == yenifiyat.varisID).Count() > 0)
                    {
                        Session["mesaj"] = "Böyle bir kayıt zaten var.";
                        Session["mesajderecesi"] = 1;
                        return RedirectToAction("YTransfer");
                    }
                    else
                    {
                        fiyat.fiyat = yenifiyat.fiyat;
                        fiyat.kalkisID = yenifiyat.kalkisID;
                        fiyat.varisID = yenifiyat.varisID;
                        db.SaveChanges();
                    }
                    return RedirectToAction("YTransfer");
                }
                catch (Exception)
                {
                    Session["mesaj"] = "Güncellleştirilemedi.";
                    Session["mesajderecesi"] = 2;
                    return RedirectToAction("YTransfer");
                }
            }
            else
            {
                Session["mesaj"] = "Eksik bilgi girdiniz.";
                Session["mesajderecesi"] = 1;
                return RedirectToAction("YTransfer");

            }
        }
        [admingiris]
        public ActionResult YFiyatSil(int id)
        {
            if (id > 0)
            {
                TFiyat sil = db.TFiyat.Where(x => x.id == id).Take(1).FirstOrDefault();
                if (sil != null)
                {
                    db.TFiyat.Remove(sil);
                    db.SaveChanges();
                    Session["mesaj"] = "İşlem başarılı.";
                    Session["mesajderecesi"] = 0;
                    return RedirectToAction("YTransfer");
                }
                else
                {
                    Session["mesaj"] = "Böyle bir kayıt bulunamadı.";
                    Session["mesajderecesi"] = 1;
                    return RedirectToAction("YTransfer");
                }
            }
            else
            {
                Session["mesaj"] = "Bilinmeyen bir hata oluştu.";
                Session["mesajderecesi"] = 2;
                return RedirectToAction("YTransfer");
            }
        }
        [admingiris]
        public ActionResult YVarisSil(int id)
        {
            if (id > 0)
            {
                TVaris sil = db.TVaris.Where(x => x.id == id).Take(1).FirstOrDefault();
                List<TFiyat> sil2 = db.TFiyat.Where(x => x.varisID == id).ToList();
                if (sil != null)
                {
                    db.TVaris.Remove(sil);
                    foreach (var item in sil2)
                    {
                        db.TFiyat.Remove(item);
                    }
                    db.SaveChanges();
                    Session["mesaj"] = "İşlem başarılı.";
                    Session["mesajderecesi"] = 0;
                    return RedirectToAction("YTransfer");
                }
                else
                {
                    Session["mesaj"] = "Böyle bir kayıt bulunamadı.";
                    Session["mesajderecesi"] = 1;
                    return RedirectToAction("YTransfer");
                }
            }
            else
            {
                Session["mesaj"] = "Bilinmeyen bir hata oluştu.";
                Session["mesajderecesi"] = 2;
                return RedirectToAction("YTransfer");
            }
        }
        [admingiris]
        public ActionResult YVaris(TVaris yenivaris)
        {
            if (yenivaris != null && yenivaris.id == 0 && yenivaris.varisyer.Trim().Length > 0)
            {
                try
                {
                    if (db.TVaris.Where(x => x.varisyer == yenivaris.varisyer).Count() > 0)
                    {
                        Session["mesaj"] = "Böyle bir kayıt zaten var.";
                        Session["mesajderecesi"] = 1;
                        return RedirectToAction("YTransfer");
                    }
                    else
                    {
                        db.TVaris.Add(yenivaris);
                        db.SaveChanges();
                    }
                }
                catch (Exception)
                {
                    Session["mesaj"] = "Kaydedilemedi.";
                    Session["mesajderecesi"] = 2;
                }
                return RedirectToAction("YTransfer");
            }
            else if (yenivaris != null && yenivaris.id > 0 && yenivaris.varisyer.Trim().Length > 0)
            {
                try
                {
                    TVaris varis = db.TVaris.Where(x => x.id == yenivaris.id).Take(1).FirstOrDefault();
                    if (db.TVaris.Where(x => x.varisyer == yenivaris.varisyer).Count() > 0)
                    {
                        Session["mesaj"] = "Böyle bir kayıt zaten var.";
                        Session["mesajderecesi"] = 1;
                        return RedirectToAction("YTransfer");
                    }
                    else
                    {
                        varis.varisyer = yenivaris.varisyer;
                        db.SaveChanges();
                    }
                    return RedirectToAction("YTransfer");
                }
                catch (Exception)
                {
                    Session["mesaj"] = "Güncellleştirilemedi.";
                    Session["mesajderecesi"] = 2;
                    return RedirectToAction("YTransfer");
                }
            }
            else
            {
                Session["mesaj"] = "Eksik bilgi girdiniz.";
                Session["mesajderecesi"] = 1;
                return RedirectToAction("YTransfer");

            }
        }
        [admingiris]
        public ActionResult YKalkisSil(int id)
        {
            if (id > 0)
            {
                TKalkis sil = db.TKalkis.Where(x => x.id == id).Take(1).FirstOrDefault();
                List<TFiyat> sil2 = db.TFiyat.Where(x => x.kalkisID == id).ToList();
                if (sil != null)
                {
                    db.TKalkis.Remove(sil);
                    foreach (var item in sil2)
                    {
                        db.TFiyat.Remove(item);
                    }
                    db.SaveChanges();
                    Session["mesaj"] = "İşlem başarılı.";
                    Session["mesajderecesi"] = 0;
                    return RedirectToAction("YTransfer");
                }
                else
                {
                    Session["mesaj"] = "Böyle bir kayıt bulunamadı.";
                    Session["mesajderecesi"] = 1;
                    return RedirectToAction("YTransfer");
                }
            }
            else
            {
                Session["mesaj"] = "Bilinmeyen bir hata oluştu.";
                Session["mesajderecesi"] = 2;
                return RedirectToAction("YTransfer");
            }
        }
        [admingiris]
        public ActionResult YKalkis(TKalkis yenikalkis)
        {
            if (yenikalkis != null && yenikalkis.id == 0 && yenikalkis.kalkisyer.Trim().Length > 0)
            {
                try
                {
                    if (db.TKalkis.Where(x => x.kalkisyer == yenikalkis.kalkisyer).Count() > 0)
                    {
                        Session["mesaj"] = "Böyle bir kayıt zaten var.";
                        Session["mesajderecesi"] = 1;
                        return RedirectToAction("YTransfer");
                    }
                    else
                    {
                        db.TKalkis.Add(yenikalkis);
                        db.SaveChanges();
                    }
                }
                catch (Exception)
                {
                    Session["mesaj"] = "Kaydedilemedi.";
                    Session["mesajderecesi"] = 2;
                }
                return RedirectToAction("YTransfer");
            }
            else if (yenikalkis != null && yenikalkis.id > 0 && yenikalkis.kalkisyer.Trim().Length > 0)
            {
                try
                {
                    TKalkis kalkis = db.TKalkis.Where(x => x.id == yenikalkis.id).Take(1).FirstOrDefault();
                    if (db.TKalkis.Where(x => x.kalkisyer == yenikalkis.kalkisyer).Count() > 0)
                    {
                        Session["mesaj"] = "Böyle bir kayıt zaten var.";
                        Session["mesajderecesi"] = 1;
                        return RedirectToAction("YTransfer");
                    }
                    else
                    {
                        kalkis.kalkisyer = yenikalkis.kalkisyer;
                        db.SaveChanges();
                    }
                    return RedirectToAction("YTransfer");
                }
                catch (Exception)
                {
                    Session["mesaj"] = "Güncellleştirilemedi.";
                    Session["mesajderecesi"] = 2;
                    return RedirectToAction("YTransfer");
                }
            }
            else
            {
                Session["mesaj"] = "Eksik bilgi girdiniz.";
                Session["mesajderecesi"] = 1;
                return RedirectToAction("YTransfer");

            }
        }


        #endregion

        #region galeri
        [HttpGet]
        [admingiris]
        public ActionResult galeri(int sayfa)
        {
            return View(db.Galeri.Where(x => x.nereicin == "galeri").ToList().ToPagedList(sayfa, 10));
        }
        [HttpPost]
        [admingiris]
        public ActionResult galeri(Galeri yeni, HttpPostedFileBase resim)
        {
            if (Galeritekresimekle(resim, yeni) == 1)
            {
                Session["mesaj"] = "İşleminiz başarılı bir şekilde gercekleştirildi";
                Session["mesajderecesi"] = 0;
            }
            else
            {
                Session["mesaj"] = "İşleminiz gercekleştirilemedi";
                Session["mesajderecesi"] = 2;
            }

            return RedirectToRoute("ygaleri", new { sayfa = 1 });
        }
        [admingiris]
        public ActionResult galerisil(int id)
        {
            if (id > 0 && id < int.MaxValue)
            {
                try
                {
                    Galeri sil = db.Galeri.Where(x => x.id == id && x.nereicin == "galeri").Take(1).FirstOrDefault();
                    if (sil != null)
                    {
                        if (System.IO.File.Exists(Server.MapPath(sil.resimyol.Replace("https://izindeturizm.com", "~/"))))
                        {
                            System.IO.File.Delete(Server.MapPath(sil.resimyol.Replace("https://izindeturizm.com", "~/").Replace("\\", "/")));
                            db.Galeri.Remove(sil);
                            db.SaveChanges();
                            Session["mesaj"] = "İşleminiz başarılı";
                            Session["mesajderecesi"] = 0;
                        }
                        else
                        {
                            Session["mesaj"] = "Resim bulunamadı";
                            Session["mesajderecesi"] = 2;
                        }

                    }
                    else
                    {
                        Session["mesaj"] = "Veri bulunamadı";
                        Session["mesajderecesi"] = 2;
                    }
                }
                catch (Exception)
                {
                    Session["mesaj"] = "Bilinmeyen bir hata ile karşılaşıldı";
                    Session["mesajderecesi"] = 2;
                }
            }
            else
            {
                Session["mesaj"] = "Eksik bilgi geliyor" + id;
                Session["mesajderecesi"] = 2;
            }

            return RedirectToRoute("ygaleri", new { sayfa = 1 });
        }

        #endregion

        #region Iletisim
        [HttpPost]
        [admingiris]
        public ActionResult YMesaj_Goruldu(int iletisim_id)
        {
            if (iletisim_id > 0)
            {
                try
                {
                    Iletisim goruldu = db.Iletisim.Where(x => x.id == iletisim_id).Take(1).FirstOrDefault();
                    goruldu.goruldu = true;
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    Session["mesaj"] = "İşlem Başarısız";
                    Session["mesajderecesi"] = 0;
                }

            }
            else
            {
                Session["mesaj"] = "İşlem Başarısız";
                Session["mesajderecesi"] = 0;
            }

            return RedirectToRoute("yanasayfa");
        }

        [admingiris]
        public ActionResult YMesaj_Gonder(string mesaj, string emailsifresi, string kime)
        {
            iletisim_islemleri emailgonder = new iletisim_islemleri();
            Admin admin = (Admin)Session["admin"];
            bool cevap = emailgonder.Email_Gonder(mesaj, admin.email, emailsifresi, kime);
            if (cevap == true)
            {
                Session["mesaj"] = "Mesaj İletildi";
                Session["mesajderecesi"] = 0;
                return RedirectToRoute("yanasayfa");
            }
            else
            {
                Session["mesaj"] = "Mesaj İletimi Başarısız";
                Session["mesajderecesi"] = 2;
                return RedirectToRoute("yanasayfa");
            }
        }
        #endregion

        public int Galeriturekle(List<HttpPostedFileBase> resimler, int id)
        {
            if (resimler.Count() > 0)
            {
                Urunler urun = db.Urunler.Where(x => x.id == id).Take(1).FirstOrDefault();
                foreach (var item in resimler)
                {
                    try
                    {

                        WebImage resim = new WebImage(item.InputStream);
                        string DosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
                        string uzanti = Path.GetExtension(item.FileName);


                        resim.FileName = DosyaAdi + ".jpg";
                        resim.Save(Server.MapPath("/content/img/turlar/") + DosyaAdi + ".jpg");


                        Galeri yeniresim = new Galeri();
                        yeniresim.nereicin = "turlar";
                        yeniresim.resimyol = "https://izindeturizm.com/content/img/turlar/" + DosyaAdi + ".jpg";
                        yeniresim.anaID = id;
                        urun.Galeri.Add(yeniresim);
                        db.SaveChanges();
                    }
                    catch
                    {
                        return 0;
                    }
                }
            }
            else
                return 0;
            return 1;

        }
        public int Galeritekresimekle(HttpPostedFileBase resim, Galeri yeni)
        {
            if (resim != null)
            {
                try
                {

                    WebImage resimduzelt = new WebImage(resim.InputStream);
                    string DosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
                    string uzanti = Path.GetExtension(resim.FileName);


                    resimduzelt.FileName = DosyaAdi + ".jpg";
                    resimduzelt.Save(Server.MapPath("~/content/img/turlar/") + DosyaAdi + ".jpg");

                    yeni.nereicin = "galeri";
                    
                    yeni.resimyol = "https://izindeturizm.com/content/img/turlar/" + DosyaAdi + ".jpg";
                    db.Galeri.Add(yeni);
                    db.SaveChanges();
                    return 1;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            else
                return 0;



        }

        public int galeridensil(int anaid, string nereicin)
        {

            try
            {
                List<Galeri> silinecekler = db.Galeri.Where(x => x.anaID == anaid && x.nereicin == nereicin).ToList();

                foreach (var item in silinecekler)
                {

                    if (System.IO.File.Exists(Server.MapPath(item.resimyol.Replace("https://izindeturizm.com", "~/"))))
                    {
                        System.IO.File.Delete(Server.MapPath(item.resimyol.Replace("https://izindeturizm.com", "~/")));
                        db.Galeri.Remove(item);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                return 1;
            }

            return 0;
        }

        
    }
}