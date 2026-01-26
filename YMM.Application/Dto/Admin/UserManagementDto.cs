using System;
using System.ComponentModel.DataAnnotations;
using YMM.Application.Dto.Common;

namespace YMM.Application.Dto.Admin
{
    public class AdminUserDto
    {
        public string Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string Role { get; set; } = null!;
        public bool IsActive { get; set; }
        public bool EmailConfirmed { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalSpent { get; set; }
    }

    public class UserFilterDto : PaginationParams
    {
        public string? Role { get; set; }
        public string? Search { get; set; }
        public bool? IsActive { get; set; }
    }

    public class UpdateUserRoleDto
    {
        [Required]
        public string Role { get; set; } = null!; // Admin, Customer, etc.
    }

    public class SuspendUserDto
    {
        [Required]
        [MaxLength(500)]
        public string Reason { get; set; } = null!;

        public DateTime? SuspendUntil { get; set; }
    }
}
