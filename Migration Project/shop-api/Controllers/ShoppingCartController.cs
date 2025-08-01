using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using shop_api.Interfaces;
using shop_api.Models;
using shop_api.Models.DTO;

namespace shop_api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    
    
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private ISession Session => _httpContextAccessor.HttpContext.Session;

        public ShoppingCartController(IShoppingCartService shoppingCartService,
                                      IHttpContextAccessor httpContextAccessor)
        {
            _shoppingCartService = shoppingCartService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<ActionResult<List<Cart>>> GetCartItems()
        {
            try
            {
                var carts = await _shoppingCartService.GetCartItems(Session);
                return Ok(carts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Add/{productId}")]
        public async Task<IActionResult> AddToCart(int productId)
        {
            try
            {
                await _shoppingCartService.AddToCart(Session, productId);
                return Ok(new { message = "Product added to cart" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete/{productId}")]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            try
            {
                await _shoppingCartService.RemoveFromCart(Session, productId);
                return Ok(new { message = "Product removed from cart" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateCart([FromBody] Dictionary<int, int> productQuantities)
        {
            try
            {
                if (productQuantities == null || productQuantities.Count == 0)
                    return BadRequest("No quantities provided for update.");

                await _shoppingCartService.UpdateCart(Session, productQuantities);
                return Ok(new { message = "Shopping cart updated" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ProcessOrder")]
        public async Task<ActionResult<Order>> ProcessOrder([FromBody] OrderRequestDTO orderRequest)
        {
            try
            {
                if (orderRequest == null)
                    return BadRequest("Order request data is required.");

                var order = await _shoppingCartService.ProcessOrder(Session, orderRequest);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Clear")]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                await _shoppingCartService.ClearCart(Session);
                return Ok(new { message = "Shopping cart cleared" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}