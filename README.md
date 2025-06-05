A small personal project to keep track of the latest releases (vinyl, movie...)  
This app was built during my spare time as a way to explore and pratice full-stack development, it has no pretensions and no commercial purpose.

## :star: Features

- **Fetcher**: Back-end service that scrapes data from various public APIs and temporarily stores it  
- **Fetcher Viewer**: Desktop application for real-time monitoring of the scrapping process  
- **Web API**: Exposes the most recently scraped releases  
- **Web Client**: Front-End application that consumes the API to display the latest releases to users  

![Overview_Architecture](https://github.com/user-attachments/assets/bfef2dd4-f1c2-47f9-abac-045b89d63285)  


## :rocket: Tech Stack

- **Frontend**: Blazor (WebAssembly), MudBlazor  
- **Backend**: .NET 9 (C#), REST API, BackgroundService, WPF, Mediator  
- **Database**: SQLServer, Entity Framework (database first)  
- **API**: Spotify, Discogs  

## :wrench: Solution overview  

![Solution_Structure](https://github.com/user-attachments/assets/af7090cb-c389-4ec8-a092-2f5eb301167d)

