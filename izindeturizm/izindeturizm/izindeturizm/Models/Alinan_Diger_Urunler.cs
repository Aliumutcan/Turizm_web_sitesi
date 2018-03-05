namespace izindeturizm.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("izindetu_izinde.Alinan_Diger_Urunler")]
    public partial class Alinan_Diger_Urunler
    {
        public int id { get; set; }

        public int? iletisim_bilgileri_ID { get; set; }

        public DateTime? gidis { get; set; }

        public DateTime? donus { get; set; }

        [Column(TypeName = "money")]
        public decimal? odenen { get; set; }

        [StringLength(50)]
        public string hangi_tur_servisi { get; set; }
    }
}
