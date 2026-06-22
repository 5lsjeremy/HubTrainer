# push-dev.ps1
param(
    [string]$Message = "update"
)

Write-Host "Adding changes..."
git add .

Write-Host "Showing status..."
git status

Write-Host "Committing..."
git commit -m "$Message"

Write-Host "Pushing to dev..."
git push origin dev

Write-Host "Done."
