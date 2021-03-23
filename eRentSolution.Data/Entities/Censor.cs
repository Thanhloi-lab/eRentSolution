using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.Data.Entities
{
    public class Censor
    {
        public int Id { get; set; }
        public int ActionId { get; set; }
        public int ProductId { get; set; }
        public Guid UserId { get; set; }
        public AppUser AppUser { get; set; }
        public AdminAction AdminAction { get; set; }
        public Product Product { get; set; }
    }
}
