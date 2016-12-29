
# Import SetupDevelopmentEnvironment script
. .\SetupDevelopmentEnvironmentLib.ps1

# Modify variables below

# Hostname for the site (default binding)
$hostName = "GAReporting.local"

# Application pool name
$appPoolName = 'GAReporting'

# Application pool user
$appPoolUser = $null #'VPPWeb01'

# Application pool password
$appPoolPass = $null #'mec4Adra'

# Will be prepended to $hostName (i.e scripts.$hostName etc.)
# Setting this to null will not create any additional bindings for the site.
$additionalHostNames =  $null #"images", "ui"

$websiteFolderName = 'GAReporting.Web'

# Remove existing website from IIS
RemoveIISWebsite

# Create a new website in IIS
CreateIISWebSite

# Add additional bindings
AddAdditionalBindings

# Add the site to hosts file
AddToHostsFile

# Opens website in default browser
#openWebsite
