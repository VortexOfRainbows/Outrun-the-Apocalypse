# OutrunTheApocalypse
 
A finite state machine is not used for animation. Animation is done using transforms (rotation, translation, stretch, etc).

However, there are some finite state machines elsewhere in the code.
For example:
1. Projectiles can be in a finite number of states, which define what type of projectile they are.
	1. Bullet, Fireball, and MortarShell, are all states that a projectile can be in
2. Items can be in a finite number of states in the same way.
	1. NoItem, Pitchfork, Corn, FarmerGun, Minigun, etc. are all states that an item can be in (they are NOT different gameObjects,
	they are different states stored on teh gameObject).
3. These finite states are switched in certain situations. For example, an item's state (ItemData in code) can switch to a different
	state after the player interacts with certain game-objects on the floor (this is the logic that controls picking up items).
	If you have an inventory item of state "NoItem", the state will be switched to "FarmerGun" when you pick up the gun.
	If you drop the inventory item on the floor, your inventory item state will be changed to "NoItem" again.