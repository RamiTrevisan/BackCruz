using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public enum PaymentMethod
    {
        Cash = 1,
        CreditCard = 2,
        DebitCard = 3,
        MobilePayment = 4,
        OnlineTransfer = 5
    }
}
