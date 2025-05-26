using System.ComponentModel.DataAnnotations;

namespace Shopping.Models
{
    public class ProductModel
    {
        [Key]
        public int Id { get; set; }
        [Required, MinLength(4, ErrorMessage = "Yeu cau nhap ten san pham")]
        public string Name { get; set; }
        [Required, MinLength(4, ErrorMessage = "Yeu cau nhap mo ta san pham")]
        public string Description { get; set; }
        public string Slug {  get; set; }
        [Required, MinLength(4, ErrorMessage = "Yeu cau nhap gia san pham")]
        public decimal Price { get; set; }
        public int BrandId {  get; set; }
        public int CategoryId {  get; set; }
        public BrandModel Brand { get; set; }
        public CategoryModel Category { get; set; }
    }
}
