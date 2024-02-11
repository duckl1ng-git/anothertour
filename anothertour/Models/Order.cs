using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.ObjectModelRemoting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace anothertour.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Экскурсия")]
        public int TourId { get; set; }

        [Required]
        [Display(Name = "Дата и время начала экскурсии")]
        public DateTime TourDate { get; set; }

        [Display(Name = "Количество участников")]
        [Required]
        [Range (1, 100)]
        public int TouristsCount { get; set; }

        [Required(ErrorMessage = "Пожалуйста введите свою фамилию")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Длина строки должна быть от 2 до 50 символов")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Пожалуйста введите свое имя")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Длина строки должна быть от 2 до 50 символов")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите номер телефона")]
        [Display(Name = "Номер телефона")]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите адрес электронной почты")]
        [Display(Name = "Адрес электронной почты")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Комментарий")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        [Required]
        [Display(Name = "Статус заявки")]
        public string Status { get; set; }

        [Required]
        [Range(1, 50000, ErrorMessage = "Недопустимая сумма")]
        [Display(Name = "Общая стоимость")]
        public int TotalPrice { get; set; }

        [Required]
        [Display(Name = "Дата оформления заявки")]
        public DateTime Date { get; set; }

        [Display(Name = "Клиент")]
        [HiddenInput(DisplayValue = false)]
        [AllowNull]
        public string? ClientId { get; set; }

        [Display(Name = "Связанное событие")]
        [HiddenInput(DisplayValue = false)]
        [AllowNull]
        public int? ScheduleItemId { get; set; }
    }
}
