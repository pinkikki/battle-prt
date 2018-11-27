namespace battle.action
{
    public class Action
    {
        public Type AttackType { get; set;}
        public BattleObject BattleObject { get; set;}

        public enum Type
        {
            Normal,
            Skill1,
            Skill2
        }
    }
}
