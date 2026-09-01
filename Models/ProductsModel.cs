using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Next_Store.Infrastructure;

namespace Next_Store.Models
{
    public class ProductsModel
    {
        public int Id { get; set; }
        [Required, MinLength(2, ErrorMessage = "Minimum Length is 2 letters"), MaxLength(20, ErrorMessage = "Maximum length is 20 letters")]

        public string Name { get; set; }
        public string Slug { get; set; }
        [Required, MinLength(4, ErrorMessage = "Minimum Length is 4 letters"), MaxLength(100, ErrorMessage = "Maximum length is 100 letters")]

        public string Description { get; set; }
        [Column(TypeName="decimal(18,2)")]
        public decimal Price { get; set; }
        [Display(Name ="Category")]
        [Range(1, int.MaxValue, ErrorMessage ="Select a category")]
        // foreign key
        public int CategoryId { get; set; } 
        // Navigation property for categories
        public CategoryModel Category { get; set; }
        
        public string Image { get; set; }
        [NotMapped]
        [FileExtension]
        public IFormFile ImageUpload { get; set; }
        public ProductsModel() 
        {
        }
    }
}
