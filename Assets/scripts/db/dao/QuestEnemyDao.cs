using System.Collections.Generic;
using System.Text;
using Plugins;
using script.db.entity;

namespace db.dao
{
    public class QuestEnemyDao : MergeDao
    {

        // 唯一のインスタンス
        private static QuestEnemyDao instance;

        public static QuestEnemyDao Instance {
            get {
                if (instance == null) {
                    instance = new QuestEnemyDao();
                }
                return instance;
            }
        }

        public void Merge(SqliteDatabase oldDb) {
            foreach (QuestEnemyEntity oldData in instance.SelectAll(oldDb)) {
                if (instance.SelectByPrimaryKey(oldData.Id, oldDb) == null) {
                    Insert(oldData, oldDb);
                }
                else {
                    Update(oldData, oldDb);
                }
            }
        }
        
        public List<QuestEnemyEntity> SelectBySectionId(int id, SqliteDatabase mdb = null)
        {
            List<QuestEnemyEntity> entityList = new List<QuestEnemyEntity>();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM quest_enemy WHERE section_id = ")
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
            dataTable.Rows.ForEach(r => entityList.Add(instance.CreateEntity(r)));
            return entityList;
        }
        
        public List<QuestEnemyEntity> SelectAll(SqliteDatabase mdb = null)
        {
            List<QuestEnemyEntity> entityList = new List<QuestEnemyEntity>();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM quest_enemy;");
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

        public QuestEnemyEntity SelectByPrimaryKey(int id, SqliteDatabase mdb = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM quest_enemy WHERE id = ")
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

        public void Insert(QuestEnemyEntity entity, SqliteDatabase mdb = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO quest_enemy VALUES (")
            
                
                .Append(entity.Id)
                
                .Append(",")
            
                
                .Append(entity.SectionId)
                
                .Append(",")
            
                
                .Append(entity.EnemyId)
                
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

        public void Update(QuestEnemyEntity entity, SqliteDatabase mdb = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE quest_enemy SET ")
            
                .Append("id = ")
                
                .Append(entity.Id)
                
                .Append(",")
            
                .Append("section_id = ")
                
                .Append(entity.SectionId)
                
                .Append(",")
            
                .Append("enemy_id = ")
                
                .Append(entity.EnemyId)
                
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

        private QuestEnemyEntity CreateEntity(DataRow row)
        {
            QuestEnemyEntity entity = new QuestEnemyEntity();
            
            entity.Id = DaoSupport.GetIntValue(row, "id");
            
            entity.SectionId = DaoSupport.GetIntValue(row, "section_id");
            
            entity.EnemyId = DaoSupport.GetIntValue(row, "enemy_id");
            
            entity.Level = DaoSupport.GetIntValue(row, "level");
            
            return entity;
        }
    }
}
