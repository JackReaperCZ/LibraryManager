using LibraryManager.Models;
using LibraryManager.Repositories;
using System;
using System.Windows.Forms;

namespace LibraryManager.Forms
{
    public partial class AuthorForm : Form
    {
        private readonly AuthorRepository _repository;

        public AuthorForm()
        {
            InitializeComponent();
            _repository = new AuthorRepository();
        }

        private void AuthorForm_Load(object sender, EventArgs e)
        {
            LoadAuthors();
        }

        private void LoadAuthors()
        {
            try
            {
                var authors = _repository.GetAll();
                dgvAuthors.DataSource = authors;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading authors: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var editForm = new AuthorEditForm();
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadAuthors();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvAuthors.SelectedRows.Count > 0)
            {
                var author = (Author)dgvAuthors.SelectedRows[0].DataBoundItem;
                var editForm = new AuthorEditForm(author);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    LoadAuthors();
                }
            }
            else
            {
                MessageBox.Show("Please select an author to edit.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvAuthors.SelectedRows.Count > 0)
            {
                var author = (Author)dgvAuthors.SelectedRows[0].DataBoundItem;
                if (MessageBox.Show($"Are you sure you want to delete '{author.FullName}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        _repository.Delete(author.AuthorID);
                        LoadAuthors();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting author: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select an author to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
