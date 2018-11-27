using script.db.entity;
using UnityEngine;

namespace battle
{
    public class BattleObject
    {
        public int Id { get; set; }
        
        public string SystemName { get; set; }
        
        public string DisplayName { get; set; }
        
        public int Creator { get; set; }
        
        public int Attack { get; set; }
        
        public int Defense { get; set; }
        
        public int Mind { get; set; }
        
        public int Speed { get; set; }
        
        public int CurrentHp { get; set; }

        public int MaxHp { get; set; }
        
        public SkillEntity Skill1 { get; set; }
        
        public SkillEntity Skill2 { get; set; }
        
        public SkillEntity  LeaderSkill { get; set; }
        
        public int Sense { get; set; }
        
        public GameObject GameObject { get; set; }
        
        public GameObject SkillButtonsGameObject { get; set; }
        
        public CharacterType CharacterType { get; set; }

        public bool IsCharacter()
        {
            return CharacterType == CharacterType.Character;
        }

        public bool IsPartner()
        {
            return CharacterType == CharacterType.Partner;
        }
    }
}
