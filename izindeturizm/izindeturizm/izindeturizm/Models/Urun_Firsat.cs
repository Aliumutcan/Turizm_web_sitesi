namespace izindeturizm.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("izindetu_izinde.Urun_Firsat")]
    public partial class Urun_Firsat
    {
        public int id { get; set; }

        public int? urun_ID { get; set; }

        [StringLength(10)]
        public string indirim_turu { get; set; }

        public int? indirim_miktari { get; set; }

        public virtual Urunler Urunler { get; set; }
    }
}
