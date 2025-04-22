using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public enum OrderStatus
    {
        Pending = 1,
        Confirmed = 2,
        InPreparation = 3,
        ReadyForDelivery = 4,
        InDelivery = 5,
        Delivered = 6,
        Cancelled = 7
    }
}
