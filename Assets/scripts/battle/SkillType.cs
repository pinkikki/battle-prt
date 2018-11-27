namespace battle
{
    public enum SkillType
    {
        None = 0,
        PhysicalAttack = 1,
        MagicAttack = 2,
        Recovery = 3,
        AttackBuff = 4,
        DefenseBuff = 5,
        MindBuff = 6,
        SpeedBuff = 7,
        AttackDeBuff = 8,
        DefenseDeBuff = 9,
        MindDeBuff = 10,
        SpeedDeBuff = 11,
        PoisonDeBuff = 12,
        BurnDeBuff = 13,
        IceDeBuff = 14,
        ParalysisDeBuff = 15,
        TemptationDeBuff = 16,
        RecoveryDisturbanceDeBuff = 17,
        ProvocationDeBuff = 18,
        ConfusionDeBuff = 19,
        Release = 20
    }

    public static class SkillTypeExtensions
    {
        public static bool IsTypeAttack(this SkillType skillType)
        {
            return skillType == SkillType.PhysicalAttack || skillType == SkillType.MagicAttack;
        }

        public static bool IsTypeRecovery(this SkillType skillType)
        {
            return skillType == SkillType.Recovery;
        }

        public static bool IsTypeBuff(this SkillType skillType)
        {
            return skillType == SkillType.AttackBuff ||
                   skillType == SkillType.DefenseBuff ||
                   skillType == SkillType.MindBuff ||
                   skillType == SkillType.SpeedBuff;
        }

        public static bool IsTypeDeBuff(this SkillType skillType)
        {
            return skillType == SkillType.AttackDeBuff ||
                   skillType == SkillType.DefenseDeBuff ||
                   skillType == SkillType.MindDeBuff ||
                   skillType == SkillType.SpeedDeBuff ||
                   skillType == SkillType.PoisonDeBuff ||
                   skillType == SkillType.BurnDeBuff ||
                   skillType == SkillType.IceDeBuff ||
                   skillType == SkillType.ParalysisDeBuff ||
                   skillType == SkillType.TemptationDeBuff ||
                   skillType == SkillType.RecoveryDisturbanceDeBuff ||
                   skillType == SkillType.ProvocationDeBuff ||
                   skillType == SkillType.ConfusionDeBuff;
        }

        public static bool IsTypeBasisStatusDeBuff(this SkillType skillType)
        {
            return skillType == SkillType.AttackDeBuff ||
                   skillType == SkillType.DefenseDeBuff ||
                   skillType == SkillType.MindDeBuff ||
                   skillType == SkillType.SpeedDeBuff;
        }
        
        public static bool IsTypeRelease(this SkillType skillType)
        {
            return skillType == SkillType.Release;
        }
    }
}
