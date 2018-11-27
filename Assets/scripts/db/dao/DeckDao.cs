using System.Collections.Generic;
using System.Text;
using Plugins;
using script.db.entity;

namespace db.dao
{
    public class DeckDao : MergeDao
    {

        // 唯一のインスタンス
        private static DeckDao instance;

        public static DeckDao Instance {
            get {
                if (instance == null) {
                    instance = new DeckDao();
                }
                return instance;
            }
        }

        public void Merge(SqliteDatabase oldDb) {
            foreach (DeckEntity oldData in instance.SelectAll(oldDb)) {
                if (instance.SelectByPrimaryKey(oldData.Id, oldDb) == null) {
                    Insert(oldData, oldDb);
                }
                else {
                    Update(oldData, oldDb);
                }
            }
        }

        public List<DeckEntity> SelectAll(SqliteDatabase mdb = null)
        {
            List<DeckEntity> entityList = new List<DeckEntity>();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM deck;");
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

        public DeckEntity SelectByPrimaryKey(int id, SqliteDatabase mdb = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM deck WHERE id = ")
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

        public void Insert(DeckEntity entity, SqliteDatabase mdb = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO deck VALUES (")
            
                
                .Append(entity.Id)
                
                .Append(",")
            
                .Append("'")
                .Append(entity.DisplayName)
                .Append("'")
                .Append(",")
            
                
                .Append(entity.Character1)
                
                .Append(",")
            
                
                .Append(entity.Character2)
                
                .Append(",")
            
                
                .Append(entity.Character3)
                
                .Append(",")
            
                
                .Append(entity.Leader)
                
                
            
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

        public void Update(DeckEntity entity, SqliteDatabase mdb = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE deck SET ")
            
                .Append("id = ")
                
                .Append(entity.Id)
                
                .Append(",")
            
                .Append("display_name = ")
                .Append("'")
                .Append(entity.DisplayName)
                .Append("'")
                .Append(",")
            
                .Append("character1 = ")
                
                .Append(entity.Character1)
                
                .Append(",")
            
                .Append("character2 = ")
                
                .Append(entity.Character2)
                
                .Append(",")
            
                .Append("character3 = ")
                
                .Append(entity.Character3)
                
                .Append(",")
            
                .Append("leader = ")
                
                .Append(entity.Leader)
                
                
            
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

        private DeckEntity CreateEntity(DataRow row)
        {
            DeckEntity entity = new DeckEntity();
            
            entity.Id = DaoSupport.GetIntValue(row, "id");
            
            entity.DisplayName = DaoSupport.GetStringValue(row, "display_name");
            
            entity.Character1 = DaoSupport.GetIntValue(row, "character1");
            
            entity.Character2 = DaoSupport.GetIntValue(row, "character2");
            
            entity.Character3 = DaoSupport.GetIntValue(row, "character3");
            
            entity.Leader = DaoSupport.GetIntValue(row, "leader");
            
            return entity;
        }
    }
}