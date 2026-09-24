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
        /// Gets a list of <see cref="AktieHandel"/> objects from the database that have an ExchangePrice greater than or equal to the specified value.
        /// </summary>
        /// <param name="exchangePrice">The minimum ExchangePrice for the returned objects.</param>
        /// <param name="name">The name to filter by, or null to not filter by name.</param>
        /// <returns>A list of <see cref="AktieHandel"/> objects that match the criteria.</returns>
        Task<IEnumerable<AktieHandel>> Get(double exchangePrice, string? name);
        /// <summary>
        /// Gets all <see cref="AktieHandel"/> objects from the database.
        /// </summary>
        /// <returns>A list of all <see cref="AktieHandel"/> objects.</returns>
        Task<IEnumerable<AktieHandel>> GetAll();
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
        /// <param name="filterColumn">The column to filter by.</param>
        /// <param name="filterValue">The value to filter by.</param>
        /// <param name="sortColumn">The column to sort by.</param>
        /// <param name="sortOrder">The order to sort by.</param>
        /// <returns>A list of <see cref="AktieHandel"/> objects that match the criteria.</returns>
        /// <exception cref="ArgumentException"></exception>
        Task<IEnumerable<AktieHandel>> ListFiltered(string? filterColumn, string? filterValue, string? sortColumn, string? sortOrder);
        /// <summary>
        /// Lists <see cref="AktieHandel"/> objects from the database that match the specified comparison and sort criteria.
        /// </summary>
        /// <param name="compareColumn">The column to compare by.</param>
        /// <param name="compareValue">The value to compare by.</param>
        /// <param name="sortColumn">The column to sort by.</param>
        /// <param name="sortOrder">The order to sort by.</param>
        /// <returns>A list of <see cref="AktieHandel"/> objects that match the criteria.</returns>
        /// <exception cref="ArgumentException"></exception>
        Task<IEnumerable<AktieHandel>> ListComparable(string? compareColumn, double? compareValue, string? sortColumn, string? sortOrder);
    }
}
