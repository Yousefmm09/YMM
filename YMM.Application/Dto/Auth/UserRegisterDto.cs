//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Text;
//using YMM.Application.Dto.Address;

//namespace YMM.Application.Dto.Auth
//{
//    public class UserRegisterDto
//    {

//        [Required(ErrorMessage = "UserName must be Required")]
//        [MaxLength(50)]
//        public string UserName { get; set; }
//        [Required]
//        public string Password { get; set; }
//        [Required]
//        public string ConfirmPassowrd { get; set; }
//        [EmailAddress]
//        public string EmailAddress { get; set; } = string.Empty;
//        [Required]
//        public string Country { get; set; }
//        [Required]
//        public string PhoneNumber { get; set; }
//        [Range(18, 60, ErrorMessage = "You must be between 18 and 60 years old")]
//        public int Age { get; set; }
//        [Required]
//        public List<AddressDto> Addresses { get; set; } = new List<AddressDto>();
//        [Required]
//    }
//}
