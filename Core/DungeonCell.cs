using RogueSharp;

namespace MyFirstRogueLike.Core
{
    public class DungeonCell : Cell
    {
        public bool IsExplored { get; internal set; }
    }
}