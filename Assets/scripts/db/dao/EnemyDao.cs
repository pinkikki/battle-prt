using System;
using System.Collections.Generic;
using System.Text;
using battle;
using Plugins;
using script.db.entity;

namespace db.dao
{
    public class EnemyDao : MergeDao
    {

        // 唯一のインスタンス
        private static EnemyDao instance;

        public static EnemyDao Instance {
            get {
                if (instance == null) {
                    instance = new EnemyDao();
                }
                return instance;
            }
        }

        public void Merge(SqliteDatabase oldDb) {
            foreach (EnemyEntity oldData in instance.SelectAll(oldDb)) {
                if (instance.SelectByPrimaryKey(oldData.Id, oldDb) == null) {
                    Insert(oldData, oldDb);
                }
                else {
                    Update(oldData, oldDb);
                }
            }
        }

        public List<EnemyEntity> SelectAll(SqliteDatabase mdb = null)
        {
            List<EnemyEntity> entityList = new List<EnemyEntity>();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM enemy;");
            DataTable dataTable;
            if (mdb == null)
            {
                dataTable = DbManager.Instance.ExecuteQuery(sb.ToString());
            }
            else
            {
                dataTable = DbManager.Instance.ExecuteQuery(sb.ToString(), mdb);

            }
            dataTable.Rows.ForEach(r => entityList.Add(instance.CreateEntity(r)));
            return entityList;
        }

        public EnemyEntity SelectByPrimaryKey(int id, SqliteDatabase mdb = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM enemy WHERE id = ")
                .Append(id)
                .Append(";");
            DataTable dataTable;
            if (mdb == null)
            {
                dataTable = DbManager.Instance.ExecuteQuery(sb.ToString());
            }
            else
            {
                dataTable = DbManager.Instance.ExecuteQuery(sb.ToString(), mdb);

            }
            return dataTable.Rows.Count == 0 ? null : instance.CreateEntity(dataTable[0]);
        }

        public void Insert(EnemyEntity entity, SqliteDatabase mdb = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO enemy VALUES (")
            
                
                .Append(entity.Id)
                
                .Append(",")
            
                .Append("'")
                .Append(entity.SystemName)
                .Append("'")
                .Append(",")
            
                .Append("'")
                .Append(entity.DisplayName)
                .Append("'")
                .Append(",")
            
                .Append("'")
                .Append(entity.Lang)
                .Append("'")
                .Append(",")
            
                
                .Append(entity.Attack)
                
                .Append(",")
            
                
                .Append(entity.Defense)
                
                .Append(",")
            
                
                .Append(entity.Mind)
                
                .Append(",")
            
                
                .Append(entity.Speed)
                
                .Append(",")
            
                
                .Append(entity.MaxHp)
                
                .Append(",")
            
                
                .Append((int) entity.Skill1)
                
                .Append(",")
            
                
                .Append(entity.AttackRate)
                
                .Append(",")
            
                
                .Append(entity.DefenseRate)
                
                .Append(",")
            
                
                .Append(entity.MindRate)
                
                .Append(",")
            
                
                .Append(entity.SpeedRate)
                
                .Append(",")
            
                
                .Append(entity.MaxHpRate)
                
                .Append(",")
            
                
                .Append(entity.LevelUp)
                
                
            
                .Append(");");
                if (mdb == null)
                {
                    DbManager.Instance.ExecuteNonQuery(sb.ToString());
                }
                else
                {
                    DbManager.Instance.ExecuteNonQuery(sb.ToString(), mdb);
                }

        }

        public void Update(EnemyEntity entity, SqliteDatabase mdb = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE enemy SET ")
            
                .Append("id = ")
                
                .Append(entity.Id)
                
                .Append(",")
            
                .Append("system_name = ")
                .Append("'")
                .Append(entity.SystemName)
                .Append("'")
                .Append(",")
            
                .Append("display_name = ")
                .Append("'")
                .Append(entity.DisplayName)
                .Append("'")
                .Append(",")
            
                .Append("lang = ")
                .Append("'")
                .Append(entity.Lang)
                .Append("'")
                .Append(",")
            
                .Append("attack = ")
                
                .Append(entity.Attack)
                
                .Append(",")
            
                .Append("defense = ")
                
                .Append(entity.Defense)
                
                .Append(",")
            
                .Append("mind = ")
                
                .Append(entity.Mind)
                
                .Append(",")
            
                .Append("speed = ")
                
                .Append(entity.Speed)
                
                .Append(",")
            
                .Append("max_hp = ")
                
                .Append(entity.MaxHp)
                
                .Append(",")
            
                .Append("skill1 = ")
                
                .Append((int) entity.Skill1)
                
                .Append(",")
            
                .Append("attack_rate = ")
                
                .Append(entity.AttackRate)
                
                .Append(",")
            
                .Append("defense_rate = ")
                
                .Append(entity.DefenseRate)
                
                .Append(",")
            
                .Append("mind_rate = ")
                
                .Append(entity.MindRate)
                
                .Append(",")
            
                .Append("speed_rate = ")
                
                .Append(entity.SpeedRate)
                
                .Append(",")
            
                .Append("max_hp_rate = ")
                
                .Append(entity.MaxHpRate)
                
                .Append(",")
            
                .Append("level_up = ")
                
                .Append(entity.LevelUp)
                
                
            
                .Append(" WHERE id = ")
                .Append(entity.Id)
                .Append(";");
                if (mdb == null)
                {
                    DbManager.Instance.ExecuteNonQuery(sb.ToString());
                }
                else
                {
                    DbManager.Instance.ExecuteNonQuery(sb.ToString(), mdb);
                }

        }

        private EnemyEntity CreateEntity(DataRow row)
        {
            EnemyEntity entity = new EnemyEntity();
            
            entity.Id = DaoSupport.GetIntValue(row, "id");
            
            entity.SystemName = DaoSupport.GetStringValue(row, "system_name");
            
            entity.DisplayName = DaoSupport.GetStringValue(row, "display_name");
            
            entity.Lang = DaoSupport.GetStringValue(row, "lang");
            
            entity.Attack = DaoSupport.GetIntValue(row, "attack");
            
            entity.Defense = DaoSupport.GetIntValue(row, "defense");
            
            entity.Mind = DaoSupport.GetIntValue(row, "mind");
            
            entity.Speed = DaoSupport.GetIntValue(row, "speed");
            
            entity.MaxHp = DaoSupport.GetIntValue(row, "max_hp");
            
            entity.Skill1 = DaoSupport.GetIntValue(row, "skill1");
            
            entity.AttackRate = DaoSupport.GetIntValue(row, "attack_rate");
            
            entity.DefenseRate = DaoSupport.GetIntValue(row, "defense_rate");
            
            entity.MindRate = DaoSupport.GetIntValue(row, "mind_rate");
            
            entity.SpeedRate = DaoSupport.GetIntValue(row, "speed_rate");
            
            entity.MaxHpRate = DaoSupport.GetIntValue(row, "max_hp_rate");
            
            entity.LevelUp = DaoSupport.GetIntValue(row, "level_up");
            
            return entity;
        }
    }
}
