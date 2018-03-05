namespace izindeturizm.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("izindetu_izinde.Urunler")]
    public partial class Urunler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Urunler()
        {
            Galeri = new HashSet<Galeri>();
            Urun_PNR_Kodu = new HashSet<Urun_PNR_Kodu>();
            Urun_Rezervasyon_Iletisim_Bilgileri = new HashSet<Urun_Rezervasyon_Iletisim_Bilgileri>();
            Urun_Talep_Formu = new HashSet<Urun_Talep_Formu>();
            Urun_Tarihleri = new HashSet<Urun_Tarihleri>();
            Etiketler = new HashSet<Etiketler>();
        }

        public int id { get; set; }

        [StringLength(150)]
        public string baslik { get; set; }

        [StringLength(150)]
        public string ing_baslik { get; set; }

        [Column(TypeName = "text")]
        public string icerik { get; set; }

        [Column(TypeName = "text")]
        public string ing_icerik { get; set; }

        public DateTime? tarih { get; set; }

        [StringLength(10)]
        public string para_birimi { get; set; }

        [StringLength(50)]
        public string fiyat { get; set; }

        [StringLength(20)]
        public string kacgun_kacgece { get; set; }

        public int? menu_ID { get; set; }

        public bool? talep_formu { get; set; }

        public bool? acik_kapali { get; set; }

        public byte? urun_sinifi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Galeri> Galeri { get; set; }

        public virtual Urun_Firsat Urun_Firsat { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Urun_PNR_Kodu> Urun_PNR_Kodu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Urun_Rezervasyon_Iletisim_Bilgileri> Urun_Rezervasyon_Iletisim_Bilgileri { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Urun_Talep_Formu> Urun_Talep_Formu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Urun_Tarihleri> Urun_Tarihleri { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Etiketler> Etiketler { get; set; }
    }
}
