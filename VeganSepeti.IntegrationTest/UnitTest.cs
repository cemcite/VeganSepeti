using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VeganSepeti.CartApi.Models;
using Xunit;

namespace VeganSepeti.IntegrationTest
{
    public class UnitTest
    {
        public readonly HttpClient _client;

        public UnitTest()
        {
            _client = new TestClientProvider().Client;
        }

        [Fact]
        public async Task Get_Should_Return_Empty_List_When_Get_Success()
        {
            var getCartProductsResponse = await _client.GetAsync("/api/CartApi/GetCartProducts/666");
            List<CartProduct> cartProductList = JsonConvert.DeserializeObject<List<CartProduct>>(await getCartProductsResponse.Content.ReadAsStringAsync());
            bool result = cartProductList.Count == 0 && getCartProductsResponse.StatusCode == HttpStatusCode.OK;
            Assert.True(result);
        }

        [Fact]
        public async Task Post_Should_Return_Filled_List_After_Added_Product_To_Cart()
        {
            // AddProductToCart
            var cartProduct = new CartProduct
            {
                Id = 2,
                Count = 3,
                CustomerId = 666
            };
            var addProductToCartRequest = new StringContent(JsonConvert.SerializeObject(cartProduct), Encoding.UTF8, "application/json");
            var addProductToCartResponse = await _client.PostAsync("/api/CartApi/AddProductToCart", addProductToCartRequest);
            var addProductToCartResponseResult = JsonConvert.DeserializeObject<bool>(await addProductToCartResponse.Content.ReadAsStringAsync());
            bool addProductToCartResult = addProductToCartResponse.IsSuccessStatusCode && addProductToCartResponseResult;
            Assert.True(addProductToCartResult);

            // GetCartProducts
            var getCartProductsResponse = await _client.GetAsync("/api/CartApi/GetCartProducts/666");
            var getCartProductsResponseResult = JsonConvert.DeserializeObject<List<CartProduct>>(await getCartProductsResponse.Content.ReadAsStringAsync());
            bool getCartProductsResult = getCartProductsResponseResult.Count > 0 && getCartProductsResponse.IsSuccessStatusCode;
            Assert.True(getCartProductsResult);
        }

        [Fact]
        public async Task Post_Should_Return_Updated_List_After_Updated_Product_Count_In_Cart()
        {
            // AddProductToCart
            CartProduct cartProduct = new()
            {
                Id = 2,
                Count = 3,
                CustomerId = 666
            };
            var addProductToCartRequest = new StringContent(JsonConvert.SerializeObject(cartProduct), Encoding.UTF8, "application/json");
            var addProductToCartResponse = await _client.PostAsync("/api/CartApi/AddProductToCart", addProductToCartRequest);
            var addProductToCartResponseResult = JsonConvert.DeserializeObject<bool>(await addProductToCartResponse.Content.ReadAsStringAsync());
            bool addProductToCartResult = addProductToCartResponse.IsSuccessStatusCode && addProductToCartResponseResult;
            Assert.True(addProductToCartResult);

            // UpdateProductCountInCart
            int newCount = 2;
            cartProduct = new CartProduct
            {
                Id = 2,
                Count = newCount,
                CustomerId = 666
            };
            var updateProductCountInCartRequest = new StringContent(JsonConvert.SerializeObject(cartProduct), Encoding.UTF8, "application/json");
            var updateProductCountInCartResponse = await _client.PutAsync("/api/CartApi/UpdateProductCountInCart", updateProductCountInCartRequest);
            var updateProductCountInCartResponseResult = JsonConvert.DeserializeObject<bool>(await updateProductCountInCartResponse.Content.ReadAsStringAsync());
            bool updateProductCountInCartResult = updateProductCountInCartResponse.IsSuccessStatusCode && updateProductCountInCartResponseResult;
            Assert.True(updateProductCountInCartResult);

            // GetCartProducts
            var getCartProductsResponse = await _client.GetAsync("/api/CartApi/GetCartProducts/666");
            var getCartProductsResponseResult = JsonConvert.DeserializeObject<List<CartProduct>>(await getCartProductsResponse.Content.ReadAsStringAsync());
            bool getCartProductsResult = getCartProductsResponseResult.Count > 0 && getCartProductsResponseResult[0].Count == newCount && getCartProductsResponse.IsSuccessStatusCode;
            Assert.True(getCartProductsResult);
        }

        [Fact]
        public async Task Post_Should_Return_Empty_List_After_Removed_Product_From_In_Cart()
        {
            // AddProductToCart
            var cartProduct = new CartProduct
            {
                Id = 2,
                Count = 3,
                CustomerId = 666
            };
            var addProductToCartRequest = new StringContent(JsonConvert.SerializeObject(cartProduct), Encoding.UTF8, "application/json");
            var addProductToCartResponse = await _client.PostAsync("/api/CartApi/AddProductToCart", addProductToCartRequest);
            var addProductToCartResponseResult = JsonConvert.DeserializeObject<bool>(await addProductToCartResponse.Content.ReadAsStringAsync());
            bool addProductToCartResult = addProductToCartResponse.IsSuccessStatusCode && addProductToCartResponseResult;
            Assert.True(addProductToCartResult);

            // RemoveProductFromCart
            cartProduct = new CartProduct
            {
                Id = 2,
                CustomerId = 666
            };
            var removeProductFromCartRequest = new StringContent(JsonConvert.SerializeObject(cartProduct), Encoding.UTF8, "application/json");
            var removeProductFromCartResponse = await _client.PostAsync("/api/CartApi/RemoveProductFromCart", removeProductFromCartRequest);
            var removeProductFromCartResponseResult = JsonConvert.DeserializeObject<bool>(await removeProductFromCartResponse.Content.ReadAsStringAsync());
            bool removeProductFromCartResult = removeProductFromCartResponse.IsSuccessStatusCode && removeProductFromCartResponseResult;
            Assert.True(removeProductFromCartResult);

            // GetCartProducts
            var getCartProductsResponse = await _client.GetAsync("/api/CartApi/GetCartProducts/666");
            var getCartProductsResponseResult = JsonConvert.DeserializeObject<List<CartProduct>>(await getCartProductsResponse.Content.ReadAsStringAsync());
            bool getCartProductsResult = getCartProductsResponseResult.Count == 0 && getCartProductsResponse.IsSuccessStatusCode;
            Assert.True(getCartProductsResult);
        }

        [Fact]
        public async Task Post_Should_Return_Empty_List_After_Clear_Cart()
        {
            // AddProductToCart
            var cartProduct = new CartProduct
            {
                Id = 2,
                Count = 3,
                CustomerId = 666
            };
            var addProductToCartRequest = new StringContent(JsonConvert.SerializeObject(cartProduct), Encoding.UTF8, "application/json");
            var addProductToCartResponse = await _client.PostAsync("/api/CartApi/AddProductToCart", addProductToCartRequest);
            var addProductToCartResponseResult = JsonConvert.DeserializeObject<bool>(await addProductToCartResponse.Content.ReadAsStringAsync());
            bool addProductToCartResult = addProductToCartResponse.IsSuccessStatusCode && addProductToCartResponseResult;
            Assert.True(addProductToCartResult);

            // ClearCart
            var clearCartResponse = await _client.PostAsync("/api/CartApi/ClearCart/666", null);
            var clearCartResponseResult = JsonConvert.DeserializeObject<bool>(await clearCartResponse.Content.ReadAsStringAsync());
            bool clearCartResult = clearCartResponseResult && clearCartResponse.IsSuccessStatusCode;
            Assert.True(clearCartResult);

            // GetCartProducts
            var getCartProductsResponse = await _client.GetAsync("/api/CartApi/GetCartProducts/666");
            var getCartProductsResponseResult = JsonConvert.DeserializeObject<List<CartProduct>>(await getCartProductsResponse.Content.ReadAsStringAsync());
            bool getCartProductsResult = getCartProductsResponseResult.Count == 0 && getCartProductsResponse.IsSuccessStatusCode;
            Assert.True(getCartProductsResult);
        }
    }
}
