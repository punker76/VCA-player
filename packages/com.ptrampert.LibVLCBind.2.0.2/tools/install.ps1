param($installPath, $toolsPath, $package, $project)
Write-Host "Setting CopyToOutputDirectory"
$project.ProjectItems.Item("libvlc.dll").Properties.Item("CopyToOutputDirectory").Value = 2
Write-Host "libvlc.dll"
$project.ProjectItems.Item("libvlccore.dll").Properties.Item("CopyToOutputDirectory").Value = 2
Write-Host "libvlccore.dll"
ForEach ($item in $project.ProjectItems.Item("plugins").ProjectItems)
{
    if ($item.Name -eq "plugins.dat") 
    { 
        $item.Properties.Item("CopyToOutputDirectory").Value = 2 
        Write-Host $item.Name
    }
    else
    {
        ForEach ($item2 in $item.ProjectItems)
        {
            $item2.Properties.Item("CopyToOutputDirectory").Value = 2 
            Write-Host $item2.Name
        }
    }
}