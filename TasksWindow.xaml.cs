using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TaskManagerApp
{
    public class TaskModel
    {
        public string? TaskName { get; set; }
        public bool IsCompleted { get; set; }
    }

    public partial class TasksWindow : Window
    {
        public ObservableCollection<TaskModel> Tasks { get; set; }

        public TasksWindow(string projectName)
        {
            InitializeComponent();

            if (AppRepository.CurrentProject != null)
            {
                // Setăm DataContext-ul pe proiectul curent pentru TwoWay Binding
                this.DataContext = AppRepository.CurrentProject;
                Tasks = AppRepository.CurrentProject.Tasks;
            }
            else
            {
                Tasks = new ObservableCollection<TaskModel>();
            }

            TasksListBox.ItemsSource = Tasks;
            UpdateVisibility();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            var newTask = new TaskModel { TaskName = "New Task" };
            Tasks.Add(newTask);
            UpdateVisibility();
            AppRepository.SaveToFile();

            TasksListBox.Dispatcher.BeginInvoke(new System.Action(() =>
            {
                TasksListBox.SelectedItem = newTask;
            }));
        }

        private void ToggleTask_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is TaskModel task)
            {
                task.IsCompleted = !task.IsCompleted;
                TasksListBox.Items.Refresh();
                AppRepository.SaveToFile();
            }
        }

        private void BackToProjects_Click(object sender, RoutedEventArgs e)
        {
            ProjectsWindow projectsWin = new ProjectsWindow();
            projectsWin.Show();
            this.Close();
        }

        private void UpdateVisibility()
        {
            if (Tasks.Count > 0)
            {
                NoTasksText.Visibility = Visibility.Collapsed;
                TasksListBox.Visibility = Visibility.Visible;
            }
            else
            {
                NoTasksText.Visibility = Visibility.Visible;
                TasksListBox.Visibility = Visibility.Collapsed;
            }
        }

        // Evenimente pentru editarea titlului proiectului
        private void ProjectTitle_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.IsReadOnly = false;
                textBox.Focus();
                textBox.SelectAll();
            }
        }

        private void ProjectTitle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (sender is TextBox textBox)
                {
                    textBox.IsReadOnly = true;
                    Keyboard.ClearFocus();
                }
            }
        }

        private void ProjectTitle_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.IsReadOnly = true;
                AppRepository.SaveToFile();
            }
        }

        // Evenimente pentru editarea numelui task-urilor din listă
        private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.IsReadOnly = false;
                textBox.Focus();
                textBox.SelectAll();
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (sender is TextBox textBox)
                {
                    textBox.IsReadOnly = true;
                    Keyboard.ClearFocus();
                }
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.IsReadOnly = true;
                AppRepository.SaveToFile();
            }
        }
        private void DeleteTask_Click(object sender, RoutedEventArgs e)
      {
    if (sender is Button button && button.DataContext is TaskModel task)
    {
        Tasks.Remove(task);
        UpdateVisibility();
        AppRepository.SaveToFile(); // Salvăm modificarea pe disc
    }
      }
    }
}