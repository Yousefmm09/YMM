using System.ComponentModel.DataAnnotations;

namespace YMM.Application.Dto.Order
{
    public class AssignCourierDto
    {
        [Required]
        public int CourierId { get; set; }
    }
}
