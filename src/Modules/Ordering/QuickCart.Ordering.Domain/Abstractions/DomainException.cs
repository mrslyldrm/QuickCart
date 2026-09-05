using System;
using System.Collections.Generic;
using System.Text;

namespace QuickCart.Ordering.Domain.Abstractions
{
    public sealed class DomainException : Exception
    {
        public DomainException(string message) : base(message)
        {
        }
    }
}
