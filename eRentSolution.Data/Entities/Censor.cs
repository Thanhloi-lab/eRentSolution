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
        public int PersonId { get; set; }
        public DateTime Date { get; set; }
        public Person Person { get; set; }
        public UserAction AdminAction { get; set; }
        public Product Product { get; set; }
    }
}
