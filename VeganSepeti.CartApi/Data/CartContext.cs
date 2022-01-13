using Microsoft.EntityFrameworkCore;
using VeganSepeti.CartApi.Models;

namespace VeganSepeti.CartApi.Data
{
    public class CartContext : DbContext
    {
        public CartContext(DbContextOptions<CartContext> options) : base(options) 
        { 
        }

        public DbSet<CartProduct> CartProducts { get; set; }
    }
}
