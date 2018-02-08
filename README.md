# Tess #
Codice sorgente dell'applicazione Tess (https://play.google.com/store/apps/details?id=com.Camo.Tess). 
Tess permette di tenere traccia della propria settimana lavorativa conteggiando le ore lavorate giornalmente e quelle rimanenti rispetto al proprio piano orario. 

## Come è fatta ##
Per compilare l'applicazione è necessario Visual Studio e Xamarin. L'applicazione è sviluppata tramite Xamarin Forms Portable con alcuni Dependency Service sviluppati esclusivamente per la parte Android, non è previsto alcun supporto per la parte iOs nè ora nè mai.

## Cosa fa ##
Tramite la pagina impostazioni si decidono i parametri base necessari al calcolo delle ore (ore lavorative settimanali, numero di giorni lavorativi settimanali). Una volta fatto sarà possibile inserire quotidianamente le entrate e uscite dall'ufficio in modo tale che vengano salvate nei dati dell'app. E' possibile modificare i dati inseriti tenendo premuto il giorno da modificare e cliccando poi l'icona di modifica.
Tappando sull'icona con il pollice (verso se l'orario è attualmente sotto la media) nella pagina principale è possibile avere un resoconto sulla attuale situazione settimanale.

E' infine possibile ricevere una notifica dopo n ore dalla creazione del check-in per ricordarsi di chiuderlo.

## To Do ##
Non è previsto alcuno sviluppo futuro anche se esiste l'idea di implementare nell'applicazione la tecnologia NFC e la sincronizzazione degli intervalli lavorati con gCalendar. Chiunque volesse forkare e implementare nuove funzioni è il benvenuto.
