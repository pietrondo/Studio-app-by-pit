using System;
using System.Collections.Generic;
using System.Linq;
using System.IO; // Aggiunto per I/O file
using System.Text.Json; // Aggiunto per JSON

namespace Studio
{
    public class Exam
    {
        public int Id { get; set; }
        public string Subject { get; set; } = string.Empty;
        public DateTime ExamDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public bool IsPassed { get; set; }
        public int? Score { get; set; }

        public Exam(int id, string subject, DateTime examDate, string location)
        {
            Id = id;
            Subject = subject;
            ExamDate = examDate;
            Location = location;
            IsPassed = false;
            Score = null;
        }
    }

    public class ExamManager
    {
        private List<Exam> _exams;
        private int _nextId;
        private const string FilePath = "exams.json"; // Percorso file dati

        public ExamManager()
        {
            _exams = new List<Exam>();
            _nextId = 1;
            LoadData(); // Carica i dati all'inizializzazione
        }

        // Metodo per aggiungere un esame
        public void AddExam(string subject, DateTime examDate, string location)
        {
            Exam newExam = new Exam(_nextId, subject, examDate, location);
            _exams.Add(newExam);
            _nextId++;
        }

        // Metodo per modificare un esame
        public bool EditExam(int examId, string newSubject, DateTime newExamDate, string newLocation)
        {
            Exam? exam = _exams.Find(e => e.Id == examId);
            if (exam != null)
            {
                exam.Subject = newSubject;
                exam.ExamDate = newExamDate;
                exam.Location = newLocation;
                return true;
            }
            return false;
        }

        // Metodo per rimuovere un esame
        public bool RemoveExam(int examId)
        {
            Exam? exam = _exams.Find(e => e.Id == examId);
            if (exam != null)
            {
                _exams.Remove(exam);
                return true;
            }
            return false;
        }

        // Metodo per registrare il risultato di un esame
        public bool RecordExamResult(int examId, bool isPassed, int? score)
        {
            Exam? exam = _exams.Find(e => e.Id == examId);
            if (exam != null)
            {
                exam.IsPassed = isPassed;
                exam.Score = score;
                return true;
            }
            return false;
        }

        // Metodo per ottenere tutti gli esami
        public List<Exam> GetAllExams()
        {
            return _exams;
        }

        // Metodo per ottenere gli esami futuri
        public List<Exam> GetUpcomingExams()
        {
            return _exams.FindAll(e => e.ExamDate > DateTime.Now);
        }

        // Metodo per ottenere gli esami passati
        public List<Exam> GetPastExams()
        {
            return _exams.FindAll(e => e.ExamDate <= DateTime.Now);
        }

        // Metodo per ottenere gli esami superati
        public List<Exam> GetPassedExams()
        {
            return _exams.FindAll(e => e.IsPassed);
        }

        // Metodo per salvare i dati su file JSON
        public void SaveData()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(_exams, options);
                File.WriteAllText(FilePath, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante il salvataggio degli esami: {ex.Message}");
            }
        }

        // Metodo per caricare i dati da file JSON
        private void LoadData()
        {
            if (!File.Exists(FilePath))
            {
                _exams = new List<Exam>();
                _nextId = 1;
                return;
            }

            try
            {
                string jsonString = File.ReadAllText(FilePath);
                var loadedExams = JsonSerializer.Deserialize<List<Exam>>(jsonString);

                if (loadedExams != null)
                {
                    _exams = loadedExams;
                    _nextId = _exams.Any() ? _exams.Max(e => e.Id) + 1 : 1;
                }
                 else
                {
                     _exams = new List<Exam>();
                     _nextId = 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante il caricamento degli esami: {ex.Message}");
                _exams = new List<Exam>();
                _nextId = 1;
            }
        }
    }
}