using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ModelViews
{
    public class KategoriViewModel
    {
        public int Id { get; set; }


        [Display(Name = "Kategori Seçiniz ")]
        [Required(ErrorMessage = "Kategori Seçiniz!")]
        public string KategoriBaslik { get; set; }
    }
}
