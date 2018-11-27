using System.Collections.Generic;
using System.Linq;
using db.dao;
using script.db.entity;
using UnityEngine;

namespace battle.character
{
    public class CharacterRepository
    {
        private static CharacterRepository _instance;

        public static readonly string CHARACTER_GAME_OBJECT_KEY = "Character";
        public static readonly string CHARACTER1_GAME_OBJECT_KEY = CHARACTER_GAME_OBJECT_KEY + "1";
        public static readonly string CHARACTER2_GAME_OBJECT_KEY = CHARACTER_GAME_OBJECT_KEY + "2";
        public static readonly string CHARACTER3_GAME_OBJECT_KEY = CHARACTER_GAME_OBJECT_KEY + "3";
        public Dictionary<string, BattleObject> Characters { get; } = new Dictionary<string, BattleObject>();

        private CharacterRepository()
        {
        }

        public static CharacterRepository Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CharacterRepository();
                    // TODO クエストのデッキID
                    var deckEntity = DeckDao.Instance.SelectByPrimaryKey(1);
                    _instance.Characters.Add(CHARACTER1_GAME_OBJECT_KEY, CreateBattleObject(deckEntity.Character1));
                    _instance.Characters.Add(CHARACTER2_GAME_OBJECT_KEY, CreateBattleObject(deckEntity.Character2));
                    _instance.Characters.Add(CHARACTER3_GAME_OBJECT_KEY, CreateBattleObject(deckEntity.Character3));
                }

                return _instance;
            }
        }

        public void SetGameObject(string key, GameObject obj)
        {
            _instance.Characters[key].GameObject = obj;
        }

        public void SetSkillButtonsGameObject(string key, GameObject obj)
        {
            _instance.Characters[key].SkillButtonsGameObject = obj;
        }

        public void Save()
        {
            Characters.Values.ToList().ForEach(character =>
                TeamCharacterDao.Instance.Update(CreateTeamCharacterEntity(character)));
        }

        public static void Destroy()
        {
            _instance = null;
        }

        public bool Over()
        {
            return Characters.Values.All(battleObject => battleObject.CurrentHp == 0);
        }

        private static BattleObject CreateBattleObject(int id)
        {
            var teamCharacterEntity = TeamCharacterDao.Instance.SelectByPrimaryKey(id);
            return new BattleObject
            {
                Id = teamCharacterEntity.Id,
                SystemName = teamCharacterEntity.SystemName,
                DisplayName = teamCharacterEntity.DisplayName,
                Creator = teamCharacterEntity.Creator,
                Attack = teamCharacterEntity.Attack,
                Defense = teamCharacterEntity.Defense,
                Mind = teamCharacterEntity.Mind,
                Speed = teamCharacterEntity.Speed,
                CurrentHp = teamCharacterEntity.CurrentHp,
                MaxHp = teamCharacterEntity.MaxHp,
                Skill1 = SkillDao.Instance.SelectByPrimaryKey(teamCharacterEntity.Skill1),
                Skill2 = SkillDao.Instance.SelectByPrimaryKey(teamCharacterEntity.Skill2),
                LeaderSkill = SkillDao.Instance.SelectByPrimaryKey(teamCharacterEntity.LeaderSkill),
                Sense = teamCharacterEntity.Sense,
                CharacterType = CharacterType.Character
            };
        }

        private static TeamCharacterEntity CreateTeamCharacterEntity(BattleObject battleObject)
        {
            var teamCharacterEntity = TeamCharacterDao.Instance.SelectByPrimaryKey(battleObject.Id);
            return new TeamCharacterEntity
            {
                Id = teamCharacterEntity.Id,
                SystemName = teamCharacterEntity.SystemName,
                DisplayName = teamCharacterEntity.DisplayName,
                Lang = teamCharacterEntity.Lang,
                Creator = teamCharacterEntity.Creator,
                Level = teamCharacterEntity.Level,
                Attack = teamCharacterEntity.Attack,
                Defense = teamCharacterEntity.Defense,
                Mind = teamCharacterEntity.Mind,
                Speed = teamCharacterEntity.Speed,
                CurrentHp = battleObject.CurrentHp,
                MaxHp = teamCharacterEntity.MaxHp,
                Skill1 = teamCharacterEntity.Skill1,
                Skill2 = teamCharacterEntity.Skill2,
                LeaderSkill = teamCharacterEntity.LeaderSkill,
                Sense = teamCharacterEntity.Sense
            };
        }
    }
}
