
  # DevConfessions 

</div>
  DevConfessions é uma plataforma onde desenvolvedores podem compartilhar anonimamente suas confissões, experiências e histórias do mundo da programação. Um espaço seguro e divertido para desabafos da comunidade dev.

```mermaid
flowchart TB
    %% Presentation Layer
    Browser["User Browser\nHTML5, CSS3, Bootstrap, jQuery"]:::presentation

    %% Application Container
    subgraph "Web App Container"
        direction TB
        subgraph "Startup & Configuration"
            direction TB
            Program["Program.cs\n(Host & Middleware)"]:::application
            LaunchSettings["launchSettings.json"]:::infra
            AppDev["appsettings.Development.json"]:::infra
            AppProd["appsettings.json"]:::infra
        end
        Controllers["ConfessionsController"]:::application
        Services["DataConfessionService"]:::application
        Models["Confession.cs"]:::application
        subgraph "Razor Views"
            direction TB
            CreateView["Create.cshtml"]:::presentation
            EditView["Edit.cshtml"]:::presentation
            IndexView["Index.cshtml"]:::presentation
            ShareView["Share.cshtml"]:::presentation
            LayoutView["_Layout.cshtml"]:::presentation
            ValidationView["_ValidationScriptsPartial.cshtml"]:::presentation
            ViewImports["_ViewImports.cshtml"]:::presentation
            ViewStart["_ViewStart.cshtml"]:::presentation
        end
        subgraph "Static Assets"
            direction TB
            SiteCSS["site.css"]:::presentation
            SiteJS["site.js"]:::presentation
            BootstrapLib["bootstrap"]:::presentation
            jQueryLib["jquery"]:::presentation
            JV["jquery-validation"]:::presentation
            JU["jquery-validation-unobtrusive"]:::presentation
        end
    end

    %% Data Layer
    DataStore["confessions.json\n(JSON Data Store)"]:::data

    %% Infrastructure Files
    subgraph "Infrastructure Files"
        direction TB
        DockerfileNode["Dockerfile"]:::infra
        Solution["DevConfessions.sln"]:::infra
        Project["DevConfessions.csproj"]:::infra
    end

    %% Relationships
    Browser -->|"HTTP GET/POST"| Controllers
    Controllers -->|"calls"| Services
    Services -->|"reads/writes"| DataStore
    Services -->|"returns Domain Model"| Controllers
    Controllers -->|"renders"| CreateView
    Controllers -->|"renders"| EditView
    Controllers -->|"renders"| IndexView
    Controllers -->|"renders"| ShareView
    Controllers -->|"uses Layout"| LayoutView
    Controllers -->|"uses Validation Scripts"| ValidationView
    Program --> Controllers
    Program --> Services
    Program --> Models
    Program --> StaticAssets
    Program --> DataStore
    DockerfileNode --> Program

    %% Click Events
    click Controllers "https://github.com/gabrielvesal/devconfessions/blob/main/DevConfessions/Controllers/ConfessionsController.cs"
    click Models "https://github.com/gabrielvesal/devconfessions/blob/main/DevConfessions/Models/Confession.cs"
    click Services "https://github.com/gabrielvesal/devconfessions/blob/main/DevConfessions/Services/DataConfessionService.cs"
    click CreateView "https://github.com/gabrielvesal/devconfessions/blob/main/DevConfessions/Views/Confessions/Create.cshtml"
    click EditView "https://github.com/gabrielvesal/devconfessions/blob/main/DevConfessions/Views/Confessions/Edit.cshtml"
    click IndexView "https://github.com/gabrielvesal/devconfessions/blob/main/DevConfessions/Views/Confessions/Index.cshtml"
    click ShareView "https://github.com/gabrielvesal/devconfessions/blob/main/DevConfessions/Views/Confessions/Share.cshtml"
    click LayoutView "https://github.com/gabrielvesal/devconfessions/blob/main/DevConfessions/Views/Shared/_Layout.cshtml"
    click ValidationView "https://github.com/gabrielvesal/devconfessions/blob/main/DevConfessions/Views/Shared/_ValidationScriptsPartial.cshtml"
    click ViewImports "https://github.com/gabrielvesal/devconfessions/blob/main/DevConfessions/Views/_ViewImports.cshtml"
    click ViewStart "https://github.com/gabrielvesal/devconfessions/blob/main/DevConfessions/Views/_ViewStart.cshtml"
    click Program "https://github.com/gabrielvesal/devconfessions/blob/main/DevConfessions/Program.cs"
    click LaunchSettings "https://github.com/gabrielvesal/devconfessions/blob/main/DevConfessions/Properties/launchSettings.json"
    click AppDev "https://github.com/gabrielvesal/devconfessions/blob/main/DevConfessions/appsettings.Development.json"
    click AppProd "https://github.com/gabrielvesal/devconfessions/blob/main/DevConfessions/appsettings.json"
    click SiteCSS "https://github.com/gabrielvesal/devconfessions/blob/main/DevConfessions/wwwroot/css/site.css"
    click SiteJS "https://github.com/gabrielvesal/devconfessions/blob/main/DevConfessions/wwwroot/js/site.js"
    click BootstrapLib "https://github.com/gabrielvesal/devconfessions/tree/main/DevConfessions/wwwroot/lib/bootstrap"
    click jQueryLib "https://github.com/gabrielvesal/devconfessions/tree/main/DevConfessions/wwwroot/lib/jquery"
    click JV "https://github.com/gabrielvesal/devconfessions/tree/main/DevConfessions/wwwroot/lib/jquery-validation"
    click JU "https://github.com/gabrielvesal/devconfessions/tree/main/DevConfessions/wwwroot/lib/jquery-validation-unobtrusive"
    click DataStore "https://github.com/gabrielvesal/devconfessions/blob/main/DevConfessions/wwwroot/data/confessions.json"
    click DockerfileNode "https://github.com/gabrielvesal/devconfessions/tree/main/Dockerfile"
    click Solution "https://github.com/gabrielvesal/devconfessions/blob/main/DevConfessions.sln"
    click Project "https://github.com/gabrielvesal/devconfessions/blob/main/DevConfessions/DevConfessions.csproj"

    %% Styles
    classDef presentation fill:#E0F7FA,stroke:#006064,color:#004D40
    classDef application fill:#E8F5E9,stroke:#1B5E20,color:#2E7D32
    classDef data fill:#FFF8E1,stroke:#F57F17,color:#E65100
    classDef infra fill:#ECEFF1,stroke:#546E7A,color:#37474F

