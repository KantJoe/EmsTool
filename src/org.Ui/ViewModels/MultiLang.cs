using CommunityToolkit.Mvvm.ComponentModel;
using OpenTK.Platform.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using org.Ui.MultiLanguage;
using System.Security.RightsManagement;

namespace org.Ui.MultiLanguage
{

    public partial class MultiLang : ObservableObject
    {

        #region 模块1

        #region 模块1_界面1

        #endregion
        #region 模块1_界面2

        #endregion
        #endregion

        #region 基础内容
        public string Code => GetString();
        public string Language => GetString();
        public string 语言 => GetString();
        public string 退出登录 => GetString();
        public string 账号 => GetString();
        public string 密码 => GetString();
        public string 项目 => GetString();
        public string 导入 => GetString();
        public string 导出 => GetString();
        public string 创建 => GetString();
        public string 本地连接 => GetString();
        public string 登入 => GetString();
        public string 退出 => GetString();
        public string 亮 => GetString();
        public string 暗 => GetString();
        public string 其它 => GetString();
        public string 确定 => GetString();
        public string 取消 => GetString();
        public string 新增 => GetString();
        public string 名称 => GetString();
        public string 删除 => GetString();
        public string 角色 => GetString();
        public string 编辑 => GetString();
        public string 型号 => GetString();
        public string 未知 => GetString();
        public string 直流光伏 => GetString();
        public string 功率 => GetString();
        public string 自动生成 => GetString();
        public string 下发 => GetString();
        public string 读取 => GetString();
        public string 连接 => GetString();
        public string 取消连接 => GetString();
        public string 端口 => GetString();
        public string 协议 => GetString();
        public string 地址 => GetString();
        public string LiquidCooling => GetString();
        public string AirCooling => GetString();
        public string 指定EMS => GetString();
        public string 离网 => GetString();
        public string 并网 => GetString();
        public string 离线 => GetString();
        public string 在线 => GetString();
        public string 等待 => GetString();
        public string 故障 => GetString();
        public string 运行中 => GetString();
        public string 待机 => GetString();
        public string AC_DC => GetString();
        public string DC_AC => GetString();
        public string 否 => GetString();
        public string 并机 => GetString();
        public string 主机 => GetString();
        public string 从机 => GetString();
        public string 无效 => GetString();
        public string 警告 => GetString();
        public string 刷新 => GetString();
        public string 另存为 => GetString();
        public string 复位 => GetString();

        #endregion

        #region 菜单
        public string 菜单_主界面 => GetString();
        public string 菜单_看板 => GetString();
        public string 菜单_点位表 => GetString();
        public string 菜单_设备状态 => GetString();
        public string 菜单_系统设置 => GetString();
        public string 菜单_实时监控 => GetString();
        public string 菜单_告警监控 => GetString();
        public string 菜单_EMS故障历史 => GetString();
        public string 菜单_固件升级 => GetString();
        public string 菜单_测试自动化 => GetString();
        public string 菜单_通信模拟 => GetString();
        public string 菜单_阈值和趋势 => GetString();
        public string 菜单_脚本引擎 => GetString();
        public string 菜单_日志 => GetString();
        public string 菜单_软件设置 => GetString();
        public string 菜单_其它 => GetString();
        public string 菜单_权限控制 => GetString();
        public string 菜单_设备拓扑 => GetString();

        #endregion

        #region 点位表
        public string 点位表_单表 => GetString();
        public string 点位表_分组 => GetString();
        public string 点位表_导入功能码3 => GetString();
        public string 点位表_导入功能码4 => GetString();
        public string 点位表_导出功能码3 => GetString();
        public string 点位表_导出功能码4 => GetString();
        public string 点位表_清空功能码3 => GetString();
        public string 点位表_清空功能码4 => GetString();
        public string 点位表_功能码3 => GetString();
        public string 点位表_功能码4 => GetString();

        #endregion

        #region 设备拓扑
        public string 设备拓扑_设备连接 => GetString();
        public string 设备拓扑_设备拓扑 => GetString();
        public string 设备拓扑_一键连接 => GetString();
        public string 设备拓扑_一键断开 => GetString();
        public string 设备拓扑_新增Rtu => GetString();
        public string 设备拓扑_新增Tcp => GetString();
        public string 设备拓扑_串口号 => GetString();
        public string 设备拓扑_波特率 => GetString();
        public string 设备拓扑_数据位 => GetString();
        public string 设备拓扑_停止位 => GetString();
        public string 设备拓扑_校验位 => GetString();
        public string 设备拓扑_Ip => GetString();
        public string 设备拓扑_端口号 => GetString();
        public string 设备拓扑_标称容量 => GetString();
        public string 设备拓扑_实时充电电流 => GetString();
        public string 设备拓扑_实时放电电流 => GetString();
        #endregion

        #region 设置
        public string 设置_其它 => GetString();
        public string 设置_自动检查更新 => GetString();
        public string 设置_发现新版本 => GetString();
        public string 设置_立即升级 => GetString();
        public string 设置_工作区 => GetString();
        public string 设置_Modbus轮询间隔 => GetString();
        public string 设置_点位表设置 => GetString();
        public string 设置_点位表分组长度 => GetString();
        public string 设置_交互 => GetString();
        public string 设置_实时渲染间隔 => GetString();
        #endregion

        #region 权限控制
        public string 权限控制_账户列表 => GetString();
        public string 权限控制_角色列表 => GetString();
        public string 权限控制_权限列表 => GetString();
        public string 权限控制_角色项 => GetString();
        public string 权限控制_新增角色 => GetString();
        public string 权限控制_新增编辑账号 => GetString();

        #endregion

        #region 系统提示
        public string 系统提示_长度不在范围内 => GetString();
        public string 系统提示_仅允许英文字母和数字 => GetString();
        public string 系统提示_存在重复名称 => GetString();
        public string 系统提示_存在重复账号 => GetString();

        #endregion 

        #region 创建项目
        public string 创建项目_创建项目 => GetString();
        public string 创建项目_项目名 => GetString();
        public string 创建项目_协议类型 => GetString();
        #endregion

        #region 设备状态
        public string 设备状态_状态概览 => GetString();
        public string 设备状态_散热类型 => GetString();
        public string 设备状态_系统状态 => GetString();
        public string 设备状态_初始化 => GetString();
        public string 设备状态_自检 => GetString();
        public string 设备状态_停机 => GetString();
        public string 设备状态_旁路 => GetString();
        public string 设备状态_就绪 => GetString();
        public string 设备状态_并网充电 => GetString();
        public string 设备状态_并网运行 => GetString();
        public string 设备状态_并网放电 => GetString();
        public string 设备状态_离网充电 => GetString();
        public string 设备状态_离网放电 => GetString();
        public string 设备状态_告警 => GetString();
        public string 设备状态_预留 => GetString();
        public string 设备状态_离网预留 => GetString();
        public string 设备状态_故障 => GetString();
        public string 设备状态_升级 => GetString();
        public string 设备状态_手动调试 => GetString();
        public string 设备状态_保护性充电 => GetString();
        public string 设备状态_运行模式 => GetString();
        public string 设备状态_并网定时充放模式 => GetString();
        public string 设备状态_并网削峰填谷模式 => GetString();
        public string 设备状态_并网自发自用模式 => GetString();
        public string 设备状态_并网电池优先模式 => GetString();
        public string 设备状态_离网模式 => GetString();
        public string 设备状态_远程模式 => GetString();
        public string 设备状态_离网油机模式 => GetString();
        public string 设备状态_离并网状态 => GetString();
        public string 设备状态_联网状态 => GetString();
        public string 设备状态_EMS状态 => GetString();
        public string 设备状态_配置文件代码 => GetString();
        public string 设备状态_风扇转速 => GetString();
        public string 设备状态_系统额定功率 => GetString();
        public string 设备状态_湿度01 => GetString();
        public string 设备状态_湿度02 => GetString();
        public string 设备状态_温度01 => GetString();
        public string 设备状态_温度02 => GetString();
        public string 设备状态_温度03 => GetString();
        public string 设备状态_温度04 => GetString();
        public string 设备状态_EMS固件版本 => GetString();
        public string 设备状态_BMS固件版本 => GetString();
        public string 设备状态_HMI固件版本 => GetString();
        public string 设备状态_模组联机状态HL => GetString();
        public string 设备状态_Bit00BMS => GetString();
        public string 设备状态_Bit01消防 => GetString();
        public string 设备状态_Bit02PCS => GetString();
        public string 设备状态_Bit03DCDC => GetString();
        public string 设备状态_Bit04MPPT => GetString();
        public string 设备状态_Bit05STS => GetString();
        public string 设备状态_Bit06电表 => GetString();
        public string 设备状态_Bit07空调 => GetString();
        public string 设备状态_Bit08液冷机组 => GetString();
        public string 设备状态_Bit09HMI => GetString();
        public string 设备状态_Bit10PC => GetString();
        public string 设备状态_Bit11GPS => GetString();
        public string 设备状态_Bit12FourG => GetString();
        public string 设备状态_Bit13Wifi => GetString();
        public string 设备状态_Bit14蓝牙 => GetString();
        public string 设备状态_Bit15LAN1 => GetString();
        public string 设备状态_Bit00LAN2 => GetString();
        public string 设备状态_Bit01温湿度 => GetString();
        public string 设备状态_Bit02并机通讯 => GetString();
        public string 设备状态_系统故障 => GetString();
        public string 设备状态_系统告警 => GetString();
        public string 设备状态_电网功率 => GetString();
        public string 设备状态_电网频率 => GetString();
        public string 设备状态_电网A相电压 => GetString();
        public string 设备状态_电网A相电流 => GetString();
        public string 设备状态_电网A相功率 => GetString();
        public string 设备状态_电网B相电压 => GetString();
        public string 设备状态_电网B相电流 => GetString();
        public string 设备状态_电网B相功率 => GetString();
        public string 设备状态_电网C相电压 => GetString();
        public string 设备状态_电网C相电流 => GetString();
        public string 设备状态_电网C相功率 => GetString();
        public string 设备状态_A线电压 => GetString();
        public string 设备状态_B线电压 => GetString();
        public string 设备状态_C线电压 => GetString();
        public string 设备状态_A相功率因数 => GetString();
        public string 设备状态_B相功率因数 => GetString();
        public string 设备状态_C相功率因数 => GetString();
        public string 设备状态_总功率因数 => GetString();
        public string 设备状态_电网状态 => GetString();
        public string 设备状态_取电 => GetString();
        public string 设备状态_馈电 => GetString();

        public string 设备状态_PV输入功率 => GetString();
        public string 设备状态_MPPT路数 => GetString();
        public string 设备状态_PV1电压 => GetString();
        public string 设备状态_PV1电流 => GetString();
        public string 设备状态_PV2电压 => GetString();
        public string 设备状态_PV2电流 => GetString();
        public string 设备状态_PV3电压 => GetString();
        public string 设备状态_PV3电流 => GetString();
        public string 设备状态_PV4电压 => GetString();
        public string 设备状态_PV4电流 => GetString();
        public string 设备状态_PV5电压 => GetString();
        public string 设备状态_PV5电流 => GetString();
        public string 设备状态_PV6电压 => GetString();
        public string 设备状态_PV6电流 => GetString();
        public string 设备状态_PV7电压 => GetString();
        public string 设备状态_PV7电流 => GetString();
        public string 设备状态_PV8电压 => GetString();
        public string 设备状态_PV8电流 => GetString();

        public string 设备状态_BMS联机状态 => GetString();
        public string 设备状态_PCS联机状态 => GetString();
        public string 设备状态_DCDC联机状态 => GetString();
        public string 设备状态_MPPT联机状态 => GetString();
        public string 设备状态_Meter联机状态 => GetString();
        public string 设备状态_逆变数据 => GetString();
        public string 设备状态_逆变频率 => GetString();
        public string 设备状态_逆变总功率 => GetString();
        public string 设备状态_逆变A相电压 => GetString();
        public string 设备状态_逆变A相电流 => GetString();
        public string 设备状态_逆变A相功率 => GetString();
        public string 设备状态_逆变B相电压 => GetString();
        public string 设备状态_逆变B相电流 => GetString();
        public string 设备状态_逆变B相功率 => GetString();
        public string 设备状态_逆变C相电压 => GetString();
        public string 设备状态_逆变C相电流 => GetString();
        public string 设备状态_逆变C相功率 => GetString();
        public string 设备状态_逆变功率因数 => GetString();
        public string 设备状态_逆变状态 => GetString();
        public string 设备状态_负载数据 => GetString();
        public string 设备状态_负载A相电压 => GetString();
        public string 设备状态_负载A相电流 => GetString();
        public string 设备状态_负载A相功率 => GetString();
        public string 设备状态_负载A相功率因数 => GetString();
        public string 设备状态_负载B相电压 => GetString();
        public string 设备状态_负载B相电流 => GetString();
        public string 设备状态_负载B相功率 => GetString();
        public string 设备状态_负载B相功率因数 => GetString();
        public string 设备状态_负载C相电压 => GetString();
        public string 设备状态_负载C相电流 => GetString();
        public string 设备状态_负载C相功率 => GetString();
        public string 设备状态_负载C相功率因数 => GetString();
        public string 设备状态_负载总功率 => GetString();
        public string 设备状态_负载功率因数 => GetString();
        public string 设备状态_负载频率 => GetString();
        public string 设备状态_电池数据 => GetString();
        public string 设备状态_电池功率 => GetString();
        public string 设备状态_电池电压 => GetString();
        public string 设备状态_电池SoC => GetString();
        public string 设备状态_电池电流 => GetString();
        public string 设备状态_电池状态 => GetString();
        public string 设备状态_电网充电功率 => GetString();
        public string 设备状态_当前组合有功总电能 => GetString();
        public string 设备状态_当前组合有功尖电能 => GetString();
        public string 设备状态_当前组合有功峰电能 => GetString();
        public string 设备状态_当前组合有功平电能 => GetString();
        public string 设备状态_当前组合有功谷电能 => GetString();
        public string 设备状态_当前正向有功总电能 => GetString();
        public string 设备状态_当前正向有功尖电能 => GetString();
        public string 设备状态_当前正向有功峰电能 => GetString();
        public string 设备状态_当前正向有功平电能 => GetString();
        public string 设备状态_当前正向有功谷电能 => GetString();
        public string 设备状态_当前反向有功总电能 => GetString();
        public string 设备状态_当前反向有功尖电能 => GetString();
        public string 设备状态_当前反向有功峰电能 => GetString();
        public string 设备状态_当前反向有功平电能 => GetString();
        public string 设备状态_当前反向有功谷电能 => GetString();
        public string 设备状态_当前组合无功总电能 => GetString();
        public string 设备状态_当前组合无功尖电能 => GetString();
        public string 设备状态_当前组合无功峰电能 => GetString();
        public string 设备状态_当前组合无功平电能 => GetString();
        public string 设备状态_当前组合无功谷电能 => GetString();
        public string 设备状态_当前正向无功总电能 => GetString();
        public string 设备状态_当前正向无功尖电能 => GetString();
        public string 设备状态_当前正向无功峰电能 => GetString();
        public string 设备状态_当前正向无功平电能 => GetString();
        public string 设备状态_当前正向无功谷电能 => GetString();
        public string 设备状态_当前反向无功总电能 => GetString();
        public string 设备状态_当前反向无功尖电能 => GetString();
        public string 设备状态_当前反向无功峰电能 => GetString();
        public string 设备状态_当前反向无功平电能 => GetString();
        public string 设备状态_当前反向无功谷电能 => GetString();
        public string 设备状态_尖电价 => GetString();
        public string 设备状态_峰电价 => GetString();
        public string 设备状态_平电价 => GetString();
        public string 设备状态_谷电价 => GetString();
        public string 设备状态_4G模块硬件版本 => GetString();
        public string 设备状态_4G模块软件版本1 => GetString();
        public string 设备状态_4G模块软件版本2 => GetString();
        public string 设备状态_4G模块IMEI => GetString();
        public string 设备状态_4G模块SIM卡号 => GetString();
        #endregion

        #region 系统设置
        public string 系统设置_48时段设置 => GetString();
        public string 系统设置_对应时段表 => GetString();
        public string 系统设置_时间段 => GetString();
        public string 系统设置_功率 => GetString();
        public string 系统设置_时段1 => GetString();
        public string 系统设置_时段2 => GetString();
        public string 系统设置_时段3 => GetString();
        public string 系统设置_时段4 => GetString();
        public string 系统设置_时段5 => GetString();
        public string 系统设置_时段6 => GetString();
        public string 系统设置_时段7 => GetString();
        public string 系统设置_时段8 => GetString();
        public string 系统设置_时段9 => GetString();
        public string 系统设置_时段10 => GetString();
        public string 系统设置_时段11 => GetString();
        public string 系统设置_时段12 => GetString();
        public string 系统设置_时段13 => GetString();
        public string 系统设置_时段14 => GetString();
        public string 系统设置_时段15 => GetString();
        public string 系统设置_时段16 => GetString();
        public string 系统设置_时段17 => GetString();
        public string 系统设置_时段18 => GetString();
        public string 系统设置_时段19 => GetString();
        public string 系统设置_时段20 => GetString();
        public string 系统设置_时段21 => GetString();
        public string 系统设置_时段22 => GetString();
        public string 系统设置_时段23 => GetString();
        public string 系统设置_时段24 => GetString();
        public string 系统设置_时段25 => GetString();
        public string 系统设置_时段26 => GetString();
        public string 系统设置_时段27 => GetString();
        public string 系统设置_时段28 => GetString();
        public string 系统设置_时段29 => GetString();
        public string 系统设置_时段30 => GetString();
        public string 系统设置_时段31 => GetString();
        public string 系统设置_时段32 => GetString();
        public string 系统设置_时段33 => GetString();
        public string 系统设置_时段34 => GetString();
        public string 系统设置_时段35 => GetString();
        public string 系统设置_时段36 => GetString();
        public string 系统设置_时段37 => GetString();
        public string 系统设置_时段38 => GetString();
        public string 系统设置_时段39 => GetString();
        public string 系统设置_时段40 => GetString();
        public string 系统设置_时段41 => GetString();
        public string 系统设置_时段42 => GetString();
        public string 系统设置_时段43 => GetString();
        public string 系统设置_时段44 => GetString();
        public string 系统设置_时段45 => GetString();
        public string 系统设置_时段46 => GetString();
        public string 系统设置_时段47 => GetString();
        public string 系统设置_时段48 => GetString();
        public string 系统设置_系统参数01 => GetString();
        public string 系统设置_系统参数02 => GetString();
        #endregion
    }
}
