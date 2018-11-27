using System.Collections.Generic;
using System.Text;
using Plugins;
using script.db.entity;

namespace db.dao
{
    public class TeamDao : MergeDao
    {

        // 唯一のインスタンス
        private static TeamDao instance;

        public static TeamDao Instance {
            get {
                if (instance == null) {
                    instance = new TeamDao();
                }
                return instance;
            }
        }

        public void Merge(SqliteDatabase oldDb) {
            foreach (TeamEntity oldData in instance.SelectAll(oldDb)) {
                if (instance.SelectByPrimaryKey(oldData.Id, oldDb) == null) {
                    Insert(oldData, oldDb);
                }
                else {
                    Update(oldData, oldDb);
                }
            }
        }

        public List<TeamEntity> SelectAll(SqliteDatabase mdb = null)
        {
            List<TeamEntity> entityList = new List<TeamEntity>();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM team;");
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

        public TeamEntity SelectByPrimaryKey(int id, SqliteDatabase mdb = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM team WHERE id = ")
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

        public void Insert(TeamEntity entity, SqliteDatabase mdb = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO team VALUES (")
            
                
                .Append(entity.Id)
                
                .Append(",")
            
                
                .Append(entity.Level)
                
                
            
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

        public void Update(TeamEntity entity, SqliteDatabase mdb = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE team SET ")
            
                .Append("id = ")
                
                .Append(entity.Id)
                
                .Append(",")
            
                .Append("level = ")
                
                .Append(entity.Level)
                
                
            
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

        private TeamEntity CreateEntity(DataRow row)
        {
            TeamEntity entity = new TeamEntity();
            
            entity.Id = DaoSupport.GetIntValue(row, "id");
            
            entity.Level = DaoSupport.GetIntValue(row, "level");
            
            return entity;
        }
    }
}