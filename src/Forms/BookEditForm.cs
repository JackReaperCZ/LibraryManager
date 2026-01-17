using LibraryManager.Models;
using LibraryManager.Repositories;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LibraryManager.Forms
{
    public partial class BookEditForm : Form
    {
        private class AuthorOption
        {
            public int? AuthorID { get; set; }
            public string Name { get; set; }
        }

        private readonly BookRepository _repository;
        private readonly AuthorRepository _authorRepository;
        private readonly Book _book;
        private readonly bool _isEdit;

        public BookEditForm(Book book = null)
        {
            InitializeComponent();
            _repository = new BookRepository();
            _authorRepository = new AuthorRepository();

            cmbGenre.DataSource = Enum.GetValues(typeof(Genre));
            LoadAuthors();

            if (book != null)
            {
                _book = book;
                _isEdit = true;
                PopulateFields();
                SelectAuthorForBook();
            }
            else
            {
                _book = new Book();
                _isEdit = false;
            }
        }

        private void LoadAuthors()
        {
            var options = new List<AuthorOption>
            {
                new AuthorOption { AuthorID = null, Name = "Neznámý autor" }
            };

            foreach (var author in _authorRepository.GetAll())
            {
                options.Add(new AuthorOption
                {
                    AuthorID = author.AuthorID,
                    Name = author.FullName
                });
            }

            cmbAuthor.DataSource = options;
            cmbAuthor.DisplayMember = "Name";
            cmbAuthor.ValueMember = "AuthorID";
        }

        private void PopulateFields()
        {
            txtTitle.Text = _book.Title;
            numPrice.Value = (decimal)_book.Price;
            cmbGenre.SelectedItem = _book.Genre;
            dtpPublished.Value = _book.PublishedDate;
            chkAvailable.Checked = _book.Available;
        }

        private void SelectAuthorForBook()
        {
            var authorId = _repository.GetAuthorIdForBook(_book.BookID);
            if (authorId.HasValue)
            {
                cmbAuthor.SelectedValue = authorId.Value;
            }
            else
            {
                cmbAuthor.SelectedIndex = 0;
            }
        }

        private int? GetSelectedAuthorId()
        {
            if (cmbAuthor.SelectedItem is AuthorOption option)
            {
                return option.AuthorID;
            }
            return null;
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

                var selectedAuthorId = GetSelectedAuthorId();
                _repository.SetBookAuthor(_book.BookID, selectedAuthorId);

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
