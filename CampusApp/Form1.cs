using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CampusApp
{
    public partial class Form1 : Form
    {
        private IssueDatabase _db = new IssueDatabase();
        private int _selectedIssueID = 0;
        public Form1()
        { 
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadAllIssues();
        }
        private void LoadAllIssues()
        {
            dgvIssues.DataSource = _db.GetAllIssues();
            lblStatus.Text = dgvIssues.Rows.Count + "issues displayed";
            dgvIssues.ClearSelection();
            ClearForm();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            FacilitiesIssue issue = BuildIssueFromForm();
            List<string> errors = issue.Validate();
            if (errors.Count > 0)
            {
                ShowErrors(errors);
                return;
            }
            try
            {
                _db.AddIssue(issue); MessageBox.Show("Issue added.");
                LoadAllIssues();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_selectedIssueID == 0)
            {
                MessageBox.Show("Select an issue First.");
                return;
            }
            FacilitiesIssue issue = BuildIssueFromForm();
            issue.IssueID = _selectedIssueID;
            List<string> errors = issue.Validate();
            if (errors.Count > 0)
            {
                ShowErrors(errors);
                return;
            }
            try
            {
                _db.UpdateIssue(issue); MessageBox.Show("Issue updated.");
                LoadAllIssues();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedIssueID == 0)
            {
                MessageBox.Show("Select an issue First.");
                return;
            }
            if (MessageBox.Show("Delete the issue?", "Confirm Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    _db.DeleteIssue(_selectedIssueID); MessageBox.Show("Issue deleted.");
                    LoadAllIssues();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
        private void ClearForm()
        {
            txtIssueID.Text = "";
            txtLocation.Text = "";
            cmbCategory.SelectedIndex = -1;
            txtDescription.Text = "";
            dtpDateReported.Value = DateTime.Today;
            cmbPriority.SelectedIndex = -1;
            cmbStatus.SelectedIndex = -1;
            txtReportedBy.Text = "";
            _selectedIssueID = 0;
        }
        private void dgvIssues_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvIssues.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvIssues.SelectedRows[0];
                _selectedIssueID = Convert.ToInt32(row.Cells["IssueID"].Value);
                txtIssueID.Text = _selectedIssueID.ToString();
                txtLocation.Text = row.Cells["Location"].Value.ToString();
                cmbCategory.Text = row.Cells["Category"].Value.ToString();
                txtDescription.Text = row.Cells["Description"].Value.ToString();
                dtpDateReported.Value = Convert.ToDateTime(row.Cells["DateReported"].Value);
                cmbPriority.Text = row.Cells["Priority"].Value.ToString();
                cmbStatus.Text = row.Cells["Status"].Value.ToString();
                txtReportedBy.Text = row.Cells["ReportedBy"].Value.ToString();
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
        
            dgvIssues.DataSource = _db.SearchIssues(txtSearchLocation.Text.Trim(), cmbSearchCategory.Text.Trim(), cmbSearchStatus.Text.Trim());
            lblStatus.Text = dgvIssues.Rows.Count + " issues displayed(filtered).";
            dgvIssues.ClearSelection();
            ClearForm();
        }
        private void btnShowAll_Click(object sender, EventArgs e)
        {
            txtSearchLocation.Text = "";
            cmbSearchCategory.Text = "All";
            cmbSearchStatus.Text = "All";
            LoadAllIssues();
        }
        private FacilitiesIssue BuildIssueFromForm()
        {
            return new FacilitiesIssue
            {
                Location = txtLocation.Text.Trim(),
                Category = cmbCategory.Text,
                Description = txtDescription.Text.Trim(),
                DateReported = dtpDateReported.Value.Date,
                Priority = cmbPriority.Text,
                Status = cmbStatus.Text,
                ReportedBy = txtReportedBy.Text.Trim()
            };
        }
        private void ShowErrors(List<string>errors)
        {
            string msg = "";
            foreach (string err in errors) msg += "• " + err + "\n";
            MessageBox.Show(msg, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}