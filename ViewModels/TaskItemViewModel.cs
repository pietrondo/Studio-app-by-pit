using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Studio;

namespace Studio.ViewModels
{
    public class TaskItemViewModel : INotifyPropertyChanged
    {
        private readonly Task _task;
        private readonly TaskManager _taskManager;
        private bool _isCompleted;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? TaskUpdated;

        public string Description => _task.Description;
        public DateTime DueDate => _task.DueDate;
        public Priority Priority => _task.Priority;
        public int Id => _task.Id;

        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                if (_isCompleted != value)
                {
                    _isCompleted = value;
                    OnPropertyChanged();
                    UpdateTaskCompletion();
                }
            }
        }

        public TaskItemViewModel(Task task, TaskManager taskManager)
        {
            _task = task ?? throw new ArgumentNullException(nameof(task));
            _taskManager = taskManager ?? throw new ArgumentNullException(nameof(taskManager));
            _isCompleted = task.IsCompleted;
        }

        private void UpdateTaskCompletion()
        {
            _task.IsCompleted = this.IsCompleted;
            _taskManager.UpdateTask(_task);
            TaskUpdated?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
