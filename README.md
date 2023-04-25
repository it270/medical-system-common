# Medical System Common library

Medical system common library. Made with ASP.NET.

## Build package (NuGet)

```sh
# Restore packages
dotnet restore
# Build project
dotnet build
# Pack project as NuGet package (Release profile)
dotnet pack -c Release
# Install package (local environment)
dotnet nuget add source -n It270.Local src/MedicalSystem.Common/bin/Release/It270.MedicalSystem.Common.$VERSION.nupkg
```

## Useful commands

```sh
# List all sources
dotnet nuget list source
# Remove current project source
dotnet nuget remove source It270.Local
```