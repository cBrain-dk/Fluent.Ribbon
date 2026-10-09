param(
    [ValidateSet('Source', 'Package')]
    [string] $FluentMode = 'Source',
    [string] $Configuration = 'Release',
    [string] $ResultsDirectory = (Join-Path $PSScriptRoot 'TestResults\CBrainWrapper')
)

$ErrorActionPreference = 'Stop'
$vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
$installation = & $vswhere -latest -requires Microsoft.Component.MSBuild -property installationPath
if ($LASTEXITCODE -ne 0 -or -not $installation) { throw 'Visual Studio MSBuild installation was not found.' }
$msbuild = Join-Path $installation 'MSBuild\Current\Bin\MSBuild.exe'
$vstest = Join-Path $installation 'Common7\IDE\Extensions\TestPlatform\vstest.console.exe'
$sourceMode = ($FluentMode -eq 'Source').ToString().ToLowerInvariant()
$testProject = Join-Path $PSScriptRoot 'CBrain.Fluent.Ribbon.Tests\CBrain.Fluent.Ribbon.Tests.csproj'
$commonArguments = @("/p:Configuration=$Configuration", '/p:Platform=AnyCPU', '/m:1', '/nr:false', '/v:minimal')

& $msbuild $testProject /restore /t:Rebuild "/p:UseFluentSource=$sourceMode" @commonArguments
if ($LASTEXITCODE -ne 0) { throw 'Native wrapper test build failed.' }

foreach ($framework in @('net462', 'net6.0-windows', 'net8.0-windows')) {
    $testAssembly = Join-Path $PSScriptRoot "bin\CBrain.Fluent.Ribbon.Tests\$Configuration\$framework\CBrain.Fluent.Ribbon.Tests.dll"
    $frameworkResults = Join-Path $ResultsDirectory "$FluentMode\$framework"
    New-Item -ItemType Directory -Path $frameworkResults -Force | Out-Null
    & $vstest $testAssembly /Logger:trx "/ResultsDirectory:$frameworkResults"
    if ($LASTEXITCODE -ne 0) { throw "Native wrapper tests failed for $framework." }
}
