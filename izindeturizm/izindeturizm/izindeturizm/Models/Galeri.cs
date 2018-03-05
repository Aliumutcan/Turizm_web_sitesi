namespace izindeturizm.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("izindetu_izinde.Galeri")]
    public partial class Galeri
    {
        public int id { get; set; }

        [StringLength(150)]
        public string resimyol { get; set; }

        [StringLength(150)]
        public string link { get; set; }

        [StringLength(200)]
        public string aciklama { get; set; }

        [StringLength(50)]
        public string nereicin { get; set; }

        public int? anaID { get; set; }

        public int? Urunler_id { get; set; }

        public virtual Urunler Urunler { get; set; }
    }
}
