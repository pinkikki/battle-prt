using System;
using Plugins;

namespace db
{
    public class DaoSupport {
        public static int GetIntValue(DataRow row, string key) {
            return GetIntValue(row, key, 0);
        }

        public static int GetIntValue(DataRow row, string key, int defVal) {
            try {
                return (int) row[key];
            } catch (NullReferenceException) {
                return defVal;
            }
        }

        public static bool GetBoolValue(DataRow row, string key) {
            return GetBoolValue(row, key, false);
        }

        public static bool GetBoolValue(DataRow row, string key, bool defVal) {
            try {
                return (int) row[key] == 1;
            } catch (NullReferenceException) {
                return defVal;
            }
        }

        public static string GetStringValue(DataRow row, string key) {
            return GetStringValue(row, key, "");
        }

        public static string GetStringValue(DataRow row, string key, string defVal) {
            try {
                string getVal = (string) row[key];
                if (getVal != null) {
                    return getVal;
                } else {
                    return defVal;
                }
            } catch (NullReferenceException) {
                return defVal;
            }
        }
    }
}
