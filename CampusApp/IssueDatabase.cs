using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CampusApp
{
    public class IssueDatabase
    {
        private string _connStr = ConfigurationManager.ConnectionStrings["FacilitiesDB"].ConnectionString;
        public void AddIssue(FacilitiesIssue issue)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
         
                string sql = "INSERT INTO Issues (Location, Category, Description, DateReported, Priority, Status, ReportedBy) VALUES (@loc, @cat, @desc, @date, @prio, @stat, @rep)";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@loc", issue.Location);
                cmd.Parameters.AddWithValue("@cat", issue.Category);
                cmd.Parameters.AddWithValue("@desc", issue.Description);
                cmd.Parameters.AddWithValue("@date", issue.DateReported);
                cmd.Parameters.AddWithValue("@prio", issue.Priority);
                cmd.Parameters.AddWithValue("@stat", issue.Status);
                cmd.Parameters.AddWithValue("@rep", issue.ReportedBy);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public DataTable GetAllIssues()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                string sql = "SELECT * FROM Issues ORDER BY IssueId;";
                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                adapter.Fill(dt);
            }
            return dt;  
        }
        public void UpdateIssue(FacilitiesIssue issue)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                string sql = "UPDATE Issues SET Location=@loc, Category=@cat, Description=@desc, DateReported=@date, Priority=@prio, Status=@stat, ReportedBy=@rep WHERE IssueId=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@loc", issue.Location);
                cmd.Parameters.AddWithValue("@cat", issue.Category);
                cmd.Parameters.AddWithValue("@desc", issue.Description);
                cmd.Parameters.AddWithValue("@date", issue.DateReported);
                cmd.Parameters.AddWithValue("@prio", issue.Priority);
                cmd.Parameters.AddWithValue("@stat", issue.Status);
                cmd.Parameters.AddWithValue("@rep", issue.ReportedBy);
                cmd.Parameters.AddWithValue("@id", issue.IssueID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void DeleteIssue(int issueId)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                string sql = "DELETE FROM Issues WHERE IssueId=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", issueId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public DataTable SearchIssues(string location, string category, string status)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                
                string sql = "SELECT * FROM Issues WHERE 1=1";

                if (!string.IsNullOrWhiteSpace(location))
                    sql += " AND Location LIKE @loc";

                if (!string.IsNullOrWhiteSpace(category) && category != "All")
                    sql += " AND Category = @cat";

                if (!string.IsNullOrWhiteSpace(status) && status != "All")
                    sql += " AND Status = @stat";

                sql += " ORDER BY IssueID"; 

                SqlCommand cmd = new SqlCommand(sql, conn);

                if (!string.IsNullOrWhiteSpace(location))
                    cmd.Parameters.AddWithValue("@loc", "%" + location + "%");

                if (!string.IsNullOrWhiteSpace(category) && category != "All")
                    cmd.Parameters.AddWithValue("@cat", category);

                if (!string.IsNullOrWhiteSpace(status) && status != "All")
                    cmd.Parameters.AddWithValue("@stat", status);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            return dt;
        }
    }
}
