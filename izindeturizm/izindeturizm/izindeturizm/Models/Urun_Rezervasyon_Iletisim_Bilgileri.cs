namespace izindeturizm.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("izindetu_izinde.Urun_Rezervasyon_Iletisim_Bilgileri")]
    public partial class Urun_Rezervasyon_Iletisim_Bilgileri
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Urun_Rezervasyon_Iletisim_Bilgileri()
        {
            Urun_Rezervasyon_Kisi_Bilgileri = new HashSet<Urun_Rezervasyon_Kisi_Bilgileri>();
        }

        public int id { get; set; }

        [StringLength(20)]
        public string tel { get; set; }

        [StringLength(100)]
        public string email { get; set; }

        [StringLength(300)]
        public string adres { get; set; }

        [StringLength(300)]
        public string istek_notu { get; set; }

        public bool? odendimi { get; set; }

        public DateTime? tarih { get; set; }

        public int? oda_sayisi { get; set; }

        public DateTime? giris { get; set; }

        public DateTime? cikis { get; set; }

        public DateTime? giris_saati { get; set; }

        public DateTime? cikis_saati { get; set; }

        public bool? acik_kapali { get; set; }

        public int? urun_ID { get; set; }

        public int? konaklama_tarih_ID { get; set; }

        public int? urun_tarih_ID { get; set; }

        public virtual Urun_Tarihleri Urun_Tarihleri { get; set; }

        public virtual Urunler Urunler { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Urun_Rezervasyon_Kisi_Bilgileri> Urun_Rezervasyon_Kisi_Bilgileri { get; set; }
    }
}
