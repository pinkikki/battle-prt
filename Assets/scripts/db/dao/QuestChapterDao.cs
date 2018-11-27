using System.Collections.Generic;
using System.Text;
using Plugins;
using script.db.entity;

namespace db.dao
{
    public class QuestChapterDao : MergeDao
    {

        // 唯一のインスタンス
        private static QuestChapterDao instance;

        public static QuestChapterDao Instance {
            get {
                if (instance == null) {
                    instance = new QuestChapterDao();
                }
                return instance;
            }
        }

        public void Merge(SqliteDatabase oldDb) {
            foreach (QuestChapterEntity oldData in instance.SelectAll(oldDb)) {
                if (instance.SelectByPrimaryKey(oldData.Id, oldDb) == null) {
                    Insert(oldData, oldDb);
                }
                else {
                    Update(oldData, oldDb);
                }
            }
        }

        public List<QuestChapterEntity> SelectAll(SqliteDatabase mdb = null)
        {
            List<QuestChapterEntity> entityList = new List<QuestChapterEntity>();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM quest_chapter;");
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

        public QuestChapterEntity SelectByPrimaryKey(int id, SqliteDatabase mdb = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM quest_chapter WHERE id = ")
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

        public void Insert(QuestChapterEntity entity, SqliteDatabase mdb = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO quest_chapter VALUES (")
            
                
                .Append(entity.Id)
                
                .Append(",")
            
                
                .Append(entity.ChapterNo)
                
                .Append(",")
            
                .Append("'")
                .Append(entity.DisplayName)
                .Append("'")
                
            
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

        public void Update(QuestChapterEntity entity, SqliteDatabase mdb = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE quest_chapter SET ")
            
                .Append("id = ")
                
                .Append(entity.Id)
                
                .Append(",")
            
                .Append("chapter_no = ")
                
                .Append(entity.ChapterNo)
                
                .Append(",")
            
                .Append("display_name = ")
                .Append("'")
                .Append(entity.DisplayName)
                .Append("'")
                
            
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

        private QuestChapterEntity CreateEntity(DataRow row)
        {
            QuestChapterEntity entity = new QuestChapterEntity();
            
            entity.Id = DaoSupport.GetIntValue(row, "id");
            
            entity.ChapterNo = DaoSupport.GetIntValue(row, "chapter_no");
            
            entity.DisplayName = DaoSupport.GetStringValue(row, "display_name");
            
            return entity;
        }
    }
}