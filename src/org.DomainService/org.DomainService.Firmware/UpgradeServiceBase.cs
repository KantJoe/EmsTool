using org.Communication;
using org.Communication.Extensions;
using org.Ui;
using org.Ui.Views;
using org.Utils;
using org.Utils.Global;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TouchSocket.Modbus;

namespace org.DomainService.Firmware
{
    public class UpgradeServiceBase
    {
        public virtual async Task<bool> BurnFirmwareFlowAsync(IConnectClient clientObj, Action<string, double> progressAction, byte[] fileBuff, byte slaveId, CancellationToken token)
        {
            var result = true;
            IModbusMaster client = default;
            try
            {
                if (clientObj is ModbusRtuObject rtu)
                {
                    client = rtu.Instance;
                }
                else if (clientObj is ModbusTcpObject tcp)
                {
                    client = tcp.Instance;
                }

                progressAction("开始烧录", 0);
                
                result = await BurnFirmwareAsync(client, progressAction, fileBuff, slaveId, token);
                if (!result)
                {
                    return result;
                }

                await Task.Delay(1000, CancellationToken.None);

                double unit = 5f / 60;
                for (var increment = 0; increment < 60; increment++)
                {
                    var response = await client.GetBurnFirmwareProgress(slaveId, 5000, token);
                    var data = response.Data.ToArray();
                    var code = (data[0] << 8) + data[1];
                    if (code == 100)
                    {
                        progressAction($"升级完成, 等待重启", 100);
                        result = true;
                        return result;
                    }
                    else if (code > 100)
                    {
                        progressAction($"升级失败,错误码：{code}", 0);
                        result = false;
                        break;
                    }
                    else
                    {
                        progressAction($"升级中...", 95 + increment * unit);
                    }

                    await Task.Delay(1000);
                }

                progressAction($"升级等待超时", 0);
                result = false;
            }
            catch (Exception ex)
            {
                result = false;
                progressAction($"升级失败", 0);
                LogFactory.Error(ex, "UpgradeServiceBase.BurnFirmwareAsync");
            }
            finally
            {
                if (result)
                {
                    await clientObj?.DisconnectAsync();

                    await UiGlobalContext.ShowRootDialog(new WaitDialog("已重启完成"), closedHandler: new MaterialDesignThemes.Wpf.DialogClosedEventHandler(async (s, e) =>
                    {
                        await clientObj?.ConnectAsync();
                    }));
                }
            }

            return result;
        }

        private async Task<bool> BurnFirmwareAsync(IModbusMaster client, Action<string, double> progressAction, byte[] fileBuff, byte slaveId, CancellationToken token)
        {
            var result = true;
            var timeout = 50000;
            try
            {
                var response = await client.EntryBurnFirmwareMode(slaveId, timeout, token);
                progressAction("进入烧录模式", 5);
                await Task.Delay(5);
                response = await client.WriteFirmwareSize(fileBuff.Length, slaveId, timeout, token);
                progressAction("写入固件大小", 10);
                var crc32 = HashHelper.GetCRC32Code(fileBuff);
                await Task.Delay(5);
                await client.ValidateFirmwareCRC32(crc32, slaveId, timeout, token);
                progressAction("固件CRC32校验", 15);
                await Task.Delay(5);
                response = await client.ClearFirmwareFlash(slaveId, timeout, token);
                progressAction("清空Flash空间", 20);
                await Task.Delay(5);

                var totalLength = fileBuff.Length;
                var PAGESIZE = 256;
                var pages = totalLength / PAGESIZE + ((totalLength % PAGESIZE) != 0 ? 1 : 0);
                double unit = (90f - 20) / pages;
                for (var currPage = 0; currPage < pages; currPage++)
                {
                    var buffer = new byte[((currPage + 1) == pages ? (totalLength % PAGESIZE) : PAGESIZE) + 2];
                    buffer[0] = (byte)(currPage >> 8 & 0xFF);
                    buffer[1] = (byte)(currPage & 0xFF);
                    var currBuffer = fileBuff.Skip(currPage * PAGESIZE).Take(PAGESIZE).ToArray();
                    Array.Copy(currBuffer, 0, buffer, 2, currBuffer.Length);

                    var slot = await client.DownloadFirmware((ushort)buffer.Length, buffer, slaveId, timeout, token);
                    var data = slot.Data.ToArray();
                    var code = (data[0] << 8) + data[1];
                    if (code != 00)
                    {
                        progressAction($"下载固件失败", 0);
                    }
                    progressAction($"下载中... {currPage}/{pages}", 20f + unit * currPage);
                    await Task.Delay(5);
                }

                progressAction($"下载完成", 90);
            }
            catch (Exception ex)
            {
                result = false;
                progressAction($"烧录失败", 0);
                LogFactory.Error(ex, "UpgradeServiceBase.BurnFirmwareAsync");
            }
            finally
            {
                if (result == true)
                {
                    var response = await client.ExitBurnFirmwareMode(slaveId, timeout, token);
                    progressAction($"正在升级...", 95);
                }
            }

            return result;
        }
    }
}
