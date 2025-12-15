# Jump Master 🎮

A 2D endless runner mobile game built with Unity, inspired by Flappy Bird, Jetpack Joyride, and Super Mario.

## 📖 About

Jump Master is an endless runner where you tap to jump, collect coins, avoid obstacles, and try to get the highest score possible. The game features unique mechanics like gravity flipping and reverse mode that keep gameplay fresh and challenging.

## ✨ Features

### Core Gameplay
- **Tap-to-Jump Controls** - Simple and responsive, just tap the screen or press space
- **Endless Side-Scrolling** - Multi-layered parallax background with seamless looping
- **Progressive Difficulty** - Game speeds up 20% every 5 points

### Obstacles
- **Static Obstacles** - Fixed gaps to fly through
- **Moving Obstacles** - Grow and shrink while maintaining a passable gap
- **Rotating Obstacles** - Spinning hazards to dodge

### Portal System
- **Purple Portal** - Flips gravity so you walk on the ceiling
- **Green Portal** - Reverses direction, obstacles come from the left
- **Blue Portal** - Returns everything back to normal

### Power-Ups
- **Shield** 🛡️ - Protects you from one hit
- **Magnet** 🧲 - Attracts nearby coins to you
- **Shrink** 🔽 - Makes you smaller to fit through tight spaces

### Special Features
- **Red Coin Challenge** - Collect 8 red coins for a score multiplier bonus
- **World Themes** - Progress through Sky, Sunset, Night, Space, and Chaos worlds
- **High Score Saving** - Your best score is saved locally
- **Sound & Music** - Background music and sound effects for all actions


## 🎮 Controls

| Platform | Action |
|----------|--------|
| **Mobile** | Tap screen to jump |
| **PC** | Press Space or Left Click to jump |

## 🛠️ Built With

- **Engine:** Unity 6.3 LTS
- **Language:** C#
- **Platform:** Android (primary), PC
- **Tools:** Visual Studio, GitHub Desktop

## 📁 Project Structure

```
JumpMaster/
├── Assets/
│   ├── Animations/       # Animation clips and controllers
│   ├── Audio/           # Sound effects and music
│   │   ├── Music/
│   │   └── SFX/
│   ├── Prefabs/         # Reusable game objects
│   │   ├── Obstacles/
│   │   ├── Portals/
│   │   ├── PowerUps/
│   │   └── Collectibles/
│   ├── Scenes/          # Game scenes
│   │   ├── MainMenu.unity
│   │   └── GameScene.unity
│   ├── Scripts/         # C# scripts
│   │   ├── GameManager.cs
│   │   ├── PlayerController.cs
│   │   ├── ObstacleSpawner.cs
│   │   ├── ParallaxBackground.cs
│   │   └── ...
│   └── Sprites/         # 2D art assets
└── README.md
```

## 🎯 Core Scripts

| Script | Description |
|--------|-------------|
| `GameManager.cs` | Handles scoring, game states, difficulty, and UI updates |
| `PlayerController.cs` | Player input, jumping, power-ups, and collision |
| `ObstacleSpawner.cs` | Spawns obstacles, portals, coins, and power-ups |
| `ParallaxBackground.cs` | Multi-layer scrolling background system |
| `Portal.cs` | Mode switching (gravity flip, reverse, normal) |
| `PowerUp.cs` | Power-up collection and effects |
| `AudioManager.cs` | Sound effect playback |
| `GameMusic.cs` | Background music management |

## ⚙️ Game Parameters

| Parameter | Value | Description |
|-----------|-------|-------------|
| Gravity Scale | 4 | How fast you fall |
| Jump Force | 12 | How high you jump |
| Gap Size | 3 units | Space between obstacles |
| Speed Increase | 20% | Every 5 points |
| Portal Immunity | 3 seconds | Invincibility after portal |

## 🚀 Getting Started

### Prerequisites
- Unity 6.3 LTS or later
- Android SDK (for mobile builds)

### Installation

1. Clone the repository
```bash
git clone https://github.com/YOUR_USERNAME/JumpMaster.git
```

2. Open the project in Unity Hub

3. Open `Scenes/MainMenu.unity`

4. Press Play to test in editor

### Building for Android

1. Go to `File → Build Settings`
2. Select `Android`
3. Click `Switch Platform`
4. Click `Build` and choose output location

## 🐛 Known Issues

- High scores only save locally (no cloud sync)

## 🔮 Future Plans

- [ ] More obstacle types (moving platforms, enemies)
- [ ] Character selection with unique abilities
- [ ] Achievement system
- [ ] Online leaderboards
- [ ] iOS build
- [ ] More world themes
- [ ] Particle effects for collections
- [ ] Tutorial for new players

## 👤 Author

**Darren Ravichandra**

- GitHub: [@YOUR_USERNAME](https://github.com/darrenr21)

## 🙏 Acknowledgments

- Unity Documentation
- Unity Asset Store for sprites and audio
- Inspired by Flappy Bird, Jetpack Joyride, and Super Mario
