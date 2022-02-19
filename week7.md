# Week 7 - Post-MVP, new lobby, bugfixes, playability improvements...

## What we have accomplished

- Johnathon: Organized feedback from MVP testing and distributed bug-fixing work to the team, worked on fixing plating bugs, audio improvements, and new gameplay features for upcoming new dishes.
- Katherine: Started developing a tutorial/tips system. The tutorial system will walk the user through the roles and explain the game mechanics. 
- Laksh: Remade the Lobby scene with a settings menu, create and join room options, pre game lobby menu. A more robust role assigning system with all data being synced over custom room properties and custom player properties.
- Hritik: Started fixing bugs that were reported from user testing and working on adding new features as noted in the MVP bugs section.

## New features/functionality implemented
- Grating! Shred some cheese or truffle over a dish to instantly up the fanciness factor.
- Cooking torch! Great for making creme brulee or for finishing french onion soup...
- More realistic temperature response! Items removed from heat will now continue to cook for a little longer
- Players can now join and create rooms
- New Lobby scene with a kitchen layout
- Players not see buttons grayed out for roles already taken in a room.
- Scenes sync across clients with the master client loading scene transitions.

## Bug Fixes

- Violent plating collisions should be significantly reduced
- Items fall from plate if rotated enough
- Volume lowered in main scene
- Collisions in drawers should be more accurate
- Held objects should be less jittery now
- Lobby scene to kitchen scene transition bugs.
- Role selection and start game sync fixed

## Code review
- Plateable.cs: https://github.com/UWRealityLab/xrcapstone22wi-team5/pull/22
- AssignRoles.cs: https://github.com/UWRealityLab/xrcapstone22wi-team5/pull/21


## Blocking issues
- Looking for solutions to enable the Oculus on-screen keyboard for entering room codes, changing names etc. 

