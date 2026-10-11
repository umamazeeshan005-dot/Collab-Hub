
using System;
using System.Drawing;
using System.Windows.Forms;

namespace COLLABHUB
{
    public partial class RegisterForm : Form
    {
        private TextBox nameBox;
        private TextBox emailBox;
        private TextBox passwordBox;
        private TextBox confirmBox;
        private BlueDisabledButton createButton;

        private readonly Color navy = Color.FromArgb(31, 36, 75);
        private readonly Color purple = Color.FromArgb(101, 83, 220);

        public RegisterForm()
        {
            InitializeComponent();
            BuildRegister();
        }

        private void BuildRegister()
        {
            Controls.Clear();

            Text = "CollabHub | Create Account";
            ClientSize = new Size(1000, 620);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.White;
            Font = new Font("Segoe UI", 10);

            Panel hero = new Panel();
            hero.Bounds = new Rectangle(0, 0, 420, 620);
            hero.BackColor = navy;
            Controls.Add(hero);

            Label logo = new Label();
            logo.Text = "CollabHub";
            logo.Font = new Font("Segoe UI", 28, FontStyle.Bold);
            logo.ForeColor = Color.White;
            logo.AutoSize = true;
            logo.Location = new Point(45, 65);
            hero.Controls.Add(logo);

            Label heading = new Label();
            heading.Text = "Your next big\nproject starts here.";
            heading.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            heading.ForeColor = Color.White;
            heading.AutoSize = true;
            heading.Location = new Point(45, 190);
            hero.Controls.Add(heading);

            Label description = new Label();
            description.Text = "Create your student account.\nConnect, collaborate and grow.";
            description.Font = new Font("Segoe UI", 12);
            description.ForeColor = Color.FromArgb(205, 210, 235);
            description.AutoSize = true;
            description.Location = new Point(48, 315);
            hero.Controls.Add(description);

            Label title = new Label();
            title.Text = "Create your account";
            title.Font = new Font("Segoe UI", 23, FontStyle.Bold);
            title.ForeColor = navy;
            title.AutoSize = true;
            title.Location = new Point(510, 35);
            Controls.Add(title);

            Label subtitle = new Label();
            subtitle.Text = "Join CollabHub and start collaborating.";
            subtitle.ForeColor = Color.Gray;
            subtitle.AutoSize = true;
            subtitle.Location = new Point(513, 80);
            Controls.Add(subtitle);

            AddField("Full name", 120, out nameBox);
            AddField("Email address", 200, out emailBox);
            AddField("Password (minimum 6 characters)", 280, out passwordBox);
            AddField("Confirm password", 360, out confirmBox);

            passwordBox.PasswordChar = '*';
            confirmBox.PasswordChar = '*';

            nameBox.TextChanged += (s, e) => UpdateCreateButton();
            emailBox.TextChanged += (s, e) => UpdateCreateButton();
            passwordBox.TextChanged += (s, e) => UpdateCreateButton();
            confirmBox.TextChanged += (s, e) => UpdateCreateButton();

            createButton = new BlueDisabledButton();
            createButton.Text = "Create Account";
            createButton.Bounds = new Rectangle(515, 440, 400, 45);
            createButton.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            createButton.Click += CreateButton_Click;
            Controls.Add(createButton);

            Button cancelButton = new Button();
            cancelButton.Text = "Back to Login";
            cancelButton.Bounds = new Rectangle(515, 495, 400, 38);
            cancelButton.BackColor = Color.White;
            cancelButton.ForeColor = navy;
            cancelButton.FlatStyle = FlatStyle.Flat;
            cancelButton.FlatAppearance.BorderColor =
                Color.FromArgb(220, 220, 230);
            cancelButton.Cursor = Cursors.Hand;
            cancelButton.Click += (s, e) => Close();
            Controls.Add(cancelButton);

            UpdateCreateButton();
            AcceptButton = createButton;
        }

        private void AddField(string caption, int top, out TextBox box)
        {
            Label label = new Label();
            label.Text = caption;
            label.Location = new Point(515, top);
            label.AutoSize = true;
            Controls.Add(label);

            box = new TextBox();
            box.Bounds = new Rectangle(515, top + 27, 400, 32);
            box.Font = new Font("Segoe UI", 11);
            Controls.Add(box);
        }

        private bool IsValidEmail(string email)
        {
            int at = email.IndexOf('@');
            int dot = email.LastIndexOf('.');
            return at > 0 && dot > at + 1 && dot < email.Length - 1;
        }

        private void UpdateCreateButton()
        {
            if (createButton == null)
                return;

            createButton.Enabled =
                nameBox.Text.Trim().Length > 0 &&
                IsValidEmail(emailBox.Text.Trim()) &&
                passwordBox.Text.Length >= 6 &&
                passwordBox.Text == confirmBox.Text;
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            string name = nameBox.Text.Trim();
            string email = emailBox.Text.Trim();
            string password = passwordBox.Text;

            foreach (User user in DataStore.Users)
            {
                if (string.Equals(user.Email, email,
                    StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("This email is already registered.");
                    return;
                }
            }

            User newUser = new User();
            newUser.Name = name;
            newUser.Email = email;
            newUser.Password = password;

            DataStore.Users.Add(newUser);

            MessageBox.Show(
                "Account created successfully! You can now sign in.");

            Close();
        }
    }
}