# 💻 2D Topdown RPG Framework
**Robust, Modular 2D Framework Built in Unity:** As my first major project in Unity, this project serves as a foundational template designed to be scalable, designer-friendly, modular, and flexible. Focusing on clean architecture, S.O.L.I.D principles, popular design patterns, and decoupled systems.

---
## 🕹️ Preview

https://github.com/user-attachments/assets/3ace74ab-6fe7-47bf-9c30-5bf5f263d1f0

---



## 🎯 Project Vision
This serves as a **Core Systems Framework**. I developed this to be a reusable template where gameplay mechanics are decoupled from core logic, allowing for rapid iteration, high extensibility, and clean maintenance across multiple projects.
It will also serve as a documentation of my growth and journey as a developer, specifically focusing on these **Core Pillars.**

### Core Pillars:
* **Systems-First Architecture:** Priority is placed on modular flexibility, easy additions, and extensions rather than full on features. Every system is decoupled, allowing you to swap components without breaking the project.
* **Designer-Centric Design:** Systems are exposed via the Unity Inspector with serialized variables, allowing non-coders to tweak gameplay feel without touching the source code.
* **Professional Workflow:** Establishing good practices following Industry Standards for technical documentations, naming conventions, clean code, and GIT commits.
* **Quality Systems'** Internationally choosing to build refined and clean systems over many low quality, unfinished features.
---

## 🛠️ Technical Implementation
Brief highlights of how I applied advanced C#\Unity concepts to real-world game development challenges in the project.

### Architectural Highlights:
* **S.O.L.I.D. Compliance:** Ensuring every class has a single responsibility. Systems are designed to be open for extension but closed for modification.
* **Decoupled Communication:** Extensive use of Delegates, Events, and the **Observer Pattern** to ensure systems (like UI and Combat) never "know" about each other directly.
* **Interfac & Abstract class-Driven Design:** Using interfaces and abstract classes to create flexible, swappable components (e.g., `IDamageable`, `IInteractable`).
* **Abstractions:** Using abstract classes and inheritance to create flexible systems. allowing for systems to have shared behaviors plus unique logic, and avoid code duplication.
* **ScriptableObject Architecture:** Leveraging ScriptableObjects for data-driven design, modular stats, and game-wide event architecture.
* **Dependency Injection:** Minimizing "Singleton-dependency" to reduce hard-coded dependencies, keeping the codebase testable and clean.

---

## 🚀 Key Systems (Technical Deep Dives)
Below are links to the internal documentation detailing the mechanics, challenges, and design patterns used for each system. These files live in the `/Doc` folder.

| System | Technical Highlight | Documentation |
| :--- | :--- | :--- |
| **Input System** | Uses Unity’s Input System to decouple gameplay and UI actions across movement, interaction, item usage, pause menus, and interface navigation. | [View Doc](./Docs/InputSystem.md) |
| **Interaction System** | Interface-driven interaction framework that detects nearby objects, prioritizes valid targets, and triggers behavior without hard dependencies between systems. | [View Doc](./Docs/InteractionSystem.md) |
| **Item System** | Three-layer item architecture separating static item definitions, runtime item state, and physical world item behavior for flexible item creation and usage. | [View Doc](./Docs/ItemSystem.md) |
| **Inventory System** | Slot-based inventory framework that separates stored item data, UI rendering, hotbar behavior, and equipment logic into clean responsibilities. | [View Doc](./Docs/InventorySystem.md) |
| **Combat System** | Modular combat framework built around reusable damage, targeting, stats, and entity logic designed to support weapons, enemies, and abilities. | [View Doc](./Docs/CombatSystem.md) |
| **Stats System** | Event-driven resource and progression architecture for health, mana, level, and future stats while keeping gameplay logic separate from UI display. | [View Doc](./Docs/StatsSystem.md) |
| **State Machine** | Reusable finite state machine architecture that powers entity behavior such as idle, wandering, chasing, and action states through modular state classes. | [View Doc](./Docs/StateMachineSystem.md) |
| **Save / Load System** | Persistent game-state system designed to serialize important player, inventory, world, and progression data into JSON, while remaining expandable for future systems. | [View Doc](./Docs/Save&LoadSystem.md) |
| **Dialogue System** | NPC dialogue framework designed for branching conversations, and event actions to trigger world events and quests. | [View Doc](./Docs/DialogueSystem.md) |
| **Quest System** | Objective-based quest framework prepared for tracking player progress, NPC interactions, rewards, and progression-driven gameplay. | [View Doc](./Docs/QuestSystem.md) |
| **WorldTime System** | Centralized world clock system designed to track in-game time, drive day-cycle logic, and provide time-based hooks for future gameplay systems. | [View Doc](./Docs/WorldTimeSystem.md) |

---

## 🔧 Workflow & Tools
* **Version Control:** Practicing clean Git history and meaningful commit messages via GitHub Desktop.
* **Unity Version:** `Unity 6.3 LTS`
* **Documentation:** Every major system includes a technical breakdown of *"The Problem," "The Pattern,"* and *"The Solution."*
* **Iteration:** Regularly refactoring old code as my understanding of C# and Unity deepens. This repo serves as a record of that improvement.
* **IDE:** Jetbrains Rider.
