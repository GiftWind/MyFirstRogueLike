using RLNET;
using RogueSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstRogueLike.Core
{
    public class DungeonMap : Map<DungeonCell>
    {
        public List<Rectangle> rooms;

        public readonly FieldOfView<DungeonCell> _fieldOfView;
        private readonly List<Monster> _monsters;

        public void Draw(RLConsole mapConsole, RLConsole statConsole)
        {
            mapConsole.Clear();
            foreach (DungeonCell cell in GetAllCells())
            {
                SetSymbolForCell(mapConsole, cell);
            }

            int mosterIndex = 0;

            foreach (Monster monster in _monsters)
            {
                monster.Draw(mapConsole, this);
                if (IsInFov(monster.X, monster.Y))
                {
                    monster.DrawStats(statConsole, mosterIndex);
                    mosterIndex++;
                }
            }
            
        }

        public DungeonMap()
        {
            _fieldOfView = new FieldOfView<DungeonCell>(this);
            rooms = new List<Rectangle>();
            _monsters = new List<Monster>();
        }

        private void SetSymbolForCell(RLConsole mapConsole, DungeonCell cell)
        {

            if (!cell.IsExplored)
            {
                return;
            }

            if (IsInFov(cell))
            {
                if (cell.IsWalkable)
                    mapConsole.Set(cell.X, cell.Y, Colors.FloorFov, Colors.FloorBackgroundFov, '.');
                else
                    mapConsole.Set(cell.X, cell.Y, Colors.WallFov, Colors.WallBackgroundFov, '#');
            }
            else
            {
                if (cell.IsWalkable)
                    mapConsole.Set(cell.X, cell.Y, Colors.Floor, Colors.FloorBackground, '.');
                else
                    mapConsole.Set(cell.X, cell.Y, Colors.Wall, Colors.WallBackground, '#');
            }
        }

        public void SetCellProperties(int x, int y, bool isTransparent, bool isWalkable, bool isExplored)
        {
            this[x, y].IsTransparent = isTransparent;
            this[x, y].IsWalkable = isWalkable;
            this[x, y].IsExplored = isExplored;
        }

        public bool IsInFov(Cell cell)
        {
            return _fieldOfView.IsInFov(cell.X, cell.Y);
        }

        public bool IsInFov(int x, int y)
        {
            return _fieldOfView.IsInFov(x, y);
        }

        public void UpdatePlayerFOV()
        {
            Player player = Game.Player;
            ComputeFov(player.X, player.Y, player.Awareness, true);
            foreach (Cell cell in GetAllCells())
            {
                if (IsInFov(cell.X, cell.Y))
                    SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
            }
        }

        public ReadOnlyCollection<DungeonCell> ComputeFov(int xOrigin, int yOrigin, int radius, bool lightWalls)
        {
            return _fieldOfView.ComputeFov(xOrigin, yOrigin, radius, lightWalls);
        }

        public bool SetActorPositon(Actor actor, int x, int y)
        {
            if (GetCell(x, y).IsWalkable)
            {
                SetIsWalkable(actor.X, actor.Y, true);
                actor.X = x;
                actor.Y = y;
                SetIsWalkable(actor.X, actor.Y, false);
                if (actor is Player)
                {
                    UpdatePlayerFOV();
                }
                return true;
            }

            return false;
        }

        public void SetIsWalkable(int x, int y, bool isWalkable)
        {
            DungeonCell cell = GetCell(x, y);
            SetCellProperties(cell.X, cell.Y, cell.IsTransparent, isWalkable, cell.IsExplored);
        }

        public void AddPlayer(Player player)
        {
            Game.Player = player;
            SetIsWalkable(player.X, player.Y, false);
            UpdatePlayerFOV();
            Game.SchedulingSystem.Add(player);
        }

        public void AddMonster(Monster monster)
        {
            _monsters.Add(monster);
            SetIsWalkable(monster.X, monster.Y, false);
            Game.SchedulingSystem.Add(monster);
        }

        public void RemoveMonster(Monster monster)
        {
            _monsters.Remove(monster);
            SetIsWalkable(monster.X, monster.Y, true);
            Game.SchedulingSystem.Remove(monster);
        }

        public Monster GetMonsterAt(int x, int y)
        {
            return _monsters.FirstOrDefault(monster => monster.X == x && monster.Y == y);
        }

        // Get the random walkable cell
        public Point? GetRandomWalkableLocationInRoom(Rectangle room)
        {
            if (RoomHaveWalkableSpace(room))
            {
                for (int i = 0; i < 100; i++)
                {
                    int x = Game.Random.Next(1, room.Width - 2) + room.X;
                    int y = Game.Random.Next(1, room.Height - 2) + room.Y;
                    if (IsWalkable(x, y))
                    {
                        return new Point(x, y);
                    }
                }
            }
            return null;
        }

        private bool RoomHaveWalkableSpace(Rectangle room)
        {
            for (int x = 1; x <= room.Width - 2; x++)
            {
                for (int y = 1; y <= room.Height - 2; y++)
                {
                    if (IsWalkable(x + room.X, y + room.Y))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
