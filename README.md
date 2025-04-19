# Studio Manager

Studio Manager è un'applicazione desktop WPF progettata per aiutare gli studenti a gestire le loro attività accademiche, inclusi compiti, esami e libri.

## Funzionalità

*   **Gestione Task**: Aggiungi, modifica, elimina e segna come completate le attività di studio.
*   **Gestione Esami**: Tieni traccia degli esami imminenti, registra i risultati e visualizza lo storico.
*   **Tracciamento Libri**: Monitora i libri in lettura, registra le date di inizio/fine, valutazioni e recensioni.
*   **Tema Chiaro/Scuro**: Cambia l'aspetto dell'applicazione.
*   **Notifiche**: Ricevi notifiche per eventi importanti (funzionalità di base).
*   **Persistenza Dati**: I dati vengono salvati localmente in file JSON (`tasks.json`, `exams.json`, `books.json`).
*   **Minimizzazione nella Tray**: L'applicazione può essere minimizzata nell'area di notifica.

## Come Eseguire

1.  Clona il repository.
2.  Apri la soluzione `Studio.sln` con Visual Studio.
3.  Compila ed esegui il progetto `Studio`.

## Struttura del Progetto

*   **Studio/**: Cartella principale del progetto WPF.
    *   `App.xaml`/`.cs`: Punto di ingresso dell'applicazione WPF.
    *   `MainWindow.xaml`/`.cs`: Finestra principale dell'applicazione.
    *   `NotificationWindow.cs`: Finestra per le notifiche personalizzate.
    *   `Dialogs/`: Contiene le finestre di dialogo per aggiungere/modificare elementi (Task, Esami, Libri).
    *   `Models/`: Contiene le classi per la logica di business e la gestione dei dati (`TaskManager`, `ExamManager`, `BookTracker`).
    *   `ViewModels/`: Contiene i ViewModel per il pattern MVVM (es. `DashboardViewModel`).
    *   `Properties/`: Contiene le impostazioni del progetto (es. `launchSettings.json`).
    *   `*.json`: File di dati per la persistenza (tasks, exams, books).

## Licenza

Questo progetto è rilasciato sotto la licenza MIT. Vedi il file [LICENSE](../../LICENSE) per maggiori dettagli.
