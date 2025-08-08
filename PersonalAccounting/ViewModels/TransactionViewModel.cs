using System.ComponentModel.DataAnnotations;
using PersonalAccounting.Models;

namespace PersonalAccounting.ViewModels;

public class TransactionViewModel
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "شرح اجباری است")]
    [StringLength(200)]
    [Display(Name = "شرح")]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Display(Name = "نوع تراکنش")]
    public TransactionType Type { get; set; }

    [Required]
    [Display(Name = "مبلغ")]
    [Range(0.01, double.MaxValue, ErrorMessage = "مبلغ معتبر وارد کنید")]
    public decimal Amount { get; set; }

    [Required]
    [Display(Name = "تاریخ (شمسی)")]
    public string PersianDate { get; set; } = string.Empty;

    [Display(Name = "دسته‌بندی")]
    public int? CategoryId { get; set; }

    public string? CategoryName { get; set; }
}