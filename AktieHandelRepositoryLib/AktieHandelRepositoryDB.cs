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
        /// Deletes the <see cref="AktieHandel"/> with the specified Id from the database and returns the deleted object. If no object with the specified Id exists, returns null.
        /// </summary>
        /// <param name="id">The Id of the <see cref="AktieHandel"/> to delete.</param>
        /// <returns>The deleted <see cref="AktieHandel"/> or null if not found.</returns>
        public async Task<AktieHandel?> Delete(int id)
        {
            AktieHandel aktieHandelToDelete = await GetById(id);
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
        /// Gets a list of <see cref="AktieHandel"/> objects from the database that have an ExchangePrice greater than or equal to the specified value. If a name is provided, it will also filter by that name.
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
        /// Gets a single <see cref="AktieHandel"/> object from the database by its Id. If no object with the specified Id exists, returns null.
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
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public async Task<AktieHandel?> Update(int id, AktieHandel aktie)
        {
            if (GetById(id) == null)
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
                    return await GetById(id);
                }
            }
        }
    }
}
