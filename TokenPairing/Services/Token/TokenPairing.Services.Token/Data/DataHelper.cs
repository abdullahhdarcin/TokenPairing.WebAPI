using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using TokenPairing.Services.Token.Model;
using TokenPairing.Services.Token.Shared.Dtos;

namespace TokenPairing.Services.Token.Data
{
    public class DataHelper : IDataHelper
    {
        private readonly IConfiguration _configuration;

        public DataHelper(IConfiguration configuration)
        {
           _configuration = configuration;
        }

        public List<TokenModel> GetTokenByMemberId(string member)
        {
            var getTokenByMemberId = new List<TokenModel>();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    sqlConnection.Open();
                    SqlCommand getcmd = new SqlCommand($"SELECT MEMBER_ID,CREATED,DS_LAST_CHANGE,EMPUSH_TOKEN,STATUS,CUSTOMER_ID,EMPUSH_APP_ID FROM DATABASE.dbo.TABLE " +
                        $"WHERE MEMBER_ID='{member}'", sqlConnection);
                    getcmd.CommandType = CommandType.Text;
                    SqlDataReader reader = getcmd.ExecuteReader();


                    while (reader.Read())
                    {
                        getTokenByMemberId.Add(new TokenModel
                        {
                            MEMBER_ID = reader.IsDBNull(reader.GetOrdinal("MEMBER_ID"))? string.Empty : reader.GetString("MEMBER_ID").Trim(),
                            CREATED = reader.IsDBNull(reader.GetOrdinal("CREATED")) ? DateTime.MinValue : reader.GetDateTime("CREATED"),
                            DS_LAST_CHANGE = reader.IsDBNull(reader.GetOrdinal("DS_LAST_CHANGE")) ? DateTime.MinValue : reader.GetDateTime("DS_LAST_CHANGE"),
                            EMPUSH_TOKEN = reader.IsDBNull(reader.GetOrdinal("EMPUSH_TOKEN")) ? string.Empty : reader.GetString("EMPUSH_TOKEN"),
                            STATUS = reader.IsDBNull(reader.GetOrdinal("STATUS")) ? string.Empty : reader.GetString("STATUS"),
                            EMPUSH_APP_ID = reader.IsDBNull(reader.GetOrdinal("EMPUSH_APP_ID")) ? Guid.Empty.ToString() : reader.GetGuid("EMPUSH_APP_ID").ToString(),
                            CUSTOMER_ID = reader.IsDBNull(reader.GetOrdinal("CUSTOMER_ID")) ? string.Empty : reader.GetString("CUSTOMER_ID").Trim()
                        });
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return getTokenByMemberId;


        }


        public Response<TokenModel> UpdateToken (string memberId, string empush_token, string customer_id)
        {

            var empushModel = new Response<TokenModel>();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    sqlConnection.Open();

                    SqlCommand updatecmd = new SqlCommand($"UPDATE DATABASE.dbo.TABLE SET MEMBER_ID='{memberId}' " +
                        $"WHERE EMPUSH_TOKEN='{empush_token}' AND CUSTOMER_ID='{customer_id}'", sqlConnection);

                    updatecmd.CommandType = CommandType.Text;

                    SqlCommand isPassive = new SqlCommand($"SELECT STATUS,CUSTOMER_ID FROM DATABASE.dbo.TABLE WHERE STATUS='X' " +
                        $" AND EMPUSH_TOKEN='{empush_token}'", sqlConnection);
                    isPassive.CommandType = CommandType.Text;
                    SqlDataReader readerPassive = isPassive.ExecuteReader();

                    while (readerPassive.Read())
                    {
                        if (readerPassive.GetString(readerPassive.GetOrdinal("STATUS")) == "X")
                        {
                            return Response<TokenModel>.Fail("Token is not active. Please check token status.", 422);
                        }
                    }

                    SqlCommand isTokenFound = new SqlCommand($"SELECT COUNT(*) FROM DATABASE.dbo.TABLE WHERE" +
                        $" EMPUSH_TOKEN='{empush_token}'", sqlConnection);
                    Int32 count = (Int32)isTokenFound.ExecuteScalar();
                    SqlDataReader readerFound = isTokenFound.ExecuteReader();

                        if (count < 1)
                        {
                            return Response<TokenModel>.Fail("Token is not found.", 404);
                        }


                    SqlCommand isActive = new SqlCommand($"SELECT STATUS,CUSTOMER_ID,EMPUSH_TOKEN FROM DATABASE.dbo.TABLE WHERE" +
                        $" STATUS='A' AND EMPUSH_TOKEN='{empush_token}'", sqlConnection);
                    isActive.CommandType = CommandType.Text;
                    SqlDataReader readerActive = isActive.ExecuteReader();

                    while (readerActive.Read())
                    {
                        if (readerActive.GetString(readerActive.GetOrdinal("CUSTOMER_ID")).Trim() != customer_id)
                        {
                            return Response<TokenModel>.Fail("Customer ID is not true. Please check Customer ID.", 400);
                        }

                        if (readerActive.GetString(readerActive.GetOrdinal("STATUS")) == "A")
                        {
                            updatecmd.ExecuteNonQuery();
                            return Response<TokenModel>.Fail("Token paired. Thank you ! ", 200);
                        }
                    }



                }
            }
            catch (Exception)
            {
                throw;
            }

            return empushModel;
        }

    }
}
