# DR Platformer
Simple infinite platformer game.

Requires Unity3D 2019.1.1f1

### Controlls
**Up** - Jump (hold for long jump)
**Space/Left/Right** - Change color
**Esc** - Pause/Resume game
**R** - Restart game

### Game configuration
See (and edit) `GameConfiguration` for gameplay parameters.

### What to improve
* Implement camera movement.
* Add some visual effects (player trail, platform hit, death explosion...).
* Improve changing color implementation. I'm currently changing material color, which is not optimal.
* Add scheduler (custom calls to `Update(float deltaTime)`).
* Add AudioManager to control music and sound effects.
* Configure key mapping in a file (similar to GameConfiguration).

### Discussion
Why is moving the world (platforms) and not the player? Because it is an infinite runner and if some skilled player will run for a long time (far far away), there could occur float number precision issues and therefore unpredictable visual and physics glitches.

The music is original and an intelectual property with copyright.
