using org.Models.Priviledge;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.Utils.Global
{
    public static class ApplicationPriviledge
    {
        #region PointMap
        public const PriviledgeLevel PointMap_Function04_Import = PriviledgeLevel.Executable;

        #endregion

        #region 权限控制
        public const PriviledgeLevel PriviledgeManage_Role_List = PriviledgeLevel.Visible;
        public const PriviledgeLevel PriviledgeManage_Account_List = PriviledgeLevel.All;
        public const PriviledgeLevel PriviledgeManage_Resource_List = PriviledgeLevel.Executable;

        #endregion

        #region test
        public const PriviledgeLevel Main_Test = PriviledgeLevel.All;
        #endregion
        static ApplicationPriviledge()
        {

        }
    }
}
