@echo off

set filter=%~1
set configuration=%~2
set openHtml=%~3

if "%filter%"=="" set filter=""
if "%configuration%"=="" set configuration=Debug
if "%openHtml%"=="" set openHtml=openHtml

set startDirectory=%~dp0
set workingDirectory="%~dp0Tests\bin\%configuration%\net10.0"
set appsettings=%startDirectory%Tests\appsettings.json

:: Set XmlView to true before running tests
powershell -Command "(Get-Content '%appsettings%') -replace '\"XmlView\": false', '\"XmlView\": true' | Set-Content '%appsettings%'"

dotnet test %startDirectory%Tests\Tests.csproj --filter %filter% --configuration %configuration% --logger trx --results-directory %workingDirectory%

:: Set XmlView back to false after tests
powershell -Command "(Get-Content '%appsettings%') -replace '\"XmlView\": true', '\"XmlView\": false' | Set-Content '%appsettings%'"

cd %workingDirectory%

dotnet %startDirectory%HtmlReport\bin\%configuration%\net10.0\HtmlReport.dll %workingDirectory% %openHtml% "filter=%filter:"=%;configuration=%configuration%"

cd %startDirectory%
