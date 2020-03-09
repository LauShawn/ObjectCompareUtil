using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectCompareUtil
{
    /// <summary>
    /// 示例数据库对象
    /// </summary>
    public class UserDO
    {
        /// <summary>
        /// 主键
        /// </summary>
        [CompareDiff(Name = "主键")]
        public long Id { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [CompareDiff(Name = "姓名")]
        public string Name { get; set; }
        /// <summary>
        /// 门店ID集合
        /// </summary>
        [CompareDiff(Name = "门店ID集合")]
        public List<long> ShopIds { get; set; }
        /// <summary>
        /// 朋友
        /// </summary>
        [CompareDiff(Name = "朋友")]
        public UserDO Firend { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
