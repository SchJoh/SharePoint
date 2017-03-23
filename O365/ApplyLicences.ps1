$UserCredential = Get-Credential
Connect-MsolService -Credential $UserCredential
$AccSkuId = " *tenant* :ENTERPRISEPREMIUM"

$Loptions = New-MsolLicenseOptions -AccountSkuId $AccSkuId -DisabledPlans YAMMER_ENTERPRISE


$temp=gc "Workbook.csv"
$array=@()

$temp | Foreach{
    $elements=$_.split(";")
    $array+= ,@($elements[0],$elements[1],$elements[2])
}

$count = 0

foreach($value in $array)
{
    $count += 1
    $user = Get-MsolUser -SearchString $value[0] -MaxResults 2000


    if($user -is [system.array]){
        write-Host " Multiple result for " $value[0] " check and apply licences by hand." -ForegroundColor Red
    }

    if (!$user){
        Write-Host " No user found for " $value[0] -ForegroundColor Red
    }

    if ($user -and ($user -isnot [system.array] )){
        if($user.isLicensed –eq $TRUE){
             Write-Host "User already licenced, check Additional Plans for " $user.DisplayName -ForegroundColor Yellow
            
        } else {
            
            Set-MsolUser -UserPrincipalName $user.UserPrincipalName -UsageLocation DE
            Set-MsolUserLicense -UserPrincipalName $user.UserPrincipalName -AddLicenses $AccSkuId
            Set-MsolUserLicense -UserPrincipalName $user.UserPrincipalName -LicenseOptions $Loptions

            Write-Host "Licence added to " $user.DisplayName -ForegroundColor Green
        }
    }
}

Write-Host $count





