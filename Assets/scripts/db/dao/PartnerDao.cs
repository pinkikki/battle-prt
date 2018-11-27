using System.Collections.Generic;
using System.Text;
using Plugins;
using script.db.entity;

namespace db.dao
{
    public class PartnerDao : MergeDao
    {

        // 唯一のインスタンス
        private static PartnerDao instance;

        public static PartnerDao Instance {
            get {
                if (instance == null) {
                    instance = new PartnerDao();
                }
                return instance;
            }
        }

        public void Merge(SqliteDatabase oldDb) {
            foreach (PartnerEntity oldData in instance.SelectAll(oldDb)) {
                if (instance.SelectByPrimaryKey(oldData.Id, oldDb) == null) {
                    Insert(oldData, oldDb);
                }
                else {
                    Update(oldData, oldDb);
                }
            }
        }

        public List<PartnerEntity> SelectAll(SqliteDatabase mdb = null)
        {
            List<PartnerEntity> entityList = new List<PartnerEntity>();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM partner ORDER BY id;");
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

        public PartnerEntity SelectByPrimaryKey(int id, SqliteDatabase mdb = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM partner WHERE id = ")
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

        public void Insert(PartnerEntity entity, SqliteDatabase mdb = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO partner VALUES (")
            
                
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
            
                
                .Append(entity.Skill1)
                
                
            
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

        public void Update(PartnerEntity entity, SqliteDatabase mdb = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE partner SET ")
            
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
            
                .Append("skill1 = ")
                
                .Append(entity.Skill1)
                
                
            
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

        private PartnerEntity CreateEntity(DataRow row)
        {
            PartnerEntity entity = new PartnerEntity();
            
            entity.Id = DaoSupport.GetIntValue(row, "id");
            
            entity.SystemName = DaoSupport.GetStringValue(row, "system_name");
            
            entity.DisplayName = DaoSupport.GetStringValue(row, "display_name");
            
            entity.Lang = DaoSupport.GetStringValue(row, "lang");
            
            entity.Skill1 = DaoSupport.GetIntValue(row, "skill1");
            
            return entity;
        }
    }
}
