Get-ChildItem -Filter "*.csproj" -Recurse | ForEach-Object {
    $project = $_.FullName
    Write-Host "Checking $project"
    [xml] $p = Get-Content $project
    $fw = $p.Project.PropertyGroup.TargetFramework
    if ($fw -eq "net6.0") {
        Write-Host "Project up-to-date"
        return
    }
    Write-Host "Upgrading project"
    $p.Project.PropertyGroup.TargetFramework = "net6.0"
    $usings = $p.CreateElement("ImplicitUsings")
    $null = $usings.InnerText = "enable"
    $null = $p.Project.PropertyGroup.AppendChild($usings)
    Write-Host $p.OuterXml
    $p.Save($project)
}