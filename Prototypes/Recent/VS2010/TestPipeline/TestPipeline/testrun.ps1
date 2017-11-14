param([string] $FromHostApp)
# Test run spaces
Write-Output "Executing" "`n"
if ( $FromHostApp -ne $null )
{
    Write-Output "Trimming ""$FromHostApp""" "`n"
    $FromHostApp=$FromHostApp.Trim()
}
else
{
    Write-Output "Parameter is null" "`n"
}
Write-Output "Hello world ""$FromHostApp""" "`n"
if ( $thing -ne $null )
{
    Write-Output "thing parameter is =""$thing""" "`n"
}
else
{
    Write-Output "thing parameter is empty" "`n"
}
Write-Error "An error string"
