using Microsoft.AspNetCore.Mvc;
using HopelessFilmiesMVC.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using HopelessFilmies.Domain.Interfaces.ICart;

namespace HopelessFilmiesMVC.Controllers
{
    [Route("cart")]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private const int StandardPrice = 199;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        public IActionResult Index()
        {
            
            return View(); 
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            var ids = GetCartIds();
            var items = await _cartService.GetCartAsync(ids);
            return Json(new
            {
                count = items.Count,
                total = items.Count * StandardPrice,
                items
            });
        }


        [HttpPost("toggle")]
        public IActionResult Toggle([FromBody] int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(new { redirectUrl = Url.Action("SignIn", "Account", new { message = "Please sign in to use the cart." }) });
            }

            var cart = GetCartIds();

            if (cart.Contains(id))
                cart.Remove(id);
            else
                cart.Add(id);

            SaveCartIds(cart);

            return Json(new { success = true, inCart = cart.Contains(id), count = cart.Count });
        }

        [HttpPost("remove")]
        public IActionResult Remove([FromBody] int id)
        {
            var cart = GetCartIds();
            cart.Remove(id);
            SaveCartIds(cart);
            return Json(new { success = true, count = cart.Count });
        }

        private List<int> GetCartIds()
        {
            return HttpContext.Session.GetString("Cart")?.Split(',')
                .Where(x => int.TryParse(x, out _)).Select(int.Parse).ToList() ?? new List<int>();
        }

        private void SaveCartIds(List<int> cart)
        {
            HttpContext.Session.SetString("Cart", string.Join(",", cart.Distinct()));
        }

        [HttpPost]
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(new
                {
                    redirectUrl = Url.Action("SignIn", "Account", new { message = "Please sign in to proceed." })
                });
            }

            try
            {
                var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
                if (emailClaim == null)
                {
                    return StatusCode(500, new { success = false, message = "User email not found in claims." });
                }

                var userEmail = emailClaim.Value;
                var user = await _cartService.GetUserByEmailAsync(userEmail);

                if (user == null)
                {
                    return StatusCode(500, new { success = false, message = "User not found in database." });
                }

                var cartIds = GetCartIds();
                var films = await _cartService.GetCartAsync(cartIds);

                if (!films.Any())
                {
                    return Json(new { success = false, message = "No movies in cart to purchase." });
                }

                // Add purchased movies
                var movieNames = films.Select(f => f.Heading).ToList();
                var currentList = user.PurchasedMovies ?? "";
                var existing = currentList.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();

                existing.AddRange(movieNames.Where(m => !existing.Contains(m)));
                user.PurchasedMovies = string.Join(", ", existing.Distinct());

                await _cartService.SaveChangesAsync();
                HttpContext.Session.Remove("Cart");

                return Json(new { success = true, message = "Payment Successful, Movies Added to your List." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Something went wrong during checkout.",
                    error = ex.Message
                });
            }
        }



    }

}