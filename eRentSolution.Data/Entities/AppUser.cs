using eRentSolution.Data.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace eRentSolution.Data.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public DateTime Dob { get; set; }
        //public List<Censor> Censors { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Dob { get; set; }
        public List<Censor> Censors { get; set; }
        public Status Status { get; set; }
        public DateTime DateChangePassword { get; set; }
        public string AvatarFilePath { get; set; }
        public long AvatarFileSize { get; set; }
    }
}
