# event-driven-architecture-template

This project is a barebone template based on [Unity Open Project Chop-Chop initiative](https://github.com/UnityTechnologies/open-project-1), already setup for you, so you can start your game with a bunch of utilities already implemented, such as:
- Modular scene workflow using scriptable objects events and addressables to load unload scenes as required
  - Initialization Scene, PersistentManager Scene, Gameplay Scene, Main Menu Scene.
- Save System using Scriptable Objects
- Audio System wrapped with a Scriptable Object
- New Input System with a Scriptable Object Wrapper that can be injected as a dependency to any component that requires it.
- Several Runtime Sets already defined that are handy to provide with the correct object representation at runtime.
- Pool System using Scriptable Objects
- Factory Pattern Implementation (used in the pool system)

For a full list of features and more detailed description, please check the Unity Open Project Wiki [here](https://github.com/UnityTechnologies/open-project-1/wiki).
