using VeganSepeti.CartApi.Business;

namespace VeganSepeti.CartApi.Models
{
    public class CartProduct
    {
        /// <summary>
        /// Product Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Count of the product in the cart
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// The customer who has cart
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// The related product details
        /// </summary>
        public Product Product
        {
            get
            {
                return ProductManager.Instance.GetProduct(Id);
            }
        }
        /// <summary>
        /// The calculated total amount for this product in the cart
        /// </summary>
        public decimal TotalAmount
        {
            get
            {
                return Product.CalculatedAmount * Count;
            }
        }
    }
}
