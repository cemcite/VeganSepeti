using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VeganSepeti.CartApi.Auth;
using VeganSepeti.CartApi.Common;
using VeganSepeti.CartApi.Data;
using VeganSepeti.CartApi.Models;

namespace VeganSepeti.CartApi.Services
{
    public class CartApiRepository : ICartApiRepository
    {
        private readonly CartContext _context;
        private readonly AppSettings _appSettings;


        public CartApiRepository(CartContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// Authentication
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string Authenticate(AuthUser authUser)
        {
            bool isAuthenticationValuesCorrect = authUser.UserName.Equals(Authentication.UserName) && authUser.Password.Equals(Authentication.Password);
            if (!isAuthenticationValuesCorrect)
            {
                return string.Empty;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, "1")
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        /// <summary>
        /// Adds the product to the customer's cart
        /// </summary>
        /// <param name="cartProduct"></param>
        /// <returns></returns>
        public async Task<ActionResult<bool>> AddProductToCart(CartProduct cartProduct)
        {
            _context.CartProducts.Add(cartProduct);
            return await _context.SaveChangesAsync() > 0;
        }
        /// <summary>
        /// Returns product list in the customer's cart
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<ActionResult<List<CartProduct>>> GetCartProducts(int customerId)
        {
            return await _context.CartProducts.Where(entity => entity.CustomerId == customerId).ToListAsync();
        }
        /// <summary>
        /// Removes the product from the customer's cart
        /// </summary>
        /// <param name="cartProduct"></param>
        /// <returns></returns>
        public async Task<ActionResult<bool>> RemoveProductFromCart(CartProduct cartProduct)
        {
            var record = await _context.CartProducts.SingleAsync(entity => entity.CustomerId == cartProduct.CustomerId && entity.Id == cartProduct.Id);
            _context.CartProducts.Remove(record);
            return await _context.SaveChangesAsync() > 0;
        }
        /// <summary>
        /// Updates the product count in the customer's cart
        /// If count is zero, then removes the product from the cart
        /// </summary>
        /// <param name="cartProduct"></param>
        /// <returns></returns>
        public async Task<ActionResult<bool>> UpdateProductCountInCart(CartProduct cartProduct)
        {
            if (cartProduct.Count == 0)
            {
                return await RemoveProductFromCart(cartProduct);
            }
            _context.Entry(cartProduct).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }
        /// <summary>
        /// Clears the customer's cart
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<ActionResult<bool>> ClearCart(int customerId)
        {
            var cartProductList = _context.CartProducts.Where(entity => entity.CustomerId == customerId);
            _context.CartProducts.RemoveRange(cartProductList);
            return await _context.SaveChangesAsync() > 0;
        }
        /// <summary>
        /// Checks the product existence in the customer's cart
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsProductExistInCart(int id)
        {
            return _context.CartProducts.Any(entity => entity.Id == id);
        }
        /// <summary>
        /// Checks the customer's cart is empty
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public bool IsCartEmpty(int customerId)
        {
            return !_context.CartProducts.Any(entity => entity.CustomerId == customerId);
        }
    }
}
