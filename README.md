# AkfiFramework

**🚀 High-Performance ECS Framework for Unity**  
*LeoECS Lite + Custom DI Container + Code Generation*

A lightweight, production-ready template for scalable Unity games. Combines **blazing-fast ECS** with a **custom IoC/DI container**, codegen tools, and battle-tested services — less boilerplate, full ECS integration for entities, data, UI, and more.

---

## ✨ Features

- **🔧 LeoECS Lite Core**  
  Fast ECS runtime with **code generation** for systems and component providers. Auto-injects via generated MonoBehaviour providers.

- **🧩 Custom DI Container (AKContainer)**  
  VContainer-inspired IoC: reflection-based injection for `IAKInjectable` types. Supports `Bind<T>`, `Resolve<T>`, field/property/method injection, and GameObject scanning. Seamless for ECS, MonoBehaviours, and data.

- **🏷️ Custom Tags (AKTagsService)**  
  Database-driven tagging system for **all entities** (ECS, data, services). Groups, ID/name lookups.

- **📢 Custom Events (AKEventsService)**  
  Type-safe multicast events (0–4 parameters). Safe broadcasting with null cleanup, Unity Object checks, and performance-optimized tables. Decoupled across ECS and MonoBehaviours.

- **📦 Addressables Pooling System**  
  Smart prefab/particle/effect and other stuff pooling via Unity Addressables.

- **🎮 UI Manager**  
  Full UI orchestration (NodeCanvas optional). Screens, transitions, popups, and ECS hooks.

- **💾 Save System**  
  JSON/binary persistence with auto-save triggers and cloud-ready extensions.

- **🌐 Scene Manager**  
  Async loading, additive scenes, and stateful transitions integrated with ECS.

- **⚡ 3D Physics & Triggers in ECS**  
  Raycasts, collisions, and triggers handled natively in systems — no GameObject overhead.

- **⚙️ Code Generation**  
  Auto-generates providers, systems, and bindings from attributes.

---


A pilot project on the use of the framework in practice
https://github.com/Akfi23/CrowdBattle
