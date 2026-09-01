using System.ComponentModel.DataAnnotations;

namespace Next_Store.Models
{
    public class Page
    {
        public int Id { get; set; }
        [Required, MinLength(2, ErrorMessage ="Minimum Length is 2 letters"), MaxLength(20, ErrorMessage ="Maximum length is 20 letters")]
        public string Title { get; set; }
       
        public string Slug { get; set; }
        [Required, MinLength(10, ErrorMessage = "Minimum Length is 10 letters"), MaxLength(200, ErrorMessage = "Maximum length is 200 letters")]
        public string Content { get; set; }
        public int Sorting { get; set; }
        public List<Page> page { get; set; } = [];
        public Page()
        { }
        public Page(List<Page> pages)
        {
            page = pages;
        }
    }
}
