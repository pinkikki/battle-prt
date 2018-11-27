using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace battle.action
{
    public class DamageCalculator
    {
        private static DamageCalculator _instance;

        private DamageCalculator()
        {
        }

        public static DamageCalculator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DamageCalculator();
                }

                return _instance;
            }
        }

        public int Calculate(Action action, BattleObject targetBattleObject, int index)
        {
            switch (action.AttackType)
            {
                case Action.Type.Normal:
                    return Calculate(action.BattleObject.Attack, targetBattleObject.Defense);
                case Action.Type.Skill1:
                    return CalculateForSkill(action, targetBattleObject,
                        index == 1 ? action.BattleObject.Skill1.Power1 : action.BattleObject.Skill1.Power2,
                        index == 1 ? action.BattleObject.Skill1.Type1 : action.BattleObject.Skill1.Type2);
                case Action.Type.Skill2:
                    return CalculateForSkill(action, targetBattleObject,
                        index == 1 ? action.BattleObject.Skill2.Power1 : action.BattleObject.Skill2.Power2,
                        index == 1 ? action.BattleObject.Skill2.Type1 : action.BattleObject.Skill2.Type2);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private int CalculateForSkill(Action action, BattleObject targetBattleObject, int power, SkillType type)
        {
            switch (type)
            {
                case SkillType.PhysicalAttack:
                    return Calculate(action.BattleObject.Attack * power / 100, targetBattleObject.Defense);
                case SkillType.MagicAttack:
                    return Calculate(action.BattleObject.Mind * power / 100, targetBattleObject.Mind);
                case SkillType.Recovery:
                    return Calculate(action.BattleObject.Mind * power / 100, targetBattleObject.Mind);
                default:
                    throw new ArgumentOutOfRangeException();
                
            }
        }

        private int Calculate(int attack, int defense)
        {
            var a1 = attack - defense;
            var a2 = 0 < a1 ? a1 : 0;

            var b1 = attack * Random.Range(0.8f, 1.2f);
            var b2 = Mathf.CeilToInt(b1 * 0.4f);

            var c1 = Mathf.CeilToInt((a2 + b2) * Random.Range(0.9f, 1.1f));

            Debug.Log($"damage: {c1}, attack: {attack}, defense: {defense}");

            return c1;
        }
    }
}
