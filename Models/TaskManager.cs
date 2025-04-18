using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Studio
{
    public class Task
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }
        public bool IsCompleted { get; set; }

        public Task(int id, string description, DateTime dueDate, Priority priority)
        {
            Id = id;
            Description = description;
            DueDate = dueDate;
            Priority = priority;
            IsCompleted = false;
        }
    }

    public enum Priority
    {
        Low,
        Medium,
        High,
        Urgent
    }

    public class TaskManager
    {
        private List<Task> _tasks;
        private int _nextId;

        public TaskManager()
        {
            _tasks = new List<Task>();
            _nextId = 1;
        }

        // Metodo per aggiungere una task
        public void AddTask(string description, DateTime dueDate, Priority priority)
        {
            Task newTask = new Task(_nextId, description, dueDate, priority);
            _tasks.Add(newTask);
            _nextId++;
        }

        // Metodo per modificare una task
        public bool EditTask(int taskId, string newDescription, DateTime newDueDate, Priority newPriority)
        {
            Task? task = _tasks.Find(t => t.Id == taskId);
            if (task != null)
            {
                task.Description = newDescription;
                task.DueDate = newDueDate;
                task.Priority = newPriority;
                return true;
            }
            return false;
        }

        // Metodo per rimuovere una task
        public bool RemoveTask(int taskId)
        {
            Task? task = _tasks.Find(t => t.Id == taskId);
            if (task != null)
            {
                _tasks.Remove(task);
                return true;
            }
            return false;
        }

        // Metodo per segnare una task come completata
        public bool CompleteTask(int taskId)
        {
            Task? task = _tasks.Find(t => t.Id == taskId);
            if (task != null)
            {
                task.IsCompleted = true;
                return true;
            }
            return false;
        }

        // Metodo per ottenere tutte le task
        public List<Task> GetAllTasks()
        {
            return _tasks;
        }

        // Metodo per ottenere le task non completate
        public List<Task> GetPendingTasks()
        {
            return _tasks.FindAll(t => !t.IsCompleted);
        }

        // Metodo per ottenere le task in scadenza entro un certo periodo
        public List<Task> GetDueTasks(int days)
        {
            DateTime cutoffDate = DateTime.Now.AddDays(days);
            return _tasks.FindAll(t => !t.IsCompleted && t.DueDate <= cutoffDate);
        }
    }
}