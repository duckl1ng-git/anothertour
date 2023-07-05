using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace anothertour.Models
{
    [PrimaryKey(nameof(Id))]
    [Table("Tours")]
    public class Tour
    {
        [HiddenInput(DisplayValue = false)]
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Название экскурсии")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 50 символов")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Цена билета")]
        [Range(1, 10000, ErrorMessage = "Цена должна быть в диапазоне от 1 до 10000")]
        public int TicketPrice { get; set; }

        [AllowNull]
        [Display(Name = "Обложка")]
        [HiddenInput(DisplayValue = false)]
        public string? MainImage { get; set; }

        [AllowNull]
        [Display(Name = "Дополнительные изображения")]
        [HiddenInput(DisplayValue = false)]
        public string? AdditionalImages { get; set; }

        [AllowNull]
        [Display(Name = "Видео")]
        [HiddenInput(DisplayValue = false)]
        public string? Video { get; set; }
    }
}
