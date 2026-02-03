using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMM.Application.Abstract.Repositories;
using YMM.Application.Dto.Address;
using YMM.Data.Entities;
using YMM.Data.Entities.Identity;
using YMM.Infrastructure.Context;

namespace YMM.Infrastructure.Immplementation
{
    public class AdressRepo : IAdress
    {
        private readonly AppDb _appDb;
        private readonly UserManager<User> _userManager;
        public AdressRepo(AppDb appDb,UserManager<User> userManager)
        {
            _appDb = appDb;
            _userManager = userManager;
        }
        public async Task<CreateAddressDto> CreateAddressAsync(CreateAddressDto createAddressDto, string userId)
        {
          
            var Addres = new Address
            {
                UserId = userId,
                FullName = createAddressDto.FullName,
                City = createAddressDto.City,
                Country = createAddressDto.Country,
                State = createAddressDto.State,
                Street = createAddressDto.Street,
                PostalCode = createAddressDto.PostalCode,
                Apartment = createAddressDto.Apartment,
                PhoneNumber = createAddressDto.PhoneNumber,
                Building = createAddressDto.Building,
                AddressType = createAddressDto.AddressType,
                IsDefault = createAddressDto.IsDefault,
            };
            var AddressDto = new CreateAddressDto
            {
                FullName = Addres.FullName,
                City = Addres.City,
                Country = Addres.Country,
                State = Addres.State,
                Street = Addres.Street,
                PostalCode = Addres.PostalCode,
                Apartment = Addres.Apartment,
                PhoneNumber = Addres.PhoneNumber,
                Building = Addres.Building,
                AddressType = Addres.AddressType,
                IsDefault = Addres.IsDefault,
            };
            await _appDb.Addresses.AddAsync(Addres);
            await _appDb.SaveChangesAsync();
            return AddressDto;
        }
    }
}
