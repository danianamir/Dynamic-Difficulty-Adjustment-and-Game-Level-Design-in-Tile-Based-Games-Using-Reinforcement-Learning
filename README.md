# Dynamic-Difficulty-Adjustment-and-Game-Level-Design-in-Tile-Based-Games-Using-Reinforcement-Learning


# Reinforcement Learning for Dynamic Game Level Generation and Difficulty Adjustment

## Overview

In this project, we utilize reinforcement learning (RL) to train an agent capable of generating game levels and dynamically adjusting their difficulty based on the player's experience. The goal is to create a balanced and engaging gameplay experience that evolves with the player’s skill level.

## Game Description

The game environment used in this task is a 2D tile-based game where the player's objective is to collect as many treasures as possible within a limited time. The player must navigate through the level, avoid enemies, and find paths to each treasure to maximize their score.

### Gameplay Mechanics

- **Player Objective**: Collect the maximum amount of treasure within a set time limit.
- **Challenges**: The player must avoid enemies strategically placed within the level and find valid paths to reach each treasure.

## Level Generation Constraints

The RL agent is tasked with generating game levels that meet the following constraints:

- **Treasure Quantity**: The number of treasures must be between 5 and 15.
- **Items Quantity**: The total number of items (including enemies, power-ups, etc.) should be between 30 and 40.
- **Enemy Distance**: Each enemy must be positioned at least one tile away from the player to ensure fairness.
- **Player Presence**: Exactly one player must be present in the level.
- **All Items**: All specified types of items must be included in the level.
- **Path to Treasures**: There must be a valid path from the player's starting position to each treasure, ensuring that the level is playable.

## Dynamic Difficulty Adjustment

The project also focuses on dynamically adjusting the difficulty of the generated levels based on the player’s performance. The goal is to maintain an ideal balance where the game is challenging but not overly difficult.

### Difficulty Metric

To quantify and adjust the difficulty, we use the following metric:
- **Difficulty Score**: \( \text{Difficulty} = \left(\frac{\text{Gained Treasure}}{\text{Total Treasure}}\right) - \frac{1}{2} \)

This score should be as close to 0 as possible, indicating that the level is balanced. A score close to 0 means the player is neither overwhelmed nor under-challenged.

## Project Structure

The project is divided into two main sections:

### 1. Unity Side

This section contains the game environment where the agent generates levels. The Unity side includes:
- **Prefabs**: All game items (e.g., player, treasures, enemies) are stored as prefabs in the `Assets/Prefabs` folder.
- **Scripts**: The game logic, including player movement, enemy behavior, and other game mechanics, is implemented in the `Assets/Scripts` folder.
- **Main Scene**: The primary game scene is located in the `Assets/Scenes` folder.

The Unity project can be built on a server-side platform or Windows and then used as an environment in RLlib to train the agent.

### 2. RLlib Side

The reinforcement learning component is implemented using RLlib and is located in the `rllib_code` folder. This section includes:

- **Environment Connection**: The Unity environment is connected to RLlib via the `env` section.
- **Training**: The agent is trained using various configurations, such as action and observation spaces, and the PPO (Proximal Policy Optimization) algorithm settings.
- **Testing**: After the training phase, the algorithm is tested within the Unity environment to assess its performance and ensure that the generated levels meet the specified constraints and difficulty adjustment criteria.

## Conclusion

This project demonstrates how reinforcement learning can be applied to game level generation and difficulty adjustment, creating a dynamic and adaptive gaming experience. By training an agent to generate levels that meet specific constraints and adjusting the difficulty based on player performance, we aim to enhance the overall engagement and challenge of the game.

