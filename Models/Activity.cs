using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace the_project.Models
{
    public class Activity : BaseEntity
    {
        [Key]
        public int Activity_Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public string Duration { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public string Coordinator { get; set; }
        public int User_Id { get; set; }
        public List<Participant> Participants { get; set; }

        public Activity ()
        {
            Participants = new List<Participant>();
        }
    }
}