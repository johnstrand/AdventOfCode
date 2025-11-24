[CmdletBinding()]
param(
    [string] $Solution = "AdventOfCode.sln",
    [switch] $IncludeTransitive,
    [switch] $PreRelease,
    [switch] $WhatIf
)

if (-not (Test-Path $Solution)) {
    Write-Error "Solution file '$Solution' not found."
    exit 1
}

Write-Host "Ensuring dotnet-outdated global tool is installed" -ForegroundColor Cyan
$toolList = dotnet tool list -g 2>$null
if ($toolList -notmatch "dotnet-outdated") {
    Write-Host "Installing dotnet-outdated-tool" -ForegroundColor Yellow
    dotnet tool install --global dotnet-outdated-tool || exit 1
}
else {
    Write-Host "dotnet-outdated already installed" -ForegroundColor DarkGreen
}

# dotnet outdated expects solution path first; add flags conditionally
$upgradeArgs = @(
    $Solution,
    "--no-restore"
)
if (-not $WhatIf) { $upgradeArgs += "--upgrade" }
if ($IncludeTransitive) { $upgradeArgs += "--transitive" }
if ($PreRelease) { $upgradeArgs += "--include-prerelease" }

Write-Host "Restoring solution before upgrade" -ForegroundColor Cyan
if (dotnet restore $Solution) { }
else { Write-Error "Initial restore failed"; exit 1 }

if ($WhatIf) {
    Write-Host "Running in WhatIf mode (no upgrades will be applied)" -ForegroundColor Yellow
    $upgradeArgs = $upgradeArgs | Where-Object { $_ -ne "--upgrade" }
}

Write-Host "Upgrading NuGet packages" -ForegroundColor Cyan
Write-Host "dotnet outdated $($upgradeArgs -join ' ')" -ForegroundColor DarkGray
if (dotnet outdated @upgradeArgs) {
    Write-Host "Package upgrade command completed" -ForegroundColor DarkGreen
}
else {
    Write-Warning "dotnet outdated reported issues (some packages may not have upgraded). Continuing to build."
}

Write-Host "Restoring solution after upgrade" -ForegroundColor Cyan
if (dotnet restore $Solution) { }
else { Write-Error "Restore after upgrade failed"; exit 2 }

Write-Host "Building solution to verify upgrades" -ForegroundColor Cyan
if (dotnet build $Solution -warnaserror) {
    Write-Host "Build succeeded after package upgrades" -ForegroundColor Green
    exit 0
}
else {
    Write-Error "Build failed after package upgrades" -ForegroundColor Red
    exit 3
}
