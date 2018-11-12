namespace Ctr.AhphOcelot.Configuration
{
    /// <summary>
    /// 金焰的世界
    /// 2018-11-11
    /// 自定义配置信息
    /// </summary>
    public class AhphOcelotConfiguration
    {
        /// <summary>
        /// 数据库连接字符串，使用不同数据库时自行修改,默认实现了SQLSERVER
        /// </summary>
        public string DbConnectionStrings { get; set; }

        /// <summary>
        /// 金焰的世界
        /// 2018-11-12
        /// 是否启用定时器，默认不启动
        /// </summary>
        public bool EnableTimer { get; set; } = false;

        /// <summary>
        /// 金焰的世界
        /// 2018-11.12
        /// 定时器周期，单位（毫秒），默认30分总自动更新一次
        /// </summary>
        public int TimerDelay { get; set; } = 30 * 60 * 1000;
    }
}
