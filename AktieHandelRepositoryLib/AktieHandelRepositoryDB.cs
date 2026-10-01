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


        public Dictionary<string, string> SortableColumns { get; } = new Dictionary<string, string>
        {
            { "id", "Id" },
            { "name", "Name" },
            { "amount", "Amount" },
            { "exchangeprice", "Exchange Price" }
        };




        /// <inheritdoc/>
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

        /// <inheritdoc/>
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
        #region Old GetAll
        ///// <summary>
        ///// Gets all <see cref="AktieHandel"/> objects from the database.
        ///// </summary>
        ///// <returns>A list of all <see cref="AktieHandel"/> objects.</returns>
        //public async Task<IEnumerable<AktieHandel>> GetAll()
        //{
        //    List<AktieHandel> aktieHandels = new List<AktieHandel>();
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        SqlCommand cmd = new SqlCommand("SELECT * FROM AktieHandel", connection);
        //        await connection.OpenAsync();
        //        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
        //        {
        //            while (await reader.ReadAsync())
        //            {
        //                AktieHandel aktieHandel = new AktieHandel
        //                {
        //                    Id = reader.GetInt32("Id"),
        //                    Name = reader.GetString("Name"),
        //                    Amount = reader.GetInt32("Amount"),
        //                    ExchangePrice = reader.GetDouble("ExchangePrice")
        //                };
        //                aktieHandels.Add(aktieHandel);
        //            }
        //        }
        //        return aktieHandels;
        //    }
        //}
        #endregion

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public async Task<IEnumerable<AktieHandel>> GetAll(int? id = null,
                                                           string? name = null,
                                                           double? maxExchangePrice = null,
                                                           double? minExchangePrice = null,
                                                           int? maxAmount = null,
                                                           int? minAmount = null,
                                                           string? sortBy = null,
                                                           string? sortOrder = null)
        {
            List<AktieHandel> aktieHandels = new List<AktieHandel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand
                {
                    Connection = connection
                };
                StringBuilder queryString = new StringBuilder("Select * from AktieHandel");

                // Adding filter part of query
                if (id.HasValue || !string.IsNullOrWhiteSpace(name) || 
                    maxExchangePrice.HasValue || minExchangePrice.HasValue || 
                    maxAmount.HasValue || minAmount.HasValue)
                {
                    queryString.Append(" WHERE ");
                    List<string> conditions = new List<string>();
                    if (id.HasValue)
                    {
                        conditions.Add("Id = @Id");
                        cmd.Parameters.AddWithValue("@Id", id.Value);
                    }
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        conditions.Add("Name = @Name");
                        cmd.Parameters.AddWithValue("@Name", name);
                    }
                    if (maxExchangePrice.HasValue)
                    {
                        conditions.Add("ExchangePrice <= @maxExchangePrice");
                        cmd.Parameters.AddWithValue("@maxExchangePrice", maxExchangePrice.Value);
                    }
                    if (minExchangePrice.HasValue)
                    {
                        conditions.Add("ExchangePrice >= @minExchangePrice");
                        cmd.Parameters.AddWithValue("@minExchangePrice", minExchangePrice.Value);
                    }
                    if (maxAmount.HasValue)
                    {
                        conditions.Add("Amount <= @maxAmount");
                        cmd.Parameters.AddWithValue("@maxAmount", maxAmount.Value);
                    }
                    if (minAmount.HasValue)
                    {
                        conditions.Add("Amount >= @minAmount");
                        cmd.Parameters.AddWithValue("@minAmount", minAmount.Value);
                    }
                    queryString.Append(string.Join(" AND ", conditions));
                }
                // Adding sort part of query
                if (!string.IsNullOrWhiteSpace(sortBy))
                {
                    if (!SortableColumns.ContainsKey(sortBy.ToLower()))
                    {
                        throw new ArgumentException("Invalid sort column");
                    }
                    if (string.IsNullOrWhiteSpace(sortOrder) || !sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
                    {
                        sortOrder = "ASC";
                    }
                    queryString.Append($" ORDER BY {sortBy} {sortOrder}");
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
