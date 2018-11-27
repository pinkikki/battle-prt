using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using DG.Tweening;
using script.db.entity;
using UnityEngine;
using UnityEngine.UI;

namespace battle.action
{
    public class ActionSequenceGenerator
    {
        private static ActionSequenceGenerator _instance;

        public static ActionSequenceGenerator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ActionSequenceGenerator();
                }

                return _instance;
            }
        }

        public Sequence GenerateInitialActionSequence(Action action)
        {
            if (action.BattleObject.IsPartner()) return null;
            var actionTransform = action.BattleObject.GameObject.transform;
            const float actionMoveTime = 0.05f;
            const float actionMoveX = 0.2f;
            var sequence = DOTween.Sequence();
            sequence.Append(actionTransform.DOMoveX(actionTransform.position.x + actionMoveX, actionMoveTime))
                .Append(actionTransform.DOMoveX(actionTransform.position.x, actionMoveTime))
                .Append(actionTransform.DOMoveX(actionTransform.position.x - actionMoveX, actionMoveTime))
                .Append(actionTransform.DOMoveX(actionTransform.position.x, actionMoveTime));
            return sequence;
        }

        public Sequence GenerateReactionSequence(Action action, List<TargetData> targets)
        {
            switch (action.AttackType)
            {
                case Action.Type.Normal:
                    return GenerateDamagedActionSequence(action, targets, 1);
                case Action.Type.Skill1:
                    return GenerateSkillReactionSequence(action, targets, action.BattleObject.Skill1);
                case Action.Type.Skill2:
                    return GenerateSkillReactionSequence(action, targets, action.BattleObject.Skill2);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Sequence GenerateSkillReactionSequence(Action action, List<TargetData> targets, SkillEntity skillEntity)
        {
            var sequence = DOTween.Sequence();
            var skillTypes = new List<SkillType>
            {
                skillEntity.Type1,
                skillEntity.Type2
            };

            2.Times(index =>
            {
                if (skillTypes[index - 1].IsTypeAttack())
                {
                    sequence.Append(GenerateDamagedActionSequence(action, targets, index));
                }
            });

            2.Times(index =>
            {
                if (skillTypes[index - 1].IsTypeRecovery())
                {
                    sequence.Append(GenerateRecoveryActionSequence(action, targets, index));
                }

                if (skillTypes[index - 1].IsTypeBuff())
                {
                    sequence.Append(GenerateBuffActionSequence());
                }

                if (skillTypes[index - 1].IsTypeDeBuff())
                {
                    sequence.Append(GenerateDeBuffActionSequence());
                }

                if (skillTypes[index - 1].IsTypeRelease())
                {
                    sequence.Append(GenerateReleaseActionSequence());
                }
            });

            return sequence;
        }

        private Sequence GenerateDamagedActionSequence(Action action, List<TargetData> targets, int index)
        {
            targets.ForEach(target =>
            {
                var damagedPoint = DamageCalculator.Instance.Calculate(action, target.BattleObject, index);
                var battleObject = target.BattleObject;
                var damagedHp = battleObject.CurrentHp - damagedPoint;
                battleObject.CurrentHp = 0 < damagedHp ? damagedHp : 0;
                var reactionEffectGameObject =
                    index == 1 ? target.ReactionEffectType1GameObject : target.ReactionEffectType2GameObject;
                reactionEffectGameObject.transform.Find("Text").GetComponent<Text>().text =
                    Mathf.Abs(damagedPoint).ToString();
            });
            return GenerateDamagedActionAndRecoveryActionSequence(targets, index);
        }

        private Sequence GenerateRecoveryActionSequence(Action action, List<TargetData> targets, int index)
        {
            targets.ForEach(target =>
            {
                var damagedPoint = RecoveryCalculator.Instance.Calculate(action, index);
                var battleObject = target.BattleObject;
                var damagedHp = battleObject.CurrentHp + damagedPoint;
                battleObject.CurrentHp = damagedHp < battleObject.MaxHp ? damagedHp : battleObject.MaxHp;
                var reactionEffectGameObject =
                    index == 1 ? target.ReactionEffectType1GameObject : target.ReactionEffectType2GameObject;
                reactionEffectGameObject.transform.Find("Text").GetComponent<Text>().text =
                    Mathf.Abs(damagedPoint).ToString();
            });
            return GenerateDamagedActionAndRecoveryActionSequence(targets, index);
        }

        private Sequence GenerateDamagedActionAndRecoveryActionSequence(List<TargetData> targets, int index)
        {
            var sequence = DOTween.Sequence();


            var first = targets.First();
            var others = targets.Skip(1).ToList();

            sequence
                .Append(ToDamagedHp(first))
                .Join(FadeInDamagedEffect(first, index));
            others.ForEach(target =>
            {
                sequence
                    .Join(ToDamagedHp(target))
                    .Join(FadeInDamagedEffect(target, index));
            });

            sequence
                .Append(FadeOutDamagedEffect(first, index));

            others.ForEach(target =>
            {
                sequence
                    .Join(FadeOutDamagedEffect(target, index));
            });

            if (first.BattleObject.CurrentHp == 0)
            {
                var firstTargetSpriteRenderer = first.BattleObject.GameObject.GetComponent<SpriteRenderer>();
                var firstTargetCanvasGroup =
                    first.BattleObject.GameObject.transform.Find("HP").GetComponent<CanvasGroup>();
                sequence.Append(firstTargetSpriteRenderer.DOFade(0, 1));
                sequence.Join(firstTargetCanvasGroup.DOFade(0, 1));
            }

            others.ForEach(target =>
            {
                if (target.BattleObject.CurrentHp != 0) return;
                var otherTargetSpriteRenderer = target.BattleObject.GameObject.GetComponent<SpriteRenderer>();
                var otherTargetCanvasGroup =
                    target.BattleObject.GameObject.transform.Find("HP").GetComponent<CanvasGroup>();
                sequence.Join(otherTargetSpriteRenderer.DOFade(0, 1));
                sequence.Join(otherTargetCanvasGroup.DOFade(0, 1));
            });

            return sequence;
        }

        private Sequence GenerateBuffActionSequence()
        {
            var sequence = DOTween.Sequence();

            return sequence;
        }

        private Sequence GenerateDeBuffActionSequence()
        {
            var sequence = DOTween.Sequence();

            return sequence;
        }

        private Sequence GenerateReleaseActionSequence()
        {
            var sequence = DOTween.Sequence();

            return sequence;
        }

        private Tween ToDamagedHp(TargetData targetData)
        {
            return DOTween.To(
                () => targetData.HpSlider.value,
                value => targetData.HpSlider.value = value,
                CalculateDamagedHpSlider(targetData.BattleObject),
                0.5f);
        }

        private Tween FadeInDamagedEffect(TargetData targetData, int index)
        {
            var reactionEffectGameObject = index == 1
                ? targetData.ReactionEffectType1GameObject
                : targetData.ReactionEffectType2GameObject;
            var damagedEffectCanvasGroup = reactionEffectGameObject.GetComponent<CanvasGroup>();
            return DOTween.To(
                () => damagedEffectCanvasGroup.alpha,
                value => damagedEffectCanvasGroup.alpha = value,
                1,
                0.5f);
        }

        private Tween FadeOutDamagedEffect(TargetData targetData, int index)
        {
            var reactionEffectGameObject = index == 1
                ? targetData.ReactionEffectType1GameObject
                : targetData.ReactionEffectType2GameObject;
            var damagedEffectCanvasGroup = reactionEffectGameObject.GetComponent<CanvasGroup>();
            return DOTween.To(
                () => damagedEffectCanvasGroup.alpha,
                value => damagedEffectCanvasGroup.alpha = value,
                0,
                0.5f);
        }

        private float CalculateDamagedHpSlider(BattleObject targetBattleObject)
        {
            return 0 < targetBattleObject.CurrentHp
                ? (float) targetBattleObject.CurrentHp / targetBattleObject.MaxHp
                : 0;
        }
    }
}
