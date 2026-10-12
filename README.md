# VoxelCoreEngine
A high-performance, custom voxel terrain engine built from scratch in C# and OpenTK, featuring procedural generation, std140 UBOs, and modern OpenGL. 🌍⚡
# 🌍 TerraVox-GL (Custom Voxel Terrain Engine)

A high-performance, custom-built voxel-based terrain engine developed from scratch in **C#** and **OpenTK (OpenGL)**. This project focuses on exploring modern graphics architecture, efficient GPU-CPU data synchronization, and procedural world-building techniques.

---

## 🚀 Key Features

* **Procedural Terrain Generation:** Dynamic heightmap and density calculation powered by custom Perlin noise algorithms, featuring adjustable terrain roughness and depth intensity.
* **Two-Pass Biome & Vegetation System:** Intelligent world population pipeline that generates solid terrain foundations (stone, dirt, grass) first, followed by safe structural placement for tree trunks and leaf canopies.
* **Optimized Graphics Pipeline (`std140` UBOs):** Efficient GPU-CPU data synchronization using Uniform Buffer Objects (`std140` layout) to manage Projection and View matrices with minimal overhead.
* **Modern Chunk Architecture:** Designed to handle 3D voxel storage efficiently utilizing byte-based block IDs within optimized chunk structures ($16 \times 256 \times 16$).
* **Cubemap Texturing:** Implemented multi-face cubemap loading and rendering pipelines for environmental materials and skyboxes.
* **Camera & View Management:** Stable, free-roaming 3D camera controller with real-time VRAM matrix updates during the render loop.

---

## 🛠️ Tech Stack & Architecture

* **Language:** C# (.NET)
* **Graphics API:** OpenTK (OpenGL 4.x / Modern GLSL)
---

## ⚙️ Getting Started

### Prerequisites
* .NET SDK (10.0 or higher recommended)
* A graphics card supporting **OpenGL 4 or higher**

### Building and Running
1. Clone the repository:
   ```bash
   git clone [https://github.com/your-username/terravox-gl.git](https://github.com/your-username/terravox-gl.git)

 ## 🧠 Core Methods & Techniques

* **Procedural Noise Pipeline:** Generates organic, continuous terrain heights and structures using custom Perlin noise algorithms.
* **Two-Pass Vegetation System:** Decouples terrain creation (Pass 1: stone/dirt/grass foundation) from decoration (Pass 2: secure tree trunk and leaf placement), preventing chunk boundary corruptions.
* **`std140` Uniform Buffer Objects (UBOs):** Manages GPU-CPU synchronization efficiently by batching Projection and View matrices into aligned memory blocks.

## 🎛️ Key Control Variables

You can tweak the procedural generation and world-building behavior using the following core parameters in the engine:

* **`TERRAIN_ROUGHNESS`**: Controls the overall variance, steepness, and ruggedness of the terrain mountains. Higher values create sharp, dramatic peaks while lower values produce flat plains.
* **`row` & `col`**: Represent the 2D grid coordinates/indices of the chunk generation loops, mapping local block placements across the world space.
* **`depth`**: Determines the height intensity/elevation values derived from noise functions or texture data, acting as the foundation for surface height calculations and safe tree height bounds.
