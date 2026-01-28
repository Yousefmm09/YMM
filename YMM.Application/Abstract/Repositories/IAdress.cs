using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Dto.Address;

namespace YMM.Application.Abstract.Repositories
{
    public interface IAdress
    {
        Task<CreateAddressDto> CreateAddressAsync(CreateAddressDto createAddressDto, string userId);
    }
}
