# Diademos
Verifiable news for residents of unfree nations.

[![Codacy Badge](https://api.codacy.com/project/badge/Grade/42681fecfd834868a87e472e7a481213)](https://www.codacy.com/gh/MadeByEmil/Diademos?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=MadeByEmil/Diademos&amp;utm_campaign=Badge_Grade)
[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2FMadeByEmil%2FDiademos.svg?type=small)](https://app.fossa.com/projects/git%2Bgithub.com%2FMadeByEmil%2FDiademos?ref=badge_small)

## What is Diademos?
Diademos is a news aggregator that bypasses internet censorship to provide articles from BBC News, Reuters, the New York Times, and more, in both English and Chinese. To further the mission of bringing truthful reporting to those in censored states, Diademos uses Indiana University's [Hoaxy](https://hoaxy.iuni.iu.edu/) to provide truthness ratings for reporting, as well as [algorithmic sentiment analysis](https://github.com/thisandagain/sentiment) to determine a journalist's stance on an issue.

## Building from source:
Pre-requisites:
- [.NET Core SDK 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)
- [Node.js](https://nodejs.org/en/download/)

### Windows
```
dotnet tool install ElectronNET.CLI
dotnet build --configuration Release
dotnet electronize build /target win
```
Outputs to ```./bin/desktop/```

### MacOS
```
dotnet tool install ElectronNET.CLI
dotnet build --configuration Release
dotnet electronize build /target osx
```
Outputs to ```./bin/desktop/```

### Linux AppImage
```
dotnet tool install ElectronNET.CLI
dotnet build --configuration Release
dotnet electronize build /target linux
```
Outputs to ```./bin/Desktop/```


## License
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2FMadeByEmil%2FDiademos.svg?type=large)](https://app.fossa.io/projects/git%2Bgithub.com%2FMadeByEmil%2FDiademos?ref=badge_large)