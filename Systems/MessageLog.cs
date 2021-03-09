using RLNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstRogueLike.Systems
{
    public class MessageLog
    {
        private static readonly int _maxLines = 9;
        private readonly Queue<string> _lines;

        public MessageLog()
        {
            _lines = new Queue<string>();
        }

        public void AddMessage(string message)
        {
            _lines.Enqueue(message);
            if (_lines.Count > _maxLines)
            {
                _lines.Dequeue();
            }
        }

        public void Draw(RLConsole console)
        {
            console.Clear();
            string[] lines = _lines.ToArray();
            int counter = 0;
            foreach (var line in lines)
            {
                console.Print(1, 1 + counter, line, RLColor.White);
                counter++;
            }
        }
    }
}
