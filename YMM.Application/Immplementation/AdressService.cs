using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Abstract.Repositories;
using YMM.Application.Abstract.Services;
using YMM.Application.Dto.Address;
using YMM.Application.Dto.Cart;
using YMM.Application.Dto.Response;

namespace YMM.Application.Immplementation
{
    public class AdressService:IAddressService
    {
        private readonly IAdress _address;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AdressService(IAdress adress,IHttpContextAccessor httpContextAccessor)
        {
            _address = adress;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<CreateAddressDto>> CreateAddressAsync(CreateAddressDto createAddressDto)
        {
            var userId =   _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
                return new ApiResponse<CreateAddressDto>
                   (
                       Success: false,
                       Message: "Unauthorized",
                       Data: null,
                       Errors: new[] { "You are not authorized to Add address this item" },
                       TraceId: Guid.NewGuid().ToString()
                   );
            var address=await _address.CreateAddressAsync(createAddressDto, userId);
            return new ApiResponse<CreateAddressDto>
                   (
                       Success: address!=null ? true : false,
                       Message:address!=null ? "The address was created successfully." : "Failed to create the address.",
                       Data: address,
                       Errors: address!=null ? null : new[] { "Failed to create the address." },
                       TraceId: Guid.NewGuid().ToString()
                   );
        }
    }
}
