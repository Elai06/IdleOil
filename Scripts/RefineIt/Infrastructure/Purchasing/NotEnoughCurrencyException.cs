using System;
using Constants;

namespace Infrastructure.Purchasing
{
    class NotEnoughCurrencyException : Exception
    {
        public NotEnoughCurrencyException() : base(Exceptions.NOT_ENOUGH_CURRENCY)
        {
        }
    }
}
