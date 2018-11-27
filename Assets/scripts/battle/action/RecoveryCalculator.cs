using UnityEngine;

namespace battle.action
{
    public class RecoveryCalculator
    {
        private static RecoveryCalculator _instance;

        private RecoveryCalculator()
        {
        }

        public static RecoveryCalculator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RecoveryCalculator();
                }

                return _instance;
            }
        }

        public int Calculate(Action action, int index)
        {
            var skillEntity = action.AttackType == Action.Type.Skill1
                ? action.BattleObject.Skill1
                : action.BattleObject.Skill2;
            var power = index == 1 ? skillEntity.Power1 : skillEntity.Power2;
            var a1 = action.BattleObject.Mind * power / 100;

            var c1 = Mathf.CeilToInt(a1 * Random.Range(0.9f, 1.1f));

            Debug.Log($"damage: {c1}, magic: {action.BattleObject.Mind}");

            return c1;
        }
    }
}
