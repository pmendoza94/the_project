using System;
using System.ComponentModel.DataAnnotations;

namespace the_project.Models
{
    public abstract class BaseEntity {}
    public class User : BaseEntity
    {
        public User(){ }
        [Key]
        public int User_Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        
    }
}