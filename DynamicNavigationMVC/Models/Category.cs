using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace DynamicNavigationMVC.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        //Navigational Properties
        [JsonIgnore]
        public List<Category> ChildCategory { get; set; }
		[JsonIgnore]
		public List<Category> ParentCategory { get; set; }
        [JsonIgnore]
        public List<Product> Products { get; set; }

        [NotMapped]
        public int SelectedParentCategoryId { get; set; } = 0;

        public Category()
        {
            ChildCategory = new List<Category>();
            ParentCategory = new List<Category>();
            Products = new List<Product>();
        }
        public Category(int id, string name)
        {
            Id = id;
            Name = name;
            ChildCategory = new List<Category>();
            ParentCategory = new List<Category>();
            Products = new List<Product>();

        }
    }
}
