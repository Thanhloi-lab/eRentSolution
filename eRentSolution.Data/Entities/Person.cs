using eRentSolution.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.Data.Entities
{
    public class Person
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Dob { get; set; }
        public List<Censor> Censors { get; set; }
        public AppUser AppUser { get; set; }    
    }
}
