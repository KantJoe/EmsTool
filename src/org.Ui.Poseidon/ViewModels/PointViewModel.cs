using CommunityToolkit.Mvvm.ComponentModel;
using org.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace org.Ui.Poseidon.ViewModels
{
    public partial class PointViewModel : ObservableObject
    {
        private EmsPoint _point;

        #region properties
        public ModbusFunctionCode FunctionCode
        {
            get => _point?.FunctionCode ?? ModbusFunctionCode.DiagnosticsReturnQueryData;
            set
            {
                var v = _point?.FunctionCode ?? ModbusFunctionCode.DiagnosticsReturnQueryData;
                if (_point is not null && SetProperty(ref v, value, nameof(FunctionCode)))
                {
                    _point.FunctionCode = v;
                }
            }
        }

        public string GroupName
        {
            get => _point?.GroupName ?? string.Empty;
            set
            {
                var v = _point?.GroupName ?? string.Empty;
                if (_point is not null && SetProperty(ref v, value, nameof(GroupName)))
                {
                    _point.GroupName = v;
                }
            }
        }

        public uint Address
        {
            get => _point?.Address ?? 0;
            set
            {
                var v = _point?.Address ?? 0;
                if (_point is not null && SetProperty(ref v, value, nameof(Address)))
                {
                    _point.Address = v;
                }
            }
        }

        public string Description
        {
            get => _point?.Description ?? string.Empty;
            set
            {
                var v = _point?.Description ?? string.Empty;
                if (_point is not null && SetProperty(ref v, value, nameof(Description)))
                {
                    _point.Description = v;
                }
            }
        }

        public string DataType
        {
            get => _point?.DataType ?? string.Empty;
            set
            {
                var v = _point?.DataType ?? string.Empty;
                if (_point is not null && SetProperty(ref v, value, nameof(DataType)))
                {
                    _point.DataType = v;
                }
            }
        }

        /// <summary>
        /// 针对数据类型需要通过转换方法获取正确长度
        /// </summary>
        public int BitsLength
        {
            get => _point?.BitsLength ?? 0;
            set
            {
                var v = _point?.BitsLength ?? 0;
                if (_point is not null && SetProperty(ref v, value, nameof(BitsLength)))
                {
                    _point.BitsLength = v;
                }
            }
        }

        public ReadWriteKind Kind
        {
            get => _point?.Kind ?? ReadWriteKind.None;
            set
            {
                var v = _point?.Kind ?? ReadWriteKind.None;
                if (_point is not null && SetProperty(ref v, value, nameof(Kind)))
                {
                    _point.Kind = v;
                }
            }
        }

        public byte[] Value
        {
            get => _point?.Value ?? default;
            set
            {
                var v = _point?.Value ?? default;
                if (_point is not null && SetProperty(ref v, value, nameof(Value)))
                {
                    _point.Value = v;
                }
            }
        }

        [ObservableProperty]
        private int _actualValue;

        public byte[] DefaultValue
        {
            get => _point?.DefaultValue ?? default;
            set
            {
                var v = _point?.DefaultValue ?? default;
                if (_point is not null && SetProperty(ref v, value, nameof(DefaultValue)))
                {
                    _point.DefaultValue = v;
                }
            }
        }

        public decimal Coefficient
        {
            get => _point?.Coefficient ?? 0;
            set
            {
                var v = _point?.Coefficient ?? 0;
                if (_point is not null && SetProperty(ref v, value, nameof(Coefficient)))
                {
                    _point.Coefficient = v;
                }
            }
        }

        public string Unit
        {
            get => _point?.Unit ?? string.Empty;
            set
            {
                var v = _point?.Unit ?? string.Empty;
                if (_point is not null && SetProperty(ref v, value, nameof(Unit)))
                {
                    _point.Unit = v;
                }
            }
        }

        public string ValueScope
        {
            get => _point?.ValueScope ?? string.Empty;
            set
            {
                var v = _point?.ValueScope ?? string.Empty;
                if (_point is not null && SetProperty(ref v, value, nameof(ValueScope)))
                {
                    _point.ValueScope = v;
                }
            }
        }

        public byte[] MinValue
        {
            get => _point?.MinValue ?? default;
            set
            {
                var v = _point?.MinValue ?? default;
                if (_point is not null && SetProperty(ref v, value, nameof(MinValue)))
                {
                    _point.MinValue = v;
                }
            }
        }

        public byte[] MaxValue
        {
            get => _point?.MaxValue ?? default;
            set
            {
                var v = _point?.MaxValue ?? default;
                if (_point is not null && SetProperty(ref v, value, nameof(MaxValue)))
                {
                    _point.MaxValue = v;
                }
            }
        }

        #endregion

        public PointViewModel(EmsPoint point)
        {
            _point = point;
        }
    }
}
