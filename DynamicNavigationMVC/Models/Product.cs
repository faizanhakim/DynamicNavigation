using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicNavigationMVC.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Price { get; set; }
        [NotMapped]
        public int SelectedParentCategoryId { get; set; } = 0;
        public List<Category> Category { get; set; }
        public Product()
        {
            Category = new List<Category>();
        }
        public Product(int id, string name, int price) 
        {
            Id = id;
            Name = name;
            Price = price;
            Category = new List<Category>();
        }
    }
}
