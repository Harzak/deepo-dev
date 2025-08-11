</br>

A small personal project to keep track of the latest releases (vinyl, movie...)  
This app was built during my spare time as a way to explore and practice full-stack development, it has no pretensions and no commercial purpose.

</br>

# :star: Features

## Web Client
Front-End application that consumes the API to display the latest releases to users 

<img src="https://github.com/user-attachments/assets/4d608d1e-a9ae-4c8b-ac35-b92f824cd8e9" width="100%" alt="fetcher_web_app_demo" />
</br>
 
## Fetcher
Back-end service that scrapes data from various public APIs and temporarily stores it  

## Fetcher Viewer
Desktop application for real-time monitoring of the scrapping process  

<img src="https://github.com/user-attachments/assets/fecac2a7-e383-4d88-8fbf-418361789c07" width="85%" alt="fetcher_viewer_demo" />
</br>
 
## Web API
Exposes the most recently scraped releases  


# :wrench: Architecture

## Overview
<img src="https://github.com/user-attachments/assets/bfef2dd4-f1c2-47f9-abac-045b89d63285" width="40%" alt="overview" />

## Solution
<img src="https://github.com/user-attachments/assets/af7090cb-c389-4ec8-a092-2f5eb301167d" width="70%" alt="overview" />

</br>

# :rocket: Tech Stack

- **Frontend**: Blazor WebAssembly ([MudBlazor](https://github.com/MudBlazor/MudBlazor)), WPF XAML ([WPF UI](https://github.com/lepoco/wpfui)) 
- **Backend**: .NET 9 (C#), REST API, BackgroundService, MVVM, Mediator ([MediatR](https://github.com/LuckyPennySoftware/MediatR))
- **Database**: SQLServer, Entity Framework (database first)  
- **API**: Spotify, Discogs

</br>




