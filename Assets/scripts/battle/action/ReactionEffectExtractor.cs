using System;

namespace battle.action
{
    public class ReactionEffectExtractor
    {
        private static ReactionEffectExtractor _instance;

        public static ReactionEffectExtractor Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ReactionEffectExtractor();
                }

                return _instance;
            }
        }

        public string ExtractParticleNameForNormal()
        {
            return "prefab/DamagedEffect";
        }

        public string ExtractParticleNameForSkillType1(Action action)
        {
            switch (action.AttackType)
            {
                case Action.Type.Skill1:
                    return ExtractParticleNameForSkill(action.BattleObject.Skill1.Type1);
                case Action.Type.Skill2:
                    return ExtractParticleNameForSkill(action.BattleObject.Skill2.Type1);
                default:
                    return null;
            }
        }

        public string ExtractParticleNameForSkillType2(Action action)
        {
            switch (action.AttackType)
            {
                case Action.Type.Skill1:
                    return ExtractParticleNameForSkill(action.BattleObject.Skill1.Type2);
                case Action.Type.Skill2:
                    return ExtractParticleNameForSkill(action.BattleObject.Skill2.Type2);
                default:
                    return null;
            }
        }

        private string ExtractParticleNameForSkill(SkillType type)
        {
            if (type.IsTypeAttack())
            {
                return "prefab/DamagedEffect";
            }

            if (type.IsTypeRecovery())
            {
                return "prefab/RecoveredEffect";
            }

            if (type.IsTypeBasisStatusDeBuff())
            {
                return "prefab/BasisStatusBuffedEffect";
            }

            if (type.IsTypeDeBuff())
            {
                return "prefab/DeBuffedEffect";
            }

            if (type.IsTypeRelease())
            {
                return "prefab/ReleasedEffect";
            }

            return null;
        }
    }
}
