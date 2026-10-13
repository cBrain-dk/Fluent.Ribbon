<#
.SYNOPSIS
Build, test, pack and distribute the cBrain Fluent wrapper on the internal NuGet feed.

.DESCRIPTION
Runs the native wrapper tests and packs Fluent.Ribbon in Release configuration,
then publishes the resulting package through rdadm nuget push.
Update Version in Fluent.Ribbon/Fluent.Ribbon.csproj before each release.

.EXAMPLE
.\Publish-CBrainWrapper.ps1
#>

$ErrorActionPreference = 'Stop'

Push-Location -LiteralPath $PSScriptRoot
try {
    & ./Build-CBrainWrapper.ps1 -FluentMode Source -Configuration Release

    $projectPath = Join-Path $PSScriptRoot 'Fluent.Ribbon\Fluent.Ribbon.csproj'
    [xml] $project = Get-Content -LiteralPath $projectPath -Raw
    $packageId = $project.SelectSingleNode('/Project/PropertyGroup/PackageId').InnerText
    $version = $project.SelectSingleNode('/Project/PropertyGroup/Version').InnerText
    $packageDirectory = Join-Path $PSScriptRoot 'bin\Fluent.Ribbon\Release'
    $packagePath = Join-Path $packageDirectory "$packageId.$version.nupkg"

    & dotnet pack $projectPath -c Release --output $packageDirectory
    if ($LASTEXITCODE -ne 0) { throw 'Fluent wrapper packing failed.' }

    & rdadm nuget push $packagePath
    if ($LASTEXITCODE -ne 0) { throw 'Fluent wrapper publication failed.' }

    Write-Host -ForegroundColor Green "$packageId $version has been packaged and distributed."
}
finally {
    Pop-Location
}
