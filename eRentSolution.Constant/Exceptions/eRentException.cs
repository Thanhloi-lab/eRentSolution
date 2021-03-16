using System;
using System.Collections.Generic;
using System.Text;

namespace eRentSolution.Utilities.Exceptions
{
    public class eRentException : Exception
    {
        public eRentException() { }
        public eRentException(string message) : base(message) { }
        public eRentException(string message, Exception inner) : base(message, inner) { }
    }
}
