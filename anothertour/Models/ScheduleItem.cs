using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace anothertour.Models
{
    [Table("Schedule")]
    public class ScheduleItem
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int ScheduleItemId { get; set; }

        [Display(Name = "Экскурсия")]
        [HiddenInput(DisplayValue = false)]
        public int TourId { get; set; }

        [Display(Name = "Гид")]
        [HiddenInput(DisplayValue = false)]
        public string GuideId { get; set; }

        [AllowNull]
        [Display(Name = "Экскурсия")]
        public Tour? SelectedTour { get; set; }

        [Display(Name = "Количество участников")]
        [Required]
        public int TouristsCount { get; set; }

        [Display(Name = "Дата и время")]
        [Required]
        public DateTime Date_Time { get; set; }

        [Display(Name = "Запись отключена (Да/Нет)")]
        [Required]
        public bool OrderingDisabled { get; set; }
    }
}
