using System;
using System.Windows.Forms;
using System.Drawing;

namespace Studio
{
    public partial class MainForm : Form
    {
        private TaskManager _taskManager;
        private ExamManager _examManager;
        private BookTracker _bookTracker;

        public MainForm()
        {
            InitializeComponent();
            
            // Inizializzazione dei manager
            _taskManager = new TaskManager();
            _examManager = new ExamManager();
            _bookTracker = new BookTracker();
            
            SetupUI();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Name = "MainForm";
            this.Text = "Study Manager App";
            this.ResumeLayout(false);
        }

        private void SetupUI()
        {
            // Configurazione della Form principale
            this.Text = "Study Manager App";
            this.Width = 800;
            this.Height = 600;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Creazione della TabControl per i diversi moduli
            TabControl tabControl = new TabControl();
            tabControl.Dock = DockStyle.Fill;

            // Tab per la gestione delle Task
            TabPage taskTab = new TabPage("Task di Studio");
            SetupTaskTab(taskTab);
            tabControl.TabPages.Add(taskTab);

            // Tab per la gestione degli Esami
            TabPage examTab = new TabPage("Date Esami");
            SetupExamTab(examTab);
            tabControl.TabPages.Add(examTab);

            // Tab per il tracciamento dei Libri
            TabPage bookTab = new TabPage("Libri Studiati");
            SetupBookTab(bookTab);
            tabControl.TabPages.Add(bookTab);

            // Aggiunta della TabControl alla Form
            this.Controls.Add(tabControl);
        }

        private void SetupTaskTab(TabPage tab)
        {
            // DataGridView per visualizzare le task
            DataGridView taskGridView = new DataGridView();
            taskGridView.Dock = DockStyle.Top;
            taskGridView.Height = 400;
            taskGridView.AutoGenerateColumns = false;
            
            // Definizione delle colonne
            DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn();
            idColumn.DataPropertyName = "Id";
            idColumn.HeaderText = "ID";
            idColumn.Width = 50;
            taskGridView.Columns.Add(idColumn);
            
            DataGridViewTextBoxColumn descriptionColumn = new DataGridViewTextBoxColumn();
            descriptionColumn.DataPropertyName = "Description";
            descriptionColumn.HeaderText = "Descrizione";
            descriptionColumn.Width = 300;
            taskGridView.Columns.Add(descriptionColumn);
            
            DataGridViewTextBoxColumn dueDateColumn = new DataGridViewTextBoxColumn();
            dueDateColumn.DataPropertyName = "DueDate";
            dueDateColumn.HeaderText = "Scadenza";
            dueDateColumn.Width = 150;
            taskGridView.Columns.Add(dueDateColumn);
            
            DataGridViewTextBoxColumn priorityColumn = new DataGridViewTextBoxColumn();
            priorityColumn.DataPropertyName = "Priority";
            priorityColumn.HeaderText = "Priorità";
            priorityColumn.Width = 100;
            taskGridView.Columns.Add(priorityColumn);
            
            DataGridViewCheckBoxColumn completedColumn = new DataGridViewCheckBoxColumn();
            completedColumn.DataPropertyName = "IsCompleted";
            completedColumn.HeaderText = "Completata";
            completedColumn.Width = 80;
            taskGridView.Columns.Add(completedColumn);
            
            tab.Controls.Add(taskGridView);
            
            // Pannello per i bottoni
            Panel buttonPanel = new Panel();
            buttonPanel.Dock = DockStyle.Bottom;
            buttonPanel.Height = 100;
            
            // Bottoni per le operazioni
            Button addButton = new Button();
            addButton.Text = "Aggiungi Task";
            addButton.Location = new Point(20, 20);
            addButton.Click += (sender, e) => AddTask();
            
            Button editButton = new Button();
            editButton.Text = "Modifica Task";
            editButton.Location = new Point(150, 20);
            editButton.Click += (sender, e) => EditTask(taskGridView);
            
            Button deleteButton = new Button();
            deleteButton.Text = "Elimina Task";
            deleteButton.Location = new Point(280, 20);
            deleteButton.Click += (sender, e) => DeleteTask(taskGridView);
            
            Button completeButton = new Button();
            completeButton.Text = "Completa Task";
            completeButton.Location = new Point(410, 20);
            completeButton.Click += (sender, e) => CompleteTask(taskGridView);
            
            // Aggiunta dei bottoni al pannello
            buttonPanel.Controls.Add(addButton);
            buttonPanel.Controls.Add(editButton);
            buttonPanel.Controls.Add(deleteButton);
            buttonPanel.Controls.Add(completeButton);
            
            tab.Controls.Add(buttonPanel);
            
            // Caricamento dei dati
            RefreshTaskData(taskGridView);
        }
        
        private void SetupExamTab(TabPage tab)
        {
            // DataGridView per visualizzare gli esami
            DataGridView examGridView = new DataGridView();
            examGridView.Dock = DockStyle.Top;
            examGridView.Height = 400;
            examGridView.AutoGenerateColumns = false;
            
            // Definizione delle colonne
            DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn();
            idColumn.DataPropertyName = "Id";
            idColumn.HeaderText = "ID";
            idColumn.Width = 50;
            examGridView.Columns.Add(idColumn);
            
            DataGridViewTextBoxColumn subjectColumn = new DataGridViewTextBoxColumn();
            subjectColumn.DataPropertyName = "Subject";
            subjectColumn.HeaderText = "Materia";
            subjectColumn.Width = 200;
            examGridView.Columns.Add(subjectColumn);
            
            DataGridViewTextBoxColumn dateColumn = new DataGridViewTextBoxColumn();
            dateColumn.DataPropertyName = "ExamDate";
            dateColumn.HeaderText = "Data Esame";
            dateColumn.Width = 150;
            examGridView.Columns.Add(dateColumn);
            
            DataGridViewTextBoxColumn locationColumn = new DataGridViewTextBoxColumn();
            locationColumn.DataPropertyName = "Location";
            locationColumn.HeaderText = "Luogo";
            locationColumn.Width = 150;
            examGridView.Columns.Add(locationColumn);
            
            DataGridViewCheckBoxColumn passedColumn = new DataGridViewCheckBoxColumn();
            passedColumn.DataPropertyName = "IsPassed";
            passedColumn.HeaderText = "Superato";
            passedColumn.Width = 80;
            examGridView.Columns.Add(passedColumn);
            
            DataGridViewTextBoxColumn scoreColumn = new DataGridViewTextBoxColumn();
            scoreColumn.DataPropertyName = "Score";
            scoreColumn.HeaderText = "Voto";
            scoreColumn.Width = 50;
            examGridView.Columns.Add(scoreColumn);
            
            tab.Controls.Add(examGridView);
            
            // Pannello per i bottoni
            Panel buttonPanel = new Panel();
            buttonPanel.Dock = DockStyle.Bottom;
            buttonPanel.Height = 100;
            
            // Bottoni per le operazioni
            Button addButton = new Button();
            addButton.Text = "Aggiungi Esame";
            addButton.Location = new Point(20, 20);
            addButton.Click += (sender, e) => AddExam();
            
            Button editButton = new Button();
            editButton.Text = "Modifica Esame";
            editButton.Location = new Point(150, 20);
            editButton.Click += (sender, e) => EditExam(examGridView);
            
            Button deleteButton = new Button();
            deleteButton.Text = "Elimina Esame";
            deleteButton.Location = new Point(280, 20);
            deleteButton.Click += (sender, e) => DeleteExam(examGridView);
            
            Button recordResultButton = new Button();
            recordResultButton.Text = "Registra Risultato";
            recordResultButton.Location = new Point(410, 20);
            recordResultButton.Click += (sender, e) => RecordExamResult(examGridView);
            
            // Aggiunta dei bottoni al pannello
            buttonPanel.Controls.Add(addButton);
            buttonPanel.Controls.Add(editButton);
            buttonPanel.Controls.Add(deleteButton);
            buttonPanel.Controls.Add(recordResultButton);
            
            tab.Controls.Add(buttonPanel);
            
            // Caricamento dei dati
            RefreshExamData(examGridView);
        }
        
        private void SetupBookTab(TabPage tab)
        {
            // DataGridView per visualizzare i libri
            DataGridView bookGridView = new DataGridView();
            bookGridView.Dock = DockStyle.Top;
            bookGridView.Height = 400;
            bookGridView.AutoGenerateColumns = false;
            
            // Definizione delle colonne
            DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn();
            idColumn.DataPropertyName = "Id";
            idColumn.HeaderText = "ID";
            idColumn.Width = 50;
            bookGridView.Columns.Add(idColumn);
            
            DataGridViewTextBoxColumn titleColumn = new DataGridViewTextBoxColumn();
            titleColumn.DataPropertyName = "Title";
            titleColumn.HeaderText = "Titolo";
            titleColumn.Width = 250;
            bookGridView.Columns.Add(titleColumn);
            
            DataGridViewTextBoxColumn authorColumn = new DataGridViewTextBoxColumn();
            authorColumn.DataPropertyName = "Author";
            authorColumn.HeaderText = "Autore";
            authorColumn.Width = 150;
            bookGridView.Columns.Add(authorColumn);
            
            DataGridViewTextBoxColumn startDateColumn = new DataGridViewTextBoxColumn();
            startDateColumn.DataPropertyName = "StartDate";
            startDateColumn.HeaderText = "Data Inizio";
            startDateColumn.Width = 100;
            bookGridView.Columns.Add(startDateColumn);
            
            DataGridViewTextBoxColumn completionDateColumn = new DataGridViewTextBoxColumn();
            completionDateColumn.DataPropertyName = "CompletionDate";
            completionDateColumn.HeaderText = "Data Completamento";
            completionDateColumn.Width = 150;
            bookGridView.Columns.Add(completionDateColumn);
            
            DataGridViewTextBoxColumn ratingColumn = new DataGridViewTextBoxColumn();
            ratingColumn.DataPropertyName = "Rating";
            ratingColumn.HeaderText = "Valutazione";
            ratingColumn.Width = 80;
            bookGridView.Columns.Add(ratingColumn);
            
            tab.Controls.Add(bookGridView);
            
            // Pannello per i bottoni
            Panel buttonPanel = new Panel();
            buttonPanel.Dock = DockStyle.Bottom;
            buttonPanel.Height = 100;
            
            // Bottoni per le operazioni
            Button addButton = new Button();
            addButton.Text = "Aggiungi Libro";
            addButton.Location = new Point(20, 20);
            addButton.Click += (sender, e) => AddBook();
            
            Button editButton = new Button();
            editButton.Text = "Modifica Libro";
            editButton.Location = new Point(150, 20);
            editButton.Click += (sender, e) => EditBook(bookGridView);
            
            Button deleteButton = new Button();
            deleteButton.Text = "Elimina Libro";
            deleteButton.Location = new Point(280, 20);
            deleteButton.Click += (sender, e) => DeleteBook(bookGridView);
            
            Button completeButton = new Button();
            completeButton.Text = "Segna come Completato";
            completeButton.Location = new Point(410, 20);
            completeButton.Click += (sender, e) => CompleteBook(bookGridView);
            
            // Aggiunta dei bottoni al pannello
            buttonPanel.Controls.Add(addButton);
            buttonPanel.Controls.Add(editButton);
            buttonPanel.Controls.Add(deleteButton);
            buttonPanel.Controls.Add(completeButton);
            
            // Ricerca
            Panel searchPanel = new Panel();
            searchPanel.Dock = DockStyle.Bottom;
            searchPanel.Height = 50;
            searchPanel.Location = new Point(0, buttonPanel.Location.Y - 50);
            
            Label searchLabel = new Label();
            searchLabel.Text = "Cerca:";
            searchLabel.Location = new Point(20, 15);
            
            TextBox searchBox = new TextBox();
            searchBox.Location = new Point(70, 12);
            searchBox.Width = 300;
            
            Button searchButton = new Button();
            searchButton.Text = "Cerca";
            searchButton.Location = new Point(380, 10);
            searchButton.Click += (sender, e) => SearchBooks(searchBox.Text, bookGridView);
            
            searchPanel.Controls.Add(searchLabel);
            searchPanel.Controls.Add(searchBox);
            searchPanel.Controls.Add(searchButton);
            
            tab.Controls.Add(searchPanel);
            tab.Controls.Add(buttonPanel);
            
            // Caricamento dei dati
            RefreshBookData(bookGridView);
        }
        
        // Implementazione dei metodi per il modulo Task
        private void AddTask()
        {
            // Creazione del form per l'aggiunta di una task
            Form taskForm = new Form
            {
                Text = "Aggiungi Task",
                Size = new Size(400, 300),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            };

            // Descrizione
            Label descLabel = new Label { Text = "Descrizione:", Location = new Point(20, 20) };
            TextBox descTextBox = new TextBox { Location = new Point(120, 20), Width = 240 };
            
            // Data di scadenza
            Label dueDateLabel = new Label { Text = "Data Scadenza:", Location = new Point(20, 60) };
            DateTimePicker dueDatePicker = new DateTimePicker { Location = new Point(120, 60), Width = 240 };
            
            // Priorità
            Label priorityLabel = new Label { Text = "Priorità:", Location = new Point(20, 100) };
            ComboBox priorityComboBox = new ComboBox
            {
                Location = new Point(120, 100),
                Width = 240,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            
            // Popola il ComboBox con i valori dell'enum Priority
            priorityComboBox.Items.AddRange(Enum.GetNames(typeof(Priority)));
            priorityComboBox.SelectedIndex = 0;
            
            // Pulsanti
            Button saveButton = new Button
            {
                Text = "Salva",
                Location = new Point(120, 200),
                DialogResult = DialogResult.OK
            };
            
            Button cancelButton = new Button
            {
                Text = "Annulla",
                Location = new Point(230, 200),
                DialogResult = DialogResult.Cancel
            };
            
            // Aggiunta dei controlli al form
            taskForm.Controls.AddRange(new Control[] { 
                descLabel, descTextBox, 
                dueDateLabel, dueDatePicker, 
                priorityLabel, priorityComboBox,
                saveButton, cancelButton 
            });
            
            taskForm.AcceptButton = saveButton;
            taskForm.CancelButton = cancelButton;
            
            // Gestione del risultato del form
            if (taskForm.ShowDialog() == DialogResult.OK)
            {
                string description = descTextBox.Text.Trim();
                DateTime dueDate = dueDatePicker.Value;
                Priority priority = (Priority)Enum.Parse(typeof(Priority), priorityComboBox.SelectedItem.ToString());
                
                if (!string.IsNullOrEmpty(description))
                {
                    _taskManager.AddTask(description, dueDate, priority);
                }
                else
                {
                    MessageBox.Show("La descrizione non può essere vuota.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        private void EditTask(DataGridView grid)
        {
            if (grid.SelectedRows.Count > 0)
            {
                // Ottiene la task selezionata
                int taskId = (int)grid.SelectedRows[0].Cells["Id"].Value;
                string currentDescription = grid.SelectedRows[0].Cells["Description"].Value.ToString();
                DateTime currentDueDate = (DateTime)grid.SelectedRows[0].Cells["DueDate"].Value;
                Priority currentPriority = (Priority)grid.SelectedRows[0].Cells["Priority"].Value;
                
                // Creazione del form per la modifica
                Form taskForm = new Form
                {
                    Text = "Modifica Task",
                    Size = new Size(400, 300),
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    StartPosition = FormStartPosition.CenterParent,
                    MaximizeBox = false,
                    MinimizeBox = false
                };
                
                // Descrizione
                Label descLabel = new Label { Text = "Descrizione:", Location = new Point(20, 20) };
                TextBox descTextBox = new TextBox { 
                    Location = new Point(120, 20), 
                    Width = 240,
                    Text = currentDescription
                };
                
                // Data di scadenza
                Label dueDateLabel = new Label { Text = "Data Scadenza:", Location = new Point(20, 60) };
                DateTimePicker dueDatePicker = new DateTimePicker { 
                    Location = new Point(120, 60), 
                    Width = 240,
                    Value = currentDueDate
                };
                
                // Priorità
                Label priorityLabel = new Label { Text = "Priorità:", Location = new Point(20, 100) };
                ComboBox priorityComboBox = new ComboBox
                {
                    Location = new Point(120, 100),
                    Width = 240,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                
                // Popola il ComboBox con i valori dell'enum Priority
                priorityComboBox.Items.AddRange(Enum.GetNames(typeof(Priority)));
                priorityComboBox.SelectedItem = currentPriority.ToString();
                
                // Pulsanti
                Button saveButton = new Button
                {
                    Text = "Salva",
                    Location = new Point(120, 200),
                    DialogResult = DialogResult.OK
                };
                
                Button cancelButton = new Button
                {
                    Text = "Annulla",
                    Location = new Point(230, 200),
                    DialogResult = DialogResult.Cancel
                };
                
                // Aggiunta dei controlli al form
                taskForm.Controls.AddRange(new Control[] { 
                    descLabel, descTextBox, 
                    dueDateLabel, dueDatePicker, 
                    priorityLabel, priorityComboBox,
                    saveButton, cancelButton 
                });
                
                taskForm.AcceptButton = saveButton;
                taskForm.CancelButton = cancelButton;
                
                // Gestione del risultato del form
                if (taskForm.ShowDialog() == DialogResult.OK)
                {
                    string description = descTextBox.Text.Trim();
                    DateTime dueDate = dueDatePicker.Value;
                    Priority priority = (Priority)Enum.Parse(typeof(Priority), priorityComboBox.SelectedItem.ToString());
                    
                    if (!string.IsNullOrEmpty(description))
                    {
                        _taskManager.EditTask(taskId, description, dueDate, priority);
                        RefreshTaskData(grid);
                    }
                    else
                    {
                        MessageBox.Show("La descrizione non può essere vuota.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleziona una task da modificare");
            }
        }
        
        private void DeleteTask(DataGridView grid)
        {
            if (grid.SelectedRows.Count > 0)
            {
                int taskId = (int)grid.SelectedRows[0].Cells["Id"].Value;
                var result = MessageBox.Show("Sei sicuro di voler eliminare questa task?", "Conferma eliminazione", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    _taskManager.RemoveTask(taskId);
                    RefreshTaskData(grid);
                }
            }
            else
            {
                MessageBox.Show("Seleziona una task da eliminare");
            }
        }
        
        private void CompleteTask(DataGridView grid)
        {
            if (grid.SelectedRows.Count > 0)
            {
                int taskId = (int)grid.SelectedRows[0].Cells["Id"].Value;
                _taskManager.CompleteTask(taskId);
                RefreshTaskData(grid);
            }
            else
            {
                MessageBox.Show("Seleziona una task da completare");
            }
        }
        
        private void RefreshTaskData(DataGridView grid)
        {
            grid.DataSource = null;
            grid.DataSource = _taskManager.GetAllTasks();
        }
        
        // Implementazione dei metodi per il modulo Esami
        private void AddExam()
        {
            // TODO: Implementare form per l'aggiunta di un esame
            MessageBox.Show("Funzionalità di aggiunta esame da implementare");
        }
        
        private void EditExam(DataGridView grid)
        {
            if (grid.SelectedRows.Count > 0)
            {
                // TODO: Implementare form per la modifica di un esame
                MessageBox.Show("Funzionalità di modifica esame da implementare");
            }
            else
            {
                MessageBox.Show("Seleziona un esame da modificare");
            }
        }
        
        private void DeleteExam(DataGridView grid)
        {
            if (grid.SelectedRows.Count > 0)
            {
                int examId = (int)grid.SelectedRows[0].Cells["Id"].Value;
                var result = MessageBox.Show("Sei sicuro di voler eliminare questo esame?", "Conferma eliminazione", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    _examManager.RemoveExam(examId);
                    RefreshExamData(grid);
                }
            }
            else
            {
                MessageBox.Show("Seleziona un esame da eliminare");
            }
        }
        
        private void RecordExamResult(DataGridView grid)
        {
            if (grid.SelectedRows.Count > 0)
            {
                // TODO: Implementare form per la registrazione del risultato di un esame
                MessageBox.Show("Funzionalità di registrazione risultato da implementare");
            }
            else
            {
                MessageBox.Show("Seleziona un esame per registrare il risultato");
            }
        }
        
        private void RefreshExamData(DataGridView grid)
        {
            grid.DataSource = null;
            grid.DataSource = _examManager.GetAllExams();
        }
        
        // Implementazione dei metodi per il modulo Libri
        private void AddBook()
        {
            // TODO: Implementare form per l'aggiunta di un libro
            MessageBox.Show("Funzionalità di aggiunta libro da implementare");
        }
        
        private void EditBook(DataGridView grid)
        {
            if (grid.SelectedRows.Count > 0)
            {
                // TODO: Implementare form per la modifica di un libro
                MessageBox.Show("Funzionalità di modifica libro da implementare");
            }
            else
            {
                MessageBox.Show("Seleziona un libro da modificare");
            }
        }
        
        private void DeleteBook(DataGridView grid)
        {
            if (grid.SelectedRows.Count > 0)
            {
                int bookId = (int)grid.SelectedRows[0].Cells["Id"].Value;
                var result = MessageBox.Show("Sei sicuro di voler eliminare questo libro?", "Conferma eliminazione", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    _bookTracker.RemoveBook(bookId);
                    RefreshBookData(grid);
                }
            }
            else
            {
                MessageBox.Show("Seleziona un libro da eliminare");
            }
        }
        
        private void CompleteBook(DataGridView grid)
        {
            if (grid.SelectedRows.Count > 0)
            {
                // TODO: Implementare form per segnare un libro come completato
                MessageBox.Show("Funzionalità di completamento libro da implementare");
            }
            else
            {
                MessageBox.Show("Seleziona un libro da completare");
            }
        }
        
        private void SearchBooks(string searchTerm, DataGridView grid)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                RefreshBookData(grid);
            }
            else
            {
                grid.DataSource = null;
                grid.DataSource = _bookTracker.SearchBooks(searchTerm);
            }
        }
        
        private void RefreshBookData(DataGridView grid)
        {
            grid.DataSource = null;
            grid.DataSource = _bookTracker.GetAllBooks();
        }
    }
}