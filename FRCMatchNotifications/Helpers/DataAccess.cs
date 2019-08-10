using FRCMatchNotifications.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace FRCMatchNotifications.Helpers
{
    public class DataAccess
    {
        private static string ConnectionString
        {
            get
            {
                string retVal = "";
                try
                {
                    string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "keys.json");
                    if (File.Exists(filepath))
                    {
                        var json = File.ReadAllText(filepath);
                        dynamic data = JsonConvert.DeserializeObject(json);
                        retVal = data.connection_strings.petz_db;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                return retVal;
            }
        }

        private static string AirtableURL
        {
            get
            {
                string retVal = "";
                try
                {
                    string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "keys.json");
                    if (File.Exists(filepath))
                    {
                        var json = File.ReadAllText(filepath);
                        dynamic data = JsonConvert.DeserializeObject(json);
                        retVal = data.airtable.url;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                return retVal;
            }
        }
        private static string AirtableAPIKey
        {
            get
            {
                string retVal = "";
                try
                {
                    string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "keys.json");
                    if (File.Exists(filepath))
                    {
                        var json = File.ReadAllText(filepath);
                        dynamic data = JsonConvert.DeserializeObject(json);
                        retVal = data.airtable.api_key;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                return retVal;
            }
        }

        private static string NumberAndTeamToAirtableAPIJson(string number, string team)
        {
            StringBuilder retVal = new StringBuilder();
            retVal.Append($"'fields':{{'Phone Number':'{number}','Team Number':{team}}}");
            return retVal.ToString();
        }

        public static List<string> GetNumbersForTeamsDB(string team1,
                                                string team2 = "",
                                                string team3 = "",
                                                string team4 = "",
                                                string team5 = "",
                                                string team6 = "")
        {
            List<string> retVal = new List<string>();

            try
            {
                using (var con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("FRCNotifications.dbo.usp_GetNumbersForTeams", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.Add(new SqlParameter("@team1", team1));
                    cmd.Parameters.Add(new SqlParameter("@team2", team2));
                    cmd.Parameters.Add(new SqlParameter("@team3", team3));
                    cmd.Parameters.Add(new SqlParameter("@team4", team4));
                    cmd.Parameters.Add(new SqlParameter("@team5", team5));
                    cmd.Parameters.Add(new SqlParameter("@team6", team6));

                    using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                retVal.Add(dr.GetString(0));
                            }
                        }
                    }

                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return retVal;
        }

        public static List<string> GetNumbersForTeams(string team1,
                                                string team2 = "",
                                                string team3 = "",
                                                string team4 = "",
                                                string team5 = "",
                                                string team6 = "")
        {
            List<string> retVal = new List<string>();

            try
            {
                using (var con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("FRCNotifications.dbo.usp_GetNumbersForTeams", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.Add(new SqlParameter("@team1", team1));
                    cmd.Parameters.Add(new SqlParameter("@team2", team2));
                    cmd.Parameters.Add(new SqlParameter("@team3", team3));
                    cmd.Parameters.Add(new SqlParameter("@team4", team4));
                    cmd.Parameters.Add(new SqlParameter("@team5", team5));
                    cmd.Parameters.Add(new SqlParameter("@team6", team6));

                    using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                retVal.Add(dr.GetString(0));
                            }
                        }
                    }

                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return retVal;
        }

        public static int InsertNumberForTeamDB(string number, string team)
        {
            int retVal = 0;
            number = number.Replace("-", "").Replace("(", "").Replace(")", "");

            if (number.Length > 11) throw new ArgumentException("Phone number is too long.");
            if (!String.Equals(team.Substring(0, 3), "frc")) team = "frc" + team;

            try
            {
                using (var con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("FRCNotifications.dbo.usp_InsertNumberWithTeam", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.Add(new SqlParameter("@number", number));
                    cmd.Parameters.Add(new SqlParameter("@team", team));

                    retVal = cmd.ExecuteNonQuery();

                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return retVal;
        }

        public static int InsertNumberForTeam(string number, string team)
        {
            int retVal = 0;
            number = number.Replace("-", "").Replace("(", "").Replace(")", "");

            if (number.Length > 11) throw new ArgumentException("Phone number is too long.");
            //if (!String.Equals(team.Substring(0, 3), "frc")) team = "frc" + team;

            try
            {
                var client = new RestClient(AirtableURL);
                

                var request = new RestRequest("resource/{id}", Method.POST);
                request.AddParameter("name", "value"); // adds to POST or URL querystring based on Method
                request.AddUrlSegment("id", "123"); // replaces matching token in request.Resource

                request.AddParameter("Authorization", $"Bearer {AirtableAPIKey}", ParameterType.HttpHeader);
                request.AddParameter("Content-type", "application/json", ParameterType.HttpHeader);
                request.AddJsonBody(JsonConvert.SerializeObject(NumberAndTeamToAirtableAPIJson(number, team)));

                // execute the request
                IRestResponse response = client.Execute(request);
                var content = response.Content; // raw content as string
                
                using (var con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("FRCNotifications.dbo.usp_InsertNumberWithTeam", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.Add(new SqlParameter("@number", number));
                    cmd.Parameters.Add(new SqlParameter("@team", team));

                    retVal = cmd.ExecuteNonQuery();

                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return retVal;
        }
    }
}