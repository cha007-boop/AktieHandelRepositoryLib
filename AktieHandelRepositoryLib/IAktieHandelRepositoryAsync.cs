using System;
using System.Collections.Generic;
using System.Text;

namespace AktieHandelRepositoryLib
{
    public interface IAktieHandelRepositoryAsync
    {
        Task<AktieHandel> Add(AktieHandel aktieHandel);
        Task<AktieHandel?> Delete(int id);
        Task<IEnumerable<AktieHandel>> Get(double exchangePrice, string? name);
        Task<IEnumerable<AktieHandel>> GetAll();
        Task<AktieHandel?> GetById(int id);
        Task<AktieHandel?> Update(int id, AktieHandel aktie);
        Task<IEnumerable<AktieHandel>> ListFiltered(string? filterColumn, string? filterValue, string? sortColumn, string? sortOrder);
        Task<IEnumerable<AktieHandel>> ListComparable(string? compareColumn, double? compareValue, string? sortColumn, string? sortOrder);
    }
}
