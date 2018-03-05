namespace izindeturizm.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("izindetu_izinde.Urun_PNR_Kodu")]
    public partial class Urun_PNR_Kodu
    {
        public int id { get; set; }

        public int? urun_ID { get; set; }

        [StringLength(10)]
        public string kod { get; set; }

        public virtual Urunler Urunler { get; set; }
    }
}
