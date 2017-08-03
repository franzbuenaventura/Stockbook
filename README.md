# Stockbook
Stockbook, an application design for inventory and sales of a small or medium business for tracking the records of all of its stocks, sales, and purchases.

Originally design as a exclusive business application for Northern Ventures but decided to make the project flexible that can be use by other companies and easy to modify. The Northern Ventures application is already a finished application, but it lacks proper software engineering principles and methodology. 
ads

I document all the process and updates in my blog: http://blog.franzbuenaventura.com/

## Instructions on running in Visual Studio
1.	After downloading and opening the solution, we need to install all the NuGet Packages by <br>
	Project -> Manage NuGet Packages -> Restore Packages 
2.	You can compile and run it with no problems 

## Notes
If the excel files are not getting copied, check your solution explorer, the excel files should be:
Build Action: Content
Copy to Output Directory: Copy Always
