using LibraryManager.Models;
using LibraryManager.Repositories;
using System;
using System.Windows.Forms;
using LibraryManager.Services;
using System.IO;

namespace LibraryManager.Forms
{
    public partial class MemberForm : Form
    {
        private readonly MemberRepository _repository;
        private readonly ImportService _importService;

        public MemberForm()
        {
            InitializeComponent();
            _repository = new MemberRepository();
            _importService = new ImportService();
        }

        private void MemberForm_Load(object sender, EventArgs e)
        {
            LoadMembers();
        }

        private void LoadMembers()
        {
            try
            {
                var members = _repository.GetAll();
                dgvMembers.DataSource = members;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading members: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var editForm = new MemberEditForm();
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadMembers();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvMembers.SelectedRows.Count > 0)
            {
                var member = (Member)dgvMembers.SelectedRows[0].DataBoundItem;
                var editForm = new MemberEditForm(member);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    LoadMembers();
                }
            }
            else
            {
                MessageBox.Show("Please select a member to edit.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvMembers.SelectedRows.Count > 0)
            {
                var member = (Member)dgvMembers.SelectedRows[0].DataBoundItem;
                if (MessageBox.Show($"Are you sure you want to delete '{member.FullName}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        _repository.Delete(member.MemberID);
                        LoadMembers();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting member: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a member to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnImportCsv_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "CSV Files (*.csv)|*.csv";
                ofd.InitialDirectory = AppConfig.ImportExportPath;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _importService.ImportMembersFromCsv(ofd.FileName);
                        LoadMembers();
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
                        _importService.ImportMembersFromJson(ofd.FileName);
                        LoadMembers();
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
