using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagerLib;
using TaskManagerLib.Models;

namespace TaskManagerUI
{
    public partial class MainWindow : Window
    {
        private TaskManager _taskManager = new TaskManager();
        private string _filePath = "tasks.json";

        public MainWindow()
        {
            InitializeComponent();
            RefreshTaskList();
        }

        private void RefreshTaskList()
        {
            List<TaskItem> tasks = _taskManager.GetAllTasks();

            string statusFilter = (StatusFilterCombo.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Все";
            if (statusFilter != "Все" && !string.IsNullOrEmpty(statusFilter))
            {
                MyTaskStatus status = (MyTaskStatus)Enum.Parse(typeof(MyTaskStatus), statusFilter);
                tasks = tasks.Where(t => t.Status == status).ToList();
            }

            string keyword = SearchBox.Text;
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                tasks = tasks.Where(t => t.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                         t.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                             .ToList();
            }

            TasksListBox.ItemsSource = tasks;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new TaskEditWindow(null);
            if (dialog.ShowDialog() == true)
            {
                _taskManager.AddTask(dialog.GetTask());
                RefreshTaskList();
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (TasksListBox.SelectedItem is TaskItem selected)
            {
                var dialog = new TaskEditWindow(selected);
                if (dialog.ShowDialog() == true)
                {
                    var updated = dialog.GetTask();
                    _taskManager.UpdateTask(selected.Id, updated.Title, updated.Description,
                                            updated.Priority, updated.DueDate, updated.Status,
                                            updated.IsImportant);
                    RefreshTaskList();
                }
            }
            else
                MessageBox.Show("Выберите задачу для редактирования", "Внимание");
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (TasksListBox.SelectedItem is TaskItem selected)
            {
                if (MessageBox.Show($"Удалить задачу \"{selected.Title}\"?", "Подтверждение",
                                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _taskManager.DeleteTask(selected.Id);
                    RefreshTaskList();
                }
            }
            else
                MessageBox.Show("Выберите задачу для удаления", "Внимание");
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshTaskList();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            _taskManager.SaveToFile(_filePath);
            MessageBox.Show("Задачи сохранены!", "Успех");
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            _taskManager.LoadFromFile(_filePath);
            RefreshTaskList();
            MessageBox.Show("Задачи загружены из файла", "Успех");
        }

        private void StatusFilterCombo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            RefreshTaskList();
        }

        private void SearchBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            RefreshTaskList();
        }

        private void BtnStat_Click(object sender, RoutedEventArgs e)
        {
            var stats = _taskManager.GetStatistics();
            MessageBox.Show($"Завершено: {stats.completed}\nПросрочено: {stats.overdue}",
                            "Статистика");
        }
    }
}