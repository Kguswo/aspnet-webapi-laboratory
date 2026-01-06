using System.ComponentModel.DataAnnotations;

namespace DapperMvcDemo.Models
{
    public class ProductModel
    {
        public Guid ProductId { get; set; }

        [Display(Name = "상품명")]
        public string ProductName { get; set; }

        [Display(Name = "가격")]
        public decimal Price { get; set; }

        [Display(Name = "상품 설명")]
        public string ProductDescription { get; set; }

        [Display(Name = "등록일")]
        public DateTime? CreatedOn { get; set; }

        [Display(Name = "수정일")]
        public DateTime? UpdatedOn { get; set; }
    }
}
