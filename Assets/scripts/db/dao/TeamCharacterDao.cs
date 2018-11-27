using System;
using System.Collections.Generic;
using System.Text;
using battle;
using Plugins;
using script.db.entity;
using UnityEngine;

namespace db.dao
{
    public class TeamCharacterDao : MergeDao
    {

        // 唯一のインスタンス
        private static TeamCharacterDao instance;

        public static TeamCharacterDao Instance {
            get {
                if (instance == null) {
                    instance = new TeamCharacterDao();
                }
                return instance;
            }
        }

        public void Merge(SqliteDatabase oldDb) {
            foreach (TeamCharacterEntity oldData in instance.SelectAll(oldDb)) {
                if (instance.SelectByPrimaryKey(oldData.Id, oldDb) == null) {
                    Insert(oldData, oldDb);
                }
                else {
                    Update(oldData, oldDb);
                }
            }
        }

        public List<TeamCharacterEntity> SelectAll(SqliteDatabase mdb = null)
        {
            List<TeamCharacterEntity> entityList = new List<TeamCharacterEntity>();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM team_character;");
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

        public TeamCharacterEntity SelectByPrimaryKey(int id, SqliteDatabase mdb = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM team_character WHERE id = ")
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

        public void Insert(TeamCharacterEntity entity, SqliteDatabase mdb = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO team_character VALUES (")
            
                
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
            
                
                .Append(entity.Level)
                
                .Append(",")
            
                
                .Append(entity.Attack)
                
                .Append(",")
            
                
                .Append(entity.Defense)
                
                .Append(",")
            
                
                .Append(entity.Mind)
                
                .Append(",")
            
                
                .Append(entity.Speed)
                
                .Append(",")
            
                
                .Append(entity.CurrentHp)
                
                .Append(",")
            
                
                .Append(entity.MaxHp)
                
                .Append(",")
            
                
                .Append((int) entity.Skill1)
                
                .Append(",")
            
                
                .Append((int) entity.Skill2)
                
                .Append(",")
            
                
                .Append(entity.LeaderSkill)
                
                .Append(",")
            
                
                .Append(entity.Sense)
                
                
            
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

        public void Update(TeamCharacterEntity entity, SqliteDatabase mdb = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE team_character SET ")
            
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
            
                .Append("level = ")
                
                .Append(entity.Level)
                
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
            
                .Append("current_hp = ")
                
                .Append(entity.CurrentHp)
                
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
            
                .Append("leader_skill = ")
                
                .Append(entity.LeaderSkill)
                
                .Append(",")
            
                .Append("sense = ")
                
                .Append(entity.Sense)
                
                
            
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

        private TeamCharacterEntity CreateEntity(DataRow row)
        {
            TeamCharacterEntity entity = new TeamCharacterEntity();
            
            entity.Id = DaoSupport.GetIntValue(row, "id");
            
            entity.SystemName = DaoSupport.GetStringValue(row, "system_name");
            
            entity.DisplayName = DaoSupport.GetStringValue(row, "display_name");
            
            entity.Lang = DaoSupport.GetStringValue(row, "lang");
            
            entity.Creator = DaoSupport.GetIntValue(row, "creator");
            
            entity.Level = DaoSupport.GetIntValue(row, "level");
            
            entity.Attack = DaoSupport.GetIntValue(row, "attack");
            
            entity.Defense = DaoSupport.GetIntValue(row, "defense");
            
            entity.Mind = DaoSupport.GetIntValue(row, "mind");
            
            entity.Speed = DaoSupport.GetIntValue(row, "speed");
            
            entity.CurrentHp = DaoSupport.GetIntValue(row, "current_hp");
            
            entity.MaxHp = DaoSupport.GetIntValue(row, "max_hp");
            
            entity.Skill1 = DaoSupport.GetIntValue(row, "skill1");

            entity.Skill2 = DaoSupport.GetIntValue(row, "skill2");
            
            entity.LeaderSkill = DaoSupport.GetIntValue(row, "leader_skill");
            
            entity.Sense = DaoSupport.GetIntValue(row, "sense");
            
            return entity;
        }
    }
}
