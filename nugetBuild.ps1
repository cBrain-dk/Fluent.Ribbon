<#
.SYNOPSIS
Script for packing changes to a local version of Fluent and distributing it on the internal Fluent network

.DESCRIPTION
This script will take the latest build from the Release folder of Fluent and pack it into a nugetpackage, 
    which will then be installed into the path specified in the nugetBuild.config file.
    Try to avoid packing too much into a single release, rather perform several incremental packings - its easier to debug.

.PARAMETER version
The name that this version will be known under - currently we employ a double versioning which starts with the latest version of Fluent f.ex. (6.1.0)
    and then it increments the current cBrain version by 1 on the least significant digit f.ex. (1.04 => 1.05)
    finally we combine these into a single with a dash between so the final result is f.ex. 6.1.0-1.05

.PARAMETER releaseNote
A text used to identity why this package was made - it is not supposed to describe every change made, but rather the overall reason for its existince compared to the last version

.EXAMPLE
.\nugetBuild 7.0.0-0.33 "Made the backstage accessible for keyboard users."
#>

Param (
  [Parameter(Mandatory)]
  [ValidateNotNullOrEmpty()]
	[string]$version,

  [Parameter(Mandatory)]
  [ValidateNotNullOrEmpty()]
  [string]$releaseNote
)

$ErrorActionPreference = 'Stop'

if(!(Test-Path -Path ".\nugetBuild.config"))
{
	Throw "Missing nugetBuild.config specifying where to place the package"
}

$dotNetVersion = "net472"
$nugetRepository = (Get-Content -Path ".\nugetBuild.config")

if([string]::IsNullOrEmpty($nugetRepository))
{
	Throw "Missing path in nugetBuild.config ($nugetRepository) specifying where to place the package"
}

Set-Location -Path ".\bin\Fluent.Ribbon\Release\"

if(!(Test-Path -Path ".\root"))
{
	New-Item -Path ".\root" -ItemType Directory
}
if(!(Test-Path -Path ".\root\lib"))
{
	New-Item -Path ".\root\lib" -ItemType Directory
}
if(!(Test-Path -Path ".\root\lib\$dotNetVersion"))
{
	New-Item -Path ".\root\lib\$dotNetVersion" -ItemType Directory
}

Write-Host -ForegroundColor YELLOW "ATT: Fluent will be packed from the Release build!"
$nuspecData = @"
<?xml version=`"1.0`"?> 
<package xmlns=`"http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd`">
	<metadata>
		<id>cFluent</id>
		<version>$version</version>
		<title>Fluent</title>
		<authors>CAA</authors>
		<owners>CAA</owners>
		<requireLicenseAcceptance>false</requireLicenseAcceptance>
		<description>cBrain extended implementation of Fluent</description>
		<releaseNotes>$releaseNote</releaseNotes>
		<copyright>Copyright 2019</copyright>
		<tags>Fluent Ribbon</tags>
	</metadata>
</package>
"@

Out-File -FilePath ".\root\Fluent.dll.nuspec" -InputObject $nuspecData

Copy-Item -Path ".\$dotNetVersion" -Destination ".\root\lib" -Recurse -Force

#The following should probably be controlled through the packagereference shenanigans in our own solution, but we havent reached that point yet
# Remove the Annotations dll as we do not have a need for it in our projects moving forward (currently only used in Fluent for code-writing purposes)
Remove-Item -Path ".\root\lib\$dotNetVersion\JetBrains.Annotations.dll"
# Remove the Scheme generator as it is only used when Fluent is built. We just want the package
Remove-Item -Path ".\root\lib\$dotNetVersion\XamlColorSchemeGenerator.exe"
# Also remove the JSON parser as that is only used by the Scheme generator
Remove-Item -Path ".\root\lib\$dotNetVersion\Newtonsoft.Json.dll"

$nugetExe = "nuget.exe"
&$nugetExe pack .\root\Fluent.dll.nuspec -OutputDirectory .\root\
if($LASTEXITCODE -NE 0)
{
  Set-Location -Path "..\..\.."
	Throw "nuget pack command failed"
}

&$nugetExe add .\root\cFluent.$version.nupkg -Source $nugetRepository
if($LASTEXITCODE -NE 0)
{
  Set-Location -Path "..\..\.."
	Throw "nuget add command (installing the package) failed"
}

Remove-Item -Path ".\root\cFluent.$version.nupkg"
Set-Location -Path "..\..\.."

Write-Host -ForegroundColor GREEN "Fluent version $version has been packaged and installed at $nugetRepository"