using LibraryManager.Models;
using LibraryManager.Repositories;
using System;
using System.Windows.Forms;

namespace LibraryManager.Forms
{
    public partial class MemberEditForm : Form
    {
        private readonly MemberRepository _repository;
        private readonly Member _member;
        private readonly bool _isEdit;

        public MemberEditForm(Member member = null)
        {
            InitializeComponent();
            _repository = new MemberRepository();

            if (member != null)
            {
                _member = member;
                _isEdit = true;
                PopulateFields();
            }
            else
            {
                _member = new Member();
                _isEdit = false;
            }
        }

        private void PopulateFields()
        {
            txtFirstName.Text = _member.FirstName;
            txtLastName.Text = _member.LastName;
            txtEmail.Text = _member.Email;
            dtpRegistered.Value = _member.RegisteredDate;
            chkActive.Checked = _member.IsActive;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) || string.IsNullOrWhiteSpace(txtLastName.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("First Name, Last Name and Email are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _member.FirstName = txtFirstName.Text;
            _member.LastName = txtLastName.Text;
            _member.Email = txtEmail.Text;
            _member.RegisteredDate = dtpRegistered.Value;
            _member.IsActive = chkActive.Checked;

            try
            {
                if (_isEdit)
                {
                    _repository.Update(_member);
                }
                else
                {
                    _repository.Add(_member);
                }
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving member: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
