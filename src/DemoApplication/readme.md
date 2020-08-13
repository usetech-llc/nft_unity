## How to run this demo

1. Download and install [the Unity Hub](https://unity3d.com/ru/get-unity/download)
2. Clone or download this repository.
3. Open the Unity Hub and click "add" in it's projects tab then select the repository's "DemoApplication" folder location.
4. Click on the DemoApplication project to open it.
5. Open File/Build Settings... to select target platform and build settings. Make sure you have platform module installed in Unity Hub installs tab. Click "Build and Run" button.

## Project structure

#### Data
Contains data structures used by this demo.

#### Libs
Exterinal libraries used by this demo. It includes nft client library from this repository. If you want to update it init or update git submodules.
Move to the repository's root then run commands `git submodule init` + `git submodule update` to init or `git submodule update --rebase --remote` to update.
To build all dependencies use `dotnet publish .\src\NftUnity\NftUnity.csproj` command, it will put all dependencies in the `/src/NftUnity/bin/Debug/netstandard2.0/publish/` folder.

#### Scenes
Scenes. Login to set connection and account settings. CollecionList to manage collections.

#### Templates
Templates for reusable UI elements like items in a list.

#### Validators
User input validation utility.
