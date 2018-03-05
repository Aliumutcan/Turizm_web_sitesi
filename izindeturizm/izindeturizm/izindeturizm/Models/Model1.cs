namespace izindeturizm.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model13")
        {
        }

        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<Alinan_Diger_Urunler> Alinan_Diger_Urunler { get; set; }
        public virtual DbSet<Etiketler> Etiketler { get; set; }
        public virtual DbSet<Galeri> Galeri { get; set; }
        public virtual DbSet<Iletisim> Iletisim { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<TFiyat> TFiyat { get; set; }
        public virtual DbSet<TKalkis> TKalkis { get; set; }
        public virtual DbSet<Transfer> Transfer { get; set; }
        public virtual DbSet<TVaris> TVaris { get; set; }
        public virtual DbSet<Urun_Firsat> Urun_Firsat { get; set; }
        public virtual DbSet<Urun_PNR_Kodu> Urun_PNR_Kodu { get; set; }
        public virtual DbSet<Urun_Rezervasyon_Iletisim_Bilgileri> Urun_Rezervasyon_Iletisim_Bilgileri { get; set; }
        public virtual DbSet<Urun_Rezervasyon_Kisi_Bilgileri> Urun_Rezervasyon_Kisi_Bilgileri { get; set; }
        public virtual DbSet<Urun_Talep_Formu> Urun_Talep_Formu { get; set; }
        public virtual DbSet<Urun_Tarihleri> Urun_Tarihleri { get; set; }
        public virtual DbSet<Urunler> Urunler { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>()
                .Property(e => e.Hakkimizda)
                .IsUnicode(false);

            modelBuilder.Entity<Alinan_Diger_Urunler>()
                .Property(e => e.odenen)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Etiketler>()
                .HasMany(e => e.Urunler)
                .WithMany(e => e.Etiketler)
                .Map(m => m.ToTable("Etiket_Urun", "izindetu_izinde").MapLeftKey("etiket_id").MapRightKey("urun_id"));

            modelBuilder.Entity<TKalkis>()
                .HasOptional(e => e.TFiyat)
                .WithRequired(e => e.TKalkis);

            modelBuilder.Entity<TVaris>()
                .HasMany(e => e.TFiyat)
                .WithOptional(e => e.TVaris)
                .HasForeignKey(e => e.varisID);

            modelBuilder.Entity<Urun_Rezervasyon_Iletisim_Bilgileri>()
                .HasMany(e => e.Urun_Rezervasyon_Kisi_Bilgileri)
                .WithOptional(e => e.Urun_Rezervasyon_Iletisim_Bilgileri)
                .HasForeignKey(e => e.iletisim_bilgileri_ID);

            modelBuilder.Entity<Urun_Tarihleri>()
                .HasMany(e => e.Urun_Rezervasyon_Iletisim_Bilgileri)
                .WithOptional(e => e.Urun_Tarihleri)
                .HasForeignKey(e => e.urun_tarih_ID);

            modelBuilder.Entity<Urunler>()
                .Property(e => e.icerik)
                .IsUnicode(false);

            modelBuilder.Entity<Urunler>()
                .Property(e => e.ing_icerik)
                .IsUnicode(false);

            modelBuilder.Entity<Urunler>()
                .HasMany(e => e.Galeri)
                .WithOptional(e => e.Urunler)
                .HasForeignKey(e => e.Urunler_id);

            modelBuilder.Entity<Urunler>()
                .HasOptional(e => e.Urun_Firsat)
                .WithRequired(e => e.Urunler);

            modelBuilder.Entity<Urunler>()
                .HasMany(e => e.Urun_PNR_Kodu)
                .WithOptional(e => e.Urunler)
                .HasForeignKey(e => e.urun_ID);

            modelBuilder.Entity<Urunler>()
                .HasMany(e => e.Urun_Rezervasyon_Iletisim_Bilgileri)
                .WithOptional(e => e.Urunler)
                .HasForeignKey(e => e.urun_ID);

            modelBuilder.Entity<Urunler>()
                .HasMany(e => e.Urun_Talep_Formu)
                .WithOptional(e => e.Urunler)
                .HasForeignKey(e => e.urun_ID);

            modelBuilder.Entity<Urunler>()
                .HasMany(e => e.Urun_Tarihleri)
                .WithOptional(e => e.Urunler)
                .HasForeignKey(e => e.urun_ID);
        }
    }
}
