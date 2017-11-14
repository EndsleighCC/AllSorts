param([string] $FromHostApp)
# Test run spaces
Write-Output "Executing" "`n"
if ( $FromHostApp -ne $null )
{
    Write-Output "Trimming" "`n"
    $FromHostApp=$FromHostApp.Trim()
}
else
{
    Write-Output "Parameter is null" "`n"
}
Write-Output "Hello world ""$FromHostApp""" "`n"
