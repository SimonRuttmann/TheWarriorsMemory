# ⚔️ The Warrior’s Memory

**The Warrior’s Memory** is a tactical 2.5D turn-based strategy game, designed and developed in **Unity** during my time at **Aalen University**.

Fight a battle of wits and strategy against a clever AI opponent! Choose your units wisely, take advantage of terrain, and exploit character class synergies to emerge victorious.

![Gameplay Preview](media/img/screenshot.png)

---

## 🎮 Gameplay Overview

In this turn-based fantasy battle, you control six unique characters:

- 🛡️ 2 Warriors
- 🏹 2 Archers
- 🔮 1 Mage
- ✝️ 1 Paladin

The AI controls an identical team. You and the enemy take turns moving and attacking with each character on a **hexagonal battlefield**.  
Movement and attack ranges are visually indicated using blue and red hex markers.

![Battlefield Movement GIF](media/gifs/movement.gif)

---

## 🧠 Strategic Depth

Each character class is effective against another – this “advantage circle” adds a layer of tactical depth.

- ✅ Attacking an enemy you counter will deal **+50% damage**
- ❌ Attacking an enemy that counters you may result in disadvantageous trades

![Advantage Wheel](media/img/advantage-wheel.png)

Use this mechanic wisely to:

- Eliminate dangerous enemies first
- Keep your support units safe
- Use terrain to your advantage (e.g., mountains and lakes block movement)

---

## 🕹️ How to Play

- 🔁 Turns are taken alternately by both teams
- 🔥 The current active unit is marked on the battlefield
- 🚶 Blue markers = movement options
- 🎯 Red markers = valid attack targets
- ⌨️ Press `ESC` to pause or return to the main menu

![Attack Execution GIF](media/gifs/attack.gif)

The battle continues until one side has no units left. A win screen lets you start over or exit.

---

## 🎨 Visual Style

- Cel-Shaded 3D characters on a clean stylized grid
- Combined use of low-res pixel backgrounds and high-res 3D units
- Audio and animation for every class: idle, move, attack, hit, and death

![Combat Animation GIF](media/gifs/combat.gif)

---

## 🤖 Smart AI

The AI uses a simple but effective evaluation system:

- 📌 Prioritizes low-health or high-damage enemies for attacks
- 📈 Tries to maximize distance when safe, or close gaps when in range
- 🚫 Avoids unnecessary danger when possible

You’ll need to think ahead and position carefully!

---

## 🔧 Technologies Used

- 🎮 Unity (C#)
- 🎞️ Custom animation scheduler for timed actions
- 📊 ScriptableObject-driven setup and runtime logic
- 🔊 Audio feedback for every action and animation

---

## 🧑‍💻 Teamwork makes the Dream Work

Developed by:

- Simon Ruttmann
- Veronika Scheller
- Michael Ulrich

---

## 📜 License

This project is licensed under the [Apache 2.0 License](LICENSE).  
Feel free to fork or adapt it — just give credit. 🤝

---

## 📦 Download & Releases

Want to play?

➡️ **[Click here to download the latest release](https://github.com/SimonRuttmann/TheWarriorsMemory/releases/tag/v1.0.0)**

---

## 🎥 Videos & GIFs

| ![](media/gifs/menu.gif) | ![](media/gifs/turn.gif) | ![](media/gifs/end.gif) |
|--------------------------|--------------------------|--------------------------|

More visuals will be added soon!