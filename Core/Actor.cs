using MyFirstRogueLike.Interfaces;
using RLNET;
using RogueSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstRogueLike.Core
{
    public class Actor : IActor, IDrawable, ISchedulable
    {
        public string Name { get; set; }
        public int Awareness { get; set; }

        public RLColor Color { get; set; }
        public char Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Attack { get ; set ; }
        public int AttackChance { get; set; }
        public int Defense { get; set; }
        public int DefenseChance { get; set; }
        public int Gold { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Speed { get; set; }

        public int Time { get => Speed; }

        public void Draw(RLConsole console, DungeonMap map)
        {
            if (!map.GetCell(X, Y).IsExplored)
                return;

            if (map.IsInFov(X, Y))
                console.Set(X, Y, Color, Colors.FloorBackgroundFov, Symbol);
            else
                console.Set(X, Y, Colors.Floor, Colors.FloorBackground, '.');
        }
    }
}
