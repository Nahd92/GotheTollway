# GotheTollway
This repository contains the codebase for Norion Bank codetest.

# Requirements 

Each passage through a toll station in Gothenburg costs 8, 13, or 18 kronor depending on the time of day. The maximum amount per day and vehicle is 60 kronor.
Congestion tax is collected for vehicles passing through a toll station Monday to Friday between 06:00 and 18:29. Tax is not collected on Saturdays, holidays, days before holidays, or during the month of July. Some vehicles are exempt from congestion tax. A car passing through multiple toll stations within 60 minutes is only taxed once. The amount to be paid is then the highest amount among the passages.

# Documentation 

The idea of the application is that it should be a simple API that has two ways of communication:

 1. Create a Tollpassage, regardless if there's a fee or not. This request and logic should only care to create the Tollpassage. 
 2. Create an Invoice. This is a GET method with its only purpose to get a specific car's all passages and total sum of fees.

Codewise, I have used the repository pattern and Entity Framework to create an easy way to store and retrieve data based on the different exemptions.

Structure 
- API: The API that's exposed to the "public".
- Domain: Holds all entities, interfaces, business logic, and some helper classes.
- Contract: Holds all the models that are used for the API to isolate the domain entities and not expose them.
- Infrastructure: Holds the needed logic to request other services (internally and externally if needed).

I have also tried to cover as much code with tests as possible. (There are some tests left to do).

# What's left to do:
Write more tests.
Finalize the logic so that it fulfills all the requirements. (A few things left).
Prepare for tomorrow's demonstration.

--------------------------------------------

# Recent Updates
- Fixed Logic for Retrieving Last Passage: Corrected the logic to retrieve the last passage within the last hour.
- Added Tests for TollService: Created tests to verify the functionality of TollService.
- Infrastructure Project Addition: Included an Infrastructure project to simulate HTTP requests to an external API for fetching vehicle data.

# Recent Development Timeline
- Apr 8, 2024: Fixed the failing tests and added ExemptionVehicleTypes entity
- Apr 5, 2024: Started implementing logic to manage toll passes.
- Apr 6, 2024: Added Infrastructure project to simulate HTTP requests to an external API for vehicle data retrieval.
- 3 Days Ago: Created entities, database configuration, and initial migration.
- 3 Days Ago: Structured and organized folders, created necessary projects.
 
