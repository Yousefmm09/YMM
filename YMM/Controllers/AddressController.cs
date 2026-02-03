using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YMM.Application.Abstract.Services;
using YMM.Application.Dto.Address;

namespace YMM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AddressController> _logger;
        private readonly IAddressService _addressService;
        public AddressController(IHttpContextAccessor httpContextAccessor, ILogger<AddressController> logger, IAddressService addressService)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _addressService = addressService;
        }
        [HttpPost("CreateAddress")]
        public async Task<IActionResult> CreateAddress(CreateAddressDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _addressService.CreateAddressAsync(dto);
                return Ok(result);
            }
            return BadRequest(ModelState);

        }
    }
}
