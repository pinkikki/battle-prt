using battle;

namespace script.db.entity
{
    public class SkillEntity {
        
        public int Id { get; set; }
        
        public string SystemName { get; set; }
        
        public string DisplayName { get; set; }
        
        public string Lang { get; set; }
        
        public int Level { get; set; }
        
        public SkillType Type1 { get; set; }
        
        public int Power1 { get; set; }
        
        public int Turn1 { get; set; }
        
        public SkillType Type2 { get; set; }
        
        public int Power2 { get; set; }
        
        public int Turn2 { get; set; }
        
        public int Seconds { get; set; }
        
        public SkillRange Range { get; set; }
        
    }
}
