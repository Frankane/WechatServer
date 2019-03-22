using SqlSugar;

namespace WeChatServer.Models {
    public class SqlSugarHelper {
        /// <summary>
        /// 连接本地SqlServer数据库
        /// </summary>
        /// <returns></returns>
        public static SqlSugarClient ConnectSqlServer() {
            SqlSugarClient db = new SqlSugarClient(
            new ConnectionConfig() {
                ConnectionString = "server=.;uid=fireflies;pwd=Fireflies;database=BookShare",
                DbType = DbType.SqlServer,//设置数据库类型
                IsAutoCloseConnection = true,//自动释放数据务，如果存在事务，在事务结束后释放
                InitKeyType = InitKeyType.SystemTable //从实体特性中读取主键自增列信息
            });
            return db;
        }
        /// <summary>
        /// 连接服务器MariaDB数据库
        /// </summary>
        /// <returns></returns>
        public static SqlSugarClient ConnectMariaDB() {
            SqlSugarClient db = new SqlSugarClient(
            new ConnectionConfig() {
                ConnectionString = "server=119.29.114.207;port=3306;uid=root;pwd=fireflies;database=BookShare",
                DbType = DbType.MySql,//设置数据库类型
                IsAutoCloseConnection = true,//自动释放数据务，如果存在事务，在事务结束后释放
                InitKeyType = InitKeyType.SystemTable //从实体特性中读取主键自增列信息
            });
            return db;
        }
    }
}
