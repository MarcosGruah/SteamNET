# SteamNET - Project Requirements (Version 0.00)

## User Input and Data Retrieval

- Users are required to input their Steam ID, which should consist of numeric characters rather than their username.
- The system is designed to check the SQL database to determine the presence of an existing entry based on the provided Steam ID.
- If the entry is found, the system evaluates the timestamp of the last update; in cases where it surpasses the 24-hour threshold, a new request is initiated to the Steam API to refresh the database.
- In instances where data in the database has been updated within a 24-hour window, the system utilizes the existing database information and communicates to the user when the next data refresh will be possible.

## Display Functionality

- The primary objective for this iteration is the establishment of fundamental functionality:
  - Display of the User Profile, encompassing the user's name and avatar.
  - Display of a list of games owned by the user, displaying the Game Name and respective Game Image.

## Custom API Development

- The project's core emphasis revolves around the selective extraction of specific data elements from the Steam API.
- The project aims to construct a customized API designed to retrieve data from the SQL database and seamlessly deliver this information to the application.
