using System.Collections.Generic;
using System.Text;
using Plugins;
using script.db.entity;

namespace db.dao
{
    public class SenseDao : MergeDao
    {

        // 唯一のインスタンス
        private static SenseDao instance;

        public static SenseDao Instance {
            get {
                if (instance == null) {
                    instance = new SenseDao();
                }
                return instance;
            }
        }

        public void Merge(SqliteDatabase oldDb) {
            foreach (SenseEntity oldData in instance.SelectAll(oldDb)) {
                if (instance.SelectByPrimaryKey(oldData.Id, oldDb) == null) {
                    Insert(oldData, oldDb);
                }
                else {
                    Update(oldData, oldDb);
                }
            }
        }

        public List<SenseEntity> SelectAll(SqliteDatabase mdb = null)
        {
            List<SenseEntity> entityList = new List<SenseEntity>();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM sense;");
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

        public SenseEntity SelectByPrimaryKey(int id, SqliteDatabase mdb = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM sense WHERE id = ")
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

        public void Insert(SenseEntity entity, SqliteDatabase mdb = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO sense VALUES (")
            
                
                .Append(entity.Id)
                
                .Append(",")
            
                .Append("'")
                .Append(entity.DisplayName)
                .Append("'")
                .Append(",")
            
                
                .Append(entity.Level)
                
                .Append(",")
            
                
                .Append(entity.Type)
                
                .Append(",")
            
                
                .Append(entity.Power)
                
                
            
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

        public void Update(SenseEntity entity, SqliteDatabase mdb = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE sense SET ")
            
                .Append("id = ")
                
                .Append(entity.Id)
                
                .Append(",")
            
                .Append("display_name = ")
                .Append("'")
                .Append(entity.DisplayName)
                .Append("'")
                .Append(",")
            
                .Append("level = ")
                
                .Append(entity.Level)
                
                .Append(",")
            
                .Append("type = ")
                
                .Append(entity.Type)
                
                .Append(",")
            
                .Append("power = ")
                
                .Append(entity.Power)
                
                
            
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

        private SenseEntity CreateEntity(DataRow row)
        {
            SenseEntity entity = new SenseEntity();
            
            entity.Id = DaoSupport.GetIntValue(row, "id");
            
            entity.DisplayName = DaoSupport.GetStringValue(row, "display_name");
            
            entity.Level = DaoSupport.GetIntValue(row, "level");
            
            entity.Type = DaoSupport.GetIntValue(row, "type");
            
            entity.Power = DaoSupport.GetIntValue(row, "power");
            
            return entity;
        }
    }
}