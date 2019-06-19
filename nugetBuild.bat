@ECHO off
SET version=%1

IF NOT DEFINED version (
	ECHO No versionnumber provided
	EXIT /B 1
)
IF NOT EXIST ".\nugetBuild.config" (
	ECHO Missing nugetBuild.config specifying where to place the package
	EXIT /B 1
)

SET netVersion=net472
SET /P nugetRepository=<nugetBuild.config

IF NOT DEFINED nugetRepository (
	ECHO Missing path in nugetBuild.config specifying where to place the package
	EXIT /B 1
)

REM Move into debug folder from Fluent main folder
CD .\bin\Fluent.Ribbon\Debug\

IF NOT EXIST ".\root\" (
	MKDIR .\root 
	ECHO Created root folder for copying
)
IF NOT EXIST ".\root\lib\" (
	MKDIR .\root\lib
	ECHO Created lib folder for copying
)
IF NOT EXIST ".\root\lib\%netVersion%\" (
	MKDIR .\root\lib\%netVersion%
	ECHO Created %netVersion% folder for copying
)

SET /P releasenote="Provide a reason/relasenote for the new package: "

REM Create nuspec file based on version and releasenote
(
ECHO ^<?xml version="1.0"?^> 
ECHO ^<package xmlns="http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd"^>
ECHO   ^<metadata^>
ECHO     ^<id^>cFluent^</id^>
ECHO     ^<version^>%version%^</version^>
ECHO 	^<title^>Fluent^</title^>
ECHO     ^<authors^>CAA^</authors^>
ECHO     ^<owners^>CAA^</owners^>
ECHO     ^<requireLicenseAcceptance^>false^</requireLicenseAcceptance^>
ECHO     ^<description^>cBrain extended implementation of Fluent^</description^>
ECHO     ^<releaseNotes^>%releasenote%^</releaseNotes^>
ECHO     ^<copyright^>Copyright 2019^</copyright^>
ECHO     ^<tags^>Fluent Ribbon^</tags^>
ECHO   ^</metadata^>
ECHO ^</package^>
) > .\root\Fluent.dll.nuspec
	
XCOPY /s /y .\%netVersion% .\root\lib\%netVersion%
nuget pack .\root\Fluent.dll.nuspec -OutputDirectory .\root\
nuget add .\root\cFluent.%version%.nupkg -Source %nugetRepository%