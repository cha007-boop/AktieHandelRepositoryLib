using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AktieHandelRepositoryLib
{
    public class AktieHandelRepositoryDB : IAktieHandelRepositoryAsync
    {
        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=AktieHandelDB;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30";

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
