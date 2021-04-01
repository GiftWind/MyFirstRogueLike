using MyFirstRogueLike.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstRogueLike.Systems
{
    public class SchedulingSystem
    {
        private int _time;
        private readonly SortedDictionary<int, List<ISchedulable>> _schedulables;

        public SchedulingSystem()
        {
            _time = 0;
            _schedulables = new SortedDictionary<int, List<ISchedulable>>();
        }

        public void Add(ISchedulable schedulable)
        {
            int key = _time + schedulable.Time;
            if (!_schedulables.ContainsKey(key))
            {
                _schedulables.Add(key, new List<ISchedulable>());
            }
            _schedulables[key].Add(schedulable);
        }

        public void Remove(ISchedulable schedulable)
        {
            var schedulableListFound = new KeyValuePair<int, List<ISchedulable>>(-1, null);
            foreach (var schedulablesList in _schedulables)
            {
                if (schedulablesList.Value.Contains(schedulable))
                {
                    schedulableListFound = schedulablesList;
                    break;
                }    
            }
            if (schedulableListFound.Value != null)
            {
                schedulableListFound.Value.Remove(schedulable);
                if (schedulableListFound.Value.Count <= 0)
                {
                    _schedulables.Remove(schedulableListFound.Key);
                }
            }

        }

        public ISchedulable GetNextSchedulable()
        {
            var firstSchedulableEntry = _schedulables.First();
            var firstSchedulable = firstSchedulableEntry.Value.First();
            Remove(firstSchedulable);
            _time = firstSchedulableEntry.Key;
            return firstSchedulable;
        }

        public int GetCurrentTime()
        {
            return _time;
        }

        public void Clear()
        {
            _time = 0;
            _schedulables.Clear();
        }
    }
}
