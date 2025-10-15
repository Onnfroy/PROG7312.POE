using System;
using System.Drawing;
using System.Windows.Forms;
using PROG7312.POE.Models;

namespace PROG7312.POE.UI
{
    public partial class EventsForm : Form
    {
        private ComboBox cboCategory, cboSort;
        private DateTimePicker dtpDate;
        private TextBox txtSearch;
        private Button btnSearch, btnClear, btnClose, btnAddSample, btnProcess, btnLastViewed;
        private ListView lv;
        private Label lblRecommend, lblPending;

        public EventsForm()
        {
            InitializeComponent();
            BuildUi();

            AppState.SeedEventsIfEmpty();
            FillCategoryCombo();

            // Seed an initial “date” search so label shows something at startup
            dtpDate.Value = DateTime.Today;
            AppState.Events.RecordSearchDate(dtpDate.Value.Date);
            UpdateRecommendations();

            RefreshList();
        }

        private void BuildUi()
        {
            BackColor = Color.FromArgb(230, 220, 200);
            MinimumSize = new Size(1000, 650);
            Text = "Local Events & Announcements";

            var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(16), RowCount = 3, ColumnCount = 1 };
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            Controls.Add(root);

            var rowTop = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 10, Padding = new Padding(0, 0, 0, 10) };
            rowTop.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            rowTop.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            rowTop.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            rowTop.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            rowTop.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            rowTop.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            rowTop.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            rowTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            rowTop.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            rowTop.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            rowTop.Controls.Add(new Label { Text = "Date:", Font = new Font("Segoe UI", 10, FontStyle.Bold), AutoSize = true, Margin = new Padding(0, 6, 6, 0) }, 0, 0);
            dtpDate = new DateTimePicker { Width = 160, Format = DateTimePickerFormat.Short, Margin = new Padding(0, 2, 16, 0) };
            rowTop.Controls.Add(dtpDate, 1, 0);

            rowTop.Controls.Add(new Label { Text = "Category:", Font = new Font("Segoe UI", 10, FontStyle.Bold), AutoSize = true, Margin = new Padding(0, 6, 6, 0) }, 2, 0);
            cboCategory = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 200, Margin = new Padding(0, 2, 16, 0) };
            rowTop.Controls.Add(cboCategory, 3, 0);

            rowTop.Controls.Add(new Label { Text = "Title:", Font = new Font("Segoe UI", 10, FontStyle.Bold), AutoSize = true, Margin = new Padding(0, 6, 6, 0) }, 4, 0);
            txtSearch = new TextBox { Width = 220, Margin = new Padding(0, 2, 16, 0) };
            rowTop.Controls.Add(txtSearch, 5, 0);

            cboSort = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 180, Margin = new Padding(0, 2, 0, 0) };
            cboSort.Items.AddRange(new object[] { "Sort: Date ↑", "Sort: Date ↓", "Sort: Title", "Sort: Category" });
            cboSort.SelectedIndex = 0;
            rowTop.Controls.Add(cboSort, 6, 0);

            rowTop.Controls.Add(new Panel() { Dock = DockStyle.Fill }, 7, 0);

            lblRecommend = new Label
            {
                Text = "Recommendations: —",
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Italic),
                ForeColor = Color.Black,
                Margin = new Padding(0, 6, 12, 0),
                Anchor = AnchorStyles.Left
            };
            rowTop.Controls.Add(lblRecommend, 8, 0);

            lblPending = new Label
            {
                Text = "Queued: 0",
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.Black,
                Margin = new Padding(0, 6, 0, 0),
                Anchor = AnchorStyles.Left
            };
            rowTop.Controls.Add(lblPending, 9, 0);

            root.Controls.Add(rowTop, 0, 0);

            // Wire user actions (record searches HERE, not in ApplyFiltersAndSort)
            dtpDate.ValueChanged += (s, e) =>
            {
                AppState.Events.RecordSearchDate(dtpDate.Value.Date);
                ApplyFiltersAndSort();
            };
            cboCategory.SelectedIndexChanged += (s, e) =>
            {
                if (cboCategory.SelectedIndex > 0)
                    AppState.Events.RecordSearchCategory(cboCategory.SelectedItem.ToString());
                ApplyFiltersAndSort();
            };
            txtSearch.TextChanged += (s, e) =>
            {
                if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                    AppState.Events.RecordSearchTitle(txtSearch.Text.Trim());
                ApplyFiltersAndSort();
            };
            cboSort.SelectedIndexChanged += (s, e) => ApplyFiltersAndSort();

            // List
            lv = new ListView { Dock = DockStyle.Fill, View = View.Details, FullRowSelect = true, BackColor = Color.White };
            lv.Columns.Add("Title", 280);
            lv.Columns.Add("Date", 120);
            lv.Columns.Add("Category", 120);
            lv.Columns.Add("Location", 220);
            lv.Columns.Add("Description", 400);
            lv.SelectedIndexChanged += (s, e) =>
            {
                if (lv.SelectedItems.Count == 0) return;
                if (lv.SelectedItems[0].Tag is Guid id) AppState.Events.PushViewed(id);
            };
            root.Controls.Add(lv, 0, 1);

            // Footer
            var rowBottom = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 7, Padding = new Padding(0, 10, 0, 0) };
            rowBottom.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            rowBottom.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            rowBottom.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            rowBottom.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            rowBottom.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            rowBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            rowBottom.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            btnSearch = MakeButton("Search / Filter");
            btnSearch.Click += (s, e) => ApplyFiltersAndSort();
            rowBottom.Controls.Add(btnSearch, 0, 0);

            btnClear = MakeButton("Clear Filters");
            btnClear.Click += (s, e) =>
            {
                dtpDate.Value = DateTime.Today;
                if (cboCategory.Items.Count > 0) cboCategory.SelectedIndex = 0;
                txtSearch.Text = "";
                // Seed date so we still have a rec after clearing
                AppState.Events.RecordSearchDate(dtpDate.Value.Date);
                ApplyFiltersAndSort();
            };
            rowBottom.Controls.Add(btnClear, 1, 0);

            btnAddSample = MakeButton("Add Sample Submission");
            btnAddSample.Click += (s, e) =>
            {
                var ev = new EventItem("Pop-up Clinic", DateTime.Today.AddDays(2), "Ward 9 Clinic",
                                       EventCategory.Other, "Vaccinations available.");
                AppState.Events.NewSubmissions.Enqueue(ev);
                UpdatePendingLabel();
                MessageBox.Show(this, "Queued a new event submission (FIFO).", "Queued",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            rowBottom.Controls.Add(btnAddSample, 2, 0);

            btnProcess = MakeButton("Process Next");
            btnProcess.Click += (s, e) =>
            {
                if (AppState.Events.NewSubmissions.IsEmpty())
                {
                    MessageBox.Show(this, "Queue is empty.", "Queue", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                var next = AppState.Events.NewSubmissions.Dequeue();
                AppState.Events.Add(next);
                if (!cboCategory.Items.Contains(next.Category.ToString()))
                    cboCategory.Items.Add(next.Category.ToString());
                RefreshList();
                UpdatePendingLabel();
            };
            rowBottom.Controls.Add(btnProcess, 3, 0);

            btnLastViewed = MakeButton("Last Viewed");
            btnLastViewed.Click += (s, e) =>
            {
                var id = AppState.Events.PopViewed();
                if (id == null)
                {
                    MessageBox.Show(this, "History empty.", "Last Viewed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                for (int i = 0; i < lv.Items.Count; i++)
                {
                    if (lv.Items[i].Tag is Guid g && g == id.Value)
                    {
                        lv.Items[i].Selected = true;
                        lv.EnsureVisible(i);
                        return;
                    }
                }
                if (AppState.Events.TryGet(id.Value, out var item))
                    MessageBox.Show(this, $"Last viewed: {item.Title} ({item.Date:yyyy-MM-dd})", "Last Viewed");
            };
            rowBottom.Controls.Add(btnLastViewed, 4, 0);

            rowBottom.Controls.Add(new Panel() { Dock = DockStyle.Fill }, 5, 0);
            btnClose = MakeButton("Close");
            btnClose.Click += (s, e) => Close();
            rowBottom.Controls.Add(btnClose, 6, 0);

            root.Controls.Add(rowBottom, 0, 2);
        }

        private Button MakeButton(string text)
        {
            var b = new Button
            {
                Text = text,
                Height = 36,
                BackColor = Color.FromArgb(230, 220, 200),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Margin = new Padding(8)
            };
            b.FlatAppearance.BorderSize = 0;
            b.MouseEnter += (s, e) => { b.BackColor = Color.FromArgb(210, 200, 180); b.Cursor = Cursors.Hand; };
            b.MouseLeave += (s, e) => { b.BackColor = Color.FromArgb(230, 220, 200); b.Cursor = Cursors.Default; };
            return b;
        }

        private void FillCategoryCombo()
        {
            cboCategory.Items.Clear();
            cboCategory.Items.Add("(Any)");
            foreach (var c in AppState.Events.Categories) cboCategory.Items.Add(c);
            cboCategory.SelectedIndex = 0;
        }

        private void RefreshList()
        {
            lv.BeginUpdate();
            lv.Items.Clear();
            foreach (var e in AppState.Events.AllFlat())
            {
                var item = new ListViewItem(new[]
                {
                    e.Title,
                    e.Date.ToString("yyyy-MM-dd"),
                    e.Category.ToString(),
                    e.Location,
                    e.Description
                });
                item.Tag = e.Id;
                lv.Items.Add(item);
            }
            lv.EndUpdate();

            ApplyFiltersAndSort();
        }

        private void ApplyFiltersAndSort()
        {
            string wantedCategory = cboCategory.SelectedIndex > 0 ? cboCategory.SelectedItem.ToString() : null;
            string titleFilter = txtSearch.Text.Trim();
            int sortMode = cboSort.SelectedIndex;

            lv.BeginUpdate();
            var items = lv.Items;
            items.Clear();

            foreach (var e in AppState.Events.AllFlat())
            {
                if (wantedCategory != null &&
                    !string.Equals(e.Category.ToString(), wantedCategory, StringComparison.OrdinalIgnoreCase))
                    continue;

                if (titleFilter.Length > 0 &&
                    e.Title.IndexOf(titleFilter, StringComparison.OrdinalIgnoreCase) < 0)
                    continue;

                var li = new ListViewItem(new[]
                {
                    e.Title,
                    e.Date.ToString("yyyy-MM-dd"),
                    e.Category.ToString(),
                    e.Location,
                    e.Description
                });
                li.Tag = e.Id;

                int insertAt = 0;
                for (; insertAt < items.Count; insertAt++)
                {
                    int cmp = Compare(li, items[insertAt], sortMode);
                    if (cmp <= 0) break;
                }
                items.Insert(insertAt, li);
            }

            lv.EndUpdate();

            UpdateRecommendations();
            HighlightRecommendations();
        }

        private int Compare(ListViewItem a, ListViewItem b, int mode)
        {
            switch (mode)
            {
                case 0:
                    return DateTime.Parse(a.SubItems[1].Text).CompareTo(DateTime.Parse(b.SubItems[1].Text));
                case 1:
                    return DateTime.Parse(b.SubItems[1].Text).CompareTo(DateTime.Parse(a.SubItems[1].Text));
                case 2:
                    return string.Compare(a.SubItems[0].Text, b.SubItems[0].Text, StringComparison.OrdinalIgnoreCase);
                case 3:
                    return string.Compare(a.SubItems[2].Text, b.SubItems[2].Text, StringComparison.OrdinalIgnoreCase);
                default: return 0;
            }
        }

        private void UpdateRecommendations()
        {
            lblRecommend.Text = AppState.Events.RecommendationText();
            UpdatePendingLabel();
        }

        private void UpdatePendingLabel()
        {
            lblPending.Text = $"Queued: {AppState.Events.PendingSubmissionsCount()}";
        }

        private void HighlightRecommendations()
        {
            var key = AppState.Events.LastSearchKey ?? AppState.Events.TopSearchKey();
            if (key == null) return;

            foreach (ListViewItem it in lv.Items)
            {
                it.BackColor = Color.White;
                it.Font = new Font(it.Font, FontStyle.Regular);

                if (key.StartsWith("cat:"))
                {
                    var cat = key.Substring(4);
                    if (string.Equals(it.SubItems[2].Text, cat, StringComparison.OrdinalIgnoreCase))
                    {
                        it.BackColor = Color.FromArgb(255, 250, 235);
                        it.Font = new Font(it.Font, FontStyle.Bold);
                    }
                }
                else if (key.StartsWith("date:"))
                {
                    var d = key.Substring(5);
                    if (it.SubItems[1].Text == d)
                    {
                        it.BackColor = Color.FromArgb(255, 250, 235);
                        it.Font = new Font(it.Font, FontStyle.Bold);
                    }
                }
                else if (key.StartsWith("title:"))
                {
                    var t = key.Substring(6);
                    if (it.SubItems[0].Text.IndexOf(t, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        it.BackColor = Color.FromArgb(255, 250, 235);
                        it.Font = new Font(it.Font, FontStyle.Bold);
                    }
                }
            }
        }
    }
}