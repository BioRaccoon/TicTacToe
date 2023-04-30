Copy-Item -Path 'Assets\Scenes\*.unity' -Destination 'Assets\'
Remove-Item -Path 'Assets\Scenes\*.unity'
Move-Item -Path 'Assets\*.unity' -Destination 'Assets\Scenes\'
.\Assets\Scenes\*.unity