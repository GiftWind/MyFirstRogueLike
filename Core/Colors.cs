using RLNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstRogueLike.Core
{
    public class Colors
    {
        public static RLColor Player = Palette.DbLight;

        public static RLColor FloorBackground = RLColor.Black;
        public static RLColor Floor = Palette.AlternateDarkest;
        public static RLColor FloorBackgroundFov = Palette.DbDark;
        public static RLColor FloorFov = Palette.Alternate;

        public static RLColor WallBackground = Palette.SecondaryDarkest;
        public static RLColor Wall = Palette.Secondary;
        public static RLColor WallBackgroundFov = Palette.SecondaryDarker;
        public static RLColor WallFov = Palette.SecondaryLighter;

        public static RLColor TextHeading = Palette.DbLight;
        public static RLColor MapConsoleBackground = RLColor.Black;
        public static RLColor MessageConsoleBackground = Palette.DbDeepWater;
        public static RLColor StatsConsoleBackground = Palette.DbOldStone;
        public static RLColor InventoryConsoleBackground = Palette.DbWood;

        public static RLColor Text = Palette.DbLight;
        public static RLColor Gold = Palette.DbSun;

        public static RLColor KoboldColor = Palette.DbBrightWood;
    }
}
