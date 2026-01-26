using System;
using System.ComponentModel.DataAnnotations;

namespace YMM.Application.Dto.Admin
{
    public class ExportReportFilterDto
    {
        [Required]
        public string Type { get; set; } = null!; // sales, orders, customers, inventory, products

        [Required]
        public string Format { get; set; } = "csv"; // csv, excel, pdf

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
