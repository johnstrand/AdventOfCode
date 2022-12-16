$targetFramework = "net7.0"

Get-ChildItem -Filter "*.csproj" -Recurse | ForEach-Object {
    $project = $_.FullName
    Write-Host "Checking $project"
    [xml] $p = Get-Content $project
    $fw = $p.Project.PropertyGroup.TargetFramework
    if ($fw -eq $targetFramework) {
        Write-Host "Project up-to-date"
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
}