### Game Framework ###
This is an all-encompassing game development framework built in Unity. The framework was developed with extensibility, modularity, and reusability in mind. It provides a strong base for rapid game development and prototyping while remaining adaptable for use in full production.

### Features ###
Game Modes and States: The framework provides an abstract GameMode class to dictate the rules of your game, such as victory conditions, number of players, etc.

## Player Controller and Pawns ##
The PlayerController and Pawn classes make it easy to control characters or vehicles. The PlayerController class interprets user input and commands the Pawn accordingly.

## AI and AI States ##
A powerful AI system has been implemented, utilizing a finite state machine structure to enable a variety of AI behaviors.

## Health and Damage System ## 
The framework provides a robust health and damage system, including a DamageSource class for different types of damage and a HealthComponent class for managing an entity's health.

## Pickup System ## 
Includes a comprehensive pickup system with a Pickable interface, allowing you to make any game object pickable.

## ScriptableObjects Data ## 
Utilizes Unity's ScriptableObject system for storing game data separately from game logic, leading to more maintainable and scalable code.

## HUD and UI System ## 
Contains a base HUD class for managing the in-game heads-up display, displaying real-time information to the player.

### Getting Started ###
Download or Clone the Repository: Clone this repository to your local machine or download the zip.

Explore the Sample Scenes: There are several example scenes showcasing various aspects of the framework. Run these scenes to see the framework in action.

### Documentation ###
Full documentation for each system is available in the respective system's directory. This includes detailed descriptions of classes, methods, interfaces, and usage examples.

### Contributing ###
Contributions, issues, and feature requests are welcome. 