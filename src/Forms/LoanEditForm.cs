using LibraryManager.Models;
using LibraryManager.Repositories;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace LibraryManager.Forms
{
    public partial class LoanEditForm : Form
    {
        private readonly LoanRepository _loanRepository;
        private readonly BookRepository _bookRepository;
        private readonly MemberRepository _memberRepository;

        public LoanEditForm()
        {
            InitializeComponent();
            _loanRepository = new LoanRepository();
            _bookRepository = new BookRepository();
            _memberRepository = new MemberRepository();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var books = _bookRepository.GetAll().Where(b => b.Available).ToList();
                cmbBooks.DataSource = books;
                cmbBooks.DisplayMember = "Title";
                cmbBooks.ValueMember = "BookID";

                var members = _memberRepository.GetAll().Where(m => m.IsActive).ToList();
                cmbMembers.DataSource = members;
                cmbMembers.DisplayMember = "FullName";
                cmbMembers.ValueMember = "MemberID";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbBooks.SelectedItem == null || cmbMembers.SelectedItem == null)
            {
                MessageBox.Show("Please select a book and a member.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var loan = new Loan
            {
                BookID = (int)cmbBooks.SelectedValue,
                MemberID = (int)cmbMembers.SelectedValue,
                LoanDate = dtpLoanDate.Value,
                Returned = false
            };

            try
            {
                _loanRepository.CreateLoan(loan);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating loan: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
