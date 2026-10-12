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
* **Shaders:** Custom Vertex/Fragment shaders with UBO binding integration.
* **Math:** OpenTK Mathematics library (`Matrix4`, `Vector3`, `Vector4`).

---

## ⚙️ Getting Started

### Prerequisites
* .NET SDK (6.0 or higher recommended)
* A graphics card supporting **OpenGL 4.3 or higher**

### Building and Running
1. Clone the repository:
   ```bash
   git clone [https://github.com/your-username/terravox-gl.git](https://github.com/your-username/terravox-gl.git)
