[CmdletBinding()]
param (
    [Parameter()] [int] $year,
    [Parameter()] [int] $day
)

if (-not $year) {
    $year = (Get-Date).ToString("yyyy")
}

$_day = if ($day) {
    $day = $day.ToString("00")
}
else {
    (Get-Date).ToString("dd")
}

Read-Host "Scaffolding $year/$_day, press enter to continue or ctrl+c to cancel"

if (-not (Test-Path $year)) {
    Write-Host "Creating $year directory"
    $null = New-Item -Name "$year" -ItemType Directory
}

Push-Location "$year"

if (Test-Path "Day$_day") {
    Write-Error "Directory Day$_day already exists, quitting"
    return
}

Write-Host "Creating Day$_day directory"
$null = New-Item -Name "Day$_day" -ItemType Directory
Push-Location "Day$_day"

Write-Host "Running " -NoNewline
Write-Host "dotnet new console" -ForegroundColor DarkGreen
dotnet new console

Write-Host "Adjusting project name"
$null = Rename-Item -Path "Day$_day.csproj" "$year.$_day.csproj"

Write-Host "Creating placeholder input files"
$null = New-Item -Name "input.txt" -ItemType File
$null = New-Item -Name "input-test.txt" -ItemType File

Pop-Location
Pop-Location

Write-Host "Adding created project to solution"
dotnet sln .\AdventOfCode.sln add --solution-folder $year "$year/Day$_day/$year.$_day.csproj"

.\include-input.ps1