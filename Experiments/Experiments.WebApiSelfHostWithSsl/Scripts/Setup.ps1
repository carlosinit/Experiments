$x = New-SelfSignedCertificate -DnsName localhost -CertStoreLocation Cert:\LocalMachine\My
$guid = [guid]::NewGuid()
$certHash = $x.Thumbprint
$ip = "0.0.0.0" # This means all IP addresses
$port = "9443" # the default HTTPS port
"http add sslcert ipport=$($ip):$port certhash=$certHash appid={$guid}" | netsh


"http add urlacl url=https://+:9443/ user=""[ENTER USERNAME WITH DOMAIN HERE]""" | netsh