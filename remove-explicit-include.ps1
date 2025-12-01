foreach ($project in (Get-ChildItem -Recurse -Filter *.csproj)) {
    Write-Host "Processing project file: $($project.FullName)"
    [xml]$xml = Get-Content $project.FullName

    $changed = $false

    $includeNodes = $xml.SelectNodes("//ItemGroup/None")

    foreach ($node in $includeNodes) {
        if ($node.Attributes["Update"].Value -like "*.txt") {
            $node.ParentNode.RemoveChild($node) | Out-Null
            $changed = $true
        }
    }

    if ($changed) {
        $xml.Save($project.FullName)
        Write-Host "Updated project file: $($project.FullName)"
    }
}