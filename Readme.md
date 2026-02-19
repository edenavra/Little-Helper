# 🦊 The Little Helper

A 2.5D procedural roguelike in Unity where a tiny fox gathers soup ingredients for Grandma. Navigate hazardous environments, dodge dynamic obstacles, and unlock permanent upgrades to survive.

---

## 🔗 Links

- 🕹️ **Play the Game:** [The Little Helper on Itch.io](https://danaeck.itch.io/the-little-helper)
- 📹 **Gameplay video:** [https://www.youtube.com/watch?v=vAzwCNk-x_8]
  
---

## 🎮 Controls

| Action | Key |
| :--- | :--- |
| **Move** | `WASD` / `Arrow Keys` |
| **Search / Interact** | `F` |

---

## ✨ Key Features

- **Procedural Generation:** Dynamic room layouts, randomized containers, and item generation logic ensure high replayability and varied environments in every run.
- **Optimized Enemy Logic:** Aggressive wildlife (Moles, Rats) and environmental hazards (Freezing Rooms) are managed via a custom Object Pooling system (`EnemyPool`) to optimize memory allocation and avoid Garbage Collection spikes during runtime.
- **Event-Driven Architecture:** Core game loops, inventory management, round progression, and UI updates (Health and Currency) are decoupled using global event channels.
- **Roguelike Progression System:** A die-and-retry core loop featuring a persistent upgrade manager where players use collected currency at Grandma's medicine cabinet to enhance stats.
- **State & Animation Syncing:** Complex animation management for player states, precise hitboxes, and responsive 2.5D interactions.

---

## 🧠 Team

- **Programming:** Eden Avrahami, Shir Seroussi, Rami Hubeishi
- **Art & Design:** Dana Eckstein, Omri Benbenisty

---

## 💡 Notes

- Developed in Unity 2D with C#
- Created within the Department of Visual Communication at Bezalel Academy of Arts and Design, in collaboration with The Hebrew University of Jerusalem
