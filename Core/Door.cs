using MyFirstRogueLike.Interfaces;
using RLNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstRogueLike.Core
{
    public class Door : IDrawable
    {
        public Door()
        {
            Symbol = '+';
            Color = Colors.Door;
            BackGroundColor = Colors.DoorBackground;
        }
        public bool IsOpen { get; set; }
        public RLColor Color { get; set; }
        public RLColor BackGroundColor { get; private set; }
        public char Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public void Draw(RLConsole console, DungeonMap map)
        {
            if (!map.GetCell(X, Y).IsExplored)
            {
                return;
            }
            Symbol = IsOpen ? '-' : '+';
            if (map.IsInFov(X, Y))
            {
                Color = Colors.DoorFov;
                BackGroundColor = Colors.DoorBackgroundFov;
            }
            else
            {
                Color = Colors.Door;
                BackGroundColor = Colors.DoorBackground;
            }
            console.Set(X, Y, Color, BackGroundColor, Symbol);
        }
    }
}
