$null = (dotnet --list-sdks | Select-Object -Last 1) -match "^(\d+\.\d+)"
$targetFramework = "net$($matches[1])"

if ($targetFramework -notmatch "^net\d+\.\d+$") {
    Write-Host "Could not determine target framework"
    return
}

Write-Host "Setting target framework to $targetFramework"

Push-Location $PSScriptRoot
Get-ChildItem -Filter "*.csproj" -Recurse | ForEach-Object {
    Push-Location $_.Directory
    if ( $_.Name -ne "AoC.Common.csproj" ) {
        dotnet add reference ..\..\AoC.Common\AoC.Common.csproj
    }
    $project = $_.FullName
    Write-Host "Checking $project"
    [xml] $p = Get-Content $project
    $fw = $p.Project.PropertyGroup.TargetFramework
    if ($fw -eq $targetFramework) {
        Write-Host "Project up-to-date"
        Pop-Location
        return
    }
    Write-Host "`tUpgrading project to $targetFramework"
    $p.Project.PropertyGroup.TargetFramework = $targetFramework
    if (-not $p.Project.PropertyGroup.ImplicitUsings) {
        Write-Host "`tEnabling implicit usings"
        $usings = $p.CreateElement("ImplicitUsings")
        $null = $usings.InnerText = "enable"
        $null = $p.Project.PropertyGroup.AppendChild($usings)
    }
    # Write-Host $p.OuterXml
    $p.Save($project)
    Pop-Location
}
Pop-Location