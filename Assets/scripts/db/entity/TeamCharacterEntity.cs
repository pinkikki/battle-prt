using battle;

namespace script.db.entity
{
    public class TeamCharacterEntity {
        
        public int Id { get; set; }
        
        public string SystemName { get; set; }
        
        public string DisplayName { get; set; }
        
        public string Lang { get; set; }
        
        public int Creator { get; set; }
        
        public int Level { get; set; }
        
        public int Attack { get; set; }
        
        public int Defense { get; set; }
        
        public int Mind { get; set; }
        
        public int Speed { get; set; }
        
        public int CurrentHp { get; set; }
        
        public int MaxHp { get; set; }
        
        public int Skill1 { get; set; }
        
        public int Skill2 { get; set; }
        
        public int LeaderSkill { get; set; }
        
        public int Sense { get; set; }
        
    }
}
