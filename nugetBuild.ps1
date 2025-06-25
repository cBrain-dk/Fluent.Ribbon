<#
.SYNOPSIS
Script for packing changes to a local version of Fluent and distributing it on the internal nuget network

.DESCRIPTION
This script will take the latest build from the Release folder of Fluent and pack it into a nugetpackage.
	Remember to update the version number in the Fluent.Ribbon.csproj file.
    Try to avoid packing too much into a single release, rather perform several incremental packings - its easier to debug.

.EXAMPLE
.\nugetBuild
#>

$ErrorActionPreference = 'Stop'

# Only pack the Ribbon itself, ignoring sandbox and test
Set-Location -Path Fluent.Ribbon

& dotnet publish -f net472
& dotnet pack

# Move to the package location
Set-Location -Path "..\bin\Fluent.Ribbon\Release\"

$latest = (Get-ChildItem -Attributes !Directory | Sort-Object -Descending -Property LastWriteTime | select -First 1)
$latestName = $latest.Name
& rdadm nuget push $latestName

# Return to original location
Set-Location -Path "..\..\..\"

Write-Host -ForegroundColor GREEN "cFluent $latestName has been packaged and distributed"
