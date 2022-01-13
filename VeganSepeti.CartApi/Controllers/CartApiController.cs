using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VeganSepeti.CartApi.Business;
using VeganSepeti.CartApi.Common;
using VeganSepeti.CartApi.Models;
using VeganSepeti.CartApi.Services;

namespace VeganSepeti.CartApi.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartApiController : ControllerBase
    {
        private readonly ICartApiRepository _cartApiRepository;

        public CartApiController(ICartApiRepository cartManagementRepository)
        {
            _cartApiRepository = cartManagementRepository;
        }

        //POST: api/CartApi/Authenticate
        /// <summary>
        /// Authentication
        /// </summary>
        /// <param name="authUser"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public IActionResult Authenticate([FromBody]AuthUser authUser)
        {
            string token = _cartApiRepository.Authenticate(authUser);
            if(string.IsNullOrEmpty(token))
            {
                return BadRequest(new { message = ResourceManager.Instance.GetResource(ResourceKeys.UserIsInvalid) });
            }

            return Ok(token);
        }
        // GET: api/CartApi/GetCartProducts/{customerId}
        /// <summary>
        /// Returns product list in the customer's cart
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet("GetCartProducts/{customerId}")]
        public async Task<ActionResult<List<CartProduct>>> GetCartProducts(int customerId)
        {
            return await _cartApiRepository.GetCartProducts(customerId);
        }

        // POST: api/CartApi/AddProductToCart
        /// <summary>
        /// Adds the product to the customer's cart
        /// </summary>
        /// <param name="cartProduct"></param>
        /// <returns></returns>
        [HttpPost("AddProductToCart")]
        public async Task<ActionResult<bool>> AddProductToCart(CartProduct cartProduct)
        {
            if (_cartApiRepository.IsProductExistInCart(cartProduct.Id))
            {
                return await UpdateProductCountInCart(cartProduct);
            }
            if (!ProductManager.Instance.IsProductExist(cartProduct.Id))
            {
                return ValidationProblem(ResourceManager.Instance.GetResource(ResourceKeys.ProductIsNotExistInStocks));
            }

            return await _cartApiRepository.AddProductToCart(cartProduct);

        }

        // PUT: api/CartApi/UpdateProductCountInCart
        /// <summary>
        /// Updates the product count in the customer's cart
        /// If count is zero, then removes product from cart
        /// </summary>
        /// <param name="cartProduct"></param>
        /// <returns></returns>
        [HttpPut("UpdateProductCountInCart")]
        public async Task<ActionResult<bool>> UpdateProductCountInCart(CartProduct cartProduct)
        {
            if (!_cartApiRepository.IsProductExistInCart(cartProduct.Id))
            {
                return ValidationProblem(ResourceManager.Instance.GetResource(ResourceKeys.ProductIsNotExistInCart));
            }

            return await _cartApiRepository.UpdateProductCountInCart(cartProduct);
        }

        // DELETE: api/CartApi/RemoveProductFromCart
        /// <summary>
        /// Removes the product from the customer's cart
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("RemoveProductFromCart")]
        public async Task<ActionResult<bool>> RemoveProductFromCart(CartProduct cartProduct)
        {
            if (!_cartApiRepository.IsProductExistInCart(cartProduct.Id))
            {
                return ValidationProblem(ResourceManager.Instance.GetResource(ResourceKeys.ProductIsNotExistInCart));
            }

            return await _cartApiRepository.RemoveProductFromCart(cartProduct);
        }

        // POST: api/CartApi/ClearCart/{customerId}
        /// <summary>
        /// Clears the customer's cart
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpPost("ClearCart/{customerId}")]
        public async Task<ActionResult<bool>> ClearCart(int customerId)
        {
            return await _cartApiRepository.ClearCart(customerId);
        }
    }
}
