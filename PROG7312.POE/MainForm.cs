using System;
using System.Drawing;
using System.Windows.Forms;
using PROG7312.POE.UI; // ReportIssueForm, EventsForm, RequestStatusForm

namespace PROG7312.POE
{
    public partial class MainForm : Form
    {
        private Button btnReportIssues, btnEvents, btnStatus;
        private ToolTip tips;

        private PictureBox pbHero;
        private Panel marqueePanel;
        private Label marqueeLabel;
        private Timer marqueeTimer;
        private Label lblReportDesc;

        // Fade labels
        private Label lblEventsDesc, lblStatusDesc;
        private Timer fadeTimer;
        private int fadeDirection = 0;   // 1 = fade in, -1 = fade out
        private float fadeAlphaEvents = 0.4f;
        private float fadeAlphaStatus = 0.4f;

        public MainForm()
        {
            InitializeComponent();

            // Seed data for all parts
            AppState.SeedEventsIfEmpty();    // Part 2
            AppState.SeedRequestsIfEmpty();  // Part 3

            BuildUi();
        }

        private void BuildUi()
        {
            // === COLORS ===
            var softBeige = Color.FromArgb(230, 220, 200);
            var lightBeige = Color.FromArgb(245, 245, 235);
            var hoverBeige = Color.FromArgb(210, 200, 180);

            // === FORM SETTINGS ===
            Text = "Municipal Services";
            MinimumSize = new Size(900, 600);
            BackColor = lightBeige;

            tips = new ToolTip();

            // Root layout
            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 3,
                ColumnCount = 1
            };
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 30));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 70));
            Controls.Add(root);

            // === HEADER ===
            var header = new Panel { Dock = DockStyle.Fill, BackColor = softBeige, Padding = new Padding(0, 12, 0, 12) };
            var headerStack = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3 };
            headerStack.RowStyles.Add(new RowStyle(SizeType.Percent, 40));
            headerStack.RowStyles.Add(new RowStyle(SizeType.Percent, 30));
            headerStack.RowStyles.Add(new RowStyle(SizeType.Percent, 30));

            pbHero = new PictureBox { SizeMode = PictureBoxSizeMode.Zoom, Anchor = AnchorStyles.None, Dock = DockStyle.Fill, Visible = false };
            try
            {
                string imagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "TableMountain.jpg");
                if (System.IO.File.Exists(imagePath))
                {
                    pbHero.Image = Image.FromFile(imagePath);
                    pbHero.Visible = true;
                }
            }
            catch { pbHero.Visible = false; }

            var title = new Label
            {
                Text = "City of Cape Town — Municipal Services",
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };

            var subtitle = new Label
            {
                Text = "Helping you engage with your municipality easily and effectively.",
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 11, FontStyle.Italic),
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };

            headerStack.Controls.Add(pbHero, 0, 0);
            headerStack.Controls.Add(title, 0, 1);
            headerStack.Controls.Add(subtitle, 0, 2);
            header.Controls.Add(headerStack);
            root.Controls.Add(header, 0, 0);

            // === MARQUEE STRIP ===
            marqueePanel = new Panel { Dock = DockStyle.Fill, BackColor = lightBeige };
            marqueeLabel = new Label
            {
                Text = "Report Issues   •   View Events   •   Track Requests",
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                AutoSize = true
            };
            marqueePanel.Controls.Add(marqueeLabel);
            root.Controls.Add(marqueePanel, 0, 1);

            marqueePanel.Resize += delegate
            {
                marqueeLabel.Top = (marqueePanel.Height - marqueeLabel.Height) / 2;
                if (marqueeLabel.Left <= -marqueeLabel.Width || marqueeLabel.Left >= marqueePanel.Width)
                    marqueeLabel.Left = -marqueeLabel.Width;
            };
            marqueeTimer = new Timer { Interval = 20 };
            marqueeTimer.Tick += delegate
            {
                marqueeLabel.Left += 2;
                if (marqueeLabel.Left > marqueePanel.Width)
                    marqueeLabel.Left = -marqueeLabel.Width;
            };
            marqueeTimer.Start();

            // === BUTTON ROW ===
            var cards = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                Padding = new Padding(40, 10, 40, 20)
            };
            cards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            cards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34));
            cards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            root.Controls.Add(cards, 0, 2);

            // Buttons
            btnReportIssues = MakeShadowButton("Report Issues", true, softBeige, hoverBeige);
            btnEvents = MakeShadowButton("Local Events & Announcements", true, softBeige, hoverBeige);
            btnStatus = MakeShadowButton("Service Request Status", true, softBeige, hoverBeige); // now enabled for Part 3

            // LEFT: Report issues
            var leftCol = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2, ColumnCount = 1 };
            leftCol.RowStyles.Add(new RowStyle(SizeType.Percent, 75));
            leftCol.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            lblReportDesc = new Label
            {
                Text = "Submit municipal issues with location, category, description, and file attachments.",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            leftCol.Controls.Add(btnReportIssues, 0, 0);
            leftCol.Controls.Add(lblReportDesc, 0, 1);

            // MIDDLE: Events
            var midCol = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2, ColumnCount = 1 };
            midCol.RowStyles.Add(new RowStyle(SizeType.Percent, 75));
            midCol.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            lblEventsDesc = new Label
            {
                Text = "Browse, search and sort local events and announcements.",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(90, 50, 50, 50),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            midCol.Controls.Add(btnEvents, 0, 0);
            midCol.Controls.Add(lblEventsDesc, 0, 1);

            // RIGHT: Status
            var rightCol = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2, ColumnCount = 1 };
            rightCol.RowStyles.Add(new RowStyle(SizeType.Percent, 75));
            rightCol.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            lblStatusDesc = new Label
            {
                Text = "Manage and track service requests in real-time.",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(90, 50, 50, 50),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            rightCol.Controls.Add(btnStatus, 0, 0);
            rightCol.Controls.Add(lblStatusDesc, 0, 1);

            cards.Controls.Add(leftCol, 0, 0);
            cards.Controls.Add(midCol, 1, 0);
            cards.Controls.Add(rightCol, 2, 0);

            // Fade timer (still used for subtle text fade if you like)
            fadeTimer = new Timer { Interval = 50 };
            fadeTimer.Tick += FadeTimer_Tick;

            btnEvents.MouseEnter += (s, e) => { fadeDirection = 1; fadeTimer.Start(); };
            btnEvents.MouseLeave += (s, e) => { fadeDirection = -1; fadeTimer.Start(); };

            btnStatus.MouseEnter += (s, e) => { fadeDirection = 1; fadeTimer.Start(); };
            btnStatus.MouseLeave += (s, e) => { fadeDirection = -1; fadeTimer.Start(); };

            // === BUTTON CLICK ACTIONS ===

            // Part 1: Report issues
            btnReportIssues.Click += delegate
            {
                using (var f = new ReportIssueForm())
                {
                    f.ShowDialog(this);
                }
            };

            // Part 2: Events
            btnEvents.Click += delegate
            {
                using (var f = new EventsForm())
                {
                    f.ShowDialog(this);
                }
            };

            // Part 3: Requests
            btnStatus.Click += delegate
            {
                using (var f = new RequestStatusForm())
                {
                    f.ShowDialog(this);
                }
            };

            // Tooltips
            tips.SetToolTip(btnReportIssues, "Report municipal issues");
            tips.SetToolTip(btnEvents, "Open Local Events & Announcements");
            tips.SetToolTip(btnStatus, "Open Service Request Status");
        }

        // Shadow button factory (unchanged)
        private Button MakeShadowButton(string text, bool enabled, Color baseColor, Color hoverColor)
        {
            var btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = baseColor,
                FlatStyle = FlatStyle.Flat,
                Enabled = enabled,
                Dock = DockStyle.Fill,
                Margin = new Padding(16),
                TextAlign = ContentAlignment.MiddleCenter
            };
            btn.FlatAppearance.BorderSize = 0;

            int shadowOffset = 4;

            btn.Paint += (s, e) =>
            {
                var b = (Button)s;
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                var shadowRect = new Rectangle(shadowOffset, shadowOffset, b.Width - shadowOffset, b.Height - shadowOffset);
                using (var shadowBrush = new SolidBrush(Color.FromArgb(100, 0, 0, 0)))
                    e.Graphics.FillRectangle(shadowBrush, shadowRect);

                var buttonRect = new Rectangle(0, 0, b.Width - shadowOffset, b.Height - shadowOffset);
                var path = new System.Drawing.Drawing2D.GraphicsPath();
                int r = 18;
                path.AddArc(buttonRect.X, buttonRect.Y, r, r, 180, 90);
                path.AddArc(buttonRect.Right - r, buttonRect.Y, r, r, 270, 90);
                path.AddArc(buttonRect.Right - r, buttonRect.Bottom - r, r, r, 0, 90);
                path.AddArc(buttonRect.X, buttonRect.Bottom - r, r, r, 90, 90);
                path.CloseAllFigures();

                using (var brush = new SolidBrush(b.BackColor))
                    e.Graphics.FillPath(brush, path);

                TextRenderer.DrawText(e.Graphics, b.Text, b.Font, buttonRect, Color.Black,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };

            btn.MouseEnter += (s, e) => { if (btn.Enabled) btn.BackColor = hoverColor; btn.Invalidate(); };
            btn.MouseLeave += (s, e) => { if (btn.Enabled) btn.BackColor = baseColor; btn.Invalidate(); };

            return btn;
        }

        // Fade effect (subtle text fade on hover)
        private void FadeTimer_Tick(object sender, EventArgs e)
        {
            float step = 0.1f * fadeDirection;

            fadeAlphaEvents = Math.Max(0.4f, Math.Min(1f, fadeAlphaEvents + step));
            fadeAlphaStatus = Math.Max(0.4f, Math.Min(1f, fadeAlphaStatus + step));

            if (lblEventsDesc != null)
                lblEventsDesc.ForeColor = Color.FromArgb((int)(fadeAlphaEvents * 255), Color.Gray);
            if (lblStatusDesc != null)
                lblStatusDesc.ForeColor = Color.FromArgb((int)(fadeAlphaStatus * 255), Color.Gray);

            if ((fadeDirection == 1 && fadeAlphaEvents >= 1f && fadeAlphaStatus >= 1f) ||
                (fadeDirection == -1 && fadeAlphaEvents <= 0.4f && fadeAlphaStatus <= 0.4f))
            {
                fadeTimer.Stop();
            }
        }
    }
}