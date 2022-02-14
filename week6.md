# Week 6 - MVP week! Multiplayer beta, end-game screen, recipe UI, and ticketing system...

## What we have accomplished

- Johnathon: Finished adding sound effects for collisions and cooking, redesigned kitchen, added in-game clock, dialed in cooking times, and helped set up the game manager and object serialization to be used for multiplayer
- Katherine: Finished object information UI with added toggle feature, created recipe posters for players to follow.
- Laksh: Finished adding rooms and network features, added XR grab interactables for network and back to menu features. Helped serializing data to be synced over the network. Implemented end game screen and scores display.
- Hritik: Finished creating the ticketing system and incorporating it with the main application. Also did testing with other memebers to check the current app and do bug fixing.

## New features/functionality implemented

- Brand new kitchen! The original kitchen has been redesigned to be smaller and tighter to improve performance and comfort
- New fries! Having individual fries proved too computationally intensive for the Quest, so we've resorted to bundles of fries.
- New clock! See how much time is left by reading the clock over the turn-in zone.
- End-game screen! See your team's score and total covers completed after service is over.
- Real-time feedback! After sending in an order, another ticket is printed telling the head chef how it was.
- Bearnaise sauce! Add sauce to a saucepan then heat it up to get it ready to serve.
- XR Grab interactables now sync over the network
- Players teleport to their respective stations based on their roles.
- Object characteristics (like temp, amount of salt etc.) update over the network.
- End game screen where head chef can view the final score for the game.
- Object information ui can be toggled.
- Recipe posters provide players cooking instructions.

## Bug Fixes

- The in-game clock is now synchronized between players
- Steak cooking has been slowed to allow for proper time to sear before reaching cooked temps
- Spawned-in objects now spawn in for all players
- Reduced plated object collisions so plates should freak out less now
- Fixed bug with XR grab interactables not syncing over network
- Fixed serializing datafields
- Fixed network voice bug.

## Code review

- GameManager.cs: https://github.com/UWRealityLab/xrcapstone22wi-team5/pull/20
- CollisionSound.cs: https://github.com/UWRealityLab/xrcapstone22wi-team5/pull/18
- NetworkPlayerSpawner.cs https://github.com/UWRealityLab/xrcapstone22wi-team5/pull/16
- TriggerCanvas.cs https://github.com/UWRealityLab/xrcapstone22wi-team5/pull/11

## Blocking issues
- Multiplayer: Serialized fields record different values for different players.
- Multiplayer: Master client always gets assigned to the first player to join the room.
