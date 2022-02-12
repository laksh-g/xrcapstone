# Week 6 - MVP week! Multiplayer beta, end-game screen, recipe UI, and ticketing system...

## What we have accomplished

- Johnathon: Finished adding sound effects for collisions and cooking, redesigned kitchen, added in-game clock, dialed in cooking times, and helped set up the game manager and object serialization to be used for multiplayer
- Katherine: 
- Laksh: 
- Hritik:

## New features/functionality implemented

- Brand new kitchen! The original kitchen has been redesigned to be smaller and tighter to improve performance and comfort
- New fries! Having individual fries proved too computationally intensive for the Quest, so we've resorted to bundles of fries.
- New clock! See how much time is left by reading the clock over the turn-in zone.
- End-game screen! See your team's score and total covers completed after service is over.
- Real-time feedback! After sending in an order, another ticket is printed telling the head chef how it was.
- Bearnaise sauce! Add sauce to a saucepan then heat it up to get it ready to serve.

## Bug Fixes

- The in-game clock is now synchronized between players
- Steak cooking has been slowed to allow for proper time to sear before reaching cooked temps
- Spawned-in objects now spawn in for all players
- Reduced plated object collisions so plates should freak out less now

## Code review

- GameManager.cs: https://github.com/UWRealityLab/xrcapstone22wi-team5/pull/20
- CollisionSound.cs: https://github.com/UWRealityLab/xrcapstone22wi-team5/pull/18

## Blocking issues
- Multiplayer: Serialized fields record different values for different players.
- 
