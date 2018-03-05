namespace izindeturizm.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("izindetu_izinde.Urun_Tarihleri")]
    public partial class Urun_Tarihleri
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Urun_Tarihleri()
        {
            Urun_Rezervasyon_Iletisim_Bilgileri = new HashSet<Urun_Rezervasyon_Iletisim_Bilgileri>();
        }

        public int id { get; set; }

        public DateTime? cikis_tarihi { get; set; }

        public DateTime? donus_tarihi { get; set; }

        public int? urun_ID { get; set; }

        [StringLength(10)]
        public string bebekfiyat { get; set; }

        [StringLength(5)]
        public string bebekyas1 { get; set; }

        [StringLength(5)]
        public string bebekyas2 { get; set; }

        [StringLength(10)]
        public string cocukfiyat { get; set; }

        [StringLength(5)]
        public string cocukyas1 { get; set; }

        [StringLength(5)]
        public string cocukyas2 { get; set; }

        [StringLength(10)]
        public string turfiyat { get; set; }

        public bool? acik_kapali { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Urun_Rezervasyon_Iletisim_Bilgileri> Urun_Rezervasyon_Iletisim_Bilgileri { get; set; }

        public virtual Urunler Urunler { get; set; }
    }
}
