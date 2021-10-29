using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Rocky.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        
        [DisplayName("Name")]
        [Required]
        public string Name { get; set; }
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Display order for category most be greater than 0")]
        [DisplayName("Display Order")]
        public int DisplayOrder { get; set; }
    }
}