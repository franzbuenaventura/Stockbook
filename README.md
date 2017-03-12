# Stockbook
Stockbook, an application design for inventory and sales of a small or medium business for tracking the records of all of its stocks, sales, and purchases.

Originally design as a exclusive business application for Northern Ventures but decided to make the project flexible that can be use by other companies and easy to modify. The Northern Ventures application is already a finished application, but it lacks proper software engineering principles and methodology. 
ads
I document all the process and updates in my blog: http://blog.franzbuenaventura.com/

Gathering Requirements 
As the original software design for a specific company, all of the requirements are based on them and how their current application does it. Same with the GUI, I will make some changes for it to be less prones to error and more strict so no conflict or accidental deletion occurs.

Must have:<br>
	CRUD system for Inventory<br>
	CRUD system for Transactions<br>
	Reads and outputs Excel document for Invoice<br>
	Reads and outputs Excel document for Product Stock History<br>
	Reads and outputs Excel document for Transactions(Sales/Purchased)<br>
	Basic GUI (Graphical User Interface)<br>
	Database system using flat files<br>
	<br>
Should have:<br>
	Authentication System<br>
		CRUD system for Accounts<br>
		Account Roles (At least 2 for role in Account Creation)<br>
		GUI for Login, Logout & CRUD system for Accounts<br>
	Logging System<br>
		CRUD system and GUI for Logging<br>
			Implementation of events for user tracking<br>
		Basic encryption for flat files<br>
	Settings menu<br>
		Logo (What logo should be used)<br>
		Company Name (Will reflect on all the output templates)<br>
		Modify database folder (Adding support for network file share drives)<br>
	File lock to avoid conflict modifications<br>
	Fullscreen Lock<br>
	Daily/Weekly/Monthly Backup files (Compress?)<br>
	Unit Testing<br>
	Documentation<br>
<br>Could have:<br>
	Flexible template (More options on what it will output)<br>
	Advanced encryption for flat files<br>
	Online Backup System<br>
