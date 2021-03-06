# Progetto corso Applicazioni Web: Linguaggi &amp; Architetture 2019/2020

[![CodeFactor](https://www.codefactor.io/repository/github/ilgianfri/upo-applicazioniweb-linguaggiarchitetture/badge)](https://www.codefactor.io/repository/github/ilgianfri/upo-applicazioniweb-linguaggiarchitetture)
![Last commit](https://img.shields.io/github/last-commit/ilGianfri/upo-applicazioniweb-linguaggiarchitetture.svg?style=popout-square)
![License](https://img.shields.io/github/license/ilGianfri/upo-applicazioniweb-linguaggiarchitetture.svg?style=popout-square) 

### Le istruzioni per il setup sono contenute all'interno della [Wiki](https://github.com/ilGianfri/UPO-ApplicazioniWeb-LinguaggiArchitetture/wiki)

**[ITALIANO]**

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

**********************

**[ENGLISH]**

The student is required to implement a job scheduler similar to http://cronicle.net/, able to launch
processes (jobs) in a scheduled way on one or more nodes, even geographically distributed.

* The cheduler must be able to orchestrate a cluster  of nodes in which
  * Master: is the only node responsible of interacting with the database containing all data (schedules, nodes IP, ...)

  * Slave: all nodes, including Master, that can run a scheduled job

* Slave nodes can be organized in groups (a node might be part of multiple groups)
* The job is the object of the scheduling and is described by:
  * (Required) Path (local to the Slave file system) of the executable that will be launched
  * (Optional) Arguments: a string containing all parameters to pass command-line to the executable
  * (Optional) Group of nodes: on which group of nodes the job must be executed. If not specified, by default it will run on all nodes (Master included)
* A job can be executed on command or scheduled with a CRON syntax

* It is required to keep track of all jobs runs, their ExitCode and their standard output. Keep track of the PID too (Process Identifier).
* Provide the ability to stop a running job
* There are two user roles:
  * Admin: can access any functionality
  * Editor: can schedule jobs and read execution logs but cannot administer nodes, groups and create/delete/edit other users (role included) 
* The application must create an "admin" user (with Admin role) by default with a password from appSettings.json
* An admin can create/edit/delete other users, add or change their existing role
* All the features must be available as REST API and be manageable with a web app
* REST API must be protected with JWT Authentication
