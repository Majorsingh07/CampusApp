using System;
using System.Collections.Generic;

namespace CampusApp
{
    public class FacilitiesIssue
    {
        public int IssueID { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public DateTime DateReported { get; set; }
        public string Priority   { get; set; }
        public string Status { get; set; }
        public string ReportedBy { get; set; }
        public List<string> Validate()
        {
            List<string> errors = new List<string>();
            if (string.IsNullOrWhiteSpace(Location))
            
                errors.Add("Location is required.");
            
            if (string.IsNullOrWhiteSpace(Description))
            
                errors.Add("Description is required.");
            
            if (DateReported.Date > DateTime.Today)
            
                errors.Add("Date reported cannot be in the future.");
            
            if (Priority != "Low" && Priority != "Medium" && Priority != "High")
            
                errors.Add("Please select a valid priority.");
            
            if (Status != "Open" && Status != "In Progress" && Status != "Resolved")
            
                errors.Add("Please select a valid status.");
            
            if (string.IsNullOrWhiteSpace(ReportedBy))
            
                errors.Add("Reported by is required.");
            
            return errors;
        }
    }
}
