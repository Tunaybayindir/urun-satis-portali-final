using System.ComponentModel.DataAnnotations;
using UrunSatisPortali.Models;
namespace UrunSatisPortali.Models
{
    public class Urun
    {
        public int urunID { get; set; }
        public string urunAD { get; set; }
        public int urunETIKETNO { get; set; }
        public int urunSTOCK { get; set; }
        public string Aciklama { get; set; }
        public string resim { get; set; }
        public decimal kategoriFiyat { get; set; }

        public int kategoriID { get; set; }
        public Kategori kategori { get; set; }
    }
}
