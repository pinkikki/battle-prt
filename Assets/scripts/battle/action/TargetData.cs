using UnityEngine;
using UnityEngine.UI;

namespace battle.action
{
    public class TargetData
    {
        public BattleObject BattleObject { get; set; }

        public GameObject ReactionEffectType1GameObject { get; set; }
        
        public GameObject ReactionEffectType2GameObject { get; set; }

        public Slider HpSlider { get; set; }
    }
}
