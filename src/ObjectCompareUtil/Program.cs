using System;
using System.Collections.Generic;

namespace ObjectCompareUtil
{
    class Program
    {
        static void Main(string[] args)
        {
            var oldObj = new UserDO()
            {
                Id = 1,
                Name = "张三",
                ShopIds = new List<long>() { 1, 2 },
                Firend = new UserDO() { Id = 2 },
                CreateTime = DateTime.Now
            };
            var newObj = new UserDO()
            {
                Id = 2,
                Name = "李四",
                ShopIds = new List<long>() { 1, 3 },
                Firend = new UserDO() { Id = 3 },
                CreateTime = DateTime.Now
            };
            try
            {
                var diffSummary = CompareUtil.GetDiffSummary(oldObj, newObj);
                Console.WriteLine(diffSummary);
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
