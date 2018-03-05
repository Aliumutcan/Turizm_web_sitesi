namespace izindeturizm.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("izindetu_izinde.Urun_Rezervasyon_Kisi_Bilgileri")]
    public partial class Urun_Rezervasyon_Kisi_Bilgileri
    {
        public int id { get; set; }

        [StringLength(50)]
        public string adi { get; set; }

        [StringLength(50)]
        public string soyadi { get; set; }

        public DateTime? dogum_tarihi { get; set; }

        [StringLength(11)]
        public string tckimlik { get; set; }

        public bool? cinsiyet { get; set; }

        [StringLength(10)]
        public string pasaportno { get; set; }

        public int? iletisim_bilgileri_ID { get; set; }

        public virtual Urun_Rezervasyon_Iletisim_Bilgileri Urun_Rezervasyon_Iletisim_Bilgileri { get; set; }
    }
}
