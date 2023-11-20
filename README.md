# SteamNET

SteamNET is a .NET 8 project designed to retrieve data from the Steam API, process it, and store it in a SQL Database. It leverages an ASP.NET Core Minimal Web API to initiate GET requests to the Steam API. Requests are executed selectively, only when the required information is not already present in the SQL Database or is deemed outdated. If the data is available and up-to-date, it is directly retrieved from the database; otherwise, a request to the Steam API is triggered. Dapper is employed to streamline database communication within the project.

## The Profile UI

The user interface is simple, focusing on backend functionality. Input a valid SteamID to obtain basic information such as the user's avatar, Steam account creation date, and data age.

![Profile UI](https://i.imgur.com/kdowMba.png)

## List of Owned Games

Under the user info, a table shows owned games with prices and playtime. Click "Update Game Database" to fetch missing information. Once updated, the button changes to "Game Database is up to date." For subsequent submissions with the same Steam ID, details for previously missing games will be displayed.

![Update Database](https://i.imgur.com/kIJnLoI.png)

## The Games Table

This section showcases every game in the database, providing information on names and prices.

![Games Table](https://i.imgur.com/4w7Ju4T.png)