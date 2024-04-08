Project  # GotheTollway
This repository contains the codebase for Norion Bank codetest.

# Requirements 

Each passage through a toll station in Gothenburg costs 8, 13, or 18 kronor depending on the time of day. The maximum amount per day and vehicle is 60 kronor.
Congestion tax is collected for vehicles passing through a toll station Monday to Friday between 06:00 and 18:29. Tax is not collected on Saturdays, holidays, days before holidays, or during the month of July. Some vehicles are exempt from congestion tax. A car passing through multiple toll stations within 60 minutes is only taxed once. The amount to be paid is then the highest amount among the passages.


# Recent Updates
- Fixed Logic for Retrieving Last Passage: Corrected the logic to retrieve the last passage within the last hour.
- Added Tests for TollService: Created tests to verify the functionality of TollService.
- Infrastructure Project Addition: Included an Infrastructure project to simulate HTTP requests to an external API for fetching vehicle data.

# Recent Development Timeline
- Apr 5, 2024: Started implementing logic to manage toll passes.
- Apr 6, 2024: Added Infrastructure project to simulate HTTP requests to an external API for vehicle data retrieval.
- 3 Days Ago: Created entities, database configuration, and initial migration.
- 3 Days Ago: Structured and organized folders, created necessary projects.
 
