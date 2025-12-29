namespace LibraryManager;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.btnBooks = new System.Windows.Forms.Button();
        this.btnAuthors = new System.Windows.Forms.Button();
        this.btnMembers = new System.Windows.Forms.Button();
        this.btnLoans = new System.Windows.Forms.Button();
        this.btnReports = new System.Windows.Forms.Button();
        this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
        this.lblTitle = new System.Windows.Forms.Label();
        this.flowLayoutPanel1.SuspendLayout();
        this.SuspendLayout();
        // 
        // btnBooks
        // 
        this.btnBooks.Location = new System.Drawing.Point(3, 3);
        this.btnBooks.Name = "btnBooks";
        this.btnBooks.Size = new System.Drawing.Size(150, 50);
        this.btnBooks.TabIndex = 0;
        this.btnBooks.Text = "Správa knih";
        this.btnBooks.UseVisualStyleBackColor = true;
        this.btnBooks.Click += new System.EventHandler(this.btnBooks_Click);
        // 
        // btnAuthors
        // 
        this.btnAuthors.Location = new System.Drawing.Point(159, 3);
        this.btnAuthors.Name = "btnAuthors";
        this.btnAuthors.Size = new System.Drawing.Size(150, 50);
        this.btnAuthors.TabIndex = 1;
        this.btnAuthors.Text = "Správa autorů";
        this.btnAuthors.UseVisualStyleBackColor = true;
        this.btnAuthors.Click += new System.EventHandler(this.btnAuthors_Click);
        // 
        // btnMembers
        // 
        this.btnMembers.Location = new System.Drawing.Point(315, 3);
        this.btnMembers.Name = "btnMembers";
        this.btnMembers.Size = new System.Drawing.Size(150, 50);
        this.btnMembers.TabIndex = 2;
        this.btnMembers.Text = "Správa členů";
        this.btnMembers.UseVisualStyleBackColor = true;
        this.btnMembers.Click += new System.EventHandler(this.btnMembers_Click);
        // 
        // btnLoans
        // 
        this.btnLoans.Location = new System.Drawing.Point(471, 3);
        this.btnLoans.Name = "btnLoans";
        this.btnLoans.Size = new System.Drawing.Size(150, 50);
        this.btnLoans.TabIndex = 3;
        this.btnLoans.Text = "Výpůjčky";
        this.btnLoans.UseVisualStyleBackColor = true;
        this.btnLoans.Click += new System.EventHandler(this.btnLoans_Click);
        // 
        // btnReports
        // 
        this.btnReports.Location = new System.Drawing.Point(3, 59);
        this.btnReports.Name = "btnReports";
        this.btnReports.Size = new System.Drawing.Size(150, 50);
        this.btnReports.TabIndex = 4;
        this.btnReports.Text = "Reporty";
        this.btnReports.UseVisualStyleBackColor = true;
        this.btnReports.Click += new System.EventHandler(this.btnReports_Click);
        // 
        // flowLayoutPanel1
        // 
        this.flowLayoutPanel1.Controls.Add(this.btnBooks);
        this.flowLayoutPanel1.Controls.Add(this.btnAuthors);
        this.flowLayoutPanel1.Controls.Add(this.btnMembers);
        this.flowLayoutPanel1.Controls.Add(this.btnLoans);
        this.flowLayoutPanel1.Controls.Add(this.btnReports);
        this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 50);
        this.flowLayoutPanel1.Name = "flowLayoutPanel1";
        this.flowLayoutPanel1.Size = new System.Drawing.Size(776, 388);
        this.flowLayoutPanel1.TabIndex = 5;
        // 
        // lblTitle
        // 
        this.lblTitle.AutoSize = true;
        this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.lblTitle.Location = new System.Drawing.Point(12, 9);
        this.lblTitle.Name = "lblTitle";
        this.lblTitle.Size = new System.Drawing.Size(306, 30);
        this.lblTitle.TabIndex = 6;
        this.lblTitle.Text = "Library Management System";
        // 
        // Form1
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 450);
        this.Controls.Add(this.lblTitle);
        this.Controls.Add(this.flowLayoutPanel1);
        this.Name = "Form1";
        this.Text = "Library Manager";
        this.flowLayoutPanel1.ResumeLayout(false);
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnBooks;
    private System.Windows.Forms.Button btnAuthors;
    private System.Windows.Forms.Button btnMembers;
    private System.Windows.Forms.Button btnLoans;
    private System.Windows.Forms.Button btnReports;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    private System.Windows.Forms.Label lblTitle;
}
