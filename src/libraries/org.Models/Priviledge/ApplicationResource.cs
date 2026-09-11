using System;
using System.Collections.Generic;
using System.Text;

namespace org.Models.Priviledge
{
    public class ApplicationResource
    {
        #region PointMap
        public PriviledgeLevel PointMap_Function04_Import { get; set; }

        #endregion

        #region 权限控制
        public PriviledgeLevel PriviledgeManage_Role_List { get; set; }
        public PriviledgeLevel PriviledgeManage_Account_List { get; set; }
        public PriviledgeLevel PriviledgeManage_Resource_List { get; set; }

        #endregion

        #region test
        public PriviledgeLevel Main_Test {get;set;}
        #endregion
    }
}
