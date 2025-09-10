using System;
using System.Drawing;
using System.Windows.Forms;
using PROG7312.POE.DataStructures;
using PROG7312.POE.Models;

namespace PROG7312.POE.UI
{
    public sealed class ReportIssueForm : Form
    {
        private TextBox txtLocation;
        private ComboBox cboCategory;
        private RichTextBox rtbDescription;
        private Button btnAttach, btnSubmit, btnBack;
        private ListView lvAttachments;
        private ProgressBar pbProgress;
        private Label lblEngagement;
        private Timer timerProgress;

        private LinkedList<Attachment> _currentAttachments = new LinkedList<Attachment>();

        public ReportIssueForm()
        {
            BuildUi();
        }

        private void BuildUi()
        {
            var softBeige = Color.FromArgb(230, 220, 200);
            var hoverBeige = Color.FromArgb(210, 200, 180);
            var inputBg = Color.White;

            Text = "Report an Issue";
            MinimumSize = new Size(950, 650);
            BackColor = softBeige;

            // Root layout
            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                RowCount = 3,
                ColumnCount = 1
            };
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 80)); // main inputs
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));   // engagement + progress
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));   // buttons
            Controls.Add(root);

            // === MAIN GRID ===
            var grid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 4,
                Padding = new Padding(10)
            };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80));
            root.Controls.Add(grid, 0, 0);

            Label MakeLabel(string text) =>
                new Label
                {
                    Text = text,
                    AutoSize = true,
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    ForeColor = Color.Black,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Dock = DockStyle.Fill
                };

            // === Location ===
            grid.Controls.Add(MakeLabel("Location:"), 0, 0);
            txtLocation = new TextBox { Dock = DockStyle.Fill };
            grid.Controls.Add(txtLocation, 1, 0);

            // === Category ===
            grid.Controls.Add(MakeLabel("Category:"), 0, 1);
            cboCategory = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Left, Width = 200 };
            cboCategory.Items.AddRange(new object[]
            {
                IssueCategory.Sanitation,
                IssueCategory.Roads,
                IssueCategory.Water,
                IssueCategory.Electricity,
                IssueCategory.PublicSafety,
                IssueCategory.Other
            });
            cboCategory.SelectedIndex = 0;
            grid.Controls.Add(cboCategory, 1, 1);

            // === Description ===
            grid.Controls.Add(MakeLabel("Description:"), 0, 2);
            rtbDescription = new RichTextBox { Dock = DockStyle.Fill, Height = 200 };
            grid.Controls.Add(rtbDescription, 1, 2);

            // === Attachments ===
            grid.Controls.Add(MakeLabel("Attachments:"), 0, 3);

            var attachRow = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2 };
            attachRow.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            attachRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            btnAttach = MakeShadowButton("Add File...");
            btnAttach.Click += OnAttachClicked;

            lvAttachments = new ListView
            {
                View = View.Details,
                FullRowSelect = true,
                Dock = DockStyle.Fill,
                BackColor = inputBg,
                Height = 120
            };
            lvAttachments.Columns.Add("File Name", 600);

            attachRow.Controls.Add(btnAttach, 0, 0);
            attachRow.Controls.Add(lvAttachments, 1, 0);
            grid.Controls.Add(attachRow, 1, 3);

            // === ENGAGEMENT + PROGRESS BAR ===
            var statusPanel = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2, ColumnCount = 1 };
            statusPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            statusPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.Controls.Add(statusPanel, 0, 1);

            lblEngagement = new Label
            {
                Text = "Ready to submit your report.",
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Italic),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            statusPanel.Controls.Add(lblEngagement, 0, 0);

            pbProgress = new ProgressBar
            {
                Dock = DockStyle.Top,
                Height = 8,
                Style = ProgressBarStyle.Continuous,
                Minimum = 0,
                Maximum = 100,
                Value = 0,
                ForeColor = Color.DarkGreen
            };
            statusPanel.Controls.Add(pbProgress, 0, 1);

            // === BUTTONS (bottom row) ===
            var buttonPanel = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, Padding = new Padding(10) };
            buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            root.Controls.Add(buttonPanel, 0, 2);

            btnBack = MakeShadowButton("Back");
            btnBack.Click += (s, e) => Close();

            btnSubmit = MakeShadowButton("Submit");
            btnSubmit.Click += OnSubmitClicked;

            buttonPanel.Controls.Add(btnBack, 0, 0);
            buttonPanel.Controls.Add(btnSubmit, 1, 0);

            // Progress animation
            timerProgress = new Timer { Interval = 35 };
            timerProgress.Tick += (s, e) =>
            {
                if (pbProgress.Value < 100) pbProgress.Value += 2;
                else timerProgress.Stop();
            };
        }

        private Button MakeShadowButton(string text)
        {
            var btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(230, 220, 200),
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Fill,
                Height = 40,
                Margin = new Padding(8)
            };
            btn.FlatAppearance.BorderSize = 0;

            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = Color.FromArgb(210, 200, 180);
                btn.Cursor = Cursors.Hand;
                btn.FlatAppearance.BorderColor = Color.Gray;
                btn.Padding = new Padding(2, 2, 4, 4);
            };
            btn.MouseLeave += (s, e) =>
            {
                btn.BackColor = Color.FromArgb(230, 220, 200);
                btn.Cursor = Cursors.Default;
                btn.Padding = new Padding(0);
            };

            return btn;
        }

        private void OnAttachClicked(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog
            {
                Title = "Select an image or document",
                Filter = "Images or Documents|*.png;*.jpg;*.jpeg;*.pdf;*.docx;*.xlsx;*.txt|All files|*.*",
                Multiselect = false
            })
            {
                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    var att = new Attachment(ofd.FileName);
                    _currentAttachments.AddLast(att);
                    lvAttachments.Items.Add(new ListViewItem(att.FileName));
                }
            }
        }

        private void OnSubmitClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLocation.Text))
            {
                MessageBox.Show(this, "Please enter the location.", "Missing data",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLocation.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(rtbDescription.Text))
            {
                MessageBox.Show(this, "Please enter a description.", "Missing data",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                rtbDescription.Focus();
                return;
            }

            var issue = new Issue(
                txtLocation.Text.Trim(),
                (IssueCategory)cboCategory.SelectedItem,
                rtbDescription.Text.Trim()
            );

            foreach (var a in _currentAttachments)
                issue.Attachments.AddLast(a);

            AppState.Issues.Add(issue);

            lblEngagement.Text = "Thanks! Your report was submitted.";
            pbProgress.Value = 0;
            timerProgress.Start();

            txtLocation.Clear();
            rtbDescription.Clear();
            cboCategory.SelectedIndex = 0;
            _currentAttachments = new LinkedList<Attachment>();
            lvAttachments.Items.Clear();

            MessageBox.Show(this, "Report submitted successfully.", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}