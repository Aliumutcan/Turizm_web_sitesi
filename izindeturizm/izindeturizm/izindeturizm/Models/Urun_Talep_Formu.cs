namespace izindeturizm.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("izindetu_izinde.Urun_Talep_Formu")]
    public partial class Urun_Talep_Formu
    {
        public int id { get; set; }

        [StringLength(100)]
        public string adsoyad { get; set; }

        [StringLength(100)]
        public string eposta { get; set; }

        [StringLength(20)]
        public string tel { get; set; }

        [StringLength(300)]
        public string istek_notu { get; set; }

        public int? urun_ID { get; set; }

        public virtual Urunler Urunler { get; set; }
    }
}
