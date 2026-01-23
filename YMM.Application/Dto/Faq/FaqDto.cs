using System.Collections.Generic;

namespace YMM.Application.Dto.Faq
{
    public class FaqDto
    {
        public int Id { get; set; }
        public string Question { get; set; } = null!;
        public string Answer { get; set; } = null!;
        public string Category { get; set; } = null!;
        public int DisplayOrder { get; set; }
    }

    public class FaqCategoryDto
    {
        public string Category { get; set; } = null!;
        public List<FaqDto> Questions { get; set; } = new List<FaqDto>();
    }
}
