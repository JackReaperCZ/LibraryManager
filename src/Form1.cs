using LibraryManager.Forms;
using System;
using System.Windows.Forms;

namespace LibraryManager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnBooks_Click(object sender, EventArgs e)
        {
            var form = new BookForm();
            form.ShowDialog();
        }

        private void btnAuthors_Click(object sender, EventArgs e)
        {
            var form = new AuthorForm();
            form.ShowDialog();
        }

        private void btnMembers_Click(object sender, EventArgs e)
        {
             var form = new MemberForm();
             form.ShowDialog();
        }

        private void btnLoans_Click(object sender, EventArgs e)
        {
             var form = new LoanForm();
             form.ShowDialog();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            var form = new ReportForm();
            form.ShowDialog();
        }
    }
}
