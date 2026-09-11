using org.Models;
using org.Utils.Global;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace org.Utils
{
    public static class ExcelHelper
    {

        public static void Initialize()
        {
            ExcelPackage.License.SetNonCommercialPersonal("Anonymous");
        }

        public static List<EmsPoint> ImportToEmsPoint(string filePath, int functionCode)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not Founded", filePath);
            }

            if (Path.GetExtension(filePath)?.ToLower() != FilePathConst.PointMapExt)
            {
                throw new NotSupportedException("File extension not match,target ext [.xlsx]");
            }

            var result = new List<EmsPoint>();
            try
            {
                using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath), string.Empty))
                {
                    if (package.InitializationErrors?.Any() == true)
                    {
                        LogFactory.Error(
                            $"ExcelHelper.ImportToEmsPoint({filePath}){Environment.NewLine}" +
                            string.Join(",", package.InitializationErrors));
                        return result;
                    }

                    foreach (var sheet in package.Workbook?.Worksheets)
                    {
                        result.AddRange(AddExcelSheet(sheet, functionCode.ToString()));
                    }
                }

            }
            catch (Exception ex)
            {
                LogFactory.Error(ex,
                    $"ExcelHelper.ImportToEmsPoint({filePath})");
            }


            return result;
        }

        private static List<EmsPoint> AddExcelSheet(ExcelWorksheet sheet, string functionCodeStr)
        {
            var tmpList = new List<EmsPoint>();

            var startRow = sheet.Dimension.Start.Row;
            var startCol = sheet.Dimension.Start.Column;

            // first row headers

            // second row to end
            startRow++;
            var functionCode = (ModbusFunctionCode)int.Parse(sheet.Cells[startRow, 1].Value?.ToString() ?? functionCodeStr);
            var groupName = sheet.Cells[startRow, 2].Value?.ToString();

            var endRow = sheet.Dimension.End.Row;
            var endCol = sheet.Dimension.End.Column;
            var startAddr = uint.Parse(sheet.Cells[startRow, 3].Value?.ToString());
            for (int row = startRow; row <= endRow; row++)
            {
                var point = new EmsPoint()
                {
                    FunctionCode = functionCode,
                    GroupName = groupName
                };

                try
                {

                    int addressCol = 3, descripCol = 4, bitsCol = 5, kindCol = 6
                        , defCol = 8, coefCol = 9, unitCol = 10, scopeCol = 11
                        , minCol = 12, maxCol = 13;

                    if (string.IsNullOrEmpty(sheet.Cells[row, descripCol].Value?.ToString())
                        && string.IsNullOrEmpty(sheet.Cells[row, bitsCol].Value?.ToString()))
                    {
                        continue;
                    }

                    if (sheet.Cells[row, descripCol].Merge
                        && sheet.Cells[sheet.MergedCells[row, descripCol]].Start.Row != row)
                    {
                        continue;
                    }

                    if (!uint.TryParse(sheet.Cells[row, addressCol].Value?.ToString(), out uint addr)
                        || addr < 0 || ((addr - startAddr) > (endRow - 2)))
                    {
                        continue;
                    }

                    point.Address = addr;

                    var description = sheet.Cells[row, descripCol].Value?.ToString();
                    if (string.IsNullOrEmpty(description))
                    {
                        LogFactory.Info($"{sheet.Name} Sheet:{sheet.Name} {row},{descripCol} is empty");
                    }

                    point.Description = description;

                    var bitsLength = sheet.Cells[row, bitsCol].Value?.ToString();
                    if (string.IsNullOrEmpty(bitsLength))
                    {
                        LogFactory.Info($"{sheet.Name} Sheet:{sheet.Name} {row},{bitsCol} is empty");
                    }

                    point.DataType = bitsLength;
                    point.BitsLength = point.DataTypeToBitsLength(
                        sheet.MergedCells[row, descripCol] is null
                            ? null : sheet.Cells[sheet.MergedCells[row, descripCol]]);

                    var kind = EmsPoint.KindConvertFromStr(
                        sheet.Cells[row, kindCol]?.Value?.ToString() ?? (functionCode == ModbusFunctionCode.ReadHoldingRegisters ? "R/W" : "R"));
                    if (kind == ReadWriteKind.None)
                    {
                        LogFactory.Info($"{sheet.Name} Sheet:{sheet.Name} {row},{kindCol} is empty");
                    }

                    point.Kind = kind;

                    var defValue = sheet.Cells[row, defCol].Value?.ToString();
                    point.DefaultValue = point.EmsPoint_StringToBytes(defValue);

                    var coefStr = sheet.Cells[row, coefCol].Value?.ToString() ?? "1";
                    if (string.IsNullOrEmpty(coefStr)
                        || !decimal.TryParse(coefStr, out decimal coef))
                    {
                        coef = 1;
                    }

                    point.Coefficient = coef;

                    var unit = sheet.Cells[row, unitCol].Value?.ToString();
                    if (!string.IsNullOrEmpty(unit))
                    {
                        point.Unit = unit;
                    }

                    var scope = sheet.Cells[row, scopeCol].Value?.ToString();
                    if (!string.IsNullOrEmpty(scope))
                    {
                        point.ValueScope = scope;
                    }

                    var minValue = sheet.Cells[row, minCol].Value?.ToString();
                    point.MinValue = point.EmsPoint_StringToBytes(minValue);
                    var maxValue = sheet.Cells[row, maxCol].Value?.ToString();
                    point.MaxValue = point.EmsPoint_StringToBytes(maxValue);

                    tmpList.Add(point);

                }
                catch (Exception ex)
                {
                    LogFactory.Error(ex, $"ExcelHelper.ImportToEmsPoint() Sheet:{sheet.Name} Row:{row}");
                }
            }

            Debug.WriteLine($"Sheet {sheet.Name} Count: {tmpList.Count}");
            return tmpList;
        }


        /// <summary>
        /// 大端字节序
        /// </summary>
        /// <param name="point"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] EmsPoint_StringToBytes(this EmsPoint point, string str)
        {
            var buff = new byte[] { 0 };
            switch (point.ValueType.FullName)
            {
                case "System.UInt16":
                    if (!ushort.TryParse(str, out ushort us))
                    {
                        buff = new byte[] { 0, 0 };
                        break;
                    }

                    buff = BitConverter.GetBytes(us);
                    break;
                case "System.Int16":
                    if (!short.TryParse(str, out short s))
                    {
                        buff = new byte[] { 0, 0 };
                        break;
                    }

                    buff = BitConverter.GetBytes(s);
                    break;
                case "System.Int32":
                    if (!Int32.TryParse(str, out Int32 i))
                    {
                        buff = new byte[] { 0, 0, 0, 0 };
                        break;
                    }

                    buff = BitConverter.GetBytes(i);
                    break;
                case "System.UInt32":
                    if (!UInt32.TryParse(str, out UInt32 ui))
                    {
                        buff = new byte[] { 0, 0, 0, 0 };
                        break;
                    }

                    buff = BitConverter.GetBytes(ui);
                    break;
                case "System.Int64":
                    if (!long.TryParse(str, out long l))
                    {
                        buff = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
                        break;
                    }

                    buff = BitConverter.GetBytes(l);
                    break;
                case "System.UInt64":
                    if (!ulong.TryParse(str, out ulong ul))
                    {
                        buff = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
                        break;
                    }

                    buff = BitConverter.GetBytes(ul);
                    break;
                case "System.String":
                    buff = new byte[0];
                    break;
                default:
                    break;
            }

            return buff?.AsEnumerable()?.Reverse()?.ToArray();
        }
        public static int DataTypeToBitsLength(this EmsPoint point, ExcelRange cells)
        {
            switch (point.DataType?.ToLower() ?? "u16")
            {
                case "u16":
                    point.ValueType = typeof(ushort);
                    return 16;
                case "s16":
                    point.ValueType = typeof(short);
                    return 16;
                case "u32":
                    point.ValueType = typeof(UInt32);
                    return 32;
                case "s32":
                    point.ValueType = typeof(Int32);
                    return 32;
                case "u64":
                    point.ValueType = typeof(ulong);
                    return 64;
                case "s64":
                    point.ValueType = typeof(long);
                    return 64;
                case "ascii":
                default:
                    point.ValueType = typeof(string);
                    return cells.Rows * 16;
            }
        }
    }
}
