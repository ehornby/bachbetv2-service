$version = Get-Content -Path version
$length = $version.length
$end = $version.substring($length-4, 4)
$start = $version.substring(0, $length-4)
$bumped = [int]$end
$bumped++
$bumpedAsString = $bumped.ToString("0000")
$finalVer = ("{0}{1}" -f $start, $bumpedAsString)
Set-Content -Path version -Value $finalVer
Write-Host "Bumped $version --> $finalVer"

Write-Host "[1/4] starting docker build..."
docker build --build-arg ENVIRONMENT=Staging --build-arg VERSION=$finalVer --file Dockerfile -t bachbetv2-service ./src
Write-Host "[2/4] AWS login..."
aws ecr get-login-password --region ca-central-1 | docker login --username AWS --password-stdin 623772476924.dkr.ecr.ca-central-1.amazonaws.com
Write-Host "[3/4] tagging docker image..."
docker tag bachbetv2-service:latest 623772476924.dkr.ecr.ca-central-1.amazonaws.com/bachbetv2:latest
Write-Host "[4/4] pushing docker image..."
docker push 623772476924.dkr.ecr.ca-central-1.amazonaws.com/bachbetv2:latest