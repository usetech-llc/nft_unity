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
External libraries used by this demo. It includes nft client library from this repository. If you want to update it init or update git submodules.
Move to the repository's root then run commands `git submodule init` + `git submodule update` to init or `git submodule update --rebase --remote` to update.
To build all dependencies use `dotnet publish .\src\NftUnity\NftUnity.csproj` command, it will put all dependencies in the `/src/NftUnity/bin/Debug/netstandard2.0/publish/` folder.

#### Scenes
Scenes. Login to set connection and account settings. CollecionList to manage collections.

#### Templates
Templates for reusable UI elements like items in a list.

#### Validators
User input validation utility.

## How To Use DemoApplication

#### Login Screen

1. Input your node uri. There is some dns resolving troubles with websocket-sharp on Unity. Notably it resolves the `localhost` using ipv6 if available, so using 127.0.0.1 is preferable.
2. Input private key in hexadecimal format e.g. 0xC0DE. This key will be used to sign transactions. You can get the key by cloning [our fork of substrate]( https://github.com/usetech-llc/substrate) and running bin/utils/subkey/ project using `cargo run -- -s generate`. Please note that that the demo stores private key unprotected.
3. Input account address. This account will be used to sign transaction and to query balance. You can get account using [substrate web application](https://polkadot.js.org/apps/#/accounts).
4. Press Login button to continue to Collections List.

#### Collections List Screen

1. Use "+ New Collection" to create a new collection. Input collection's name and description. Then design custom data structure of tokens by adding fields, naming them and specifying their size. Clicking on "+ Add" button will add a field. All new collections will automatically be added into the list, even if they were added by other means e.g. [substrate web application](https://polkadot.js.org/apps/#/).

2. You can mint new tokens by pressing "+ Mint". Fill all the fields with integer values and press "Mint". Minting a token takes about 10 seconds, it will be added to token list once minted.
