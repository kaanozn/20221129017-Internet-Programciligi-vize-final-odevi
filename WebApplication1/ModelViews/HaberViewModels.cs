using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Models;

namespace WebApplication1.ModelViews
{
    public class HaberViewModels
    {
        public int Id { get; set; }


        [Display(Name = "Haber Başlık Giriniz :")]
        [Required(ErrorMessage = "Haber Başlığını Giriniz!")]
        public string HaberBaslik { get; set; }

        [Display(Name = "Haber Detayını Giriniz : ")]
        [Required(ErrorMessage = "Haber Başlığını Giriniz!")]
        public string HaberDetay { get; set; }

        [Display(Name = "Haber Kateogirisini Seçiniz :")]
        [Required(ErrorMessage = "Haber Kateogirisini Seçiniz !")]
        public int KategoriId { get; set; }

        [Display(Name = "Haber Resmini Seçiniz :")]
        [Required(ErrorMessage = "Haber Resmini Seçiniz!")]
        public IFormFile HaberResim { get; set; }

        [NotMapped]
        public string HaberResimData { get; set; }
    
    }
}
