using System;
using System.Collections.Generic;
using System.Text;

namespace AktieHandelRepositoryLib
{
    public interface IAktieHandelRepositoryAsync
    {
        /// <summary>
        /// Adds a new <see cref="AktieHandel"/> to the database and returns the added object with its generated Id.
        /// </summary>
        /// <param name="aktieHandel">The <see cref="AktieHandel"/> to add.</param>
        /// <returns>The added <see cref="AktieHandel"/> with its generated Id.</returns>
        Task<AktieHandel> Add(AktieHandel aktieHandel);
        /// <summary>
        /// Deletes the <see cref="AktieHandel"/> with the specified Id from the database and returns the deleted object.
        /// </summary>
        /// <param name="id">The Id of the <see cref="AktieHandel"/> to delete.</param>
        /// <returns>The deleted <see cref="AktieHandel"/> or null if not found.</returns>
        Task<AktieHandel?> Delete(int id);
        /// <summary>
        /// Gets a single <see cref="AktieHandel"/> object from the database by its Id.
        /// </summary>
        /// <param name="id">The Id of the <see cref="AktieHandel"/> to retrieve.</param>
        /// <returns>The <see cref="AktieHandel"/> object with the specified Id, or null if not found.</returns>
        Task<AktieHandel?> GetById(int id);
        /// <summary>
        /// Updates an existing <see cref="AktieHandel"/> in the database with the specified Id.
        /// </summary>
        /// <param name="id">The Id of the <see cref="AktieHandel"/> to update.</param>
        /// <param name="aktie">The updated <see cref="AktieHandel"/> object.</param>
        /// <returns>The updated <see cref="AktieHandel"/> object, or null if not found.</returns>
        Task<AktieHandel?> Update(int id, AktieHandel aktie);
        /// <summary>
        /// Lists <see cref="AktieHandel"/> objects from the database that match the specified filter and sort criteria.
        /// </summary>
        /// <param name="id">The Id to filter by, or null to not filter by Id.</param>
        /// <param name="name">The name to filter by, or null to not filter by name.</param>
        /// <param name="maxExchangePrice">The maximum ExchangePrice to filter by, or null to not filter by ExchangePrice.</param>
        /// <param name="maxAmount">The maximum Amount to filter by, or null to not filter by Amount.</param>
        /// <param name="sortBy">The column to sort by.</param>
        /// <param name="sortOrder">The order to sort by.</param>
        /// <returns>A list of <see cref="AktieHandel"/> objects that match the criteria.</returns>
        /// <exception cref="ArgumentException"></exception>
        Task<IEnumerable<AktieHandel>> GetAll(int? id = null, string? name = null, double? maxExchangePrice = null, double? minExchangePrice = null, int? maxAmount = null, int? minAmount = null, string? sortBy = null, string? sortOrder = null);
    }
}
