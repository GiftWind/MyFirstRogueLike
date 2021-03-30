using RLNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstRogueLike.Core
{
    public class Monster : Actor
    {
        public void DrawStats(RLConsole statConsole, int position)
        {
            // Start just below the player stats at y = 13
            // Make a space between each monster stats line
            int yPosition = 13 + position * 2;
            // Stats line begins with monsters symbol
            statConsole.Print(1, yPosition, Symbol.ToString(), Color);

            // Draw monsters healthbar
            int healthBarWidth = Convert.ToInt32(((Health / MaxHealth) * 16.0));
            int remainigHBWidth = 16 - healthBarWidth;
            statConsole.SetBackColor(3, yPosition, healthBarWidth, 1, Palette.Primary);
            statConsole.SetBackColor(3 + healthBarWidth, yPosition, remainigHBWidth, 1, Palette.PrimaryDarkest);
            statConsole.Print(2, yPosition, $": {Name}", Palette.DbLight);
        }
    }
}
