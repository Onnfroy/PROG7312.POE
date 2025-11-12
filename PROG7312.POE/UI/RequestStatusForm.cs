using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PROG7312.POE.Models;

namespace PROG7312.POE.UI
{
    public partial class RequestStatusForm : Form
    {
        private TextBox txtId, txtFrom, txtTo;
        private ComboBox cboCat, cboStatus, cboSort;
        private Button btnLookup, btnNext, btnPopNext, btnResolve, btnPath, btnClose;
        private ListView lv;
        private Label lblNext;

        public RequestStatusForm()
        {
            InitializeComponent();
            BuildUi();

            // Requests are seeded by AppState.SeedRequestsIfEmpty() in MainForm
            RefreshList();
            RefreshNext();
        }

        private void BuildUi()
        {
            BackColor = Color.FromArgb(230, 220, 200);
            Text = "Service Request Status";
            MinimumSize = new Size(1100, 700);

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 3,
                ColumnCount = 1,
                Padding = new Padding(16)
            };
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));     // top controls
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // list
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));     // footer
            Controls.Add(root);

            // ===== TOP ROW: 3 areas = Lookup | Filters | Route planner =====
            var top = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                Padding = new Padding(0, 0, 0, 10)
            };
            top.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35)); // Lookup
            top.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35)); // Filters
            top.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30)); // Route planner
            root.Controls.Add(top, 0, 0);

            // --- Lookup area (left) ---
            var lookupPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2
            };
            lookupPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            lookupPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            lookupPanel.Controls.Add(new Label
            {
                Text = "Request ID:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 6, 6, 0)
            }, 0, 0);

            txtId = new TextBox { Dock = DockStyle.Fill, Margin = new Padding(0, 2, 8, 0) };
            lookupPanel.Controls.Add(txtId, 1, 0);

            btnLookup = MakeButton("Lookup");
            btnLookup.Dock = DockStyle.Left;
            btnLookup.Click += (s, e) => LookupById();
            lookupPanel.Controls.Add(btnLookup, 1, 1);

            top.Controls.Add(lookupPanel, 0, 0);

            // --- Filter area (middle) ---
            var filterPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 2
            };
            filterPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // Cat label
            filterPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // Cat combo
            filterPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // Status label
            filterPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // Status combo

            // Row 0: Category + Status
            filterPanel.Controls.Add(new Label
            {
                Text = "Category:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 6, 6, 0)
            }, 0, 0);

            cboCat = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 160,
                Margin = new Padding(0, 2, 16, 0)
            };
            cboCat.Items.Add("(Any)");
            foreach (var name in Enum.GetNames(typeof(IssueCategory)))
                cboCat.Items.Add(name);
            cboCat.SelectedIndex = 0;
            filterPanel.Controls.Add(cboCat, 1, 0);

            filterPanel.Controls.Add(new Label
            {
                Text = "Status:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 6, 6, 0)
            }, 2, 0);

            cboStatus = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 160,
                Margin = new Padding(0, 2, 0, 0)
            };
            cboStatus.Items.AddRange(new object[] { "(Any)", "Submitted", "InProgress", "Resolved" });
            cboStatus.SelectedIndex = 0;
            filterPanel.Controls.Add(cboStatus, 3, 0);

            // Row 1: Sort
            filterPanel.Controls.Add(new Label
            {
                Text = "Sort:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 6, 6, 0)
            }, 0, 1);

            cboSort = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 180,
                Margin = new Padding(0, 2, 0, 0)
            };
            cboSort.Items.AddRange(new object[]
            {
                "Priority ↑",
                "Due date ↑",
                "Category",
                "Status"
            });
            cboSort.SelectedIndex = 0;
            filterPanel.Controls.Add(cboSort, 1, 1);

            // make filters react live
            cboCat.SelectedIndexChanged += (s, e) => RefreshList();
            cboStatus.SelectedIndexChanged += (s, e) => RefreshList();
            cboSort.SelectedIndexChanged += (s, e) => RefreshList();

            top.Controls.Add(filterPanel, 1, 0);

            // --- Route planner (right) ---
            var routePanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4
            };
            routePanel.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // heading
            routePanel.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // To label
            routePanel.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // To textbox
            routePanel.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Find button

            routePanel.Controls.Add(new Label
            {
                Text = "Route planner",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 4)
            }, 0, 0);

            routePanel.Controls.Add(new Label
            {
                Text = "To (Ward):",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 2, 0, 0)
            }, 0, 1);

            txtTo = new TextBox
            {
                Width = 150,
                Margin = new Padding(0, 2, 0, 0),
                Text = "Ward 4"
            };
            routePanel.Controls.Add(txtTo, 0, 2);

            btnPath = MakeButton("Find route from Depot A");
            btnPath.Dock = DockStyle.Left;
            btnPath.Margin = new Padding(0, 6, 0, 0);
            btnPath.Click += (s, e) => ShowPath();
            routePanel.Controls.Add(btnPath, 0, 3);

            top.Controls.Add(routePanel, 2, 0);

            // ==== LIST ====
            lv = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                BackColor = Color.White,
                HideSelection = false // <-- keep selection visible when list loses focus
            };
            lv.Columns.Add("Id", 260);
            lv.Columns.Add("Priority", 80);
            lv.Columns.Add("Category", 110);
            lv.Columns.Add("Location", 120);
            lv.Columns.Add("Status", 100);
            lv.Columns.Add("Due", 120);
            Controls.Add(lv);
            root.Controls.Add(lv, 0, 1);

            // ==== FOOTER: next job + actions ====
            var bottom = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 6,
                Padding = new Padding(0, 10, 0, 0)
            };
            bottom.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));   // next label
            bottom.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));   // peek
            bottom.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));   // start
            bottom.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));   // resolve
            bottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            bottom.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));   // close
            root.Controls.Add(bottom, 0, 2);

            lblNext = new Label
            {
                Text = "Next to service: —",
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Italic),
                Margin = new Padding(0, 6, 12, 0)
            };
            bottom.Controls.Add(lblNext, 0, 0);

            btnNext = MakeButton("Peek");
            btnNext.Click += (s, e) => RefreshNext();
            bottom.Controls.Add(btnNext, 1, 0);

            btnPopNext = MakeButton("Start");
            btnPopNext.Click += (s, e) => StartNext();
            bottom.Controls.Add(btnPopNext, 2, 0);

            btnResolve = MakeButton("Resolve");
            btnResolve.Click += (s, e) => ResolveSelected();
            bottom.Controls.Add(btnResolve, 3, 0);

            bottom.Controls.Add(new Panel { Dock = DockStyle.Fill }, 4, 0);

            btnClose = MakeButton("Close");
            btnClose.Click += (s, e) => Close();
            bottom.Controls.Add(btnClose, 5, 0);
        }

        private Button MakeButton(string text)
        {
            var b = new Button
            {
                Text = text,
                Height = 32,
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

        // ===== LIST REFRESH & SORT =====

        private void RefreshList()
        {
            string wantCat = cboCat.SelectedIndex > 0 ? cboCat.SelectedItem.ToString() : null;
            string wantStatus = cboStatus.SelectedIndex > 0 ? cboStatus.SelectedItem.ToString() : null;
            int sortMode = cboSort.SelectedIndex;

            var items = new System.Collections.Generic.List<ListViewItem>();
            foreach (var pair in AppState.Requests.ById)
            {
                var r = pair.Value;
                if (wantCat != null && r.Category.ToString() != wantCat) continue;
                if (wantStatus != null && r.Status.ToString() != wantStatus) continue;

                var li = new ListViewItem(new[]
                {
                    r.Id.ToString(),
                    r.Priority.ToString(),
                    r.Category.ToString(),
                    r.Location,
                    r.Status.ToString(),
                    r.DueDate.ToString("yyyy-MM-dd")
                });
                li.Tag = r.Id;
                items.Add(li);
            }

            items.Sort((a, b) =>
            {
                switch (sortMode)
                {
                    case 0: // Priority ↑
                        return PriorityRank(a.SubItems[1].Text).CompareTo(PriorityRank(b.SubItems[1].Text));
                    case 1: // Due date ↑
                        return DateTime.Parse(a.SubItems[5].Text).CompareTo(DateTime.Parse(b.SubItems[5].Text));
                    case 2: // Category
                        return string.Compare(a.SubItems[2].Text, b.SubItems[2].Text, StringComparison.OrdinalIgnoreCase);
                    case 3: // Status
                        return string.Compare(a.SubItems[4].Text, b.SubItems[4].Text, StringComparison.OrdinalIgnoreCase);
                    default:
                        return 0;
                }
            });

            lv.BeginUpdate();
            lv.Items.Clear();
            foreach (var li in items) lv.Items.Add(li);
            lv.EndUpdate();
        }

        private int PriorityRank(string p)
        {
            if (p == "Critical") return 1;
            if (p == "High") return 2;
            if (p == "Normal") return 3;
            return 4; // Low
        }

        // ===== LOOKUP (grid search + BST fallback) =====

        private void LookupById()
        {
            var idText = txtId.Text.Trim();
            if (string.IsNullOrWhiteSpace(idText))
            {
                MessageBox.Show(this, "Enter a Request ID or part of it.", "Lookup",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 1) Try exact match first (grid)
            for (int i = 0; i < lv.Items.Count; i++)
            {
                var fullId = lv.Items[i].SubItems[0].Text;
                if (string.Equals(fullId, idText, StringComparison.OrdinalIgnoreCase))
                {
                    lv.Items[i].Selected = true;
                    lv.EnsureVisible(i);
                    lv.Focus();  // make highlight obvious
                    MessageBox.Show(this, "Request located in the list.", "Lookup",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            // 2) Try partial match (starts with or contains)
            for (int i = 0; i < lv.Items.Count; i++)
            {
                var fullId = lv.Items[i].SubItems[0].Text;
                if (fullId.StartsWith(idText, StringComparison.OrdinalIgnoreCase) ||
                    fullId.IndexOf(idText, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    lv.Items[i].Selected = true;
                    lv.EnsureVisible(i);
                    lv.Focus();  // keep selection visible
                    MessageBox.Show(this, "Matched by partial ID.", "Lookup",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            // 3) If still nothing, fall back to BST (in case filters hide it)
            ServiceRequest r;
            if (AppState.Requests.TryGetById(idText, out r))
            {
                MessageBox.Show(this,
                    $"Found request:\n\nName: {r.CitizenName}\nCategory: {r.Category}\nPriority: {r.Priority}\nLocation: {r.Location}\nStatus: {r.Status}\nDue: {r.DueDate:yyyy-MM-dd}",
                    "Lookup (BST)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 4) Nothing found
            MessageBox.Show(this, "No request found for that Id.", "Lookup",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        // ===== NEXT JOB (MIN-HEAP) =====

        private void RefreshNext()
        {
            ServiceRequest r;
            if (AppState.Requests.TryPeekNext(out r))
                lblNext.Text = $"Next to service: [{r.Priority}] {r.Category} @ {r.Location} (Due {r.DueDate:yyyy-MM-dd})";
            else
                lblNext.Text = "Next to service: —";
        }

        private void StartNext()
        {
            ServiceRequest r;
            if (!AppState.Requests.TryPopNext(out r))
            {
                MessageBox.Show(this, "No requests in the priority queue.", "Next",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            r.Status = RequestStatus.InProgress;
            RefreshNext();
            RefreshList();
            MessageBox.Show(this,
                $"Started: {r.Category} ({r.Priority}) at {r.Location}",
                "Next request started",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ResolveSelected()
        {
            if (lv.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "Select a request first.", "Resolve",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var idStr = lv.SelectedItems[0].SubItems[0].Text;
            ServiceRequest r;
            if (AppState.Requests.TryGetById(idStr, out r))
            {
                r.Status = RequestStatus.Resolved;
                RefreshList();
            }
        }

        // ===== GRAPH / ROUTES =====

        private void ShowPath()
        {
            var from = "Depot A";
            var to = txtTo.Text.Trim();
            var path = AppState.Requests.ShortestPath(from, to);

            if (path.IsEmpty())
            {
                MessageBox.Show(this, "No route found. Check the ward name.", "Route",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // path is reversed (goal..start); display forward
            string display = "";
            foreach (var node in path)
            {
                if (display == "") display = node;
                else display = node + " → " + display;
            }

            MessageBox.Show(this, display, "Shortest route (BFS)", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}