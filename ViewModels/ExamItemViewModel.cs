using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Studio;

namespace Studio.ViewModels
{
    public class ExamItemViewModel : INotifyPropertyChanged
    {
        private readonly Exam _exam;
        private readonly ExamManager _examManager;
        private bool _isPassed;
        private int? _score;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? ExamUpdated;

        public string Subject => _exam.Subject;
        public DateTime ExamDate => _exam.ExamDate;
        public string Location => _exam.Location;
        public int Id => _exam.Id;

        public bool IsPassed
        {
            get => _isPassed;
            set
            {
                if (_isPassed != value)
                {
                    _isPassed = value;
                    OnPropertyChanged();
                    UpdateExamResult();
                }
            }
        }

        public int? Score
        {
            get => _score;
            set
            {
                if (_score != value)
                {
                    _score = value;
                    OnPropertyChanged();
                    UpdateExamResult();
                }
            }
        }

        public ExamItemViewModel(Exam exam, ExamManager examManager)
        {
            _exam = exam ?? throw new ArgumentNullException(nameof(exam));
            _examManager = examManager ?? throw new ArgumentNullException(nameof(examManager));
            _isPassed = exam.IsPassed;
            _score = exam.Score;
        }

        private void UpdateExamResult()
        {
            _exam.IsPassed = this.IsPassed;
            _exam.Score = this.Score;
            _examManager.UpdateExam(_exam);
            ExamUpdated?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
