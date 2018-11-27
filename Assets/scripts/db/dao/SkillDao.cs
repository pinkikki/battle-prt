using System;
using System.Collections.Generic;
using System.Text;
using battle;
using Plugins;
using script.db.entity;

namespace db.dao
{
    public class SkillDao : MergeDao
    {

        // 唯一のインスタンス
        private static SkillDao instance;

        public static SkillDao Instance {
            get {
                if (instance == null) {
                    instance = new SkillDao();
                }
                return instance;
            }
        }

        public void Merge(SqliteDatabase oldDb) {
            foreach (SkillEntity oldData in instance.SelectAll(oldDb)) {
                if (instance.SelectByPrimaryKey(oldData.Id, oldDb) == null) {
                    Insert(oldData, oldDb);
                }
                else {
                    Update(oldData, oldDb);
                }
            }
        }

        public List<SkillEntity> SelectAll(SqliteDatabase mdb = null)
        {
            List<SkillEntity> entityList = new List<SkillEntity>();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM skill;");
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

        public SkillEntity SelectByPrimaryKey(int id, SqliteDatabase mdb = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM skill WHERE id = ")
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

        public void Insert(SkillEntity entity, SqliteDatabase mdb = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO skill VALUES (")
            
                
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
            
                
                .Append(entity.Level)
                
                .Append(",")
            
                
                .Append(entity.Type1)
                
                .Append(",")
            
                
                .Append(entity.Power1)
                
                .Append(",")
            
                
                .Append(entity.Turn1)
                
                .Append(",")
            
                
                .Append(entity.Type2)
                
                .Append(",")
            
                
                .Append(entity.Power2)
                
                .Append(",")
            
                
                .Append(entity.Turn2)
                
                .Append(",")
            
                
                .Append(entity.Seconds)
                
                .Append(",")
            
                
                .Append(entity.Range)
                
                
            
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

        public void Update(SkillEntity entity, SqliteDatabase mdb = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE skill SET ")
            
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
            
                .Append("level = ")
                
                .Append(entity.Level)
                
                .Append(",")
            
                .Append("type1 = ")
                
                .Append(entity.Type1)
                
                .Append(",")
            
                .Append("power1 = ")
                
                .Append(entity.Power1)
                
                .Append(",")
            
                .Append("turn1 = ")
                
                .Append(entity.Turn1)
                
                .Append(",")
            
                .Append("type2 = ")
                
                .Append(entity.Type2)
                
                .Append(",")
            
                .Append("power2 = ")
                
                .Append(entity.Power2)
                
                .Append(",")
            
                .Append("turn2 = ")
                
                .Append(entity.Turn2)
                
                .Append(",")
            
                .Append("seconds = ")
                
                .Append(entity.Seconds)
                
                .Append(",")
            
                .Append("range = ")
                
                .Append(entity.Range)
                
                
            
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

        private SkillEntity CreateEntity(DataRow row)
        {
            SkillEntity entity = new SkillEntity();
            
            entity.Id = DaoSupport.GetIntValue(row, "id");
            
            entity.SystemName = DaoSupport.GetStringValue(row, "system_name");
            
            entity.DisplayName = DaoSupport.GetStringValue(row, "display_name");
            
            entity.Lang = DaoSupport.GetStringValue(row, "lang");
            
            entity.Level = DaoSupport.GetIntValue(row, "level");
            
            entity.Type1 = (SkillType) Enum.ToObject(typeof(SkillType),  DaoSupport.GetIntValue(row, "type1"));
            
            entity.Power1 = DaoSupport.GetIntValue(row, "power1");
            
            entity.Turn1 = DaoSupport.GetIntValue(row, "turn1");
            
            entity.Type2 = (SkillType) Enum.ToObject(typeof(SkillType), DaoSupport.GetIntValue(row, "type2"));
            
            entity.Power2 = DaoSupport.GetIntValue(row, "power2");
            
            entity.Turn2 = DaoSupport.GetIntValue(row, "turn2");
            
            entity.Seconds = DaoSupport.GetIntValue(row, "seconds");
            
            entity.Range = (SkillRange) Enum.ToObject(typeof(SkillRange), DaoSupport.GetIntValue(row, "range"));
            
            return entity;
        }
    }
}
