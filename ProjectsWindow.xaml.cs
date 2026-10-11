using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Controls;

namespace TaskManagerApp
{
    public class ProjectModel
    {
        public string? ProjectName { get; set; }
        public bool IsCompleted { get; set; }
        public ObservableCollection<TaskModel> Tasks { get; set; } = new ObservableCollection<TaskModel>();
    }

    public partial class ProjectsWindow : Window
    {
        public ObservableCollection<ProjectModel> Projects { get; set; }

        public ProjectsWindow()
        {
            InitializeComponent();
            Projects = AppRepository.Projects;
            ProjectsListBox.ItemsSource = Projects;
            UpdateVisibility();
        }

        private void ProjectCard_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is System.Windows.Controls.Border border && border.DataContext is ProjectModel selectedProject)
            {
                string projectName = string.IsNullOrEmpty(selectedProject.ProjectName) ? "New Project" : selectedProject.ProjectName;
                AppRepository.CurrentProject = selectedProject;
                // Deschidem fereastra de task-uri
                TasksWindow tasksWin = new TasksWindow(projectName);
                tasksWin.Show();

                // Acum putem închide fereastra siguranți că datele sunt salvate în AppRepository
                this.Close();
            }
        }

        private void AddProject_Click(object sender, RoutedEventArgs e)
        {
            var newProject = new ProjectModel { ProjectName = "New Project" };
            Projects.Add(newProject);
            UpdateVisibility();
            AppRepository.SaveToFile();
            ProjectsListBox.Dispatcher.BeginInvoke(new System.Action(() =>
            {
                ProjectsListBox.SelectedItem = newProject;
            }));
        }

        private void UpdateVisibility()
        {
            if (Projects.Count > 0)
            {
                NoIdeasText.Visibility = Visibility.Collapsed;
                ProjectsListBox.Visibility = Visibility.Visible;
            }
            else
            {
                NoIdeasText.Visibility = Visibility.Visible;
                ProjectsListBox.Visibility = Visibility.Collapsed;
            }
        }

        private void TextBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is System.Windows.Controls.TextBox textBox)
            {
                textBox.IsReadOnly = false; // deblocheaza pt scriere
                textBox.Focus(); // cursoru este pe el
                textBox.SelectAll(); //selecteaza tot textul
            }

        }

        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (sender is System.Windows.Controls.TextBox textBox)
                {
                    textBox.IsReadOnly = true;
                    System.Windows.Input.Keyboard.ClearFocus();
                    //scoate focusul de pe textbox pentru a salva starea
                }

            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.TextBox textBox)
            {
                textBox.IsReadOnly = true;
                AppRepository.SaveToFile();
            }
        }

        private void ToggleProject_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is ProjectModel project)
            {
                // Inversăm starea de completare
                project.IsCompleted = !project.IsCompleted;

                // Forțăm lista să se actualizeze vizual
                ProjectsListBox.Items.Refresh();
                AppRepository.SaveToFile();
            }
        }

        private void BackToMain_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWin = new MainWindow();
            mainWin.Show();
            this.Close();
        }
        private void DeleteProject_Click(object sender, RoutedEventArgs e)
         {
             if (sender is Button button && button.DataContext is ProjectModel project)
         {
        Projects.Remove(project);
        UpdateVisibility();
        AppRepository.SaveToFile(); // Salvăm modificarea pe disc
        }
         }
    }
}