# Week 9 - New dishes, item spawners, new render pipeline, and new game options...

## What we have accomplished

- Johnathon: Created and implemented 4 new dishes, added item spawners, added oven appliance, and upgraded render pipeline to URP.
- Katherine:
- Laksh:
- Hritik:

## New features/functionality implemented
- New dishes! Crab cakes, table bread, roasted chicken, and French onion soup have all been added to the game
- URP! We've upgraded the render pipeline to the Universal Render Pipeline for performance and visual improvements
- Oven! A new kitchen appliance featuring cooking timers for chicken and bread
- Fry timer! The knob on the frier now works as a timer for perfect crispy fries
- Item spawners! All foods, plates, and tools now are produced using item spawners to limit the amount of objects in the scene
## Bug Fixes
- Moving between heating elements now registers correctly
- Pans and frying baskets use the plating script now in order to fix issues of items falling through hitboxes

## Code review 

## Blocking issues
- Plating synchronization and serialization of various elements are still the primary blocking issues
- Baked lighting ends up leaving a lot of artifacts in the current scene
