using System;

namespace TaskManagerLib.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public TaskPriority Priority { get; set; }

        public DateTime DueDate { get; set; }

        public MyTaskStatus Status { get; set; }

        public bool IsImportant { get; set; }

        public TaskItem() { }

        public TaskItem(int id, string title, string description,
                        TaskPriority priority, DateTime dueDate,
                        MyTaskStatus status, bool isImportant = false)
        {
            Id = id;
            Title = title;
            Description = description;
            Priority = priority;
            DueDate = dueDate;
            Status = status;
            IsImportant = isImportant;
        }

        public bool IsOverdue()
        {
            return Status != MyTaskStatus.Завершена && DueDate < DateTime.Today;
        }

        public override string ToString()
        {
            string star = IsImportant ? " ★" : "";

            string statusText = Status.ToString();
            if (statusText == "ВПроцессе")
                statusText = "В процессе";

            return $"[{Id}] {Title} ({Priority}, {statusText}){star} - до {DueDate:yyyy-MM-dd}";
        }
    }
}
