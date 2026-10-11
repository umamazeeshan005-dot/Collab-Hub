
using System;
using System.Drawing;
using System.Windows.Forms;

namespace COLLABHUB
{
    public partial class LoginForm : Form
    {
        private TextBox emailBox;
        private TextBox passwordBox;
        private Panel mainContainer;
        private Button loginButton;

        private readonly Color purple = Color.FromArgb(101, 83, 220);
        private readonly Color navy = Color.FromArgb(31, 36, 75);

        public LoginForm()
        {
            InitializeComponent();
            BuildLogin();
        }

        private void BuildLogin()
        {
            Controls.Clear();

            Text = "CollabHub | Login";
            ClientSize = new Size(1000, 620);
            MinimumSize = new Size(1000, 620);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.Sizable;
            MaximizeBox = true;
            BackColor = Color.FromArgb(245, 247, 250);
            Font = new Font("Segoe UI", 10);

            mainContainer = new Panel();
            mainContainer.Size = new Size(920, 580);
            mainContainer.BackColor = Color.White;
            Controls.Add(mainContainer);

            CenterMainContainer();
            Resize += (s, e) => CenterMainContainer();

            // LEFT PANEL
            Panel hero = new Panel();
            hero.Bounds = new Rectangle(0, 0, 420, 580);
            hero.BackColor = navy;
            mainContainer.Controls.Add(hero);

            Label logo = new Label();
            logo.Text = "CollabHub";
            logo.Font = new Font("Segoe UI", 28, FontStyle.Bold);
            logo.ForeColor = Color.White;
            logo.AutoSize = true;
            logo.Location = new Point(45, 65);
            hero.Controls.Add(logo);

            Label heroTitle = new Label();
            heroTitle.Text = "Build something\namazing together.";
            heroTitle.Font = new Font("Segoe UI", 25, FontStyle.Bold);
            heroTitle.ForeColor = Color.White;
            heroTitle.AutoSize = true;
            heroTitle.Location = new Point(45, 190);
            hero.Controls.Add(heroTitle);

            Label heroText = new Label();
            heroText.Text = "Find teammates, manage projects\nand turn your ideas into reality.";
            heroText.Font = new Font("Segoe UI", 12);
            heroText.ForeColor = Color.FromArgb(205, 210, 235);
            heroText.AutoSize = true;
            heroText.Location = new Point(48, 320);
            hero.Controls.Add(heroText);

            Label footer = new Label();
            footer.Text = "YOUR IDEAS. YOUR TEAM. YOUR FUTURE.";
            footer.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            footer.ForeColor = Color.LightGray;
            footer.AutoSize = true;
            footer.Location = new Point(45, 525);
            hero.Controls.Add(footer);

            // RIGHT PANEL
            Label title = new Label();
            title.Text = "Welcome back!";
            title.Font = new Font("Segoe UI", 25, FontStyle.Bold);
            title.ForeColor = navy;
            title.AutoSize = true;
            title.Location = new Point(480, 85);
            mainContainer.Controls.Add(title);

            Label subtitle = new Label();
            subtitle.Text = "Sign in to continue to your workspace.";
            subtitle.ForeColor = Color.Gray;
            subtitle.AutoSize = true;
            subtitle.Location = new Point(483, 135);
            mainContainer.Controls.Add(subtitle);

            Label emailLabel = new Label();
            emailLabel.Text = "Email address";
            emailLabel.Location = new Point(485, 190);
            emailLabel.AutoSize = true;
            mainContainer.Controls.Add(emailLabel);

            emailBox = new TextBox();
            emailBox.Bounds = new Rectangle(485, 220, 390, 38);
            emailBox.Font = new Font("Segoe UI", 12);
            emailBox.TextChanged += (s, e) => UpdateLoginButton();
            mainContainer.Controls.Add(emailBox);

            Label passwordLabel = new Label();
            passwordLabel.Text = "Password";
            passwordLabel.Location = new Point(485, 280);
            passwordLabel.AutoSize = true;
            mainContainer.Controls.Add(passwordLabel);

            passwordBox = new TextBox();
            passwordBox.Bounds = new Rectangle(485, 310, 390, 38);
            passwordBox.Font = new Font("Segoe UI", 12);
            passwordBox.PasswordChar = '*';
            passwordBox.TextChanged += (s, e) => UpdateLoginButton();
            mainContainer.Controls.Add(passwordBox);

            // SIGN IN BUTTON
            loginButton = new Button();
            loginButton.Text = "Sign In";
            loginButton.Bounds = new Rectangle(485, 375, 390, 45);
            StyleButton(loginButton, purple);
            loginButton.Click += LoginButton_Click;

            // Initially disabled until both fields are filled
            loginButton.Enabled = false;

            mainContainer.Controls.Add(loginButton);

            Label accountText = new Label();
            accountText.Text = "Don't have an account?";
            accountText.AutoSize = true;
            accountText.Location = new Point(500, 455);
            accountText.ForeColor = Color.Gray;
            mainContainer.Controls.Add(accountText);

            Button registerButton = new Button();
            registerButton.Text = "Create Account";
            registerButton.Bounds = new Rectangle(665, 445, 210, 35);
            registerButton.FlatStyle = FlatStyle.Flat;
            registerButton.FlatAppearance.BorderSize = 0;
            registerButton.ForeColor = purple;
            registerButton.BackColor = Color.White;
            registerButton.UseVisualStyleBackColor = false;
            registerButton.Cursor = Cursors.Hand;
            registerButton.Click += RegisterButton_Click;
            mainContainer.Controls.Add(registerButton);

            AcceptButton = loginButton;

            UpdateLoginButton();
        }

        private void CenterMainContainer()
        {
            if (mainContainer != null)
            {
                mainContainer.Location = new Point(
                    Math.Max(0, (ClientSize.Width - mainContainer.Width) / 2),
                    Math.Max(0, (ClientSize.Height - mainContainer.Height) / 2)
                );
            }
        }

        private void StyleButton(Button button, Color color)
        {
            button.BackColor = color;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            button.Cursor = Cursors.Hand;
            button.UseVisualStyleBackColor = false;
        }

        private void UpdateLoginButton()
        {
            bool emailEntered = !string.IsNullOrWhiteSpace(emailBox.Text);
            bool passwordEntered = !string.IsNullOrWhiteSpace(passwordBox.Text);

            // Button is clickable only when both fields contain text
            loginButton.Enabled = emailEntered && passwordEntered;

            // Keep the purple colour
            loginButton.BackColor = purple;
            loginButton.ForeColor = Color.White;
            loginButton.UseVisualStyleBackColor = false;
        }

        private bool IsValidEmail(string email)
        {
            int atIndex = email.IndexOf('@');
            int dotIndex = email.LastIndexOf('.');

            return atIndex > 0 &&
                   dotIndex > atIndex + 1 &&
                   dotIndex < email.Length - 1 &&
                   !email.Contains(" ");
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            string email = emailBox.Text.Trim();
            string password = passwordBox.Text;

            // Safety check; normally this code won't run if fields are empty
            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                return;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show(
                    "Please enter a valid email address.",
                    "Invalid Email",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            User found = null;

            foreach (User user in DataStore.Users)
            {
                if (string.Equals(
                        user.Email,
                        email,
                        StringComparison.OrdinalIgnoreCase) &&
                    user.Password == password)
                {
                    found = user;
                    break;
                }
            }

            if (found == null)
            {
                MessageBox.Show(
                    "Account not found. Please register first or check your details.",
                    "Login Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            DataStore.CurrentUser = found;

            DashboardForm dashboard = new DashboardForm(found.Name);

            dashboard.FormClosed += (s, args) =>
            {
                passwordBox.Clear();
                Show();
            };

            Hide();
            dashboard.Show();
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            using (RegisterForm register = new RegisterForm())
            {
                register.ShowDialog(this);
            }

            UpdateLoginButton();
        }
    }
}

