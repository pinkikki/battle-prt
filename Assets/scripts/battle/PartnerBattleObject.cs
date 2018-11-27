using UnityEngine;
using UnityEngine.UI;

namespace battle
{
    public class PartnerBattleObject : BattleObject
    {
        
        public Transform PartnerTransform { get; set; } 
        
        public Transform IconTransform { get; set; }
        
        public Transform TimeBarTransform { get; set; }
        
        public Image TimeBarImage { get; set; }
        
        public Text CommentText { get; set; }
        
        public GameObject SwipeArrowGameObject { get; set; }
        
        public GameObject PartnerSkillReadyGameObject { get; set; }
    }
}
