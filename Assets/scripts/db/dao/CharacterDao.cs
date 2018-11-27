using System;
using System.Collections.Generic;
using System.Text;
using battle;
using Plugins;
using script.db.entity;

namespace db.dao
{
    public class CharacterDao : MergeDao
    {

        // 唯一のインスタンス
        private static CharacterDao instance;

        public static CharacterDao Instance {
            get {
                if (instance == null) {
                    instance = new CharacterDao();
                }
                return instance;
            }
        }

        public void Merge(SqliteDatabase oldDb) {
            foreach (CharacterEntity oldData in instance.SelectAll(oldDb)) {
                if (instance.SelectByPrimaryKey(oldData.Id, oldDb) == null) {
                    Insert(oldData, oldDb);
                }
                else {
                    Update(oldData, oldDb);
                }
            }
        }

        public List<CharacterEntity> SelectAll(SqliteDatabase mdb = null)
        {
            List<CharacterEntity> entityList = new List<CharacterEntity>();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM character;");
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

        public CharacterEntity SelectByPrimaryKey(int id, SqliteDatabase mdb = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM character WHERE id = ")
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

        public void Insert(CharacterEntity entity, SqliteDatabase mdb = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO character VALUES (")
            
                
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
            
                
                .Append(entity.Creator)
                
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
            
                
                .Append((int) entity.Skill2)
                
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
            
                
                .Append(entity.MinLevelUp)
                
                .Append(",")
            
                
                .Append(entity.MaxLevelUp)
                
                
            
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

        public void Update(CharacterEntity entity, SqliteDatabase mdb = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE character SET ")
            
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
            
                .Append("creator = ")
                
                .Append(entity.Creator)
                
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
            
                .Append("skill2 = ")
                
                .Append((int) entity.Skill2)
                
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
            
                .Append("min_level_up = ")
                
                .Append(entity.MinLevelUp)
                
                .Append(",")
            
                .Append("max_level_up = ")
                
                .Append(entity.MaxLevelUp)
                
                
            
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

        private CharacterEntity CreateEntity(DataRow row)
        {
            CharacterEntity entity = new CharacterEntity();
            
            entity.Id = DaoSupport.GetIntValue(row, "id");
            
            entity.SystemName = DaoSupport.GetStringValue(row, "system_name");
            
            entity.DisplayName = DaoSupport.GetStringValue(row, "display_name");
            
            entity.Lang = DaoSupport.GetStringValue(row, "lang");
            
            entity.Creator = DaoSupport.GetIntValue(row, "creator");
            
            entity.Attack = DaoSupport.GetIntValue(row, "attack");
            
            entity.Defense = DaoSupport.GetIntValue(row, "defense");
            
            entity.Mind = DaoSupport.GetIntValue(row, "mind");
            
            entity.Speed = DaoSupport.GetIntValue(row, "speed");
            
            entity.MaxHp = DaoSupport.GetIntValue(row, "max_hp");
            
            entity.Skill1 = DaoSupport.GetIntValue(row, "skill1");

            entity.Skill2 = DaoSupport.GetIntValue(row, "skill2");
            
            entity.AttackRate = DaoSupport.GetIntValue(row, "attack_rate");
            
            entity.DefenseRate = DaoSupport.GetIntValue(row, "defense_rate");
            
            entity.MindRate = DaoSupport.GetIntValue(row, "mind_rate");
            
            entity.SpeedRate = DaoSupport.GetIntValue(row, "speed_rate");
            
            entity.MaxHpRate = DaoSupport.GetIntValue(row, "max_hp_rate");
            
            entity.MinLevelUp = DaoSupport.GetIntValue(row, "min_level_up");
            
            entity.MaxLevelUp = DaoSupport.GetIntValue(row, "max_level_up");
            
            return entity;
        }
    }
}
