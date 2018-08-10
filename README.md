## CallGate

API and Client projects are located under `src/` directory.

### Requirements

1. https://nodejs.org/en/download/
2. https://www.microsoft.com/net/learn/get-started/windows

### Installation

1. npm install
2. dotnet restore
3. dotnet run

### Workflow

#### Git-flow

Tutorial: [https://danielkummer.github.io/git-flow-cheatsheet/](https://danielkummer.github.io/git-flow-cheatsheet/)

### Deployed versions

**Production** - deployed from Master branch

- [http://xxxxxxxxxx.azurewebsites.net/](http://xxxxxxx.azurewebsites.net/)

**Staging** - deployed from Develop branch

- [http://xxxxxxxxxxx.azurewebsites.net/](http://xxxxxxxx.azurewebsites.net/)

## Tests projects

Tests projects are located under `tests/` directory.

### Introduction

There are two type of xUnit tests projects:
- Unit tests in directory `tests/UnitTests`
- Integration (end to end) API tests in directory `tests/EndToEnd`

`.csproj` test project files have `src/CallGate.csproj` linked to have an access to `CallGate.dll`

### Running tests

Go to `tests/UnitTests` or `tests/EndToEnd`

1. dotnet restore
2. dotnet build
3. dotnet test
