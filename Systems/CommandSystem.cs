using MyFirstRogueLike.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstRogueLike.Systems
{
    public class CommandSystem
    {
        public bool MovePlayer(Direction direction)
        {
            int x = Game.Player.X;
            int y = Game.Player.Y;

            // TODO: Write a method for all IActor instances

            switch (direction)
            {
                case Direction.None:
                    break;
                case Direction.DownLeft:
                    break;
                case Direction.Down:
                {
                    y = Game.Player.Y + 1;
                    break;
                }                 
                case Direction.DownRight:
                    break;
                case Direction.Left:
                {
                    x = Game.Player.X - 1;    
                    break;
                }                    
                case Direction.Center:
                    break;
                case Direction.Right:
                {
                    x = Game.Player.X + 1;
                    break;
                }
                case Direction.UpLeft:
                    break;
                case Direction.Up:
                {
                    y = Game.Player.Y - 1;
                    break;
                }
                case Direction.UpRight:
                    break;
                default:
                    return false;
            }

            if (Game.DungeonMap.SetActorPositon(Game.Player, x, y))
                return true;

            return false;
        }
    }
}
