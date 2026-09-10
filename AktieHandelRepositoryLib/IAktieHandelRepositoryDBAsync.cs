using System;
using System.Collections.Generic;
using System.Text;

namespace AktieHandelRepositoryLib
{
    public interface IAktieHandelRepositoryDBAsync
    {
        Task<AktieHandel> Add(AktieHandel aktieHandel);
        Task<AktieHandel?> Delete(int id);
        Task<IEnumerable<AktieHandel>> Get(double exchangePrice, string? name);
        Task<IEnumerable<AktieHandel>> GetAll();
        Task<AktieHandel?> GetById(int id);
        Task<AktieHandel?> Update(int id, AktieHandel aktie);
    }
}
