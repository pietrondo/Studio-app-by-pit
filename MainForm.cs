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
                string? selectedItem = priorityComboBox.SelectedItem?.ToString();
                
                if (!string.IsNullOrEmpty(description) && selectedItem != null)
                {
                    Priority priority = (Priority)Enum.Parse(typeof(Priority), selectedItem);
                    _taskManager.AddTask(description, dueDate, priority);
                    
                    // Aggiorna tutte le griglie delle task
                    foreach (Control control in Controls)
                    {
                        if (control is TabControl tabControl)
                        {
                            foreach (TabPage tabPage in tabControl.TabPages)
                            {
                                foreach (Control tabPageControl in tabPage.Controls)
                                {
                                    if (tabPageControl is DataGridView grid && tabPage.Text.Contains("Task"))
                                    {
                                        RefreshTaskData(grid);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("La descrizione non può essere vuota e devi selezionare una priorità.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        private void EditTask(DataGridView grid)
        {
            if (grid?.SelectedRows.Count > 0)
            {
                // Ottieni i valori in modo sicuro
                var selectedRow = grid.SelectedRows[0];
                if (selectedRow.Cells["Id"].Value == null || 
                    selectedRow.Cells["Description"].Value == null || 
                    selectedRow.Cells["DueDate"].Value == null || 
                    selectedRow.Cells["Priority"].Value == null)
                {
                    MessageBox.Show("Dati della task non validi.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                int taskId = Convert.ToInt32(selectedRow.Cells["Id"].Value);
                string currentDescription = Convert.ToString(selectedRow.Cells["Description"].Value) ?? string.Empty;
                DateTime currentDueDate = Convert.ToDateTime(selectedRow.Cells["DueDate"].Value);
                Priority currentPriority = (Priority)Convert.ToInt32(selectedRow.Cells["Priority"].Value);
                
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
                    string? selectedItem = priorityComboBox.SelectedItem?.ToString();
                    
                    if (!string.IsNullOrEmpty(description) && selectedItem != null)
                    {
                        Priority priority = (Priority)Enum.Parse(typeof(Priority), selectedItem);
                        _taskManager.EditTask(taskId, description, dueDate, priority);
                        RefreshTaskData(grid);
                    }
                    else
                    {
                        MessageBox.Show("La descrizione non può essere vuota e devi selezionare una priorità.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (grid?.SelectedRows.Count > 0)
            {
                var selectedRow = grid.SelectedRows[0];
                if (selectedRow.Cells["Id"].Value == null)
                {
                    MessageBox.Show("ID task non valido.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                int taskId = Convert.ToInt32(selectedRow.Cells["Id"].Value);
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
            if (grid?.SelectedRows.Count > 0)
            {
                var selectedRow = grid.SelectedRows[0];
                if (selectedRow.Cells["Id"].Value == null)
                {
                    MessageBox.Show("ID task non valido.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                int taskId = Convert.ToInt32(selectedRow.Cells["Id"].Value);
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
            if (grid != null)
            {
                grid.DataSource = null;
                grid.DataSource = _taskManager.GetAllTasks();
            }
        }
        
        // Implementazione dei metodi per il modulo Esami
        private void AddExam()
        {
            // Creazione del form per l'aggiunta di un esame
            Form examForm = new Form
            {
                Text = "Aggiungi Esame",
                Size = new Size(400, 300),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            };

            // Materia
            Label subjectLabel = new Label { Text = "Materia:", Location = new Point(20, 20) };
            TextBox subjectTextBox = new TextBox { Location = new Point(120, 20), Width = 240 };
            
            // Data dell'esame
            Label examDateLabel = new Label { Text = "Data Esame:", Location = new Point(20, 60) };
            DateTimePicker examDatePicker = new DateTimePicker { Location = new Point(120, 60), Width = 240 };
            
            // Luogo
            Label locationLabel = new Label { Text = "Luogo:", Location = new Point(20, 100) };
            TextBox locationTextBox = new TextBox { Location = new Point(120, 100), Width = 240 };
            
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
            examForm.Controls.AddRange(new Control[] { 
                subjectLabel, subjectTextBox, 
                examDateLabel, examDatePicker, 
                locationLabel, locationTextBox,
                saveButton, cancelButton 
            });
            
            examForm.AcceptButton = saveButton;
            examForm.CancelButton = cancelButton;
            
            // Gestione del risultato del form
            if (examForm.ShowDialog() == DialogResult.OK)
            {
                string subject = subjectTextBox.Text.Trim();
                DateTime examDate = examDatePicker.Value;
                string location = locationTextBox.Text.Trim();
                
                if (!string.IsNullOrEmpty(subject))
                {
                    _examManager.AddExam(subject, examDate, location);
                    
                    // Aggiorna tutte le griglie degli esami
                    foreach (Control control in Controls)
                    {
                        if (control is TabControl tabControl)
                        {
                            foreach (TabPage tabPage in tabControl.TabPages)
                            {
                                foreach (Control tabPageControl in tabPage.Controls)
                                {
                                    if (tabPageControl is DataGridView grid && tabPage.Text.Contains("Esami"))
                                    {
                                        RefreshExamData(grid);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Il nome della materia non può essere vuoto.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        private void EditExam(DataGridView grid)
        {
            if (grid?.SelectedRows.Count > 0)
            {
                // Ottieni i valori in modo sicuro
                var selectedRow = grid.SelectedRows[0];
                if (selectedRow.Cells["Id"].Value == null || 
                    selectedRow.Cells["Subject"].Value == null || 
                    selectedRow.Cells["ExamDate"].Value == null || 
                    selectedRow.Cells["Location"].Value == null)
                {
                    MessageBox.Show("Dati dell'esame non validi.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                int examId = Convert.ToInt32(selectedRow.Cells["Id"].Value);
                string currentSubject = Convert.ToString(selectedRow.Cells["Subject"].Value) ?? string.Empty;
                DateTime currentExamDate = Convert.ToDateTime(selectedRow.Cells["ExamDate"].Value);
                string currentLocation = Convert.ToString(selectedRow.Cells["Location"].Value) ?? string.Empty;
                
                // Creazione del form per la modifica
                Form examForm = new Form
                {
                    Text = "Modifica Esame",
                    Size = new Size(400, 300),
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    StartPosition = FormStartPosition.CenterParent,
                    MaximizeBox = false,
                    MinimizeBox = false
                };
                
                // Materia
                Label subjectLabel = new Label { Text = "Materia:", Location = new Point(20, 20) };
                TextBox subjectTextBox = new TextBox { 
                    Location = new Point(120, 20), 
                    Width = 240,
                    Text = currentSubject
                };
                
                // Data dell'esame
                Label examDateLabel = new Label { Text = "Data Esame:", Location = new Point(20, 60) };
                DateTimePicker examDatePicker = new DateTimePicker { 
                    Location = new Point(120, 60), 
                    Width = 240,
                    Value = currentExamDate
                };
                
                // Luogo
                Label locationLabel = new Label { Text = "Luogo:", Location = new Point(20, 100) };
                TextBox locationTextBox = new TextBox { 
                    Location = new Point(120, 100), 
                    Width = 240,
                    Text = currentLocation
                };
                
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
                examForm.Controls.AddRange(new Control[] { 
                    subjectLabel, subjectTextBox, 
                    examDateLabel, examDatePicker, 
                    locationLabel, locationTextBox,
                    saveButton, cancelButton 
                });
                
                examForm.AcceptButton = saveButton;
                examForm.CancelButton = cancelButton;
                
                // Gestione del risultato del form
                if (examForm.ShowDialog() == DialogResult.OK)
                {
                    string subject = subjectTextBox.Text.Trim();
                    DateTime examDate = examDatePicker.Value;
                    string location = locationTextBox.Text.Trim();
                    
                    if (!string.IsNullOrEmpty(subject))
                    {
                        _examManager.EditExam(examId, subject, examDate, location);
                        RefreshExamData(grid);
                    }
                    else
                    {
                        MessageBox.Show("Il nome della materia non può essere vuoto.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleziona un esame da modificare");
            }
        }
        
        private void DeleteExam(DataGridView grid)
        {
            if (grid?.SelectedRows.Count > 0)
            {
                var selectedRow = grid.SelectedRows[0];
                if (selectedRow.Cells["Id"].Value == null)
                {
                    MessageBox.Show("ID esame non valido.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                int examId = Convert.ToInt32(selectedRow.Cells["Id"].Value);
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
            if (grid?.SelectedRows.Count > 0)
            {
                var selectedRow = grid.SelectedRows[0];
                if (selectedRow.Cells["Id"].Value == null || selectedRow.Cells["Subject"].Value == null)
                {
                    MessageBox.Show("Dati dell'esame non validi.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                int examId = Convert.ToInt32(selectedRow.Cells["Id"].Value);
                string subject = Convert.ToString(selectedRow.Cells["Subject"].Value) ?? string.Empty;
                
                // Creazione del form per la registrazione del risultato
                Form resultForm = new Form
                {
                    Text = $"Registra Risultato: {subject}",
                    Size = new Size(350, 250),
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    StartPosition = FormStartPosition.CenterParent,
                    MaximizeBox = false,
                    MinimizeBox = false
                };
                
                // Superato
                Label passedLabel = new Label { Text = "Superato:", Location = new Point(20, 20) };
                CheckBox passedCheckBox = new CheckBox { 
                    Location = new Point(120, 20), 
                    Width = 20,
                    Checked = false
                };
                
                // Voto
                Label scoreLabel = new Label { Text = "Voto (0-30):", Location = new Point(20, 60) };
                NumericUpDown scoreNumeric = new NumericUpDown { 
                    Location = new Point(120, 60), 
                    Width = 60,
                    Minimum = 0,
                    Maximum = 30,
                    Value = 18,
                    Enabled = false  // Disabilitato inizialmente
                };
                
                // Abilita il campo del voto solo se l'esame è stato superato
                passedCheckBox.CheckedChanged += (sender, e) => {
                    scoreNumeric.Enabled = passedCheckBox.Checked;
                };
                
                // Pulsanti
                Button saveButton = new Button
                {
                    Text = "Salva",
                    Location = new Point(80, 150),
                    DialogResult = DialogResult.OK
                };
                
                Button cancelButton = new Button
                {
                    Text = "Annulla",
                    Location = new Point(180, 150),
                    DialogResult = DialogResult.Cancel
                };
                
                // Aggiunta dei controlli al form
                resultForm.Controls.AddRange(new Control[] { 
                    passedLabel, passedCheckBox, 
                    scoreLabel, scoreNumeric,
                    saveButton, cancelButton 
                });
                
                resultForm.AcceptButton = saveButton;
                resultForm.CancelButton = cancelButton;
                
                // Gestione del risultato del form
                if (resultForm.ShowDialog() == DialogResult.OK)
                {
                    bool isPassed = passedCheckBox.Checked;
                    int? score = isPassed ? (int?)scoreNumeric.Value : null;
                    
                    _examManager.RecordExamResult(examId, isPassed, score);
                    RefreshExamData(grid);
                }
            }
            else
            {
                MessageBox.Show("Seleziona un esame per registrare il risultato", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void RefreshExamData(DataGridView grid)
        {
            if (grid != null)
            {
                grid.DataSource = null;
                grid.DataSource = _examManager.GetAllExams();
            }
        }
        
        // Implementazione dei metodi per il modulo Libri
        private void AddBook()
        {
            // Creazione del form per l'aggiunta di un libro
            Form bookForm = new Form
            {
                Text = "Aggiungi Libro",
                Size = new Size(400, 300),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            };

            // Titolo
            Label titleLabel = new Label { Text = "Titolo:", Location = new Point(20, 20) };
            TextBox titleTextBox = new TextBox { Location = new Point(120, 20), Width = 240 };
            
            // Autore
            Label authorLabel = new Label { Text = "Autore:", Location = new Point(20, 60) };
            TextBox authorTextBox = new TextBox { Location = new Point(120, 60), Width = 240 };
            
            // Data inizio lettura
            Label startDateLabel = new Label { Text = "Data Inizio:", Location = new Point(20, 100) };
            DateTimePicker startDatePicker = new DateTimePicker { Location = new Point(120, 100), Width = 240 };
            
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
            bookForm.Controls.AddRange(new Control[] { 
                titleLabel, titleTextBox, 
                authorLabel, authorTextBox, 
                startDateLabel, startDatePicker,
                saveButton, cancelButton 
            });
            
            bookForm.AcceptButton = saveButton;
            bookForm.CancelButton = cancelButton;
            
            // Gestione del risultato del form
            if (bookForm.ShowDialog() == DialogResult.OK)
            {
                string title = titleTextBox.Text.Trim();
                string author = authorTextBox.Text.Trim();
                DateTime startDate = startDatePicker.Value;
                
                if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(author))
                {
                    _bookTracker.AddBook(title, author, startDate);
                    
                    // Aggiorna tutte le griglie dei libri
                    foreach (Control control in Controls)
                    {
                        if (control is TabControl tabControl)
                        {
                            foreach (TabPage tabPage in tabControl.TabPages)
                            {
                                foreach (Control tabPageControl in tabPage.Controls)
                                {
                                    if (tabPageControl is DataGridView grid && tabPage.Text.Contains("Libri"))
                                    {
                                        RefreshBookData(grid);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Titolo e autore non possono essere vuoti.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        private void EditBook(DataGridView grid)
        {
            if (grid?.SelectedRows.Count > 0)
            {
                // Ottieni i valori in modo sicuro
                var selectedRow = grid.SelectedRows[0];
                if (selectedRow.Cells["Id"].Value == null || 
                    selectedRow.Cells["Title"].Value == null || 
                    selectedRow.Cells["Author"].Value == null || 
                    selectedRow.Cells["StartDate"].Value == null)
                {
                    MessageBox.Show("Dati del libro non validi.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                int bookId = Convert.ToInt32(selectedRow.Cells["Id"].Value);
                // Uso di Convert.ToString invece di .ToString() diretto per gestire i null in modo sicuro
                string currentTitle = Convert.ToString(selectedRow.Cells["Title"].Value) ?? string.Empty;
                string currentAuthor = Convert.ToString(selectedRow.Cells["Author"].Value) ?? string.Empty;
                DateTime currentStartDate = Convert.ToDateTime(selectedRow.Cells["StartDate"].Value);
                
                // Creazione del form per la modifica
                Form bookForm = new Form
                {
                    Text = "Modifica Libro",
                    Size = new Size(400, 300),
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    StartPosition = FormStartPosition.CenterParent,
                    MaximizeBox = false,
                    MinimizeBox = false
                };
                
                // Titolo
                Label titleLabel = new Label { Text = "Titolo:", Location = new Point(20, 20) };
                TextBox titleTextBox = new TextBox { 
                    Location = new Point(120, 20), 
                    Width = 240,
                    Text = currentTitle
                };
                
                // Autore
                Label authorLabel = new Label { Text = "Autore:", Location = new Point(20, 60) };
                TextBox authorTextBox = new TextBox { 
                    Location = new Point(120, 60), 
                    Width = 240,
                    Text = currentAuthor
                };
                
                // Data inizio lettura
                Label startDateLabel = new Label { Text = "Data Inizio:", Location = new Point(20, 100) };
                DateTimePicker startDatePicker = new DateTimePicker { 
                    Location = new Point(120, 100), 
                    Width = 240,
                    Value = currentStartDate
                };
                
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
                bookForm.Controls.AddRange(new Control[] { 
                    titleLabel, titleTextBox, 
                    authorLabel, authorTextBox, 
                    startDateLabel, startDatePicker,
                    saveButton, cancelButton 
                });
                
                bookForm.AcceptButton = saveButton;
                bookForm.CancelButton = cancelButton;
                
                // Gestione del risultato del form
                if (bookForm.ShowDialog() == DialogResult.OK)
                {
                    string title = titleTextBox.Text.Trim();
                    string author = authorTextBox.Text.Trim();
                    DateTime startDate = startDatePicker.Value;
                    
                    if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(author))
                    {
                        _bookTracker.EditBook(bookId, title, author, startDate);
                        RefreshBookData(grid);
                    }
                    else
                    {
                        MessageBox.Show("Titolo e autore non possono essere vuoti.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleziona un libro da modificare");
            }
        }
        
        private void DeleteBook(DataGridView grid)
        {
            if (grid?.SelectedRows.Count > 0)
            {
                var selectedRow = grid.SelectedRows[0];
                if (selectedRow.Cells["Id"].Value == null)
                {
                    MessageBox.Show("ID libro non valido.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                int bookId = Convert.ToInt32(selectedRow.Cells["Id"].Value);
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
            if (grid?.SelectedRows.Count > 0)
            {
                var selectedRow = grid.SelectedRows[0];
                if (selectedRow.Cells["Id"].Value == null || selectedRow.Cells["Title"].Value == null)
                {
                    MessageBox.Show("Dati del libro non validi.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                int bookId = Convert.ToInt32(selectedRow.Cells["Id"].Value);
                string title = Convert.ToString(selectedRow.Cells["Title"].Value) ?? string.Empty;
                
                // Creazione del form per segnare un libro come completato
                Form completeForm = new Form
                {
                    Text = $"Completa Libro: {title}",
                    Size = new Size(400, 350),
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    StartPosition = FormStartPosition.CenterParent,
                    MaximizeBox = false,
                    MinimizeBox = false
                };
                
                // Data completamento
                Label completionDateLabel = new Label { Text = "Data Completamento:", Location = new Point(20, 20) };
                DateTimePicker completionDatePicker = new DateTimePicker { 
                    Location = new Point(150, 20), 
                    Width = 210
                };
                
                // Valutazione
                Label ratingLabel = new Label { Text = "Valutazione (1-5):", Location = new Point(20, 60) };
                NumericUpDown ratingNumeric = new NumericUpDown { 
                    Location = new Point(150, 60), 
                    Width = 60,
                    Minimum = 1,
                    Maximum = 5,
                    Value = 3
                };
                
                // Note
                Label notesLabel = new Label { Text = "Note:", Location = new Point(20, 100) };
                TextBox notesTextBox = new TextBox { 
                    Location = new Point(150, 100), 
                    Width = 210,
                    Height = 100,
                    Multiline = true
                };
                
                // Pulsanti
                Button saveButton = new Button
                {
                    Text = "Salva",
                    Location = new Point(120, 250),
                    DialogResult = DialogResult.OK
                };
                
                Button cancelButton = new Button
                {
                    Text = "Annulla",
                    Location = new Point(230, 250),
                    DialogResult = DialogResult.Cancel
                };
                
                // Aggiunta dei controlli al form
                completeForm.Controls.AddRange(new Control[] { 
                    completionDateLabel, completionDatePicker, 
                    ratingLabel, ratingNumeric,
                    notesLabel, notesTextBox,
                    saveButton, cancelButton 
                });
                
                completeForm.AcceptButton = saveButton;
                completeForm.CancelButton = cancelButton;
                
                // Gestione del risultato del form
                if (completeForm.ShowDialog() == DialogResult.OK)
                {
                    DateTime completionDate = completionDatePicker.Value;
                    int? rating = (int?)ratingNumeric.Value;  // Cast to nullable int
                    string notes = notesTextBox.Text.Trim();
                    
                    _bookTracker.CompleteBook(bookId, completionDate, rating, notes);
                    RefreshBookData(grid);
                }
            }
            else
            {
                MessageBox.Show("Seleziona un libro da completare", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void SearchBooks(string searchTerm, DataGridView grid)
        {
            if (grid != null)
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
        }
        
        private void RefreshBookData(DataGridView grid)
        {
            if (grid != null)
            {
                grid.DataSource = null;
                grid.DataSource = _bookTracker.GetAllBooks();
            }
        }
    }
}