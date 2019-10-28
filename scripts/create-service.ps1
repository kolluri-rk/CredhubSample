$serviceName = "credhub"
$servicePlan = "default"
$serviceInstanceName = "test-credhub-service"

$sqlDbUser = "shareWriteUser"
$password = "thisIs1Pass!"

$serviceTags = [string]::Format('{0},test-sql-creds',$serviceInstanceName) #comma delimited
$credsParamJSON = [string]::Format('{{\"share-username\":\"{0}\",\"share-password\":\"{1}\"}}',$sqlDbUser,$password)

#Create the service instance
cf create-service $serviceName $servicePlan $serviceInstanceName -c $credsParamJSON -t $serviceTags
