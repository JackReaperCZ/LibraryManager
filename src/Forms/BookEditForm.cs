using LibraryManager.Models;
using LibraryManager.Repositories;
using System;
using System.Windows.Forms;

namespace LibraryManager.Forms
{
    public partial class BookEditForm : Form
    {
        private readonly BookRepository _repository;
        private readonly Book _book;
        private readonly bool _isEdit;

        public BookEditForm(Book book = null)
        {
            InitializeComponent();
            _repository = new BookRepository();
            
            // Populate Genre combo
            cmbGenre.DataSource = Enum.GetValues(typeof(Genre));

            if (book != null)
            {
                _book = book;
                _isEdit = true;
                PopulateFields();
            }
            else
            {
                _book = new Book();
                _isEdit = false;
            }
        }

        private void PopulateFields()
        {
            txtTitle.Text = _book.Title;
            numPrice.Value = (decimal)_book.Price;
            cmbGenre.SelectedItem = _book.Genre;
            dtpPublished.Value = _book.PublishedDate;
            chkAvailable.Checked = _book.Available;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Title is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _book.Title = txtTitle.Text;
            _book.Price = (float)numPrice.Value;
            _book.Genre = (Genre)cmbGenre.SelectedItem;
            _book.PublishedDate = dtpPublished.Value;
            _book.Available = chkAvailable.Checked;

            try
            {
                if (_isEdit)
                {
                    _repository.Update(_book);
                }
                else
                {
                    _repository.Add(_book);
                }
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving book: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
