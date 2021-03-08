using MyFirstRogueLike.Core;
using RogueSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstRogueLike.Systems
{
    public class MapGenerator
    {
        private readonly int _width;
        private readonly int _height;
        private readonly int _maxRooms;
        private readonly int _minRoomSize;
        private readonly int _maxRoomSize;

        private readonly DungeonMap _map;

        public MapGenerator(int width, int height, int maxRooms, int minRoomSize, int maxRoomSize)
        {
            _width = width;
            _height = height;
            _maxRooms = maxRooms;
            _minRoomSize = minRoomSize;
            _maxRoomSize = maxRoomSize;
            _map = new DungeonMap();
        }

        public DungeonMap CreateMap()
        {
            _map.Initialize(_width, _height);
            //foreach (DungeonCell cell in _map.GetAllCells())
            //{
            //    _map.SetCellProperties(cell.X, cell.Y, true, true, true);
            //}

            //// Set the boundaries
            //foreach (DungeonCell cell in _map.GetCellsInRows(0, _height - 1))
            //{
            //    _map.SetCellProperties(cell.X, cell.Y, false, false, true);
            //}

            //foreach (DungeonCell cell in _map.GetCellsInColumns(0, _width - 1))
            //{
            //    _map.SetCellProperties(cell.X, cell.Y, false, false, true);
            //}
            for (int r = _maxRooms; r > 0; r--)
            {
                int roomWidth = Game.Random.Next(_minRoomSize, _maxRoomSize);
                int roomHeight = Game.Random.Next(_minRoomSize, _maxRoomSize);
                int roomXPosition = Game.Random.Next(0, _width - roomWidth - 1);
                int roomYPosition = Game.Random.Next(0, _height - roomHeight - 1);

                var newRoom = new Rectangle(roomXPosition, roomYPosition, roomWidth, roomHeight);
                bool newRoomIntersects = _map.rooms.Any(room => newRoom.Intersects(room));

                if (!newRoomIntersects)
                {
                    _map.rooms.Add(newRoom);
                }
            }

            foreach (var room in _map.rooms)
            {
                    CreateRoom(room);
            }

            PlacePlayer();

            return _map;

           
        }

        private void PlacePlayer()
        {
            Player player = Game.Player;
            if (player == null)
            {
                player = new Player();
            }
            player.X = _map.rooms[0].Center.X;
            player.Y = _map.rooms[0].Center.Y;

            _map.AddPlayer(player);
        }

        private void CreateRoom(Rectangle room)
        {
            for (int x = room.Left + 1; x < room.Right; x++)
            {
                for (int y = room.Top + 1; y < room.Bottom; y++)
                {
                    _map.SetCellProperties(x, y, true, true, true);
                }
            }
        }
    }
}
