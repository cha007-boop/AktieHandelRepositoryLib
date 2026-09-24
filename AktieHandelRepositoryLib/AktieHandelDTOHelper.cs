using System;
using System.Collections.Generic;
using System.Text;

namespace AktieHandelRepositoryLib
{
    public static class AktieHandelDTOHelper
    {
        public static AktieHandel DTOtoClass(AktieHandelDTO dto)
        {
            return new AktieHandel
            {
                Id = dto.Id,
                Name = dto.Name,
                Amount = dto.Amount,
                ExchangePrice = dto.ExchangePrice
            };
        }
    }
}
