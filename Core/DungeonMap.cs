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
        // TODO: Make it readonly later
        private readonly FieldOfView<DungeonCell> _fieldOfView;

        public void Draw(RLConsole mapConsole)
        {
            mapConsole.Clear();
            foreach (DungeonCell cell in GetAllCells())
            {
                SetSymbolForCell(mapConsole, cell);
            }
            
        }

        public DungeonMap()
        {
            _fieldOfView = new FieldOfView<DungeonCell>(this);
        }

        private void SetSymbolForCell(RLConsole mapConsole, DungeonCell cell)
        {
            // TODO: add a list of visited cells

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

        // Quick stub. The new version of RogueSharp is too different
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
                {
                    SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
                }
            }
        }

        public ReadOnlyCollection<DungeonCell> ComputeFov(int xOrigin, int yOrigin, int radius, bool lightWalls)
        {
            return _fieldOfView.ComputeFov(xOrigin, yOrigin, radius, lightWalls);
        }
    }
}
