namespace izindeturizm.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("izindetu_izinde.Iletisim")]
    public partial class Iletisim
    {
        public int id { get; set; }

        [StringLength(100)]
        public string adisoyadi { get; set; }

        [StringLength(100)]
        public string email { get; set; }

        [StringLength(11)]
        public string tel { get; set; }

        [StringLength(500)]
        public string mesaj { get; set; }

        public DateTime? tarih { get; set; }

        public bool goruldu { get; set; }
    }
}
