using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace anothertour.Models
{
    [Table("Reviews")]
    public class Review
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int TourId { get; set; }

        [Display(Name = "Клиент")]
        [Required]
        public string ClientId { get; set; }

        [Display(Name = "Отзыв")]
        [Required]
        public string Text { get; set; }

        [Display(Name = "Дата")]
        [Required]
        public DateTime Date { get; set; }

        [Display(Name = "Оценка")]
        [Range(1, 5, ErrorMessage = "Поставьте оценку от 1 до 5")]
        [Required]
        public int Score { get; set; }
    }
}
