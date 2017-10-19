using System;
using System.ComponentModel.DataAnnotations;

namespace the_project.Models
{
    public class ActivityViewModel : BaseEntity
    {
        [Required(ErrorMessage = "Title is required")]
        [MinLength(2, ErrorMessage = "Title must be at least 2 characters long")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Date is required")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Time is required")]
        [DataType(DataType.Time)]
        public DateTime Time { get; set; }
        [Required(ErrorMessage = "Duration is required")]
        public string Duration { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [MinLength(10)]
        public string Description { get; set; }
    }
}