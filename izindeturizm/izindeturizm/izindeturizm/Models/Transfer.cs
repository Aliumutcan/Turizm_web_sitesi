namespace izindeturizm.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("izindetu_izinde.Transfer")]
    public partial class Transfer
    {
        public int id { get; set; }

        [StringLength(100)]
        public string adsoyad { get; set; }

        public int? nerden { get; set; }

        public int? nereye { get; set; }

        [StringLength(500)]
        public string tamadres { get; set; }

        [StringLength(15)]
        public string tel { get; set; }

        [StringLength(30)]
        public string durum { get; set; }

        [Column(TypeName = "date")]
        public DateTime? tarih { get; set; }

        [StringLength(100)]
        public string t_not { get; set; }
    }
}
