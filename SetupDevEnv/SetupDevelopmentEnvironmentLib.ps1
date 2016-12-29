###### SetupDevelopmentEnvironment.ps1 ######
Import-Module WebAdministration

# Defaults
$hostName       = "site.local"
$appPoolName    = $hostName + "AppPool"
$appPoolUser    = $null
$appPoolPass    = $null
$websiteFolderName = $null
$frameworkVersion  = "v4.0"
[string[]] $additionalHostNames = "scripts", "images", "styles"
$executingPath = $MyInvocation.MyCommand.Path;


Function AdjustPath
{
    if(($executingPath.Contains('SetupDevEnv') -eq $True) -and ($websiteFolderName -notlike $null))
    {
        write-host "true"
        write-host $websiteFolderName
        write-host $executingPath
        $executingPath = $executingPath.Replace("SetupDevEnv",$websiteFolderName)
    }
    else
    {
    write-host "false"
    }
}
# Adds the $hostName and any $additionalHostNames to hosts file
Function AddToHostsFile
{
    $file = "C:\Windows\System32\drivers\etc\hosts"
    $ip = "127.0.0.1"
    $bindings = $hostName

    # Check for additional bindings, add them if existing.
    if ($additionalHostNames -gt 0)
    {
        for ($i = 0; $i -le $additionalHostNames.Length - 1; $i++) {
            $binding = $additionalHostNames[$i] + "." + $hostName
            $bindings = $bindings + " " + $binding
        }
    }

    "`n" + $ip + "`t" + $bindings | Out-File -encoding ASCII -append $file
}

# Creates a new website in IIS with application pool.
Function CreateIISWebsite
{
    Write-Host "Creating $hostName site."


    # Find the root directory of the site, look for a web.config file.
    
    $directory = Split-Path -parent  $executingPath
    if($websiteFolderName -notlike $null)
    {
        $directory = Split-Path -parent  $directory
        $directory = $directory + "\" + $websiteFolderName
    }

    while ($directory -ne $null -and $directory.length -gt 0)
    {
        $webConfig = Get-ChildItem $directory -filter web.config
        
        if ($webConfig -ne $null) {
            $physicalPath = $directory
             Write-Host "Found web.config in $physicalPath"
            break
        }
        $directory = Split-Path -Parent $directory
    }
    if($physicalPath -eq $null)
    {
        write-host "Could not find the website folder"
        return
    }
    # Create the application pool
    $appPool = New-WebAppPool -Name $appPoolName
    $appPool.managedRuntimeVersion = $frameworkVersion

    # Create the website
    $webSite = New-WebSite -Name $hostName -HostHeader $hostName -PhysicalPath $physicalPath -ApplicationPool $appPoolName

    Write-Host "Site created."

    # Set application pool user
    if ($appPoolUser -ne $null)
    {
        $appPool.processModel.username = $appPoolUser
        $appPool.processModel.password = $appPoolPass
        $appPool.processModel.identityType = 3 # eq "SpecificUser"

        Write-Host "Set application pool identity to $appPoolUser."
    }
    $appPool | Set-Item
}

# Adds any additional bindings to the IIS website
Function AddAdditionalBindings
{
    Write-Host "Adding additional bindings to $hostName."

    # Iterate the additionalHostNames and add them as bindings.
    for ($i = 0; $i -le $additionalHostNames.Length - 1; $i++) {
        $binding = $additionalHostNames[$i] + "." + $hostName
        Add-WebConfiguration -filter "/system.applicationHost/sites/site[@name=`"$hostName`"]/bindings" -PSPath IIS:\ -value (@{protocol="http";bindingInformation="*:80:$binding"})
    }

}

# Used to clean up IIS. (mainly for testing)
Function RemoveIISWebsite
{
    Write-Host "Deleting $hostName site."

    $webSite = Get-Website -Name $hostName

    if ($webSite -eq $null)
    {
        Write-Host "No such website ($hostName)."
    }
    else {
        Remove-Website -Name $hostName
        Remove-WebAppPool -Name $appPoolName
    }
}


# Used to clean up IIS. (mainly for testing)
Function openWebsite
{
  start "http://$hostName"
  #start "http://$hostName/umbraco"
}
