using System;
using System.Collections.Generic;
using System.Text;

namespace AktieHandelRepositoryLib
{
    public record AktieHandelDTO(
        int Id,
        string Name,
        int Amount,
        double ExchangePrice
    )
    {
    }
}
