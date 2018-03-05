namespace izindeturizm.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("izindetu_izinde.Menu")]
    public partial class Menu
    {
        public int id { get; set; }

        [StringLength(50)]
        public string adi { get; set; }

        [StringLength(50)]
        public string ingadi { get; set; }

        public int? ustid { get; set; }
    }
}
