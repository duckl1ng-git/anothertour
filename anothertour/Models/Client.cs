using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.Configuration;

namespace anothertour.Models
{
    [Table("Clients")]
    public class Client
    {
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Длина строки должна быть от 2 до 50 символов")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Длина строки должна быть от 2 до 50 символов")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Display(Name = "Номер телефона")]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [HiddenInput(DisplayValue = false)]
        public int TotalOrdersCost { get; set; }

        [Display(Name = "Количество заказов")]
        [Required]
        [HiddenInput(DisplayValue = false)]
        public int OrdersNumber { get; set; }

        [Display(Name = "Количество отзывов")]
        [HiddenInput(DisplayValue = false)]
        public int ReviewsNumber { get; set; }

        [Required]
        [HiddenInput(DisplayValue = false)]
        public int CurrentDiscount { get; set; }
    }
}