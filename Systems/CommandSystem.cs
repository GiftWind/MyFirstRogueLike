using MyFirstRogueLike.Core;
using MyFirstRogueLike.Interfaces;
using RogueSharp;
using RogueSharp.DiceNotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstRogueLike.Systems
{
    public class CommandSystem
    {
        public bool IsPlayerTurn { get; set; }
        public void EndPlayerTurn()
        {
            IsPlayerTurn = false;
        }
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

            Monster monster = Game.DungeonMap.GetMonsterAt(x, y);

            if (monster != null)
            {
                Attack(Game.Player, monster);
                return true;
            }

            return false;
        }

        public void Attack(Actor attacker, Actor defender)
        {
            StringBuilder attackMessage = new StringBuilder();
            StringBuilder defenseMessage = new StringBuilder();

            int hits = ResolveAttack(attacker, defender, attackMessage);
            int blocks = ResolveDefense(defender, hits, attackMessage, defenseMessage);

            Game.MessageLog.AddMessage(attackMessage.ToString());
            if (!string.IsNullOrWhiteSpace(defenseMessage.ToString()))
            {
                Game.MessageLog.AddMessage(defenseMessage.ToString());
            }

            int damage = hits - blocks;
            ResolveDamage(defender, damage);
        }

        private void ResolveDamage(Actor defender, int damage)
        {
            if (damage > 0)
            {
                defender.Health -= damage;
                Game.MessageLog.AddMessage($" {defender.Name} was hit for {damage} damage.");
                if (defender.Health <= 0)
                {
                    ResolveDeath(defender);
                }
            }
            else
            {
                Game.MessageLog.AddMessage($" {defender.Name} blocked all damage");
            }
        }

        private void ResolveDeath(Actor defender)
        {
            if (defender is Player)
            {
                Game.MessageLog.AddMessage($"{defender.Name} was killed. GAME OVER");
            }
            else if (defender is Monster)
            {
                Game.DungeonMap.RemoveMonster((Monster) defender);
                Game.MessageLog.AddMessage($"{defender.Name} died and dropped {defender.Gold} gold");
            }
        }

        private int ResolveDefense(Actor defender, int hits, StringBuilder attackMessage, StringBuilder defenseMessage)
        {
            int blocks = 0;
            if (hits > 0)
            {
                attackMessage.Append($" scoring {hits} hits.");
                defenseMessage.Append($" {defender.Name} defends and rolls: ");
                var defenseDice = new DiceExpression().Dice(defender.Defense, 100);
                DiceResult defenseResult = defenseDice.Roll();
                foreach (var termResult in defenseResult.Results)
                {
                    defenseMessage.Append(termResult.Value + ", ");
                    if (termResult.Value >= 100 - defender.DefenseChance)
                    {
                        blocks++;
                    }
                }
                defenseMessage.Append($" resulting in {blocks} blocks.");
            }
            else
            {
                attackMessage.Append(" and misses completely.");
            }
            return blocks;
        }

        private static int ResolveAttack(Actor attacker, Actor defender, StringBuilder attackMessage)
        {
            int hits = 0;
            attackMessage.Append($"{attacker.Name} attacks {defender.Name} and rolls: ");
            var attackDice = new DiceExpression().Dice(attacker.Attack, 100);
            DiceResult attackResult = attackDice.Roll();

            foreach (var termResult in attackResult.Results)
            {
                attackMessage.Append(termResult.Value + ", ");
                if (termResult.Value >= 100 - attacker.AttackChance)
                {
                    hits++;
                }
            }
            return hits;
        }

        public void ActivateMonsters()
        {
            ISchedulable schedulable = Game.SchedulingSystem.GetNextSchedulable();
            if (schedulable is Player)
            {
                IsPlayerTurn = true;
                Game.SchedulingSystem.Add(Game.Player);
            }
            else
            {
                Monster monster = schedulable as Monster;
                if (monster != null)
                {
                    monster.PerformAction(this);
                    Game.SchedulingSystem.Add(monster);
                }
                ActivateMonsters();
            }
        }

        public void MoveMonster(Monster monster, Cell cell)
        {
            if (!Game.DungeonMap.SetActorPositon(monster, cell.X, cell.Y))
            {
                if (Game.Player.X == cell.X && Game.Player.Y == cell.Y)
                {
                    Attack(monster, Game.Player);
                }
            }
        }
    }
}
