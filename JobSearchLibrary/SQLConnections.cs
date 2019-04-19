using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using JobSearchLibrary.Entities;


namespace JobSearchLibrary
{
    public class SQLConnections
    {
        public string strConn = @"Data Source={ComputerName}\{ServerName};Database=JobSearch;integrated security=SSPI;MultipleActiveResultSets=true;";
        //public string strConn = @"Data Source={Local sql name};Initial Catalog=JobSearch;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        #region Insert Query Statements


        public void NewJob(Jobs job)
        {
            string sqlQuery;
            sqlQuery = job.RecruiterId.HasValue ? "INSERT INTO[Jobs] (CompanyName, PositionID, LocationID, SalaryRange, RatingID, CEOName, MissionStatement, Benefits, Comments, RecruiterID, JobLink, Date)" +
                " VALUES (@CompanyName, @PositionID, @LocationID, @SalaryRange, @RatingID, @CEOName,@MissionStatement,@Benefits,@Comments,@RecruiterID,@JobLink, @Date)" : "INSERT INTO[Jobs] (CompanyName, PositionID, LocationID, SalaryRange, RatingID, CEOName, MissionStatement, Benefits, Comments, JobLink, Date)" +
                " VALUES (@CompanyName, @PositionID, @LocationID, @SalaryRange, @RatingID, @CEOName,@MissionStatement,@Benefits,@Comments,@JobLink, @Date)";
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(strConn))
                {
                    SqlCommand sqlComm = new SqlCommand(sqlQuery, sqlConnection);

                    sqlComm.Parameters.AddWithValue("@CompanyName", job.CompanyName);
                    sqlComm.Parameters.AddWithValue("@PositionID", job.PositionId);
                    sqlComm.Parameters.AddWithValue("@LocationID", job.LocationId);
                    sqlComm.Parameters.AddWithValue("@SalaryRange", job.SalaryRange);
                    sqlComm.Parameters.AddWithValue("@RatingID", job.RatingId);
                    sqlComm.Parameters.AddWithValue("@CEOName", job.CEOName);
                    sqlComm.Parameters.AddWithValue("@MissionStatement", job.MissionStatement);
                    sqlComm.Parameters.AddWithValue("@Benefits", job.Benefits);
                    sqlComm.Parameters.AddWithValue("@Comments", job.Comments);
                    sqlComm.Parameters.AddWithValue("@JobLink", job.JobLink);
                    sqlComm.Parameters.AddWithValue("@Date", System.DateTime.Now);
                    if (job.RecruiterId.HasValue)
                    {
                        sqlComm.Parameters.AddWithValue("@RecruiterID", job.RecruiterId);
                    }

                    sqlComm.Connection = sqlConnection;
                    sqlConnection.Open();
                    sqlComm.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void AddLocation(Location location)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(strConn))
                {
                    SqlCommand sqlComm = new SqlCommand("INSERT INTO Location VALUES (@City,@StateID,@Rating,@Notes)", sqlConnection);

                    sqlComm.Parameters.AddWithValue("@City", location.City);
                    sqlComm.Parameters.AddWithValue("@StateID", location.StateId);
                    sqlComm.Parameters.AddWithValue("@Rating", location.CityRating);
                    sqlComm.Parameters.AddWithValue("@Notes", location.Notes);

                    sqlComm.Connection = sqlConnection;
                    sqlConnection.Open();
                    sqlComm.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void AddPosition(string newPosition)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(strConn))
                {
                    SqlCommand sqlComm = new SqlCommand("INSERT INTO Position VALUES (@Position)", sqlConnection);

                    sqlComm.Parameters.AddWithValue("@Position", newPosition);



                    sqlComm.Connection = sqlConnection;
                    sqlConnection.Open();
                    sqlComm.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public int AddRecruiterGetID(Recruiter recruiter)
        {
            try
            {
                int recruiterID;
                using (SqlConnection sqlConnection = new SqlConnection(strConn))
                {
                    string identity = "Select @@Identity";
                    SqlCommand sqlComm = new SqlCommand("INSERT INTO Recruiters VALUES (@RecruiterName,@Email,@PhoneNumber,@LinkedIn)", sqlConnection);

                    sqlComm.Parameters.AddWithValue("@RecruiterName", recruiter.RecruiterName);
                    sqlComm.Parameters.AddWithValue("@Email", recruiter.Email);
                    sqlComm.Parameters.AddWithValue("@PhoneNumber", recruiter.PhoneNumber);
                    sqlComm.Parameters.AddWithValue("@LinkedIn", recruiter.LinkedInLink);

                    sqlComm.Connection = sqlConnection;
                    sqlConnection.Open();
                    sqlComm.ExecuteNonQuery();
                    sqlComm.CommandText = identity;
                    recruiterID = int.Parse(sqlComm.ExecuteScalar().ToString());
                }
                return recruiterID;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region Collect list data


        public List<string> FillComboBoxFromDatabase(string sqlQuery)
        {
            try
            {
                List<string> columnData = new List<string>();
                using (SqlConnection sqlConnection = new SqlConnection(strConn))
                {
                    SqlCommand sqlComm = new SqlCommand(sqlQuery, sqlConnection);

                    sqlComm.Connection = sqlConnection;
                    sqlConnection.Open();
                    using (SqlDataReader reader = sqlComm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            columnData.Add(reader.GetString(0));
                        }
                    }
                }
                return columnData;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public Dictionary<int, string> FillListBoxFromDatabase()
        {
            try
            {
                string sqlQuery = "SELECT CompanyID, CompanyName FROM Jobs ORDER BY CompanyID DESC";
                Dictionary<int, string> columnData = new Dictionary<int, string>();
                using (SqlConnection sqlConnection = new SqlConnection(strConn))
                {
                    SqlCommand sqlComm = new SqlCommand(sqlQuery, sqlConnection);

                    sqlComm.Connection = sqlConnection;
                    sqlConnection.Open();
                    using (SqlDataReader reader = sqlComm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            columnData.Add(int.Parse(reader["CompanyID"].ToString()), reader["CompanyName"].ToString());
                        }
                    }
                }
                return columnData;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        #endregion

        #region Collect Job Information

        public List<Jobs> GetAllJobs()
        {
            List<Jobs> allJobs = new List<Jobs>();
            using (SqlConnection sqlConnection = new SqlConnection(strConn))
            {
                SqlCommand sqlComm = new SqlCommand("SELECT CompanyID, CompanyName,LocationID,PositionId,SalaryRange,RatingId,CEOName,MissionStatement,Benefits,Comments, JobLink, RecruiterID, Date FROM Jobs Order by CompanyId Desc", sqlConnection);


                sqlComm.Connection = sqlConnection;
                sqlConnection.Open();

                SqlDataReader reader = sqlComm.ExecuteReader();
                while (reader.Read())
                {
                    Jobs jobs = new Jobs()
                    {
                        CompanyId = int.Parse(reader["CompanyId"].ToString()),
                        CompanyName = reader["CompanyName"].ToString(),
                        LocationId = int.Parse(reader["LocationId"].ToString()),
                        PositionId = int.Parse(reader["PositionId"].ToString()),
                        SalaryRange = reader["SalaryRange"].ToString(),
                        RatingId = int.Parse(reader["RatingId"].ToString()),
                        CEOName = reader["CEOName"].ToString(),
                        MissionStatement = reader["MissionStatement"].ToString(),
                        Benefits = reader["Benefits"].ToString(),
                        Comments = reader["Comments"].ToString(),
                        JobLink = reader["JobLink"].ToString(),
                        Date = (DateTime)reader["Date"]
                    };
                    if (!string.IsNullOrEmpty(reader["RecruiterID"].ToString()))
                    {
                        jobs.RecruiterId = int.Parse(reader["RecruiterID"].ToString());
                    }

                    allJobs.Add(jobs);
                }
                return allJobs;
            }
        }

        public List<Location> GetAllLocations()
        {
            List<Location> allLocations = new List<Location>();
            using (SqlConnection sqlConnection = new SqlConnection(strConn))
            {
                SqlCommand sqlComm = new SqlCommand("SELECT LocationId, City, StateId,CityRating,Notes FROM Location", sqlConnection);


                sqlComm.Connection = sqlConnection;
                sqlConnection.Open();

                SqlDataReader reader = sqlComm.ExecuteReader();

                while (reader.Read())
                {
                    Location location = new Location()
                    {
                        LocationId = int.Parse(reader["LocationID"].ToString()),
                        City = reader["City"].ToString(),
                        StateId = int.Parse(reader["StateId"].ToString()),
                        CityRating = int.Parse(reader["CityRating"].ToString()),
                        Notes = reader["Notes"].ToString()
                    };

                    allLocations.Add(location);
                }
                return allLocations;
            }
        }

        public List<Position> GetAllPositions()
        {
            List<Position> allPositions = new List<Position>();
            using (SqlConnection sqlConnection = new SqlConnection(strConn))
            {
                SqlCommand sqlComm = new SqlCommand("SELECT PositionId, JobTitle FROM Position", sqlConnection);


                sqlComm.Connection = sqlConnection;
                sqlConnection.Open();

                SqlDataReader reader = sqlComm.ExecuteReader();

                while (reader.Read())
                {
                    Position position = new Position()
                    {
                        PositionID = int.Parse(reader["PositionID"].ToString()),
                        JobTitle = reader["JobTitle"].ToString(),

                    };

                    allPositions.Add(position);
                }
                return allPositions;
            }
        }

        public List<States> GetAllStates()
        {
            List<States> allStates = new List<States>();
            using (SqlConnection sqlConnection = new SqlConnection(strConn))
            {
                SqlCommand sqlComm = new SqlCommand("SELECT StateId, State, Capital, StateAbbreviation, LargestCity FROM States", sqlConnection);


                sqlComm.Connection = sqlConnection;
                sqlConnection.Open();

                SqlDataReader reader = sqlComm.ExecuteReader();

                while (reader.Read())
                {
                    States state = new States()
                    {
                        StateID = int.Parse(reader["StateID"].ToString()),
                        State = reader["State"].ToString(),
                        Capital = reader["Capital"].ToString(),
                        LargestCity = reader["LargestCity"].ToString(),
                        StateAbbreviation = reader["StateAbbreviation"].ToString()

                    };

                    allStates.Add(state);
                }
                return allStates;
            }
        }

        public List<Rating> GetAllRatings()
        {
            List<Rating> allRatings = new List<Rating>();
            using (SqlConnection sqlConnection = new SqlConnection(strConn))
            {
                SqlCommand sqlComm = new SqlCommand("SELECT RatingDescription FROM Rating", sqlConnection);

                sqlComm.Connection = sqlConnection;
                sqlConnection.Open();

                SqlDataReader reader = sqlComm.ExecuteReader();

                while (reader.Read())
                {
                    Rating rating = new Rating
                    {
                        RatingDescription = reader["RatingDescription"].ToString()
                    };
                    allRatings.Add(rating);
                }
                return allRatings;
            }
        }

        public List<Recruiter> GetAllRecruiters()
        {
            List<Recruiter> allRecruiters = new List<Recruiter>();
            using (SqlConnection sqlConnection = new SqlConnection(strConn))
            {
                SqlCommand sqlComm = new SqlCommand("SELECT RecruiterID, Recruiter_Name,Email, PhoneNumber,LinkedInLink FROM Recruiters", sqlConnection);

                sqlComm.Connection = sqlConnection;
                sqlConnection.Open();

                SqlDataReader reader = sqlComm.ExecuteReader();

                while (reader.Read())
                {
                    Recruiter recruiter = new Recruiter()
                    {
                        RecruiterId = int.Parse(reader["RecruiterID"].ToString()),
                        Email = reader["Email"].ToString(),
                        RecruiterName = reader["Recruiter_Name"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString(),
                        LinkedInLink = reader["LinkedInLink"].ToString()
                    };
                    allRecruiters.Add(recruiter);
                }
                return allRecruiters;
            }
        }
        #endregion

        #region Select Statements

        public int FindPositionID(string position)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(strConn))
                {
                    SqlCommand sqlComm = new SqlCommand("SELECT positionID FROM Position where JobTitle = @Position", sqlConnection);

                    sqlComm.Parameters.AddWithValue("@Position", position);


                    sqlComm.Connection = sqlConnection;
                    sqlConnection.Open();
                    var x = sqlComm.ExecuteScalar();
                    int positionID = 0;
                    if (x != null)
                    { positionID = (int)x; }


                    return positionID;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetStateName(int id)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(strConn))
                {
                    SqlCommand sqlComm = new SqlCommand("SELECT State FROM States where StateId = @id", sqlConnection);

                    sqlComm.Parameters.AddWithValue("@id", id);


                    sqlComm.Connection = sqlConnection;
                    sqlConnection.Open();
                    var x = sqlComm.ExecuteScalar();
                    string stateName = x.ToString();


                    return stateName;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetPositionName(int? id)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(strConn))
                {
                    SqlCommand sqlComm = new SqlCommand("SELECT JobTitle FROM Position where PositionId = @id", sqlConnection);

                    sqlComm.Parameters.AddWithValue("@id", id);


                    sqlComm.Connection = sqlConnection;
                    sqlConnection.Open();
                    var x = sqlComm.ExecuteScalar();
                    string positionName = x.ToString();


                    return positionName;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetCityName(int? id)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(strConn))
                {
                    SqlCommand sqlComm = new SqlCommand("SELECT City FROM Location where LocationId = @id", sqlConnection);

                    sqlComm.Parameters.AddWithValue("@id", id);


                    sqlComm.Connection = sqlConnection;
                    sqlConnection.Open();
                    var x = sqlComm.ExecuteScalar();
                    string cityName = x.ToString();


                    return cityName;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetRatingString(int? id)
        {

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(strConn))
                {
                    SqlCommand sqlComm = new SqlCommand("SELECT RatingDescription FROM Rating where RatingId = @id", sqlConnection);

                    sqlComm.Parameters.AddWithValue("@id", id);

                    sqlComm.Connection = sqlConnection;
                    sqlConnection.Open();
                    var x = sqlComm.ExecuteScalar();
                    string ratingName = x.ToString();



                    return ratingName;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetLargestCity(int? id)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(strConn))
                {
                    SqlCommand sqlComm = new SqlCommand("SELECT LargestCity FROM States where StateId = @id", sqlConnection);

                    sqlComm.Parameters.AddWithValue("@id", id);


                    sqlComm.Connection = sqlConnection;
                    sqlConnection.Open();
                    var x = sqlComm.ExecuteScalar();
                    string cityName = x.ToString();


                    return cityName;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetStateCapital(int? id)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(strConn))
                {
                    SqlCommand sqlComm = new SqlCommand("SELECT Capital FROM States where StateId = @id", sqlConnection);

                    sqlComm.Parameters.AddWithValue("@id", id);


                    sqlComm.Connection = sqlConnection;
                    sqlConnection.Open();
                    var x = sqlComm.ExecuteScalar();
                    string cityName = x.ToString();



                    return cityName;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetCompanyRatingID(string rating)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(strConn))
                {
                    SqlCommand sqlComm = new SqlCommand("SELECT RatingID FROM Rating where RatingDescription = @rating", sqlConnection);

                    sqlComm.Parameters.AddWithValue("@rating", rating);


                    sqlComm.Connection = sqlConnection;
                    sqlConnection.Open();
                    var x = sqlComm.ExecuteScalar();
                    int ratingID = int.Parse(x.ToString());


                    return ratingID;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region Update Statements

        public void UpdateRecruiter(Recruiter recruiter)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(strConn))
                {
                    SqlCommand sqlComm = new SqlCommand("UPDATE [dbo].[Recruiters] SET[Recruiter_Name] = @recName ,[Email] = @recEmail,[PhoneNumber] = @recPhone,[LinkedInLink] = @recLinkedIn WHERE RecruiterID = @recID", sqlConnection);

                    sqlComm.Parameters.AddWithValue("@recName", recruiter.RecruiterName);
                    sqlComm.Parameters.AddWithValue("@recEmail", recruiter.Email);
                    sqlComm.Parameters.AddWithValue("@recPhone", recruiter.PhoneNumber);
                    sqlComm.Parameters.AddWithValue("@recLinkedIn", recruiter.LinkedInLink);
                    sqlComm.Parameters.AddWithValue("@recID", recruiter.RecruiterId);

                    sqlComm.Connection = sqlConnection;
                    sqlConnection.Open();
                    sqlComm.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateJobWithRecruiterID(int recruiterId, int companyId)
        {
                try
                {
                    using (SqlConnection sqlConnection = new SqlConnection(strConn))
                    {
                        SqlCommand sqlComm = new SqlCommand("UPDATE [dbo].[Jobs] SET [RecruiterID] = @RecruiterID WHERE CompanyID = @CompanyID", sqlConnection);

                        sqlComm.Parameters.AddWithValue("@RecruiterID", recruiterId);
                        sqlComm.Parameters.AddWithValue("@CompanyID", companyId);


                        sqlComm.Connection = sqlConnection;
                        sqlConnection.Open();
                        sqlComm.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
        }

        public void UpdateJobLocationID(int locationId, int companyId)
        {

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(strConn))
                {
                    SqlCommand sqlComm = new SqlCommand("UPDATE [dbo].[Jobs] SET LocationId = @LocationID WHERE CompanyID = @CompanyID", sqlConnection);

                    sqlComm.Parameters.AddWithValue("@LocationID", locationId);
                    sqlComm.Parameters.AddWithValue("@CompanyID", companyId);

                    sqlComm.Connection = sqlConnection;
                    sqlConnection.Open();
                    sqlComm.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void UpdateJobInformation(Jobs job)
        {

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(strConn))
                {
                    SqlCommand sqlComm = new SqlCommand("UPDATE [dbo].[Jobs] SET CompanyName = @CompanyName, PositionID = @PositionID, SalaryRange = @Salary, RatingID = @RatingID, CEOName = @CEOName, MissionStatement = @Mission, Benefits = @Benefits, Comments = @Comments, JobLink = @JobLink WHERE CompanyID = @CompanyID", sqlConnection);

                    sqlComm.Parameters.AddWithValue("@CompanyID", job.CompanyId);
                    sqlComm.Parameters.AddWithValue("@CompanyName", job.CompanyName);
                    sqlComm.Parameters.AddWithValue("@PositionID", job.PositionId);
                    sqlComm.Parameters.AddWithValue("@Salary", job.SalaryRange);
                    sqlComm.Parameters.AddWithValue("@RatingID", job.RatingId);
                    sqlComm.Parameters.AddWithValue("@CEOName", job.CEOName);
                    sqlComm.Parameters.AddWithValue("@Mission", job.MissionStatement);
                    sqlComm.Parameters.AddWithValue("@Benefits", job.Benefits);
                    sqlComm.Parameters.AddWithValue("@Comments", job.Comments);
                    sqlComm.Parameters.AddWithValue("@JobLink", job.JobLink);

                    sqlComm.Connection = sqlConnection;
                    sqlConnection.Open();
                    sqlComm.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        #endregion

        #region Delete records from database

        public void DeleteSpecificJobWithRecruiter(int companyId, int recruiterId)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(strConn))
                {
                    SqlCommand sqlComm = new SqlCommand("DELETE FROM Jobs WHERE CompanyID = @CompanyID", sqlConnection);
                    sqlComm.Parameters.AddWithValue("@CompanyID", companyId);

                    sqlComm.Connection = sqlConnection;
                    sqlConnection.Open();
                    sqlComm.ExecuteNonQuery();
                
                    sqlComm = new SqlCommand("DELETE FROM Recruiters WHERE RecruiterID = @RecruiterID", sqlConnection);
                    sqlComm.Parameters.AddWithValue("@RecruiterID", recruiterId);
                    
                    sqlComm.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void DeleteSpecificJob(int companyId)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(strConn))
                {
                    SqlCommand sqlComm = new SqlCommand("DELETE FROM Jobs WHERE CompanyID = @CompanyID", sqlConnection);
                    sqlComm.Parameters.AddWithValue("@CompanyID", companyId);

                    sqlComm.Connection = sqlConnection;
                    sqlConnection.Open();
                    sqlComm.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteLastJobRecord()
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(strConn))
                {
                    SqlCommand sqlComm = new SqlCommand("DELETE FROM Jobs WHERE CompanyID =(SELECT MAX(CompanyID) FROM Jobs)", sqlConnection);
                    sqlComm.Connection = sqlConnection;
                    sqlConnection.Open();
                    sqlComm.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void DeleteLastLocationRecord()
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(strConn))
                {
                    SqlCommand sqlComm = new SqlCommand("DELETE FROM Location WHERE LocationID =(SELECT MAX(LocationID) FROM Location)", sqlConnection);
                    sqlComm.Connection = sqlConnection;
                    sqlConnection.Open();
                    sqlComm.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
    }
}
