using LibraryManager.Models;
using LibraryManager.Repositories;
using System;
using System.Windows.Forms;

namespace LibraryManager.Forms
{
    public partial class LoanForm : Form
    {
        private readonly LoanRepository _repository;

        public LoanForm()
        {
            InitializeComponent();
            _repository = new LoanRepository();
        }

        private void LoanForm_Load(object sender, EventArgs e)
        {
            LoadLoans();
        }

        private void LoadLoans()
        {
            try
            {
                var loans = _repository.GetAll();
                dgvLoans.DataSource = loans;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading loans: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNewLoan_Click(object sender, EventArgs e)
        {
            var editForm = new LoanEditForm();
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadLoans();
            }
        }

        private void btnReturnBook_Click(object sender, EventArgs e)
        {
            if (dgvLoans.SelectedRows.Count > 0)
            {
                var loan = (Loan)dgvLoans.SelectedRows[0].DataBoundItem;
                if (loan.Returned)
                {
                    MessageBox.Show("This book is already returned.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (MessageBox.Show($"Return book for loan ID {loan.LoanID}?", "Confirm Return", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        _repository.ReturnLoan(loan.LoanID);
                        LoadLoans();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error returning book: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a loan to return.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
