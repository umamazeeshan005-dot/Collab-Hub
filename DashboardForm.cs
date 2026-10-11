
using System;
using System.Drawing;
using System.Windows.Forms;

namespace COLLABHUB
{
    public partial class DashboardForm : Form
    {
        private Panel sidebar;
        private Panel contentPanel;
        private Label pageTitle;
        private string studentName;

        private readonly Color purple = Color.FromArgb(101, 83, 220);
        private readonly Color navy = Color.FromArgb(31, 36, 75);
        private readonly Color background = Color.FromArgb(246, 247, 252);
        private readonly Color muted = Color.FromArgb(105, 110, 130);

        private TextBox projectTitleBox;
        private TextBox projectDescriptionBox;
        private BlueDisabledButton addProjectButton;
        private FlowLayoutPanel projectCards;

        private TextBox taskTitleBox;
        private ComboBox taskPriorityBox;
        private BlueDisabledButton addTaskButton;
        private ListBox taskList;

        public DashboardForm(string name = "Student")
        {
            InitializeComponent();

            studentName = DataStore.CurrentUser != null
                ? DataStore.CurrentUser.Name : name;

            BuildDashboard();
            ShowHome();
        }

        private void BuildDashboard()
        {
            Controls.Clear();

            Text = "CollabHub | Workspace";
            ClientSize = new Size(1180, 720);
            MinimumSize = new Size(1000, 650);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = background;
            Font = new Font("Segoe UI", 10);

            sidebar = new Panel();
            sidebar.Dock = DockStyle.Left;
            sidebar.Width = 225;
            sidebar.BackColor = navy;

            Label logo = new Label();
            logo.Text = "CollabHub";
            logo.Font = new Font("Segoe UI", 23, FontStyle.Bold);
            logo.ForeColor = Color.White;
            logo.Location = new Point(22, 25);
            logo.AutoSize = true;
            sidebar.Controls.Add(logo);

            Label tagline = new Label();
            tagline.Text = "STUDENT WORKSPACE";
            tagline.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            tagline.ForeColor = Color.FromArgb(185, 190, 220);
            tagline.Location = new Point(26, 70);
            tagline.AutoSize = true;
            sidebar.Controls.Add(tagline);

            AddNavigation("Dashboard", 130, ShowHome);
            AddNavigation("My Profile", 180, ShowProfile);
            AddNavigation("Projects", 230, ShowProjects);
            AddNavigation("Teams", 280, ShowTeams);
            AddNavigation("Tasks", 330, ShowTasks);
            AddNavigation("Notifications", 380, ShowNotifications);

            Button logout = new Button();
            logout.Text = "Logout";
            logout.Bounds = new Rectangle(20, 620, 185, 42);
            logout.BackColor = Color.FromArgb(190, 65, 85);
            logout.ForeColor = Color.White;
            logout.FlatStyle = FlatStyle.Flat;
            logout.FlatAppearance.BorderSize = 0;
            logout.Cursor = Cursors.Hand;
            logout.Click += Logout_Click;
            sidebar.Controls.Add(logout);

            Panel top = new Panel();
            top.Dock = DockStyle.Top;
            top.Height = 80;
            top.BackColor = Color.White;

            pageTitle = new Label();
            pageTitle.Text = "Dashboard";
            pageTitle.Font = new Font("Segoe UI", 19, FontStyle.Bold);
            pageTitle.ForeColor = navy;
            pageTitle.AutoSize = true;
            pageTitle.Location = new Point(30, 22);
            top.Controls.Add(pageTitle);

            Label userLabel = new Label();
            userLabel.Text = "Welcome, " + studentName;
            userLabel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            userLabel.ForeColor = muted;
            userLabel.AutoSize = true;
            userLabel.Location = new Point(790, 30);
            top.Controls.Add(userLabel);

            contentPanel = new Panel();
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.BackColor = background;
            contentPanel.AutoScroll = true;

            Controls.Add(contentPanel);
            Controls.Add(top);
            Controls.Add(sidebar);
        }

        private void AddNavigation(string text, int top, Action action)
        {
            Button button = new Button();
            button.Text = "   " + text;
            button.Bounds = new Rectangle(15, top, 195, 42);
            button.BackColor = navy;
            button.ForeColor = Color.White;
            button.Font = new Font("Segoe UI", 10);
            button.TextAlign = ContentAlignment.MiddleLeft;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = purple;
            button.Cursor = Cursors.Hand;
            button.Click += (s, e) => action();
            sidebar.Controls.Add(button);
        }

        private void SetPage(string title)
        {
            pageTitle.Text = title;
            contentPanel.Controls.Clear();

            projectTitleBox = null;
            projectDescriptionBox = null;
            addProjectButton = null;
            projectCards = null;

            taskTitleBox = null;
            taskPriorityBox = null;
            addTaskButton = null;
            taskList = null;
        }

        private Label AddLabel(string text, int x, int y,
            int fontSize = 10, bool bold = false)
        {
            Label label = new Label();
            label.Text = text;
            label.Location = new Point(x, y);
            label.AutoSize = true;
            label.ForeColor = navy;
            label.Font = new Font("Segoe UI", fontSize,
                bold ? FontStyle.Bold : FontStyle.Regular);
            contentPanel.Controls.Add(label);
            return label;
        }

        private Panel AddCard(int x, int y, int width, int height)
        {
            Panel card = new Panel();
            card.Location = new Point(x, y);
            card.Size = new Size(width, height);
            card.BackColor = Color.White;
            card.BorderStyle = BorderStyle.FixedSingle;
            contentPanel.Controls.Add(card);
            return card;
        }

        private Button MakeButton(string text, int x, int y,
            int width, EventHandler click)
        {
            Button button = new Button();
            button.Text = text;
            button.Location = new Point(x, y);
            button.Size = new Size(width, 40);
            button.BackColor = purple;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            button.Cursor = Cursors.Hand;
            button.Click += click;
            contentPanel.Controls.Add(button);
            return button;
        }

        private void ShowHome()
        {
            SetPage("Dashboard");

            AddLabel("Your workspace, all in one place.", 30, 25, 17, true);
            AddLabel("Manage your student projects and tasks.", 30, 60);

            int[] values =
            {
                DataStore.Projects.Count,
                DataStore.Tasks.Count,
                DataStore.Users.Count
            };

            string[] titles = { "Total Projects", "Total Tasks", "Students" };
            int[] xs = { 30, 270, 510 };

            for (int i = 0; i < titles.Length; i++)
            {
                Panel card = AddCard(xs[i], 110, 215, 125);

                Label heading = new Label();
                heading.Text = titles[i];
                heading.Font = new Font("Segoe UI", 10);
                heading.ForeColor = muted;
                heading.Location = new Point(18, 18);
                heading.AutoSize = true;
                card.Controls.Add(heading);

                Label number = new Label();
                number.Text = values[i].ToString();
                number.Font = new Font("Segoe UI", 27, FontStyle.Bold);
                number.ForeColor = purple;
                number.Location = new Point(18, 48);
                number.AutoSize = true;
                card.Controls.Add(number);
            }

            AddLabel("Quick actions", 30, 275, 14, true);

            MakeButton("Manage Projects", 30, 320, 180,
                (s, e) => ShowProjects());

            MakeButton("Manage Tasks", 230, 320, 180,
                (s, e) => ShowTasks());

            Panel welcomeCard = AddCard(30, 390, 700, 120);

            Label welcome = new Label();
            welcome.Text = "Welcome to CollabHub!";
            welcome.Font = new Font("Segoe UI", 17, FontStyle.Bold);
            welcome.ForeColor = navy;
            welcome.Location = new Point(20, 20);
            welcome.AutoSize = true;
            welcomeCard.Controls.Add(welcome);

            Label description = new Label();
            description.Text =
                "Create projects, organize your work and collaborate with students.";
            description.Font = new Font("Segoe UI", 10);
            description.ForeColor = muted;
            description.Location = new Point(22, 60);
            description.AutoSize = true;
            welcomeCard.Controls.Add(description);
        }

        private void ShowProfile()
        {
            SetPage("My Profile");

            Panel card = AddCard(30, 30, 650, 270);

            Label heading = new Label();
            heading.Text = "Student Information";
            heading.Font = new Font("Segoe UI", 17, FontStyle.Bold);
            heading.ForeColor = navy;
            heading.Location = new Point(25, 22);
            heading.AutoSize = true;
            card.Controls.Add(heading);

            string email = DataStore.CurrentUser != null
                ? DataStore.CurrentUser.Email : "Not available";

            AddProfileText(card, "Full name", studentName, 85);
            AddProfileText(card, "Email address", email, 145);
            AddProfileText(card, "Account type", "Student", 205);
        }

        private void AddProfileText(Panel card,
            string title, string value, int y)
        {
            Label label = new Label();
            label.Text = title;
            label.ForeColor = muted;
            label.Location = new Point(25, y);
            label.AutoSize = true;
            card.Controls.Add(label);

            Label valueLabel = new Label();
            valueLabel.Text = value;
            valueLabel.ForeColor = navy;
            valueLabel.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            valueLabel.Location = new Point(200, y);
            valueLabel.AutoSize = true;
            card.Controls.Add(valueLabel);
        }

        private void ShowProjects()
        {
            SetPage("Projects");
            AddLabel("Create and manage your projects.", 30, 25, 15, true);

            Panel formCard = AddCard(30, 65, 700, 220);

            Label titleLabel = new Label();
            titleLabel.Text = "Project title";
            titleLabel.Location = new Point(20, 18);
            titleLabel.AutoSize = true;
            titleLabel.ForeColor = navy;
            formCard.Controls.Add(titleLabel);

            projectTitleBox = new TextBox();
            projectTitleBox.Location = new Point(20, 45);
            projectTitleBox.Size = new Size(650, 30);
            projectTitleBox.Font = new Font("Segoe UI", 11);
            formCard.Controls.Add(projectTitleBox);

            Label descLabel = new Label();
            descLabel.Text = "Project description";
            descLabel.Location = new Point(20, 85);
            descLabel.AutoSize = true;
            descLabel.ForeColor = navy;
            formCard.Controls.Add(descLabel);

            projectDescriptionBox = new TextBox();
            projectDescriptionBox.Location = new Point(20, 110);
            projectDescriptionBox.Size = new Size(650, 35);
            projectDescriptionBox.Font = new Font("Segoe UI", 10);
            formCard.Controls.Add(projectDescriptionBox);

            addProjectButton = new BlueDisabledButton();
            addProjectButton.Text = "+  Add Project";
            addProjectButton.Location = new Point(20, 160);
            addProjectButton.Size = new Size(160, 40);
            addProjectButton.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            addProjectButton.Click += AddProject_Click;
            formCard.Controls.Add(addProjectButton);

            projectTitleBox.TextChanged += (s, e) => UpdateProjectButton();
            projectDescriptionBox.TextChanged += (s, e) => UpdateProjectButton();

            AddLabel("Your projects", 30, 305, 14, true);

            projectCards = new FlowLayoutPanel();
            projectCards.Location = new Point(30, 340);
            projectCards.Size = new Size(800, 260);
            projectCards.AutoScroll = true;
            projectCards.WrapContents = true;
            projectCards.BackColor = background;
            contentPanel.Controls.Add(projectCards);

            UpdateProjectButton();
            RefreshProjectCards();
        }

        private void UpdateProjectButton()
        {
            if (addProjectButton == null)
                return;

            addProjectButton.Enabled =
                !string.IsNullOrWhiteSpace(projectTitleBox.Text) &&
                !string.IsNullOrWhiteSpace(projectDescriptionBox.Text);
        }

        private void AddProject_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(projectTitleBox.Text) ||
                string.IsNullOrWhiteSpace(projectDescriptionBox.Text))
                return;

            Project project = new Project();
            project.Title = projectTitleBox.Text.Trim();
            project.Description = projectDescriptionBox.Text.Trim();
            project.Owner = studentName;

            DataStore.Projects.Add(project);

            projectTitleBox.Clear();
            projectDescriptionBox.Clear();

            RefreshProjectCards();
            MessageBox.Show("Project added successfully!");
        }

        private void RefreshProjectCards()
        {
            if (projectCards == null)
                return;

            projectCards.Controls.Clear();

            if (DataStore.Projects.Count == 0)
            {
                Label empty = new Label();
                empty.Text = "No projects yet. Add your first project!";
                empty.AutoSize = true;
                empty.ForeColor = muted;
                empty.Margin = new Padding(5, 10, 5, 5);
                projectCards.Controls.Add(empty);
                return;
            }

            foreach (Project project in DataStore.Projects.ToArray())
            {
                Panel card = new Panel();
                card.Size = new Size(350, 135);
                card.BackColor = Color.White;
                card.BorderStyle = BorderStyle.FixedSingle;
                card.Margin = new Padding(5);
                projectCards.Controls.Add(card);

                Label title = new Label();
                title.Text = project.Title;
                title.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                title.ForeColor = purple;
                title.Location = new Point(12, 12);
                title.Size = new Size(215, 25);
                title.AutoEllipsis = true;
                card.Controls.Add(title);

                Button deleteButton = new Button();
                deleteButton.Text = "Delete";
                deleteButton.Location = new Point(255, 8);
                deleteButton.Size = new Size(80, 30);
                deleteButton.BackColor = Color.FromArgb(190, 65, 85);
                deleteButton.ForeColor = Color.White;
                deleteButton.FlatStyle = FlatStyle.Flat;
                deleteButton.FlatAppearance.BorderSize = 0;
                deleteButton.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                deleteButton.Cursor = Cursors.Hand;

                deleteButton.Click += (sender, e) =>
                {
                    string currentOwner = DataStore.CurrentUser != null
                        ? DataStore.CurrentUser.Name : studentName;

                    if (!string.Equals(
                        project.Owner,
                        currentOwner,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show(
                            "You can only delete your own projects.",
                            "Permission Denied",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        return;
                    }

                    DialogResult result = MessageBox.Show(
                        "Are you sure you want to delete \"" +
                        project.Title + "\"?",
                        "Confirm Delete",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        DataStore.Projects.Remove(project);

                        RefreshProjectCards();

                        MessageBox.Show(
                            "Project deleted successfully!",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                };

                card.Controls.Add(deleteButton);

                Label description = new Label();
                description.Text = project.Description;
                description.Font = new Font("Segoe UI", 9);
                description.ForeColor = muted;
                description.Location = new Point(12, 48);
                description.Size = new Size(320, 40);
                card.Controls.Add(description);

                Label owner = new Label();
                owner.Text = "Created by: " + project.Owner;
                owner.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                owner.ForeColor = navy;
                owner.Location = new Point(12, 100);
                owner.AutoSize = true;
                card.Controls.Add(owner);
            }
        }

        private void ShowTeams()
        {
            SetPage("Teams");

            AddLabel("Student community", 30, 25, 16, true);
            AddLabel("Registered student accounts", 30, 60);

            ListBox list = new ListBox();
            list.Location = new Point(30, 105);
            list.Size = new Size(650, 350);
            list.Font = new Font("Segoe UI", 11);
            list.BackColor = Color.White;

            foreach (User user in DataStore.Users)
                list.Items.Add(user.Name + "  |  " + user.Email);

            contentPanel.Controls.Add(list);

            if (DataStore.Users.Count == 0)
                AddLabel("No students registered yet.", 35, 470);
        }

        private void ShowTasks()
        {
            SetPage("Tasks");
            AddLabel("Organize your work and track progress.", 30, 25, 15, true);

            Panel formCard = AddCard(30, 65, 700, 175);

            Label titleLabel = new Label();
            titleLabel.Text = "Task title";
            titleLabel.Location = new Point(20, 18);
            titleLabel.AutoSize = true;
            titleLabel.ForeColor = navy;
            formCard.Controls.Add(titleLabel);

            taskTitleBox = new TextBox();
            taskTitleBox.Location = new Point(20, 45);
            taskTitleBox.Size = new Size(400, 30);
            taskTitleBox.Font = new Font("Segoe UI", 11);
            formCard.Controls.Add(taskTitleBox);

            Label priorityLabel = new Label();
            priorityLabel.Text = "Priority";
            priorityLabel.Location = new Point(445, 18);
            priorityLabel.AutoSize = true;
            priorityLabel.ForeColor = navy;
            formCard.Controls.Add(priorityLabel);

            taskPriorityBox = new ComboBox();
            taskPriorityBox.Location = new Point(445, 45);
            taskPriorityBox.Size = new Size(220, 30);
            taskPriorityBox.DropDownStyle = ComboBoxStyle.DropDownList;
            taskPriorityBox.Items.AddRange(new object[]
                { "Low", "Medium", "High" });
            taskPriorityBox.SelectedIndex = 1;
            formCard.Controls.Add(taskPriorityBox);

            addTaskButton = new BlueDisabledButton();
            addTaskButton.Text = "+  Add Task";
            addTaskButton.Location = new Point(20, 100);
            addTaskButton.Size = new Size(160, 40);
            addTaskButton.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            addTaskButton.Click += AddTask_Click;
            formCard.Controls.Add(addTaskButton);

            taskTitleBox.TextChanged += (s, e) => UpdateTaskButton();

            AddLabel("Your task list", 30, 265, 14, true);

            taskList = new ListBox();
            taskList.Location = new Point(30, 300);
            taskList.Size = new Size(700, 220);
            taskList.Font = new Font("Segoe UI", 11);
            taskList.BackColor = Color.White;
            contentPanel.Controls.Add(taskList);

            MakeButton("Mark Selected as Done", 30, 540, 220,
                MarkTaskDone_Click);

            UpdateTaskButton();
            RefreshTaskList();
        }

        private void UpdateTaskButton()
        {
            if (addTaskButton == null)
                return;

            addTaskButton.Enabled =
                !string.IsNullOrWhiteSpace(taskTitleBox.Text);
        }

        private void AddTask_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(taskTitleBox.Text))
                return;

            TaskItem task = new TaskItem();
            task.Title = taskTitleBox.Text.Trim();
            task.Priority = taskPriorityBox.SelectedItem != null
                ? taskPriorityBox.SelectedItem.ToString() : "Medium";
            task.IsDone = false;

            DataStore.Tasks.Add(task);

            taskTitleBox.Clear();
            RefreshTaskList();

            MessageBox.Show("Task added successfully!");
        }

        private void RefreshTaskList()
        {
            if (taskList == null)
                return;

            taskList.Items.Clear();

            foreach (TaskItem task in DataStore.Tasks)
                taskList.Items.Add(task.ToText());
        }

        private void MarkTaskDone_Click(object sender, EventArgs e)
        {
            int index = taskList.SelectedIndex;

            if (index < 0 || index >= DataStore.Tasks.Count)
            {
                MessageBox.Show("Pehle task select karo.");
                return;
            }

            DataStore.Tasks[index].IsDone = true;
            RefreshTaskList();
            taskList.SelectedIndex = index;
        }

        private void ShowNotifications()
        {
            SetPage("Notifications");

            AddLabel("Notifications & activity", 30, 25, 16, true);

            Panel card = AddCard(30, 75, 650, 180);

            Label heading = new Label();
            heading.Text = "Your workspace summary";
            heading.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            heading.ForeColor = navy;
            heading.Location = new Point(20, 20);
            heading.AutoSize = true;
            card.Controls.Add(heading);

            Label projects = new Label();
            projects.Text = "Projects created: " + DataStore.Projects.Count;
            projects.Location = new Point(25, 70);
            projects.AutoSize = true;
            projects.ForeColor = muted;
            card.Controls.Add(projects);

            Label tasks = new Label();
            tasks.Text = "Tasks created: " + DataStore.Tasks.Count;
            tasks.Location = new Point(25, 110);
            tasks.AutoSize = true;
            tasks.ForeColor = muted;
            card.Controls.Add(tasks);
        }

        private void Logout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Do you want to log out?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                DataStore.CurrentUser = null;
                Close();
            }
        }
    }
}
