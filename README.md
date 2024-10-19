[![Roy Theunissen](Documentation~/Github%20Header.jpg)](http://roytheunissen.com)
[![License: MIT](https://img.shields.io/badge/License-MIT-brightgreen.svg)](LICENSE.md)
![GitHub Follow](https://img.shields.io/github/followers/RoyTheunissen?label=RoyTheunissen&style=social) ![Twitter](https://img.shields.io/twitter/follow/Roy_Theunissen?style=social)

_Do you want to be able to view GBuffers in the scene view like you used to in BRP? Now you can!_

## About the Project

`URP Debug Draw Modes` uses Unity 6's new Render Graph framework to add various useful debug draw modes for viewing things like Albedo, Normals, the Depth Buffer, or even raw Gbuffer contents (useful when customizing the gbuffer layout).

It also serves as a useful starting point for making your own debug draw modes for your project-specific debugging needs. Especially useful if you're using a customized gbuffer layout.

Or if you miss BRP's debug draw modes. It's okay, I miss them too. The Rendering Debugger is a decent substitute but it isn't quite as convenient.

![URP Debug Draw Modes](https://github.com/user-attachments/assets/b9bdaab2-f245-43b3-be5e-f2da4aae3a82)

## Getting Started

- The first time opening the project the debug draw modes config might not load correctly. It's unclear why. If this happens it will tell you so, and it should work again once you restart.
- After that, it should just work out of the box. New debug draw modes will appear in the Debug Draw Modes dropdown under `Deferred`![image](https://github.com/user-attachments/assets/9f19fdfe-82a1-4a7b-8e89-ddfdd44c7adb)
- If you want to use the advanced debug draw modes, for example to view the unfiltered Gbuffers, head to `Edit > Preferences... > URP Debug Draw Modes > Active Categories` and enable the `Gbuffer` category![image](https://github.com/user-attachments/assets/edefc99d-bf15-4705-9111-88b1d599a5db)

- If you want to add your own debug draw modes, you can do so via the Custom Debug Draw Modes config. It's recommended to check out this package as a submodule in your `Packages` folder so it's easy to branch off / fork this repository and customize it to your liking. ![image](https://github.com/user-attachments/assets/cfa15641-06d9-46ce-8949-c0e59ceca7ed)



## Compatibility

- This package depends on the Render Graph framework which was introduced in Unity 6, so this package works for Unity 6 and above.
- This package focuses on visualizing Gbuffer contents so it expects that you're using the **_Deferred rendering path_** for your project.
- The Render Graph framework is brand new and so is this package, so things might go wrong. If they do, feel free to let me know and I will do what I can to help fix the issue.

## Known Issues
- Config fails to load on first time startup. Unclear why. For now I've added a log message to inform users and prevent exceptions.
- Enabling the debug draw modes for the optional buffers can throw an exception if that buffer is not in use (depends on your project settings whether it is used or not)
- Re-enabling the last used debug draw mode by pressing the bug shaped button is bugged. This seems to be a Unity bug, from what I can tell it expects the last used debug draw mode to be a built-in draw mode, but these days they can also be user-made debug draw modes so it may not return a built-in draw mode and throw an exception. I recommend always using the dropdown instead.
- If you have custom user debug draw modes specified in some way other than this package, they will probably not show up. Unity's functionality for managing debug draw modes is very limited, it's necessary to clear and re-add them sometimes. You should try and move everything into one system (for example, this one).

## Feature Wishlist
- Perhaps support multiple configs? That way it's easier to add project-specific configs without having to modify the contents of the package itself.

## Installation

### Package Manager

Go to `Edit > Project Settings > Package Manager`. Under 'Scoped Registries' make sure there is an OpenUPM entry.

If you don't have one: click the `+` button and enter the following values:

- Name: `OpenUPM` <br />
- URL: `https://package.openupm.com` <br />

Then under 'Scope(s)' press the `+` button and add `com.roytheunissen`.

It should look something like this: <br />
![image](https://user-images.githubusercontent.com/3997055/185363839-37b3bb3d-f70c-4dbd-b30d-cc8a93b592bb.png)

<br />
All of my packages will now be available to you in the Package Manager in the 'My Registries' section and can be installed from there.
<br />


### Git Submodule

You can check out this repository as a submodule into your project's Packages folder. This is recommended if you intend to contribute to the repository yourself.

_**NOTE**: If you do, it's best to name the folder `com.roytheunissen.urp-debug-draw-modes` to avoid a Unity warning about the folder name not matching the package name._

### OpenUPM
The package is available on the [openupm registry](https://openupm.com). It's recommended to install it via [openupm-cli](https://github.com/openupm/openupm-cli).

```
openupm add com.roytheunissen.urp-debug-draw-modes
```

### Manifest
You can also install via git URL by adding this entry in your **manifest.json**

```
"com.roytheunissen.urp-debug-draw-modes": "https://github.com/RoyTheunissen/URP-Debug-Draw-Modes.git"
```

### Unity Package Manager
```
from Window->Package Manager, click on the + sign and Add from git: https://github.com/RoyTheunissen/URP-Debug-Draw-Modes.git
```


## Contact
[Roy Theunissen](https://roytheunissen.com)

[roy.theunissen@live.nl](mailto:roy.theunissen@live.nl)
