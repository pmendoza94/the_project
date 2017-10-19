using System;
using System.ComponentModel.DataAnnotations;

namespace the_project.Models
{
    public class Participant : BaseEntity
    {
        [Key]
        public int Participant_Id { get; set; }
        public int Activity_Id { get; set; }
        public Activity Activity { get; set; }
        public int User_Id { get; set; }
        public User User { get; set; }
    }
}