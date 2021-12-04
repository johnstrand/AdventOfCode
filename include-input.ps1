function script:CreateNodes {
    [CmdletBinding()]
    param (
        [Parameter()] [xml] $root,
        [Parameter()] $parent,
        [Parameter()] $tree
    )
    

    if ($tree -is [hashtable]) {
        return $tree.keys | ForEach-Object {
            if ($_.StartsWith("@")) {
                $attr = $root.CreateAttribute($_.Substring(1))
                $attr.Value = $tree[$_]
                $parent.SetAttributeNode($attr);
                return @()
            }

            $current = $root.CreateElement($_)
            
            $null = (script:CreateNodes $root $current $tree[$_]) | ForEach-Object {
                if ($_ -is [System.Xml.XmlAttribute]) {
                    # Skip
                }
                else {
                    $current.AppendChild($_)
                }
            }
            return $current
        }
    }
    else {
        $parent.InnerText = $tree
        return @()
    }
}

$projectList = Get-ChildItem -Recurse -Filter *.csproj
$index = 0
$max = $projectList.Length
$found = 0
$added = 0
$projectList | ForEach-Object {
    $target = $_.FullName
    $index = $index + 1
    $pct = [int](($index / $max) * 100)

    Write-Host "$target ($pct %)  `r" -NoNewline

    [xml] $x = Get-Content -Path $target

    Get-ChildItem -Filter *.txt -Path $_.DirectoryName | ForEach-Object {
        $found = $found + 1
        $file = $_.Name
        $current = $x.SelectSingleNode("//None[@Update = '$file']")
        if (-not $current) {
            $added = $added + 1
            $n = script:CreateNodes $x $x.Project @{
                ItemGroup = @{
                    None = @{
                        "@Update"             = $file
                        CopyToOutputDirectory = "Always"
                    }
                }
            }

            $null = $x.Project.AppendChild($n)

            $null = $x.Save($target)
        }
    }
}
$c = "`e[33;1m"
$r = "`e[0m"
Write-Host ""
Write-Host "Scanned $($c)$max$r projects"
Write-Host "`tFound $c$($found.ToString().PadLeft(3, ' '))$r txt files"
Write-Host "`tAdded $c$($added.ToString().PadLeft(3, ' '))$r txt files"
