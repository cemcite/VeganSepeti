using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using VeganSepeti.CartApi.Models;

namespace VeganSepeti.CartApi.Services
{
    public interface ICartApiRepository
    {
        /// <summary>
        /// Authentication
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        string Authenticate(AuthUser authUser);
        /// <summary>
        /// Returns product list in the customer's cart
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task<ActionResult<List<CartProduct>>> GetCartProducts(int customerId);
        /// <summary>
        /// Adds the product to the customer's cart
        /// </summary>
        /// <param name="cartProduct"></param>
        /// <returns></returns>
        Task<ActionResult<bool>> AddProductToCart(CartProduct cartProduct);
        /// <summary>
        /// Updates the product count in the customer's cart
        /// If count is zero, then removes the product from the cart
        /// </summary>
        /// <param name="cartProduct"></param>
        /// <returns></returns>
        Task<ActionResult<bool>> UpdateProductCountInCart(CartProduct cartProduct);
        /// <summary>
        /// Removes the product from the customer's cart
        /// </summary>
        /// <param name="cartProduct"></param>
        /// <returns></returns>
        Task<ActionResult<bool>> RemoveProductFromCart(CartProduct cartProduct);
        /// <summary>
        /// Clears the customer's cart
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task<ActionResult<bool>> ClearCart(int customerId);
        /// <summary>
        /// Checks the product existence in the customer's cart 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool IsProductExistInCart(int id);
        /// <summary>
        /// Checks the customer's cart is empty
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        bool IsCartEmpty(int customerId);
    }
}
