using org.Models.Priviledge;
using org.Ui;
using org.Utils.Global;
using Rougamo;
using Rougamo.Context;
using Rougamo.Metadatas;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.Privilege.Attributes
{
    [Pointcut(AccessFlags.Method)]
    public class PriviledgeAttribute : MoAttribute
    {
        /// <summary>
        /// 权限key
        /// </summary>
        public string PriviledgeKey { get; set; }

        /// <summary>
        /// 默认/异常返回值
        /// </summary>
        public object FallbackValue { get; set; }

        public override void OnEntry(MethodContext context)
        {
            if (LoginAccountContext.CurrentAccount is null
                || LoginAccountContext.CurrentAccount == LoginAccountInfo.Default)
            {
                context.ReplaceReturnValue(this, FallbackValue);
                UiGlobalContext.EnqueueRootMessage("缺少权限,请变更账号或调整角色");
                return;
            }

            var value =(PriviledgeLevel) LoginAccountContext.AggregatePriviledges
                .GetType()
                .GetProperty(PriviledgeKey)
                .GetValue(LoginAccountContext.AggregatePriviledges);
            var priviledge = (PriviledgeLevel)typeof(ApplicationPriviledge).GetField(PriviledgeKey)
                .GetValue(null);
            if (value< priviledge)
            {
                context.ReplaceReturnValue(this, FallbackValue);
                UiGlobalContext.EnqueueRootMessage("缺少权限,请变更账号或调整角色");
                return;
            }

        }

        public override void OnException(MethodContext context)
        {
            LogFactory.Error(context.Exception, "AOP catch: {p},{p1}", context.Method, context.Arguments);
        }

        public override void OnExit(MethodContext context)
        {
        }

        public override void OnSuccess(MethodContext context)
        {

        }
    }
}
