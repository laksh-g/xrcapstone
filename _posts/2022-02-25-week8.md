# Week 8 - Tutorial scene, gameplay improvements, ticket rail, and game lobby improvements...

## What we have accomplished

- Johnathon: Created a roadmap for the rest of the quarter, laid groundwork for new french onion soup dish, implemented plating improvements, and worked on fixing more gameplay bugs from the MVP demo.
- Katherine: Created a tutorial scene that highlighted objects and led players through the steps of cooking steak frites.
- Laksh: Improved the lobby scene and added new create and join lobby features, Added new end game scene. Added tutorial screen. 
- Hritik: Fixed ticketing bugs, added the new ticket rail that allows users to easily manage tickets, and worked on other bugs/testing for user testing activity.

## New features/functionality implemented
- Soup bowls! These will be handy for one of our upcoming dishes.
- Gruyere cheese! You can shred and melt this stuff. Great for a certain type of soup...
- New steak appearance! We thought that the hyperrealistic appearance seemed odd for the low-poly style, so we gave steaks a new look.
- Tutorial scene for steak frites.
- New create and Join lobby menus. 
- New end game scene

## Bug Fixes
- Plating has been significantly improved, however some bugs regarding turn in are still present with the new system
- Feedback has been made a little more concise for each dish
- Seasoning while on heating elements and plates have been fixed
- Grating now works correctly
- Fixed bugs with room creation 
- Fixed end game scene

## Code review 
- Tutorial.cs

## Blocking issues
- Plating synchronization not persisting between users, causing bugs for turn in. We may have to serialize this somehow.

