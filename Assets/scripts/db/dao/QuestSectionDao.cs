using System.Collections.Generic;
using System.Text;
using Plugins;
using script.db.entity;

namespace db.dao
{
    public class QuestSectionDao : MergeDao
    {

        // 唯一のインスタンス
        private static QuestSectionDao instance;

        public static QuestSectionDao Instance {
            get {
                if (instance == null) {
                    instance = new QuestSectionDao();
                }
                return instance;
            }
        }

        public void Merge(SqliteDatabase oldDb) {
            foreach (QuestSectionEntity oldData in instance.SelectAll(oldDb)) {
                if (instance.SelectByPrimaryKey(oldData.Id, oldDb) == null) {
                    Insert(oldData, oldDb);
                }
                else {
                    Update(oldData, oldDb);
                }
            }
        }

        public List<QuestSectionEntity> SelectAll(SqliteDatabase mdb = null)
        {
            List<QuestSectionEntity> entityList = new List<QuestSectionEntity>();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM quest_section;");
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

        public QuestSectionEntity SelectByPrimaryKey(int id, SqliteDatabase mdb = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM quest_section WHERE id = ")
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

        public void Insert(QuestSectionEntity entity, SqliteDatabase mdb = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO quest_section VALUES (")
            
                
                .Append(entity.Id)
                
                .Append(",")
            
                
                .Append(entity.ChapterNo)
                
                .Append(",")
            
                
                .Append(entity.SectionNo)
                
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

        public void Update(QuestSectionEntity entity, SqliteDatabase mdb = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE quest_section SET ")
            
                .Append("id = ")
                
                .Append(entity.Id)
                
                .Append(",")
            
                .Append("chapter_no = ")
                
                .Append(entity.ChapterNo)
                
                .Append(",")
            
                .Append("section_no = ")
                
                .Append(entity.SectionNo)
                
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

        private QuestSectionEntity CreateEntity(DataRow row)
        {
            QuestSectionEntity entity = new QuestSectionEntity();
            
            entity.Id = DaoSupport.GetIntValue(row, "id");
            
            entity.ChapterNo = DaoSupport.GetIntValue(row, "chapter_no");
            
            entity.SectionNo = DaoSupport.GetIntValue(row, "section_no");
            
            entity.DisplayName = DaoSupport.GetStringValue(row, "display_name");
            
            return entity;
        }
    }
}