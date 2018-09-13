using Newtonsoft.Json;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using System;
using System.Data.Common;

namespace FluentFramework.Types
{
    [Serializable]
    public class Serialized<T> : IUserType where T : class
    {
        public new bool Equals(object x, object y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return JsonConvert.SerializeObject(x) == JsonConvert.SerializeObject(y);
        }

        public int GetHashCode(object x)
            => x == null ? 0 : x.GetHashCode();

        public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
        {
            if (names.Length != 1)
                throw new InvalidOperationException("Only expecting one column...");

            return rs[names[0]] is string val && !string.IsNullOrWhiteSpace(val) ? JsonConvert.DeserializeObject<T>(val) : null;
        }

        public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
        {
            var parameter = cmd.Parameters[index];

            if (value == null)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = JsonConvert.SerializeObject(value);
        }

        public object DeepCopy(object value)
            => value == null
                ? null
                : JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(value));

        public object Replace(object original, object target, object owner)
            => original;

        public object Assemble(object cached, object owner)
        {
            var str = (string)cached;
            return string.IsNullOrWhiteSpace(str) ? null : JsonConvert.DeserializeObject<T>(str);
        }

        public object Disassemble(object value)
            => value == null ? null : JsonConvert.SerializeObject(value);

        public SqlType[] SqlTypes
            => new SqlType[] { new StringSqlType() };

        public Type ReturnedType
            => typeof(T);

        public bool IsMutable
            => true;
    }
}