using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyFirstRogueLike.Core;
using RLNET;

namespace MyFirstRogueLike
{
    class Game
    {
        // Tiles settings
        private static readonly int _tileSize = 8;
        private static readonly float _tileScale = 1F;

        // Root console
        private static readonly int _screenWidth = 100;
        private static readonly int _screenHeight = 70;
        private static RLRootConsole _rootConsole;

        // Map subconsole
        private static readonly int _mapWidth = 80;
        private static readonly int _mapHeight = 48;
        private static RLConsole _mapConsole;

        // Message subconsole
        private static readonly int _messageWidth = 80;
        private static readonly int _messageHeight = 11;
        private static RLConsole _messageConsole;

        // Stats subconsole
        private static readonly int _statsWidth = 20;
        private static readonly int _statsHeight = 70;
        private static RLConsole _statsConsole;

        // Inventory subconsole
        private static readonly int _inventoryWidth = 80;
        private static readonly int _inventoryHeight = 11;
        private static RLConsole _inventoryConsole;

        static void Main(string[] args)
        {
            // Bitmap font file:
            string fontFileName = "terminal8x8.png";

            string consoleTitle = "MyRogueLike - Phase 1";

            // Instantiating the root console
            _rootConsole = new RLRootConsole(fontFileName, _screenWidth, _screenHeight, _tileSize, _tileSize, _tileScale, consoleTitle);

            // Instantiating subconsoles
            _mapConsole = new RLConsole(_mapWidth, _mapHeight);
            _messageConsole = new RLConsole(_messageWidth, _messageHeight);
            _statsConsole = new RLConsole(_statsWidth, _statsHeight);
            _inventoryConsole = new RLConsole(_inventoryWidth, _inventoryHeight);

            _rootConsole.Update += OnRootConsoleUpdate;
            _rootConsole.Render += OnRootConsoleRender;

            // Begin the game loop
            _rootConsole.Run();
        }

        private static void OnRootConsoleRender(object sender, UpdateEventArgs e)
        {
            // Blit the sub consoles to the root console in the correct locations
            RLConsole.Blit(_mapConsole, 0, 0, _mapWidth, _mapHeight, _rootConsole, 0, _inventoryHeight);
            RLConsole.Blit(_messageConsole, 0, 0, _messageWidth, _messageHeight, _rootConsole, 0, _screenHeight - _messageHeight);
            RLConsole.Blit(_statsConsole, 0, 0, _statsWidth, _statsHeight, _rootConsole, _mapWidth, 0);
            RLConsole.Blit(_inventoryConsole, 0, 0, _inventoryWidth, _inventoryHeight, _rootConsole, 0, 0);

            _rootConsole.Draw();
        }

        private static void OnRootConsoleUpdate(object sender, UpdateEventArgs e)
        {
            _mapConsole.SetBackColor(0, 0, _mapWidth, _mapHeight, Colors.MapConsoleBackground);
            _mapConsole.Print(1, 1, "Map", Colors.TextHeading);

            _messageConsole.SetBackColor(0, 0, _messageWidth, _messageHeight, Colors.MessageConsoleBackground);
            _messageConsole.Print(1, 1, "Message", Colors.TextHeading);

            _statsConsole.SetBackColor(0, 0, _statsWidth, _statsHeight, Colors.StatsConsoleBackground);
            _statsConsole.Print(1, 1, "Stats", Colors.TextHeading);

            _inventoryConsole.SetBackColor(0, 0, _inventoryWidth, _inventoryHeight, Colors.InventoryConsoleBackground);
            _inventoryConsole.Print(1, 1, "Inventory", Colors.TextHeading);
        }

    }
}
