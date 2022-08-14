using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.Data.Entities
{
    public class Censor
    {
        public int Id { get; set; }
        public int ActionId { get; set; }
        public int NewsId { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public AppUser User { get; set; }
        public UserAction AdminAction { get; set; }
        public News News { get; set; }
    }
}
