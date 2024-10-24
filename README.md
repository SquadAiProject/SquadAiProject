# Squad AI
By Christophe Huang and Maxime Leguevacques.  
  
  
# Run the project and Play
  
Clone the project.  
Open in Unity with version 2022.3.47f1  
Click Play Button.  
  
  
## Gameplay :
There are 3 Spawners located within the map that spawn Enemies. Find them and shoot at them to destroy them before the 
whole map gets invaded with Enemies. Beware of the few Turrets near the Spawners which will target you from afar with a 
high shooting rate.  
*Here is a Turret :*
![Turret](./Screenshots/Turret.png)
  
Your Guardians, the agents with the big shields, will protect you from enemy bullets.  
Your Attackers, the white agents, can be placed strategically and shoot constantly in a single direction.  
Your Healers, the green agents, will heal the entire squad if needed.  

## Controls :
- ESC to quit.
- WASD to move.
- Right Click to make the Ally Attackers perform a backup shoot at the target’s position.
- Left Click to shoot where the target is at.
- F to change formation.
- C to call back Ally Attackers.
- G to call back Guardians. This action will cost the player some life.
  
  
# AI Decision System
The chosen AI decision system is the **Behavior Tree** because we thought it was important to learn this skill since it 
is the most used decision system for AI in video games currently. But it is also a very flexible system which would 
allow us to easily create different behaviors for the different situations that can occur described in the subject (heal 
player, protect him, and shooting).  
  
The behavior tree consists of **nodes** linked together. These nodes either control the **flow** of the behavior tree 
execution or are **tasks** that the agent will do. The 2 different flow nodes are the **selector**, who selects which 
task will be executed depending on the return state of the following task, and the **sequence** who runs a series of 
tasks. When nodes are **evaluated** (when they are runned in the behavior tree), they will always return one of the 
three values : **RUNNING**, **SUCCESS**, **FAILURE**.At its **root** (first node of the tree), the behavior tree has 
public variables but has also another variable storage system : the **blackboard**. The blackboard can create, store and 
manage dynamic variables which will be heavily used to control the flow of the behavior tree, thus defining the agent’s 
behavior. It is a Dictionary containing the key to access a specific variable and the value of the variable.  
  
Here is an example of the Enemy Behavior Tree :
![BT_Enemy](./Screenshots/BT_Enemy.png)
  
  
# Agents
  
## Ally Agents
### Guardian
#### Overview
The Guardian type ally will always follow the player closely. If the player's health falls below a certain threshold, 
it will automatically engage in defense by immediately positioning itself in front of the player to block incoming 
attacks, with its shield capable of deflecting enemy bullets.
  
#### Behavior Tree
- SELECTOR (root)
  - SEQUENCE
    - LookAtMousePos
    - Attack
    - NPCFollow
  - SEQUENCE
    - Defend
  
### Attacker
#### Overview
The Attacker type ally will automatically attack based on the player's commands. When the player right-clicks, 
the Attacker will continue firing in the direction of the last attack. Pressing C will recall them to the player's side.
  
#### Behavior Tree
- SELECTOR (root)
  - SEQUENCE
    - LookAtMousePos
    - Attack
    - NPCFollow
  - SEQUENCE
    - AutoAttack
  
  
### Healer
#### Overview
The Healer type ally will stay near the player and automatically heal when the player or other allies lose health.

#### Behavior Tree
- SEQUENCE (root)
  - LookAtMousePos
  - Attack
  - Heal
  - NPCFollow
    
    
## Enemy Agents
### Enemy
#### Overview
The Enemy agent roams around randomly on the level by creating a **wanderPoint** a certain distance away from him. 
However, if an ally enters his **sightRadius**, the Enemy agent will pursue it until the ally is in the ally is in 
**shootingRange**. In that case, the Enemy agent will fire.
#### Behavior Tree
- SELECTOR (root)
  - SEQUENCE
    - IsAllyInSightRange
    - SELECTOR
      - SEQUENCE
        - IsAllyInShootingRange
        - Shoot
      - GoToTargetShootPoint
  - SELECTOR
    - SEQUENCE
      - IsAtWanderLocation
      - SetWanderLocation
    - GoToWanderPoint
  
  
### Turret
#### Overview
The Turret agent does not move around the map. It has a higher **sightRadius** than the Enemy agent so can detect allies 
at greater distance. Once an ally is spotted, the Turret agent instantly shoots towards it with a higher shooting speed 
and accuracy than the Enemy agent.
  
#### Behavior Tree
- SEQUENCE (root)
  - IsAllyInRange
  - Shoot
  
  
# References
Behavior Tree implementation in Unity :  
https://www.youtube.com/watch?v=aR6wt5BlE-E&t=1052s
