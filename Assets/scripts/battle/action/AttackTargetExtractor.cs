using System;
using System.Collections.Generic;
using System.Linq;
using battle.character;
using battle.enemy;
using script.db.entity;
using Random = UnityEngine.Random;

namespace battle.action
{
    public class AttackTargetExtractor
    {
        private static AttackTargetExtractor _instance;

        public static AttackTargetExtractor Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AttackTargetExtractor();
                }

                return _instance;
            }
        }

        public List<BattleObject> Extract(Action action)
        {
            switch (action.AttackType)
            {
                case Action.Type.Normal:
                    return AttackSingle(action);
                case Action.Type.Skill1:
                    return ExtractForSkill(action, action.BattleObject.Skill1);
                case Action.Type.Skill2:
                    return ExtractForSkill(action, action.BattleObject.Skill2);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private List<BattleObject> ExtractForSkill(Action action, SkillEntity skillEntity)
        {
            var range = skillEntity.Range;
            var type1 = skillEntity.Type1;
            var type2 = skillEntity.Type2;

            if (range == SkillRange.All)
            {
                var list = new List<BattleObject>();
                if (type1.IsTypeAttack() ||
                    type2.IsTypeAttack() ||
                    type1.IsTypeDeBuff() ||
                    type2.IsTypeDeBuff())
                {
                    list.AddRange(action.BattleObject.IsCharacter()
                        ? EnemyRepository.Instance.Enemies.Values.ToList()
                        : CharacterRepository.Instance.Characters.Values.ToList());

                    return list;
                }

                if (type1.IsTypeRecovery() ||
                    type2.IsTypeRecovery() ||
                    type1.IsTypeBuff() ||
                    type2.IsTypeBuff() ||
                    type1.IsTypeRelease() ||
                    type2.IsTypeRelease())
                {
                    list.AddRange(action.BattleObject.IsCharacter() || action.BattleObject.IsPartner()
                        ? CharacterRepository.Instance.Characters.Values.ToList()
                        : EnemyRepository.Instance.Enemies.Values.ToList());

                    return list;
                }
            }

            if (type1.IsTypeAttack() ||
                type2.IsTypeAttack() ||
                type1.IsTypeDeBuff() ||
                type2.IsTypeDeBuff())
            {
                return AttackSingle(action);
            }

            return null;
        }

        private List<BattleObject> AttackSingle(Action action)
        {
            var list = new List<BattleObject>();
            var battleObjects = action.BattleObject.IsCharacter()
                ? EnemyRepository.Instance.Enemies.Values.ToList()
                : CharacterRepository.Instance.Characters.Values.ToList();

            var survivorBattleObjects = battleObjects.Where(battleObject => battleObject.CurrentHp != 0).ToList();
            var no = Random.Range(0, survivorBattleObjects.Count);
            list.Add(survivorBattleObjects[no]);
            return list;
        }
    }
}
