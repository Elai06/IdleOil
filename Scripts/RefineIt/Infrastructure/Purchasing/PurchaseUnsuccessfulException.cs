using System;
using Constants;

namespace Infrastructure.Purchasing
{
    public class PurchaseUnsuccessfulException : Exception
    {
        public PurchaseUnsuccessfulException() : base(Exceptions.PURCHASE_UNSUCCESSFUL)
        {
        }
        
        public PurchaseUnsuccessfulException(string message) : base(message 
                                                                    ?? Exceptions.PURCHASE_UNSUCCESSFUL)
        {
        }
    }
}
