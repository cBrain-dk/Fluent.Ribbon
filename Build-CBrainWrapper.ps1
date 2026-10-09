param(
    [ValidateSet('Source', 'Package')]
    [string] $FluentMode = 'Source',
    [string] $Configuration = 'Release',
    [string] $ResultsDirectory = (Join-Path $PSScriptRoot 'TestResults\CBrainWrapper'),
    [switch] $Pack,
    [string] $PackageVersion = '11.0.2-cbrain.1',
    [string] $PackageOutputDirectory = (Join-Path $PSScriptRoot 'artifacts')
)

$ErrorActionPreference = 'Stop'
$vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
$installation = & $vswhere -latest -requires Microsoft.Component.MSBuild -property installationPath
if ($LASTEXITCODE -ne 0 -or -not $installation) { throw 'Visual Studio MSBuild installation was not found.' }
$msbuild = Join-Path $installation 'MSBuild\Current\Bin\MSBuild.exe'
$vstest = Join-Path $installation 'Common7\IDE\Extensions\TestPlatform\vstest.console.exe'
$sourceMode = ($FluentMode -eq 'Source').ToString().ToLowerInvariant()
$testProject = Join-Path $PSScriptRoot 'CBrain.Fluent.Ribbon.Tests\CBrain.Fluent.Ribbon.Tests.csproj'
$commonArguments = @("/p:Configuration=$Configuration", '/p:Platform=AnyCPU', '/m', '/nr:false', '/v:minimal')

& $msbuild $testProject /restore /t:Rebuild "/p:UseFluentSource=$sourceMode" @commonArguments
if ($LASTEXITCODE -ne 0) { throw 'Native wrapper test build failed.' }

foreach ($framework in @('net462', 'net6.0-windows', 'net8.0-windows')) {
    $testAssembly = Join-Path $PSScriptRoot "bin\CBrain.Fluent.Ribbon.Tests\$Configuration\$framework\CBrain.Fluent.Ribbon.Tests.dll"
    $frameworkResults = Join-Path $ResultsDirectory "$FluentMode\$framework"
    New-Item -ItemType Directory -Path $frameworkResults -Force | Out-Null
    & $vstest $testAssembly /Logger:trx "/ResultsDirectory:$frameworkResults"
    if ($LASTEXITCODE -ne 0) { throw "Native wrapper tests failed for $framework." }
}

if ($Pack) {
    $sourceCommit = & git -C $PSScriptRoot rev-parse HEAD
    if ($LASTEXITCODE -ne 0) { throw 'Cannot resolve the wrapper source commit.' }
    & git -C $PSScriptRoot diff --quiet HEAD
    if ($LASTEXITCODE -ne 0) { throw 'Commit tracked wrapper changes before packing.' }
    New-Item -ItemType Directory -Path $PackageOutputDirectory -Force | Out-Null
    $wrapperProject = Join-Path $PSScriptRoot 'CBrain.Fluent.Ribbon\CBrain.Fluent.Ribbon.csproj'
    & $msbuild $wrapperProject /restore /t:Pack /p:UseFluentSource=false @commonArguments `
        /p:UpdateVersionProperties=false /p:UpdateAssemblyInfo=false `
        "/p:Version=$PackageVersion" "/p:PackageVersion=$PackageVersion" `
        /p:AssemblyVersion=11.0.0.0 /p:FileVersion=11.0.2.0 `
        "/p:InformationalVersion=$PackageVersion+Sha.$sourceCommit" `
        /p:IncludeSourceRevisionInInformationalVersion=false `
        /p:RepositoryBranch=fluent-wrapper "/p:RepositoryCommit=$sourceCommit" `
        "/p:PackageOutputPath=$PackageOutputDirectory"
    if ($LASTEXITCODE -ne 0) { throw 'Native cBrain wrapper pack failed.' }
}
