# Progetto corso Applicazioni Web: Linguaggi &amp; Architetture 2019/2020

Allo studente si chiede di implementare un job scheduler simile a http://cronicle.net/, in grado
di lanciare processi (i job) in modo temporizzato su uno o più nodi, anche geograficamente
distribuiti.

* Lo scheduler deve poter orchestrare un cluster di nodi in cui
  
  * Master: è il solo nodo responsabile di interagire con il database contenente tutti i
dati (schedulazioni, indirizzo IP dei nodi, …)
  
  * Slave: sono tutti i nodi, Master compreso, in grado di eseguire un job schedulato.
  
* I nodi Slave possono essere organizzati in gruppi (un nodo può appartenere a più gruppi).
* Il job è l’oggetto della schedulazione ed è descritto da:
  * (Obbligatorio) Path (locale rispetto al file system dello Slave
) dell’eseguibile che
deve essere lanciato
  * (Opzionale) Argomenti: stringa contenente i parametri da passare command-line
all’eseguibile.
  * (Opzionale) Gruppo di nodi: su quale gruppo di nodi deve essere eseguito il job. Se
non viene indicato, di default va in esecuzione su tutti i nodi (Master compreso).
* Un job può essere eseguito a comando o essere schedulato secondo un timing definibile
con la sintassi CRON

* Occorre tenere traccia di tutti i run dei job, dell’ExitCode e di quanto eventualmente scrive
sullo standard output l’eseguibile. Tenere traccia anche del PID (Process IDentifier).
* Prevedere la possibilità di interrompere un job in esecuzione.
* Si distinguono due ruoli utente
  * Admin: può accedere a qualunque funzionalità
  * Editor: può schedulare applicazioni e consultare i log delle esecuzioni, ma non può
amministrare i nodi, i gruppi e creare/cancellare/modificare altri utenti (ruolo
compreso).
* L’applicazione deve creare un utente “admin” (con ruolo Admin) di default con una
password configurabile in appSettings.json.
* Un utente con ruolo Admin può creare/modificare/cancellare altri utenti, attribuire loro un
ruolo o modificare il ruolo di un utente esistente.
* Tutte le funzionalità sopra indicate devono essere accessibili sotto forma di API REST ed
essere gestibili anche con una WebApp.
* Le API REST devono essere protette con un JWT.

### Suggerimenti
* Per lanciare un eseguibile da C#: https://tinyurl.com/y8de8nn4
* Per lanciare da C# un eseguibile e catturare lo standard output:
https://tinyurl.com/ydz3r2ty
