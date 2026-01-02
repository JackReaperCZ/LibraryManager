using LibraryManager.Repositories;
using System;
using System.Data;
using System.Windows.Forms;

namespace LibraryManager.Forms
{
    public partial class ReportForm : Form
    {
        private readonly ReportRepository _repository;

        public ReportForm()
        {
            InitializeComponent();
            _repository = new ReportRepository();
            cmbReportType.SelectedIndex = 0;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable data = null;
                switch (cmbReportType.SelectedIndex)
                {
                    case 0: // Member Loan Counts
                        data = _repository.GetMemberLoanCounts();
                        break;
                    case 1: // Popular Books
                        data = _repository.GetPopularBooks();
                        break;
                    case 2: // Genre Statistics
                        data = _repository.GetGenreStats();
                        break;
                }

                if (data != null)
                {
                    dgvReport.DataSource = data;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
