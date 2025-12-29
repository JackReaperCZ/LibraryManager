using LibraryManager.Models;
using LibraryManager.Repositories;
using System;
using System.Windows.Forms;
using LibraryManager.Services;
using System.IO;

namespace LibraryManager.Forms
{
    public partial class BookForm : Form
    {
        private readonly BookRepository _repository;
        private readonly ImportService _importService;

        public BookForm()
        {
            InitializeComponent();
            _repository = new BookRepository();
            _importService = new ImportService();
        }

        private void BookForm_Load(object sender, EventArgs e)
        {
            LoadBooks();
        }

        private void LoadBooks()
        {
            try
            {
                var books = _repository.GetAll();
                dgvBooks.DataSource = books;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading books: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var editForm = new BookEditForm();
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadBooks();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvBooks.SelectedRows.Count > 0)
            {
                var book = (Book)dgvBooks.SelectedRows[0].DataBoundItem;
                var editForm = new BookEditForm(book);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    LoadBooks();
                }
            }
            else
            {
                MessageBox.Show("Please select a book to edit.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvBooks.SelectedRows.Count > 0)
            {
                var book = (Book)dgvBooks.SelectedRows[0].DataBoundItem;
                if (MessageBox.Show($"Are you sure you want to delete '{book.Title}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        _repository.Delete(book.BookID);
                        LoadBooks();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting book: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a book to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnImportCsv_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "CSV Files (*.csv)|*.csv";
                ofd.InitialDirectory = AppConfig.ImportExportPath;
                if (Directory.Exists(ofd.InitialDirectory))
                {
                     // Ensure directory exists
                }

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _importService.ImportBooksFromCsv(ofd.FileName);
                        LoadBooks();
                        MessageBox.Show("Import successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Import failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnImportJson_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "JSON Files (*.json)|*.json";
                ofd.InitialDirectory = AppConfig.ImportExportPath;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _importService.ImportBooksFromJson(ofd.FileName);
                        LoadBooks();
                        MessageBox.Show("Import successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                         MessageBox.Show($"Import failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
