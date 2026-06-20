using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TaskManagerLib.Models;

namespace TaskManagerUI
{
    public partial class TaskEditWindow : Window
    {
        private TaskItem _editingTask;

        public TaskEditWindow(TaskItem task)
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;

            if (task != null)
            {
                _editingTask = task;
                LoadTaskData();
            }
            else
            {
                DueDatePicker.SelectedDate = DateTime.Today.AddDays(3);
                PriorityCombo.SelectedIndex = 1;
                StatusCombo.SelectedIndex = 0;
            }
        }

        private void LoadTaskData()
        {
            TitleBox.Text = _editingTask.Title;
            DescBox.Text = _editingTask.Description;
            PriorityCombo.SelectedIndex = (int)_editingTask.Priority;
            DueDatePicker.SelectedDate = _editingTask.DueDate;
            StatusCombo.SelectedIndex = (int)_editingTask.Status;
            ImportantCheck.IsChecked = _editingTask.IsImportant;
        }

        private TaskItem GetTaskFromForm()
        {
            return new TaskItem
            {
                Title = TitleBox.Text,
                Description = DescBox.Text,
                Priority = (TaskPriority)PriorityCombo.SelectedIndex,
                DueDate = DueDatePicker.SelectedDate ?? DateTime.Today,
                Status = (MyTaskStatus)StatusCombo.SelectedIndex,
                IsImportant = ImportantCheck.IsChecked == true
            };
        }

        public TaskItem GetTask()
        {
            return GetTaskFromForm();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleBox.Text))
            {
                MessageBox.Show("Введите название задачи", "Ошибка");
                return;
            }
            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}