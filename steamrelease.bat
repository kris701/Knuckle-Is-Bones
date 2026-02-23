@echo off

echo "Building Windows Version"
dotnet publish .\Knuckle.Is.Bones.OpenGL\Knuckle.Is.Bones.OpenGL.csproj --configuration Release /p:PublishProfile="windows.pubxml"

echo "Building Linux Version"
dotnet publish .\Knuckle.Is.Bones.OpenGL\Knuckle.Is.Bones.OpenGL.csproj --configuration Release /p:PublishProfile="linux.pubxml"

D:/steam-sdk/tools/ContentBuilder/builder/steamcmd.exe +login "kris701a" "password" +run_app_build ..\Scripts\app_4456210.vdf +quit

Delete-Item D:/tmp