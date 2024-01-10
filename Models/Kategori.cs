namespace UrunSatisPortali.Models
{
    public class Kategori
    {
        public int kategoriID { get; set; }
        public string kategoriAD { get; set; }
        public IList<Urun> urunler { get; set; }

    }
}
