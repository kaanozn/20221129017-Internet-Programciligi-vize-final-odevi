using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Haber
    {
        public int Id { get; set; }

        public string HaberBaslik { get; set; }

        public string HaberDetay { get; set; }

        public string HaberResim { get; set; }

  

        [ForeignKey(nameof(Kategori))]
        public int KategoriId { get; set; }
        public Kategori Kategori { get; set; }
    }

    public class Kategori
    {
        public int Id { get; set; }

        public string KategoriBaslik { get; set; }

        public ICollection<Haber> Habers { get; set; }

    }
}
