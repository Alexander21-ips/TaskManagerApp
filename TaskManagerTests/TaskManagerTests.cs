using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerLib;
using TaskManagerLib.Models;
using Xunit;

namespace TaskManagerTests
{
    public class TaskManagerTests
    {
        [Fact]
        public void AddTask_ShouldIncreaseCount()
        {
            var manager = new TaskManager();
            var task = new TaskItem(0, "Тест", "Описание", TaskPriority.Средний, DateTime.Today, MyTaskStatus.Новая);

            manager.AddTask(task);
            var all = manager.GetAllTasks();

            Assert.Single(all);
            Assert.Equal(1, all[0].Id);
        }

        [Fact]
        public void FilterByStatus_ShouldReturnCorrect()
        {
            var manager = new TaskManager();
            manager.AddTask(new TaskItem(0, "A", "", TaskPriority.Низкий, DateTime.Today, MyTaskStatus.Новая));
            manager.AddTask(new TaskItem(0, "B", "", TaskPriority.Низкий, DateTime.Today, MyTaskStatus.Завершена));

            var filtered = manager.GetTasksByStatus(MyTaskStatus.Новая);

            Assert.Single(filtered);
            Assert.Equal("A", filtered[0].Title);
        }

        [Fact]
        public void SearchTasks_ByTitle_ShouldFind()
        {
            var manager = new TaskManager();
            manager.AddTask(new TaskItem(0, "Купить молоко", "", TaskPriority.Средний, DateTime.Today, MyTaskStatus.Новая));
            manager.AddTask(new TaskItem(0, "Позвонить", "", TaskPriority.Средний, DateTime.Today, MyTaskStatus.Новая));

            var found = manager.SearchTasks("молоко");

            Assert.Single(found);
            Assert.Contains("молоко", found[0].Title);
        }

        [Fact]
        public void DeleteTask_ShouldRemove()
        {
            var manager = new TaskManager();
            manager.AddTask(new TaskItem(0, "Задача", "", TaskPriority.Низкий, DateTime.Today, MyTaskStatus.Новая));
            int id = manager.GetAllTasks()[0].Id;

            bool deleted = manager.DeleteTask(id);

            Assert.True(deleted);
            Assert.Empty(manager.GetAllTasks());
        }

        [Fact]
        public void Statistics_CompletedAndOverdue()
        {
            var manager = new TaskManager();
            manager.AddTask(new TaskItem(0, "1", "", TaskPriority.Низкий, DateTime.Today.AddDays(-1), MyTaskStatus.Новая));
            manager.AddTask(new TaskItem(0, "2", "", TaskPriority.Низкий, DateTime.Today, MyTaskStatus.Завершена));
            manager.AddTask(new TaskItem(0, "3", "", TaskPriority.Низкий, DateTime.Today.AddDays(1), MyTaskStatus.Новая));

            var stats = manager.GetStatistics();

            Assert.Equal(1, stats.completed);
            Assert.Equal(1, stats.overdue);
        }
    }
}
