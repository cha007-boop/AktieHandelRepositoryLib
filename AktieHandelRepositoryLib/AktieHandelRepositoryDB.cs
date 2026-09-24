using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AktieHandelRepositoryLib
{
    public class AktieHandelRepositoryDB : IAktieHandelRepositoryAsync
    {
        private string connectionString = Connection.ConnectionString;

        public Dictionary<string, string> FilterableColumns { get; } = new Dictionary<string, string>
        {
            { "id", "Id" },
            { "name", "Name" },
            { "amount", "Amount" },
            { "exchangeprice", "Exchange Price" }
        };
        public Dictionary<string, string> SortableColumns { get; } = new Dictionary<string, string>
        {
            { "id", "Id" },
            { "name", "Name" },
            { "amount", "Amount" },
            { "exchangeprice", "Exchange Price" }
        };
        public Dictionary<string, string> ComparableColumns { get; } = new Dictionary<string, string>
        {
            { "amount", "Amount" },
            { "exchangeprice", "Exchange Price" }
        };



        /// <summary>
        /// Adds a new <see cref="AktieHandel"/> to the database and returns the added object with its generated Id.
        /// </summary>
        /// <param name="aktieHandel">The <see cref="AktieHandel"/> to add.</param>
        /// <returns>The added <see cref="AktieHandel"/> with its generated Id.</returns>
        public async Task<AktieHandel> Add(AktieHandel aktieHandel)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand(@"
                INSERT INTO AktieHandel (Name, Amount, ExchangePrice)
                VALUES (@Name, @Amount, @ExchangePrice);
                SELECT CAST(SCOPE_IDENTITY() AS INT)"
                , connection);

                cmd.Parameters.AddWithValue("@Name", aktieHandel.Name);
                cmd.Parameters.AddWithValue("@Amount", aktieHandel.Amount);
                cmd.Parameters.AddWithValue("@ExchangePrice", aktieHandel.ExchangePrice);

                await connection.OpenAsync();
                var result = await cmd.ExecuteScalarAsync();
                aktieHandel.Id = Convert.ToInt32(result);

                return aktieHandel;
            }
        }

        /// <summary>
        /// Deletes the <see cref="AktieHandel"/> with the specified Id from the database and returns the deleted object.
        /// </summary>
        /// <param name="id">The Id of the <see cref="AktieHandel"/> to delete.</param>
        /// <returns>The deleted <see cref="AktieHandel"/> or null if not found.</returns>
        public async Task<AktieHandel?> Delete(int id)
        {
            AktieHandel? aktieHandelToDelete = await GetById(id);
            if (aktieHandelToDelete == null)
            {
                return null;
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand cmd = new SqlCommand("DELETE FROM AktieHandel WHERE Id = @Id", connection);
                cmd.Parameters.AddWithValue("@Id", id);
                await connection.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            return aktieHandelToDelete;
        }

        /// <summary>
        /// Gets a list of <see cref="AktieHandel"/> objects from the database that have an ExchangePrice greater than or equal to the specified value.
        /// </summary>
        /// <param name="exchangePrice">The minimum ExchangePrice for the returned objects.</param>
        /// <param name="name">The name to filter by, or null to not filter by name.</param>
        /// <returns>A list of <see cref="AktieHandel"/> objects that match the criteria.</returns>
        public async Task<IEnumerable<AktieHandel>> Get(double exchangePrice, string? name)
        {
            List<AktieHandel> aktieHandels = new List<AktieHandel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM AktieHandel WHERE ExchangePrice >= @ExchangePrice" + (name != null ? " AND Name = @Name" : ""), connection);
                cmd.Parameters.AddWithValue("@ExchangePrice", exchangePrice);
                if (name != null)
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                }
                await connection.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        AktieHandel aktieHandel = new AktieHandel
                        {
                            Id = reader.GetInt32("Id"),
                            Name = reader.GetString("Name"),
                            Amount = reader.GetInt32("Amount"),
                            ExchangePrice = reader.GetDouble("ExchangePrice")
                        };
                        aktieHandels.Add(aktieHandel);
                    }
                }
            }
            return aktieHandels;
        }

        /// <summary>
        /// Gets all <see cref="AktieHandel"/> objects from the database.
        /// </summary>
        /// <returns>A list of all <see cref="AktieHandel"/> objects.</returns>
        public async Task<IEnumerable<AktieHandel>> GetAll()
        {
            List<AktieHandel> aktieHandels = new List<AktieHandel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM AktieHandel", connection);
                await connection.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        AktieHandel aktieHandel = new AktieHandel
                        {
                            Id = reader.GetInt32("Id"),
                            Name = reader.GetString("Name"),
                            Amount = reader.GetInt32("Amount"),
                            ExchangePrice = reader.GetDouble("ExchangePrice")
                        };
                        aktieHandels.Add(aktieHandel);
                    }
                }
                return aktieHandels;
            }
        }

        /// <summary>
        /// Gets a single <see cref="AktieHandel"/> object from the database by its Id.
        /// </summary>
        /// <param name="id">The Id of the <see cref="AktieHandel"/> to retrieve.</param>
        /// <returns>The <see cref="AktieHandel"/> object with the specified Id, or null if not found.</returns>
        public async Task<AktieHandel?> GetById(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM AktieHandel WHERE Id = @Id", connection);
                cmd.Parameters.AddWithValue("@Id", id);
                await connection.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        AktieHandel aktieHandel = new AktieHandel
                        {
                            Id = reader.GetInt32("Id"),
                            Name = reader.GetString("Name"),
                            Amount = reader.GetInt32("Amount"),
                            ExchangePrice = reader.GetDouble("ExchangePrice")
                        };
                        return aktieHandel;
                    }
                    return null;
                }
            }
        }

        /// <summary>
        /// Updates an existing <see cref="AktieHandel"/> in the database with the specified Id.
        /// </summary>
        /// <param name="id">The Id of the <see cref="AktieHandel"/> to update.</param>
        /// <param name="aktie">The updated <see cref="AktieHandel"/> object.</param>
        /// <returns>The updated <see cref="AktieHandel"/> object, or null if not found.</returns>
        public async Task<AktieHandel?> Update(int id, AktieHandel aktie)
        {
            AktieHandel? aktieHandelToUpdate = await GetById(id);
            if (aktieHandelToUpdate == null)
            {
                return null;
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE AktieHandel SET Name = @Name, Amount = @Amount, ExchangePrice = @ExchangePrice WHERE Id = @Id", connection);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Name", aktie.Name);
                    cmd.Parameters.AddWithValue("@Amount", aktie.Amount);
                    cmd.Parameters.AddWithValue("@ExchangePrice", aktie.ExchangePrice);
                    await connection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    aktie.Id = id;
                    return aktie;
                }
            }
        }

        /// <summary>
        /// Lists <see cref="AktieHandel"/> objects from the database that match the specified filter and sort criteria.
        /// </summary>
        /// <param name="filterColumn">The column to filter by.</param>
        /// <param name="filterValue">The value to filter by.</param>
        /// <param name="sortColumn">The column to sort by.</param>
        /// <param name="sortOrder">The order to sort by.</param>
        /// <returns>A list of <see cref="AktieHandel"/> objects that match the criteria.</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<IEnumerable<AktieHandel>> ListFiltered(string? filterColumn, string? filterValue, string? sortColumn, string? sortOrder)
        {
            List<AktieHandel> aktieHandels = new List<AktieHandel>();
            if (string.IsNullOrWhiteSpace(filterColumn) && string.IsNullOrWhiteSpace(sortColumn))
            {
                return await GetAll();
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand
                {
                    Connection = connection
                };
                StringBuilder queryString = new StringBuilder("Select * from AktieHandel");

                // Adding filter part of query
                if (!string.IsNullOrWhiteSpace(filterColumn))
                {
                    if (!FilterableColumns.ContainsKey(filterColumn.ToLower()))
                    {
                        throw new ArgumentException("Invalid column name");
                    }
                    if (!string.IsNullOrWhiteSpace(filterValue))
                    {
                        queryString.Append($" WHERE {filterColumn} LIKE @FilterValue");
                        cmd.Parameters.AddWithValue("@FilterValue", $"%{filterValue}%");
                    }
                }
                // Adding sort part of query
                if (!string.IsNullOrWhiteSpace(sortColumn))
                {
                    if (!SortableColumns.ContainsKey(sortColumn.ToLower()))
                    {
                        throw new ArgumentException("Invalid sort column");
                    }
                    if (string.IsNullOrWhiteSpace(sortOrder) || !sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
                    {
                        sortOrder = "ASC";
                    }
                    queryString.Append($" ORDER BY {sortColumn} {sortOrder}");
                }

                cmd.CommandText = queryString.ToString();
                await connection.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        AktieHandel aktieHandel = new AktieHandel
                        {
                            Id = reader.GetInt32("Id"),
                            Name = reader.GetString("Name"),
                            Amount = reader.GetInt32("Amount"),
                            ExchangePrice = reader.GetDouble("ExchangePrice")
                        };
                        aktieHandels.Add(aktieHandel);
                    }
                }
                return aktieHandels;

            }
        }


        /// <summary>
        /// Lists <see cref="AktieHandel"/> objects from the database that match the specified comparison and sort criteria.
        /// </summary>
        /// <param name="compareColumn">The column to compare by.</param>
        /// <param name="compareValue">The value to compare by.</param>
        /// <param name="sortColumn">The column to sort by.</param>
        /// <param name="sortOrder">The order to sort by.</param>
        /// <returns>A list of <see cref="AktieHandel"/> objects that match the criteria.</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<IEnumerable<AktieHandel>> ListComparable(string? compareColumn, double? compareValue, string? sortColumn, string? sortOrder)
        {
            List<AktieHandel> aktieHandels = new List<AktieHandel>();
            if (string.IsNullOrWhiteSpace(compareColumn) && string.IsNullOrWhiteSpace(sortColumn))
            {
                return await GetAll();
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand
                {
                    Connection = connection
                };
                StringBuilder queryString = new StringBuilder("Select * from AktieHandel");

                // Adding comparison part of query
                if (!string.IsNullOrWhiteSpace(compareColumn))
                {
                    if (!FilterableColumns.ContainsKey(compareColumn.ToLower()))
                    {
                        throw new ArgumentException("Invalid column name");
                    }
                    if (compareValue != null)
                    {
                        queryString.Append($" WHERE {compareColumn} >= @CompareValue");
                        cmd.Parameters.AddWithValue("@CompareValue", compareValue);
                    }
                }
                // Adding sort part of query
                if (!string.IsNullOrWhiteSpace(sortColumn))
                {
                    if (!SortableColumns.ContainsKey(sortColumn.ToLower()))
                    {
                        throw new ArgumentException("Invalid sort column");
                    }
                    if (string.IsNullOrWhiteSpace(sortOrder) || !sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
                    {
                        sortOrder = "ASC";
                    }
                    queryString.Append($" ORDER BY {sortColumn} {sortOrder}");
                }

                cmd.CommandText = queryString.ToString();
                await connection.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        AktieHandel aktieHandel = new AktieHandel
                        {
                            Id = reader.GetInt32("Id"),
                            Name = reader.GetString("Name"),
                            Amount = reader.GetInt32("Amount"),
                            ExchangePrice = reader.GetDouble("ExchangePrice")
                        };
                        aktieHandels.Add(aktieHandel);
                    }
                }
                return aktieHandels;

            }
        }
    }
}
