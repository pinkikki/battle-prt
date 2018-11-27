using Plugins;

namespace db.dao
{
    public interface MergeDao
    {
        void Merge(SqliteDatabase oldDb);
    }
}