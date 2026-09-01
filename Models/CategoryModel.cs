using System.ComponentModel.DataAnnotations;

namespace Next_Store.Models
{
    public class CategoryModel
    {
        [Required]
        public int Id { get; set; }
        [Required, MinLength(2, ErrorMessage = "Minimum Length is 2 letters"), MaxLength(20, ErrorMessage = "Maximum length is 20 letters")]
        [RegularExpression(@"^[a-zA-Z-]+$,", ErrorMessage ="Name can only contain letters of the alphabet")]
        public string Name { get; set; }
        public string Slug { get; set; }
        public int Sorting { get; set; }
       public ICollection<ProductsModel> Products { get; set; }


        public CategoryModel()
        {
            
        }
       
    }
}
