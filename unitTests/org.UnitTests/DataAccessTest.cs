using org.DataAccess;
using org.DataAccess.Entities;
using org.Utils.Global;
using Microsoft.Data.Sqlite;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml.Linq;

namespace org.UnitTests
{
    public class DataAccessTest
    {
        public DataAccessTest()
        {

            LogFactory.Initialize();

            ConfigContext.Initialize();
            DbContext.Initialize();
        }

        [Fact]
        public void AllTest()
        {
            //InsertData();
            //DeleteData(UpdateData(QueryData()));
            var newData = QueryData();
        }

        private void DeleteData(List<Order> orders)
        {
            foreach (var item in orders)
                DbContext.PoseidonDb.Deleteable(item).ExecuteCommand();

        }

        private List<Order> UpdateData(List<Order> orders)
        {
            foreach (var item in orders)
            {
                item.CustomId += 1;
                DbContext.PoseidonDb.Updateable(item).ExecuteCommand();
            }

            return orders;
        }

        private List<Order> QueryData()
        {
            return DbContext.PoseidonDb.Queryable<Order>().Where(w => w.Id > 0).ToList();
        }

        private void InsertData()
        {
            var id = DbContext.PoseidonDb.Insertable<Order>(new Order
            {
                CreateTime = DateTime.Now,
                CustomId = 2,
                Name = "name",
                Price = 234555.3233M
            }).ExecuteReturnIdentity();
        }

    }
}
