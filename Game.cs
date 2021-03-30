using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyFirstRogueLike.Core;
using MyFirstRogueLike.Systems;
using RLNET;
using RogueSharp.Random;

namespace MyFirstRogueLike
{
    class Game
    {
        private static bool _renderRequired = true;

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
                  
        public static DungeonMap DungeonMap;
        public static IRandom Random { get; private set; }
        public static Player Player { get; set; }
        public static CommandSystem CommandSystem { get; private set; }
        public static MessageLog MessageLog { get; private set; }

        static void Main(string[] args)
        {
            // Bitmap font file:
            string fontFileName = "terminal8x8.png";

            int seed = (int)DateTime.UtcNow.Ticks;
            Random = new DotNetRandom(seed);

            string consoleTitle = $"MyRogueLike - Phase 1 - Seed {seed}";

            int maxRooms = 20;
            int minRoomSize = 7;
            int maxRoomSize = 13;

            MapGenerator mapGenerator = new MapGenerator(_mapWidth, _mapHeight, maxRooms, minRoomSize, maxRoomSize);
            DungeonMap = mapGenerator.CreateMap();

            CommandSystem = new CommandSystem();

            DungeonMap.UpdatePlayerFOV();

            MessageLog = new MessageLog();
            MessageLog.AddMessage("The rogue arrives on level 1");
            MessageLog.AddMessage($"Level created with seed {seed}");

            // Instantiating the root console
            _rootConsole = new RLRootConsole(fontFileName, _screenWidth, _screenHeight, _tileSize, _tileSize, _tileScale, consoleTitle);

            // Instantiating subconsoles
            _mapConsole = new RLConsole(_mapWidth, _mapHeight);
            _messageConsole = new RLConsole(_messageWidth, _messageHeight);
            _statsConsole = new RLConsole(_statsWidth, _statsHeight);
            _inventoryConsole = new RLConsole(_inventoryWidth, _inventoryHeight);


            _inventoryConsole.SetBackColor(0, 0, _inventoryWidth, _inventoryHeight, Colors.InventoryConsoleBackground);
            _inventoryConsole.Print(1, 1, "Inventory", Colors.TextHeading);

            _rootConsole.Update += OnRootConsoleUpdate;
            _rootConsole.Render += OnRootConsoleRender;

            // Begin the game loop
            _rootConsole.Run();
        }


        private static void OnRootConsoleRender(object sender, UpdateEventArgs e)
        {
            // Blit the sub consoles to the root console in the correct locations
            
            if (_renderRequired)
            {
                _mapConsole.Clear();
                _statsConsole.Clear();
                _messageConsole.Clear();
                DungeonMap.Draw(_mapConsole, _statsConsole);
                Player.Draw(_mapConsole, DungeonMap);

                Player.DrawStats(_statsConsole);

                RLConsole.Blit(_mapConsole, 0, 0, _mapWidth, _mapHeight, _rootConsole, 0, _inventoryHeight);
                RLConsole.Blit(_messageConsole, 0, 0, _messageWidth, _messageHeight, _rootConsole, 0, _screenHeight - _messageHeight);
                RLConsole.Blit(_statsConsole, 0, 0, _statsWidth, _statsHeight, _rootConsole, _mapWidth, 0);
                RLConsole.Blit(_inventoryConsole, 0, 0, _inventoryWidth, _inventoryHeight, _rootConsole, 0, 0);

                _rootConsole.Draw();
                _renderRequired = false;
                MessageLog.Draw(_messageConsole);
            }

        }

        private static void OnRootConsoleUpdate(object sender, UpdateEventArgs e)
        {
            bool didPlayerAct = false;
            RLKeyPress keyPress = _rootConsole.Keyboard.GetKeyPress();

            if (keyPress != null)
            {
                if (keyPress.Key == RLKey.Up)
                    didPlayerAct = CommandSystem.MovePlayer(Direction.Up);
                else if (keyPress.Key == RLKey.Down)
                    didPlayerAct = CommandSystem.MovePlayer(Direction.Down);
                else if (keyPress.Key == RLKey.Left)
                    didPlayerAct = CommandSystem.MovePlayer(Direction.Left);
                else if (keyPress.Key == RLKey.Right)
                    didPlayerAct = CommandSystem.MovePlayer(Direction.Right);
                else if (keyPress.Key == RLKey.Escape)
                    _rootConsole.Close();
            }

            if (didPlayerAct)
            {
                _renderRequired = true;
            }
        }

    }
}
