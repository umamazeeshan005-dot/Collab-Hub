
using System.Drawing;
using System.Windows.Forms;

namespace COLLABHUB
{
    public class BlueDisabledButton : Button
    {
        public BlueDisabledButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            BackColor = Color.FromArgb(101, 83, 220);
            ForeColor = Color.White;
            UseVisualStyleBackColor = false;
            Cursor = Cursors.Hand;
            Font = new Font("Segoe UI", 10, FontStyle.Bold);

            SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);

            TextRenderer.DrawText(
                e.Graphics,
                Text,
                Font,
                ClientRectangle,
                ForeColor,
                TextFormatFlags.HorizontalCenter |
                TextFormatFlags.VerticalCenter |
                TextFormatFlags.EndEllipsis
            );
        }
    }
}

