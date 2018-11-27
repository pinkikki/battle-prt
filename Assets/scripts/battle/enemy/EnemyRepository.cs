using System.Collections.Generic;
using System.Linq;
using db.dao;
using UnityEngine;

namespace battle.enemy
{
    public class EnemyRepository
    {
        private static EnemyRepository _instance;
        
        public static readonly string ENEMY_GAME_OBJECT_KEY = "Enemy";
        public static readonly string ENEMY1_GAME_OBJECT_KEY = ENEMY_GAME_OBJECT_KEY + "1";
        public static readonly string ENEMY2_GAME_OBJECT_KEY = ENEMY_GAME_OBJECT_KEY + "2";
        public static readonly string ENEMY3_GAME_OBJECT_KEY = ENEMY_GAME_OBJECT_KEY + "3";
        public Dictionary<string, BattleObject> Enemies { get; } = new Dictionary<string, BattleObject>();

        private EnemyRepository(){}
        
        public static EnemyRepository Instance
        {
            get{
                if (_instance == null)
                {
                    _instance = new EnemyRepository();
                    // TODO クエストのセクションID
                    var questEnemyEntities = QuestEnemyDao.Instance.SelectBySectionId(1);
                    Enumerable.Range(1, 3).ToList().ForEach(i =>
                    {
                        var no = Random.Range(0, questEnemyEntities.Count);
                        var questEnemyEntity = questEnemyEntities[no];
                        var enemyEntity = EnemyDao.Instance.SelectByPrimaryKey(questEnemyEntity.EnemyId);
    
                        _instance.Enemies.Add(ENEMY_GAME_OBJECT_KEY + i, new BattleObject
                        {
                            Id = enemyEntity.Id,
                            SystemName = enemyEntity.SystemName,
                            DisplayName = enemyEntity.DisplayName,
                            Attack = Calculate(enemyEntity.Attack, enemyEntity.LevelUp, enemyEntity.AttackRate, questEnemyEntity.Level),
                            Defense = Calculate(enemyEntity.Defense, enemyEntity.LevelUp, enemyEntity.DefenseRate, questEnemyEntity.Level),
                            Mind = Calculate(enemyEntity.Mind, enemyEntity.LevelUp, enemyEntity.MindRate, questEnemyEntity.Level),
                            Speed = Calculate(enemyEntity.Speed, enemyEntity.LevelUp, enemyEntity.SpeedRate, questEnemyEntity.Level),
                            CurrentHp = Calculate(enemyEntity.MaxHp, enemyEntity.LevelUp, enemyEntity.MaxHpRate, questEnemyEntity.Level),
                            MaxHp = Calculate(enemyEntity.MaxHp, enemyEntity.LevelUp, enemyEntity.MaxHpRate, questEnemyEntity.Level),
                            Skill1 = SkillDao.Instance.SelectByPrimaryKey(enemyEntity.Skill1),
                            CharacterType = CharacterType.Enemy
                        });
                    });
                }
    
                return _instance;
            }
        }
        
        public void SetGameObject(string key, GameObject obj)
        {
            _instance.Enemies[key].GameObject = obj;
        }
        
        public static void Destroy()
        {
            _instance = null;
        }
        
        public bool Over()
        {
            return Enemies.Values.All(battleObject => battleObject.CurrentHp == 0);
        }

        private static int Calculate(int original, int levelUp, int rate, int level)
        {
            return original + Mathf.FloorToInt(levelUp * rate / 100 * level);
        }
    }
}
