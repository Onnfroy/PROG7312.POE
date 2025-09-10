using System.Windows.Forms;               // Needed so 'Form' is known
using System.Drawing;                     // If Designer uses Size, Point, etc.

namespace PROG7312.POE                    // <-- must be the SAME as MainForm.cs
{
    partial class MainForm : Form         // <-- class name + base type
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(900, 600);
            this.Text = "Municipal Services";
        }
    }
}