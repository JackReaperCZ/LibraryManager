using LibraryManager.Models;
using LibraryManager.Repositories;
using System;
using System.Windows.Forms;

namespace LibraryManager.Forms
{
    public partial class AuthorEditForm : Form
    {
        private readonly AuthorRepository _repository;
        private readonly Author _author;
        private readonly bool _isEdit;

        public AuthorEditForm(Author author = null)
        {
            InitializeComponent();
            _repository = new AuthorRepository();

            if (author != null)
            {
                _author = author;
                _isEdit = true;
                PopulateFields();
            }
            else
            {
                _author = new Author();
                _isEdit = false;
            }
        }

        private void PopulateFields()
        {
            txtFirstName.Text = _author.FirstName;
            txtLastName.Text = _author.LastName;
            if (_author.BirthDate.HasValue)
            {
                dtpBirth.Value = _author.BirthDate.Value;
                dtpBirth.Checked = true;
            }
            else
            {
                dtpBirth.Checked = false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) || string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("First Name and Last Name are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _author.FirstName = txtFirstName.Text;
            _author.LastName = txtLastName.Text;
            _author.BirthDate = dtpBirth.Checked ? dtpBirth.Value : (DateTime?)null;

            try
            {
                if (_isEdit)
                {
                    _repository.Update(_author);
                }
                else
                {
                    _repository.Add(_author);
                }
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving author: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
