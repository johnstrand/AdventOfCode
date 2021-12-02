function script:CreateNodes {
    [CmdletBinding()]
    param (
        [Parameter()] [xml] $root,
        [Parameter()] $parent,
        [Parameter()] $tree
    )
    

    if($tree -is [hashtable]) {
        return $tree.keys | ForEach-Object {
            if($_.StartsWith("@")) {
                $attr = $root.CreateAttribute($_.Substring(1))
                $attr.Value = $tree[$_]
                $parent.SetAttributeNode($attr);
                return @()
            }

            $current = $root.CreateElement($_)
            
            $null = (script:CreateNodes $root $current $tree[$_]) | ForEach-Object {
                if($_ -is [System.Xml.XmlAttribute]) {
                    # Skip
                } else {
                    $current.AppendChild($_)
                }
            }
            return $current
        }
    } else {
        $parent.InnerText = $tree
        return @()
    }
}

Get-ChildItem -Recurse -Filter *.csproj | ForEach-Object {
    $target = $_.FullName;
    Write-Host "Checking $target"
    [xml] $x = Get-Content -Path $target

    Get-ChildItem -Filter *.txt -Path $_.DirectoryName | ForEach-Object {
        $file = $_.Name
        Write-Host "`t$file - " -NoNewline
        $current = $x.SelectSingleNode("//None[@Update = '$file']")
        if ($current) {
            Write-Host "exists"
            return
        }
        
        Write-Host "added"

        $n = script:CreateNodes $x $x.Project @{
            ItemGroup = @{
                None = @{
                    "@Update" = $file
                    CopyToOutputDirectory = "Always"
                }
            }
        }

        $null = $x.Project.AppendChild($n)

        $x.Save($target)
    }
}