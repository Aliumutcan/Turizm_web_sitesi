namespace izindeturizm.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("izindetu_izinde.Admin")]
    public partial class Admin
    {
        public int id { get; set; }

        [StringLength(100)]
        public string kullaniciadi { get; set; }

        [StringLength(500)]
        public string sifre { get; set; }

        [StringLength(200)]
        public string adsoyad { get; set; }

        [StringLength(300)]
        public string adres { get; set; }

        [StringLength(100)]
        public string email { get; set; }

        [Column(TypeName = "text")]
        public string Hakkimizda { get; set; }

        [StringLength(20)]
        public string cep_tel { get; set; }

        [StringLength(20)]
        public string is_tel { get; set; }
    }
}
