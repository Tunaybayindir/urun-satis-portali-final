namespace UrunSatisPortali.Models
{
    public class urunEkle
    {
        public string urunAD { get; set; }
        public int urunETIKETNO { get; set; }
        public int urunSTOCK { get; set; }
        public string Aciklama { get; set; }
        public IFormFile resim { get; set; }
        public decimal kategoriFiyat { get; set; }

        public int kategoriID { get; set; }
    }
}
