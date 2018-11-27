using System.Collections.Generic;
using System.Linq;

namespace battle
{
    public class Sorter
    {
        private static Sorter _instance;
        
        private Sorter (){}

        public static Sorter Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Sorter();
                }
                return _instance;
            }
        }
        
        public List<BattleObject> SortBySpeed(List<BattleObject> targets)
        {
            var orderedEnumerable = targets.OrderBy(battleObject => battleObject.Speed);
            return orderedEnumerable.ToList();
        }
        
        public List<BattleObject> SortBySpeed(List<BattleObject> targets1, List<BattleObject> targets2)
        {
            targets1.AddRange(targets2);
            return SortBySpeed(targets1);
        }
    }
}
