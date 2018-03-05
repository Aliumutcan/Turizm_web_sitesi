namespace izindeturizm.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("izindetu_izinde.TFiyat")]
    public partial class TFiyat
    {
        public int id { get; set; }

        public int? kalkisID { get; set; }

        public int? varisID { get; set; }

        [StringLength(10)]
        public string fiyat { get; set; }

        public virtual TKalkis TKalkis { get; set; }

        public virtual TVaris TVaris { get; set; }
    }
}
