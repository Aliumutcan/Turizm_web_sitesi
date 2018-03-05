using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;

namespace izindeturizm.Controllers.Genelislemler
{
    public static class Enumlar
    {
        public enum etiketler
        {
            [Description("Etiket Yok")]
            yok,
            [Description("İndirimli")]
            indirim,
            [Description("Fırsat")]
            firsat,
            [Description("Günün Fırsatı")]
            gunun_firsati,
            [Description("Editörün Seçtikleri")]
            bizim_sectigimiz,
            [Description("Encok Tutan Turlarımız")]
            encok_tutan,
            [Description("Son Dakika")]
            sondakika
        }

        public enum Urun_Tipleri
        {
            [Description("Tur")]
            tur,
            [Description("Otel")]
            otel,
            [Description("Otobus")]
            otobus,
            [Description("Ucak")]
            ucak,
            [Description("Tren")]
            tren,
            [Description("Gemi")]
            gemi,
        }


        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }


        public static IEnumerable<T> EnumToList<T>()
        {
            Type enumType = typeof(T);

            // Can't use generic type constraints on value types,
            // so have to do check like this
            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");

            Array enumValArray = Enum.GetValues(enumType);
            List<T> enumValList = new List<T>(enumValArray.Length);

            foreach (int val in enumValArray)
            {
                enumValList.Add((T)Enum.Parse(enumType, val.ToString()));
            }

            return enumValList;
        }
    }
}