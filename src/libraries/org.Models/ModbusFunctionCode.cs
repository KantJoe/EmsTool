using System;
using System.Collections.Generic;
using System.Text;

namespace org.Models
{
    /// <summary>
    /// Supported function codes
    /// </summary>
    public enum ModbusFunctionCode:byte
    {
        DiagnosticsReturnQueryData = 0,

        #region 线圈寄存器 0xxxxx 1bit
        ReadCoils = 1,                              // 读离散输出线圈
        WriteSingleCoil = 5,                        // 写单个离散输出线圈
        WriteMultipleCoils = 15,                    // 写多个离散输出线圈
        #endregion

        #region 离散输入寄存器 1xxxxx 1bit
        ReadInputs = 2,                             // 读离散输入寄存器
        #endregion

        #region 输入寄存器 3xxxxx 16bits unsigned
        ReadInputRegisters = 4,                     // 读输入寄存器
        #endregion

        #region holdingregisters 4xxxxx 16bits unsigned
        ReadHoldingRegisters = 3,                   // 读保持寄存器
        WriteSingleRegister = 6,                    // 写单个保持寄存器
        WriteMultipleRegisters = 16,                // 写多个保持寄存器
        ReadWriteMultipleRegisters = 23,            // 保持寄存器 文件升级
        #endregion

        Diagnostics = 8,                            // 复位
        WriteSingleRegisterUpgrade = 17,            // 保持寄存器 设置配置文件

        WriteSingleRegisterExtension = 38,          // 烧录

        WriteFileRecord = 21,

    }
}
