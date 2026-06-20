using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;
using TaskManagerLib.Models;

namespace TaskManagerLib
{
    public class TaskManager
    {
        private List<TaskItem> _tasks = new List<TaskItem>();

        private int _nextId = 1;

        public List<TaskItem> GetTasksByStatus(MyTaskStatus status)
        {
            return _tasks.Where(t => t.Status == status).ToList();
        }

        public List<TaskItem> SearchTasks(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return _tasks.ToList();

            return _tasks.Where(t => t.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                     t.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                         .ToList();
        }

        public (int completed, int overdue) GetStatistics()
        {
            int completed = _tasks.Count(t => t.Status == MyTaskStatus.Завершена);
            int overdue = _tasks.Count(t => t.IsOverdue());
            return (completed, overdue);
        }

        public void AddTask(TaskItem task)
        {
            task.Id = _nextId++;
            _tasks.Add(task);
        }

        public bool UpdateTask(int id, string newTitle, string newDescription,
                               TaskPriority newPriority, DateTime newDueDate,
                               MyTaskStatus newStatus, bool newIsImportant)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return false;

            task.Title = newTitle;
            task.Description = newDescription;
            task.Priority = newPriority;
            task.DueDate = newDueDate;
            task.Status = newStatus;
            task.IsImportant = newIsImportant;
            return true;
        }

        public bool DeleteTask(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return false;

            return _tasks.Remove(task);
        }

        public List<TaskItem> GetAllTasks()
        {
            return _tasks.ToList();
        }

        public void SaveToFile(string filePath)
        {
            string json = JsonSerializer.Serialize(new
            {
                NextId = _nextId,
                Tasks = _tasks
            }, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public void LoadFromFile(string filePath)
        {
            if (!File.Exists(filePath)) return;

            string json = File.ReadAllText(filePath);
            var data = JsonSerializer.Deserialize<SaveData>(json);
            if (data != null)
            {
                _nextId = data.NextId;
                _tasks = data.Tasks ?? new List<TaskItem>();
            }
        }

        private class SaveData
        {
            public int NextId { get; set; }
            public List<TaskItem> Tasks { get; set; }
        }
    }
}
