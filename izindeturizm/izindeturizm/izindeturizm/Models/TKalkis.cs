namespace izindeturizm.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("izindetu_izinde.TKalkis")]
    public partial class TKalkis
    {
        public int id { get; set; }

        [StringLength(100)]
        public string kalkisyer { get; set; }

        public virtual TFiyat TFiyat { get; set; }
    }
}
