# Stockbook
Stockbook, an application design for inventory and sales of a small or medium business for tracking the records of all of its stocks, sales, and purchases.

Originally design as a exclusive business application for Northern Ventures but decided to make the project flexible that can be use by other companies and easy to modify. The Northern Ventures application is already a finished application, but it lacks proper software engineering principles and methodology. 
ads
I document all the process and updates in my blog: http://blog.franzbuenaventura.com/

Gathering Requirements 
As the original software design for a specific company, all of the requirements are based on them and how their current application does it. Same with the GUI, I will make some changes for it to be less prones to error and more strict so no conflict or accidental deletion occurs.

adasMust have: 
	CRUD system for Inventory <.
	CRUD system for Transactions
	Reads and outputs Excel document for Invoice
	Reads and outputs Excel document for Product Stock History
	Reads and outputs Excel document for Transactions(Sales/Purchased)
s	Database system using flat files
	
Should have:
	Authentication System
		CRUD system for Accounts
		Account Roles (At least 2 for role in Account Creation)
		GUI for Login, Logout & CRUD system for Accounts
	Logging System
		CRUD system and GUI for Logging
			Implementation of events for user tracking
		Basic encryption for flat files
	Settings menu
		Logo (What logo should be used)
		Company Name (Will reflect on all the output templates)
		Modify database folder (Adding support for network file share drives)
	File lock to avoid conflict modifications
	Fullscreen Lock
	Daily/Weekly/Monthly Backup fil
Could have:
	Flexible template (More options on what it will output)
	Advanced encryption for flat files
	Online Backup System
