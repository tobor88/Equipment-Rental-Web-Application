# This should be set up as a task in Windows Task Scheduler. 

$FromIT = 'alert@osbornepro.com'
$SmtpServer = 'mail.smtp2go.com'
$SmtpPort = '2525'
$TodaysDate = (Get-Date).ToShortDateString()

$Collection = Invoke-Sqlcmd -Query "USE SQLDBName; SELECT RenterName,BeginRental,EndRental,Info FROM dbo.Rentals" -ServerInstance "SqlServer\SQLDB_Name"

ForEach ($Rental in $Collection) 
{

    $Renter = $Rental.RenterName

    $MoreInfo = $Rental.Info

    $BeginRentals = ($Rental.BeginRental).ToShortDateString() 

    $EndRentals = ($Rental.EndRental).ToShortDateString()

    If ($BeginRentals -like $TodaysDate) 
    {


        $MailBody1 = "Equipment rental goes out today. Ensure the device is ready for pick up. `n`nRENTER: $Renter `n`nREASONS: $MoreInfo"

        Send-MailMessage -From $FromIT -To $FromIt -Subject "Equipment Rental Goes Out Today" -Body $MailBody1 -SmtpServer $SmtpServer -UseSsl -Port $SmtpPort -Priority High


        $MailBody2 = "Your equipment rental begins today. Stop by the IT office later today to pick up your device. Thank you!.`n`nRENTER: $Renter `n`nREASONS: $MoreInfo"

        $RenterEmail = $Renter.Replace(" ",".") + "@$env:USERDNSDOMAIN"

        Send-MailMessage -From $FromIt -To $RenterEmail -Subject "Equipment Rental Today" -Body $MailBody2 -SmtpServer $SmtpServer -UseSsl -Port $SmtpPort -Priority High

    } # End If
    ElseIf ($EndRentals -like $TodaysDate) 
    {


        $MailBody3 = "Equipment Rental Due Back Today.`n`nRENTER: $Renter `n`nREASONS: $MoreInfo"

        Send-MailMessage -From $FromIT -To $FromIt -Subject "Equipment Rental Overdue" -Body $MailBody3 -SmtpServer $SmtpServer -UseSsl -Port $SmtpPort -Priority High
       

        $MailBody4 = "Your equipment rental is due back today. Stop by the IT office later today to return your device or let IT know you will need the device a little longer. Thank you!.`n`nRENTER: $Renter `n`nREASONS: $MoreInfo"

        $RenterEmail = $Renter.Replace(" ",".") + "@$env:USERDNSDOMAIN"

        Send-MailMessage -From $FromIt -To $RenterEmail -Subject "Equipment Rental Today" -Body $MailBody4 -SmtpServer $SmtpServer -UseSsl -Port $SmtpPort -Priority High


    } # End ElseIf
    Else 
    {

         Write-Verbose "Completed sending email alerts."

    } # End Else

} # End ForEach
