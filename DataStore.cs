using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace COLLABHUB
{
    public class AutoSaveList<T> : List<T>
    {
        public new void Add(T item)
        {
            base.Add(item);
            DataStore.Save();
        }

        public new bool Remove(T item)
        {
            bool removed = base.Remove(item);

            if (removed)
                DataStore.Save();

            return removed;
        }

        public new void Clear()
        {
            base.Clear();
            DataStore.Save();
        }

        public void AddRangeSilently(IEnumerable<T> items)
        {
            base.AddRange(items);
        }
    }

    public static class DataStore
    {
        public static AutoSaveList<User> Users =
            new AutoSaveList<User>();

        public static AutoSaveList<Project> Projects =
            new AutoSaveList<Project>();

        public static AutoSaveList<TaskItem> Tasks =
            new AutoSaveList<TaskItem>();

        public static User CurrentUser = null;

        private static readonly string FolderPath =
            Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                "CollabHub");

        private static readonly string FilePath =
            Path.Combine(FolderPath, "collabhub-data.txt");

        static DataStore()
        {
            Load();

            AppDomain.CurrentDomain.ProcessExit +=
                (sender, e) => Save();
        }

        private static string Encode(string value)
        {
            return Convert.ToBase64String(
                Encoding.UTF8.GetBytes(value ?? ""));
        }

        private static string Decode(string value)
        {
            return Encoding.UTF8.GetString(
                Convert.FromBase64String(value));
        }

        public static void Save()
        {
            try
            {
                Directory.CreateDirectory(FolderPath);

                List<string> lines = new List<string>();

                foreach (User user in Users)
                {
                    lines.Add(
                        "U\t" +
                        Encode(user.Name) + "\t" +
                        Encode(user.Email) + "\t" +
                        Encode(user.Password));
                }

                foreach (Project project in Projects)
                {
                    lines.Add(
                        "P\t" +
                        Encode(project.Title) + "\t" +
                        Encode(project.Description) + "\t" +
                        Encode(project.Owner));
                }

                foreach (TaskItem task in Tasks)
                {
                    lines.Add(
                        "T\t" +
                        Encode(task.Title) + "\t" +
                        Encode(task.Priority) + "\t" +
                        (task.IsDone ? "1" : "0"));
                }

                string tempPath = FilePath + ".tmp";

                File.WriteAllLines(tempPath, lines, Encoding.UTF8);

                if (File.Exists(FilePath))
                    File.Delete(FilePath);

                File.Move(tempPath, FilePath);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Data save nahi ho saka.\n\n" + ex.Message,
                    "CollabHub - Save Error");
            }
        }

        private static void Load()
        {
            try
            {
                if (!File.Exists(FilePath))
                    return;

                string[] lines = File.ReadAllLines(
                    FilePath, Encoding.UTF8);

                foreach (string line in lines)
                {
                    try
                    {
                        string[] parts = line.Split('\t');

                        if (parts.Length == 4 && parts[0] == "U")
                        {
                            User user = new User();
                            user.Name = Decode(parts[1]);
                            user.Email = Decode(parts[2]);
                            user.Password = Decode(parts[3]);

                            Users.AddRangeSilently(
                                new User[] { user });
                        }
                        else if (parts.Length == 4 &&
                                 parts[0] == "P")
                        {
                            Project project = new Project();
                            project.Title = Decode(parts[1]);
                            project.Description = Decode(parts[2]);
                            project.Owner = Decode(parts[3]);

                            Projects.AddRangeSilently(
                                new Project[] { project });
                        }
                        else if (parts.Length == 4 &&
                                 parts[0] == "T")
                        {
                            TaskItem task = new TaskItem();
                            task.Title = Decode(parts[1]);
                            task.Priority = Decode(parts[2]);
                            task.IsDone = parts[3] == "1";

                            Tasks.AddRangeSilently(
                                new TaskItem[] { task });
                        }
                    }
                    catch
                    {
                        // Ignore an invalid record and load the rest.
                    }
                }

                // Login again after restarting the app.
                CurrentUser = null;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Saved data load nahi ho saka.\n\n" + ex.Message,
                    "CollabHub - Load Error");
            }
        }
    }
}