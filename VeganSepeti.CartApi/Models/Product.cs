namespace VeganSepeti.CartApi.Models
{
    public class Product
    {
        /// <summary>
        /// Product Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Product code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Product name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Product description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Product URL address
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Product image URL address
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// Product amount
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// Product discount rate
        /// </summary>
        public decimal Discount { get; set; }
        /// <summary>
        /// Is product exist in stocks
        /// </summary>
        public bool IsExist { get; set; }
        /// <summary>
        /// Is product status actice
        /// </summary>
        public bool IsActive { get; set; }
        public decimal CalculatedAmount
        {
            get
            {
                return Amount - (Amount * Discount);
            }
        }
    }
}
