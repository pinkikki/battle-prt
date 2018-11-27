using System;
using System.Collections.Generic;
using System.Linq;
using battle.action;
using battle.character;
using battle.enemy;
using battle.partner;
using DG.Tweening;
using monoBehaviour;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Action = battle.action.Action;
using Object = UnityEngine.Object;

namespace battle
{
    public class BattleSystem : SingletonMonoBehaviour<BattleSystem>
    {
        private readonly ActionPipeline _actionPipeline = new ActionPipeline();
        public Subject<Status> Subject { get; } = new Subject<Status>();

        void Start()
        {
            var characterRepository = CharacterRepository.Instance;
            InstantiateForCharactersAndSkillButtons(characterRepository);

            var enemyRepository = EnemyRepository.Instance;
            InstantiateForEnemies(enemyRepository);

            var partnerRepository = PartnerRepository.Instance;

            RegisterPartnersIconEvent(partnerRepository);

            characterRepository.Characters.Values.ToList().ForEach(battleObject =>
            {
                var skill1TimeBarGameObject = battleObject.SkillButtonsGameObject.transform.Find("Skill1/TimeBar");
                var skill2TimeBarGameObject = battleObject.SkillButtonsGameObject.transform.Find("Skill2/TimeBar");
                var skill1Button = skill1TimeBarGameObject.GetComponent<Button>();
                var skill2Button = skill2TimeBarGameObject.GetComponent<Button>();

                RegisterSkillButtonEvent(characterRepository, skill1Button, skill1TimeBarGameObject.gameObject,
                    battleObject,
                    Action.Type.Skill1);
                RegisterSkillButtonEvent(characterRepository, skill2Button, skill2TimeBarGameObject.gameObject,
                    battleObject,
                    Action.Type.Skill2);
            });


            ApplyCurrentHp(characterRepository);

            Subject.Where(status => status == Status.Prepare).Subscribe(status => Prepare());
            Subject.Where(status => status == Status.InAction).Subscribe(status => Progress());
            Subject.Where(status => status == Status.AfterAction).Subscribe(status => After(), Over);

            Observable.Timer(TimeSpan.FromSeconds(2)).Subscribe(_ => Subject.OnNext(Status.Prepare)).AddTo(gameObject);
            foreach (var character in characterRepository.Characters)
            {
                var skillButtonGameObject =
                    GameObject.Find("Control_UI/Panel/Base/Characters/" + character.Value.SystemName);
                var skill1Image = skillButtonGameObject.transform.Find("Skill1/TimeBar").transform
                    .GetComponent<Image>();
                var skill2Image = skillButtonGameObject.transform.Find("Skill2/TimeBar").transform
                    .GetComponent<Image>();

                skill1Image.DOFillAmount(1.0f, character.Value.Skill1.Seconds).Play();
                skill2Image.DOFillAmount(1.0f, character.Value.Skill2.Seconds).Play();
            }
        }

        private void Prepare()
        {
            var sortedBattleObjects = Sorter.Instance.SortBySpeed(
                CharacterRepository.Instance.Characters.Values.Where(battleObject => battleObject.CurrentHp != 0)
                    .ToList(),
                EnemyRepository.Instance.Enemies.Values.Where(battleObject => battleObject.CurrentHp != 0).ToList());
            sortedBattleObjects.ForEach(battleObject => _actionPipeline.PutForNormal(
                new Action
                {
                    AttackType = Action.Type.Normal,
                    BattleObject = battleObject
                }));
            Subject.OnNext(Status.InAction);
        }

        private void Progress()
        {
            var action = _actionPipeline.Next();

            var targetBattleObjects = AttackTargetExtractor.Instance.Extract(action);
            var targets = targetBattleObjects.Select(targetBattleObject => new TargetData
            {
                BattleObject = targetBattleObject,
                ReactionEffectType1GameObject = InstantiateForReactionEffect(targetBattleObject,
                    action.AttackType == Action.Type.Normal
                        ? ReactionEffectExtractor.Instance.ExtractParticleNameForNormal()
                        : ReactionEffectExtractor.Instance.ExtractParticleNameForSkillType1(action)),
                ReactionEffectType2GameObject = InstantiateForReactionEffect(targetBattleObject,
                    ReactionEffectExtractor.Instance.ExtractParticleNameForSkillType2(action)),
                HpSlider = targetBattleObject.GameObject.transform.Find("HP/Slider").GetComponent<Slider>()
            }).ToList();

            var sequence = DOTween.Sequence();
            var particleTime = action.AttackType == Action.Type.Normal ? 0.2f : 3.0f;
            sequence
                .Append(ActionSequenceGenerator.Instance.GenerateInitialActionSequence(action))
                .AppendCallback(() =>
                {
                    var particle = (GameObject) Instantiate(Resources.Load<Object>(
                            $"prefab/particles/{GetParticleName(action)}"),
                        Vector2.zero, Quaternion.identity);

                    if (targets.Count == 1)
                    {
                        particle.transform.SetParent(targets[0].BattleObject.GameObject.transform, false);
                    }
                    else
                    {
                        var parentObject = targets[0].BattleObject.IsCharacter() || targets[0].BattleObject.IsPartner()
                            ? CharacterRepository.Instance.Characters[CharacterRepository.CHARACTER2_GAME_OBJECT_KEY]
                                .GameObject
                            : EnemyRepository.Instance.Enemies[EnemyRepository.ENEMY2_GAME_OBJECT_KEY]
                                .GameObject;
                        particle.transform.SetParent(parentObject.transform, false);
                    }
                })
                .AppendInterval(particleTime);

            sequence
                .Append(ActionSequenceGenerator.Instance.GenerateReactionSequence(action, targets));

            targets.ForEach(target =>
            {
                sequence.AppendCallback(() =>
                {
                    Destroy(target.ReactionEffectType1GameObject);
                    Destroy(target.ReactionEffectType2GameObject);
                });
            });

            sequence.Play();
        }

        private void After()
        {
            var sequence = DOTween.Sequence();
            sequence
                .AppendInterval(1.0f)
                .OnComplete(() =>
                {
                    if (CharacterRepository.Instance.Over() || EnemyRepository.Instance.Over())
                    {
                        Subject.OnCompleted();
                    }
                    else
                    {
                        var action = _actionPipeline.Get();
                        if (action.BattleObject.IsCharacter() && action.AttackType != Action.Type.Normal)
                        {
                            CharacterRepository.Instance.Characters.Values.ToList().ForEach(character =>
                            {
                                character.SkillButtonsGameObject.transform.Find("Skill1/TimeBar")
                                    .GetComponent<Button>().interactable = true;
                                character.SkillButtonsGameObject.transform.Find("Skill2/TimeBar")
                                    .GetComponent<Button>().interactable = true;
                            });
                            var timeBarImage = action.BattleObject.SkillButtonsGameObject.transform
                                .Find($"{action.AttackType}/TimeBar").GetComponent<Image>();
                            timeBarImage.fillAmount = 0;
                            timeBarImage.DOFillAmount(1.0f, action.BattleObject.Skill1.Seconds).Play();
                        }

                        if (action.BattleObject.IsPartner())
                        {
                            var partnerBattleObject = (PartnerBattleObject) action.BattleObject;
                            partnerBattleObject.TimeBarTransform.gameObject.SetActive(true);
                            partnerBattleObject.CommentText.text =
                                PartnerCommentExtractor.Instance.ExtractWithMiddle(partnerBattleObject.PartnerTransform
                                    .name);
                            partnerBattleObject.TimeBarImage.fillAmount = 0.0f;
                            RegisterPartnersSkillEvent(PartnerRepository.Instance, partnerBattleObject.PartnerTransform,
                                partnerBattleObject.TimeBarTransform,
                                partnerBattleObject.TimeBarImage, partnerBattleObject.IconTransform,
                                partnerBattleObject.CommentText);
                        }

                        Subject.OnNext(Status.Prepare);
                    }
                });
            sequence.Play();
        }

        private void Over()
        {
            CharacterRepository.Instance.Save();
            Debug.Log("Over");
        }

        private string GetParticleName(Action action)
        {
            switch (action.AttackType)
            {
                case Action.Type.Normal:
                    return "normal_attack";
                case Action.Type.Skill1:
                    return action.BattleObject.Skill1.SystemName;
                case Action.Type.Skill2:
                    return action.BattleObject.Skill2.SystemName;
                default:
                    return "normal_attack";
            }
        }

        private GameObject InstantiateForReactionEffect(BattleObject parent, string particleName)
        {
            if (particleName == null)
            {
                return null;
            } 
            var obj = (GameObject) Instantiate(Resources.Load<Object>(particleName),
                Vector2.zero, Quaternion.identity);
            var canvasGroup = obj.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
            obj.transform.SetParent(parent.GameObject.transform, false);
            return obj;
        }

        private void RegisterSkillButtonEvent(CharacterRepository characterRepository, Button skillButton,
            GameObject skill1TimeBarGameObject, BattleObject battleObject, Action.Type actionType)
        {
            skillButton.onClick.AsObservable()
                .Where(_ => Mathf.Approximately(skill1TimeBarGameObject.GetComponent<Image>().fillAmount, 1.0f))
                .Subscribe(_ =>
                {
                    characterRepository.Characters.Values.ToList().ForEach(character =>
                    {
                        character.SkillButtonsGameObject.transform.Find("Skill1/TimeBar")
                            .GetComponent<Button>().interactable = false;
                        character.SkillButtonsGameObject.transform.Find("Skill2/TimeBar")
                            .GetComponent<Button>().interactable = false;
                    });
                    skillButton.interactable = true;
                    skillButton.enabled = false;
                    _actionPipeline.PutForSkill(
                        new Action
                        {
                            AttackType = actionType,
                            BattleObject = battleObject
                        });
                })
                .AddTo(gameObject);
        }

        private void RegisterPartnersIconEvent(PartnerRepository partnerRepository)
        {
            var partners = GameObject.Find("Control_UI/Panel/Base/Partners");
            new List<Transform>
            {
                partners.transform.Find(PartnerRepository.YUSUKE_GAME_OBJECT_KEY),
                partners.transform.Find(PartnerRepository.MASAKI_GAME_OBJECT_KEY),
                partners.transform.Find(PartnerRepository.AKO_GAME_OBJECT_KEY)
            }.ForEach(partner =>
            {
                var timeBarTransform = partner.Find("TimeBar");
                var timeBarImage = timeBarTransform.GetComponent<Image>();
                var iconTransform = partner.Find("Icon");
                var commentText = partner.Find("Comment/Text").GetComponent<Text>();
                commentText.text = PartnerCommentExtractor.Instance.ExtractWithMiddle(partner.name);
                partnerRepository.Partners[partner.name].PartnerTransform = partner;
                partnerRepository.Partners[partner.name].IconTransform = iconTransform;
                partnerRepository.Partners[partner.name].TimeBarTransform = timeBarTransform;
                partnerRepository.Partners[partner.name].TimeBarImage = timeBarImage;
                partnerRepository.Partners[partner.name].CommentText = commentText;

                iconTransform.GetComponent<Button>().onClick.AsObservable()
                    .Subscribe(_ =>
                    {
                        if (timeBarImage.fillAmount < 1.0f)
                        {
                            timeBarImage.fillAmount = timeBarImage.fillAmount + 0.51f;
                        }
                    })
                    .AddTo(gameObject);

                RegisterPartnersSkillEvent(partnerRepository, partner, timeBarTransform, timeBarImage, iconTransform,
                    commentText);
            });
        }

        private void RegisterPartnersSkillEvent(PartnerRepository partnerRepository, Transform partnerTransform,
            Transform timeBarTransform,
            Image timeBarImage,
            Transform iconTransform, Text commentText)
        {
            this.UpdateAsObservable()
                .Where(_ => Mathf.Approximately(timeBarImage.fillAmount, 1.0f))
                .Take(1)
                .Subscribe(__ =>
                {
                    timeBarTransform.gameObject.SetActive(false);
                    var swipeArrowGameObject = (GameObject) Instantiate(Resources.Load<Object>(
                            "prefab/characters/SwipeArrow"),
                        new Vector2(-10.0f, -30.0f), Quaternion.identity);
                    swipeArrowGameObject.transform.SetParent(partnerTransform, false);
                    var partnerSkillReadyGameObject = (GameObject) Instantiate(Resources.Load<Object>(
                            "prefab/particles/partner_skill_ready"),
                        new Vector2(0.0f, -10.0f), Quaternion.identity);
                    partnerSkillReadyGameObject.transform.SetParent(iconTransform, false);
                    commentText.text = PartnerCommentExtractor.Instance.ExtractWithMax(partnerTransform.name);

                    this.UpdateAsObservable()
                        .Where(___ =>
                        {
                            if (0 < Input.touchCount)
                            {
                                var t = Input.touches[0];
                                switch (t.phase)
                                {
                                    case TouchPhase.Began:
                                        _touchStartPos = new Vector2(t.position.x, t.position.y);
                                        return false;
                                    case TouchPhase.Ended:
                                    {
                                        if (Camera.main == null ||
                                            !SwipeChecker.Instance.CheckStartPosition(partnerTransform,
                                                Camera.main.ScreenToWorldPoint(_touchStartPos)))
                                        {
                                            return false;
                                        }

                                        _touchEndPos = new Vector2(t.position.x, t.position.y);
                                        break;
                                    }
                                    default:
                                        return false;
                                }
                            }

                            if (Input.GetKeyDown(KeyCode.Mouse0))
                            {
                                _touchStartPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                                return false;
                            }

                            if (!Input.GetKeyUp(KeyCode.Mouse0))
                                return SwipeChecker.Instance.CheckEndPosition(_touchStartPos, _touchEndPos);
                            if (Camera.main == null || !SwipeChecker.Instance.CheckStartPosition(partnerTransform,
                                    Camera.main.ScreenToWorldPoint(_touchStartPos)))
                            {
                                return false;
                            }

                            _touchEndPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

                            return SwipeChecker.Instance.CheckEndPosition(_touchStartPos, _touchEndPos);
                        })
                        .Take(1)
                        .Subscribe(____ =>
                        {
                            Debug.Log("character skill 発動！！");

                            _touchStartPos = Vector2.zero;
                            _touchEndPos = Vector2.zero;

                            _actionPipeline.PutForSkill(
                                new Action
                                {
                                    AttackType = Action.Type.Skill1,
                                    BattleObject = partnerRepository.Partners[partnerTransform.name]
                                });

                            Destroy(swipeArrowGameObject);
                            Destroy(partnerSkillReadyGameObject);
                        }).AddTo(gameObject);
                });
        }

        private Vector2 _touchStartPos;
        private Vector2 _touchEndPos;

        private void ApplyCurrentHp(CharacterRepository characterRepository)
        {
            characterRepository.Characters.Values.ToList().ForEach(battleObject =>
            {
                var hpSlider = battleObject.GameObject.transform.Find("HP/Slider").GetComponent<Slider>();
                hpSlider.value = (float) battleObject.CurrentHp / battleObject.MaxHp;

                if (battleObject.CurrentHp == 0)
                {
                    var spriteRenderer = battleObject.GameObject.GetComponent<SpriteRenderer>();
                    var canvasGroup = battleObject.GameObject.transform.Find("HP").GetComponent<CanvasGroup>();
                    var spriteRendererColor = spriteRenderer.color;
                    spriteRendererColor.a = 0;
                    spriteRenderer.color = spriteRendererColor;
                    canvasGroup.alpha = 0;
                }
            });
        }

        private void InstantiateForCharactersAndSkillButtons(CharacterRepository characterRepository)
        {
            var charactersObj = GameObject.Find("Fighters/Characters");
            var skillUiObj = GameObject.Find("Control_UI/Panel/Base/Characters");
            Enumerable.Range(1, characterRepository.Characters.Count).ToList().ForEach(i =>
            {
                var systemName = characterRepository.Characters[CharacterRepository.CHARACTER_GAME_OBJECT_KEY + i]
                    .SystemName;
                var obj = (GameObject) Instantiate(Resources.Load<Object>(
                        "prefab/characters/" + systemName),
                    new Vector2(-0.4f + 0.2f * i, 3.4f - 1.7f * i), Quaternion.identity);
                obj.transform.SetParent(charactersObj.transform, false);
                characterRepository.SetGameObject(CharacterRepository.CHARACTER_GAME_OBJECT_KEY + i, obj);

                var skillButtonsObj = (GameObject) Instantiate(Resources.Load<Object>(
                        "prefab/skills/" +
                        systemName),
                    Vector2.zero, Quaternion.identity);
                skillButtonsObj.name = systemName;
                skillButtonsObj.transform.SetParent(skillUiObj.transform, false);
                characterRepository.SetSkillButtonsGameObject(CharacterRepository.CHARACTER_GAME_OBJECT_KEY + i,
                    skillButtonsObj);
            });
        }

        private void InstantiateForEnemies(EnemyRepository enemyRepository)
        {
            var enemiesObj = GameObject.Find("Fighters/Enemies");
            Enumerable.Range(1, enemyRepository.Enemies.Count).ToList().ForEach(i =>
            {
                var obj = (GameObject) Instantiate(Resources.Load<Object>(
                        "prefab/enemies/" + enemyRepository.Enemies[EnemyRepository.ENEMY_GAME_OBJECT_KEY + i]
                            .SystemName),
                    new Vector2(0.4f - 0.2f * i, 3.4f - 1.7f * i), Quaternion.identity);
                obj.transform.SetParent(enemiesObj.transform, false);
                enemyRepository.SetGameObject(EnemyRepository.ENEMY_GAME_OBJECT_KEY + i, obj);
            });
        }

        public enum Status
        {
            Prepare,
            InAction,
            AfterAction,
            Over
        }
    }
}
