using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO; // Aggiunto per I/O file
using System.Text.Json; // Aggiunto per JSON
using System.Linq; // Aggiunto per Max()

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
        private const string FilePath = "tasks.json"; // Percorso file dati

        public TaskManager()
        {
            _tasks = new List<Task>();
            _nextId = 1;
            LoadData(); // Carica i dati all'inizializzazione
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

        // Metodo per salvare i dati su file JSON
        public void SaveData()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(_tasks, options);
                File.WriteAllText(FilePath, jsonString);
            }
            catch (Exception ex)
            {
                // Gestione eccezioni (es. log, messaggio utente)
                Console.WriteLine($"Errore durante il salvataggio delle task: {ex.Message}");
                // Potresti voler mostrare un messaggio all'utente qui
            }
        }

        // Metodo per caricare i dati da file JSON
        private void LoadData()
        {
            if (!File.Exists(FilePath))
            {
                _tasks = new List<Task>(); // Inizia con lista vuota se il file non esiste
                _nextId = 1;
                return;
            }

            try
            {
                string jsonString = File.ReadAllText(FilePath);
                var loadedTasks = JsonSerializer.Deserialize<List<Task>>(jsonString);

                if (loadedTasks != null)
                {
                    _tasks = loadedTasks;
                    // Aggiorna _nextId basandosi sull'ID massimo caricato + 1
                    _nextId = _tasks.Any() ? _tasks.Max(t => t.Id) + 1 : 1;
                }
                else
                {
                     _tasks = new List<Task>(); // Inizializza se la deserializzazione fallisce
                     _nextId = 1;
                }
            }
            catch (Exception ex)
            {
                // Gestione eccezioni (es. file corrotto, problemi di permessi)
                Console.WriteLine($"Errore durante il caricamento delle task: {ex.Message}");
                _tasks = new List<Task>(); // Inizia con lista vuota in caso di errore
                _nextId = 1;
                 // Potresti voler mostrare un messaggio all'utente qui
            }
        }
    }
}