using org.Ui.MultiLanguage;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;

namespace org.Ui.ValidationRules
{
    public class LengthValidRule : ValidationRule
    {
        public int MinLength { get; set; }

        public int? MaxLength { get; set; } = 20;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string val = value?.ToString() ?? string.Empty;
            int length = val.Length;
            return length < MinLength
                    || (MaxLength is not null && length > MaxLength)
                ? new ValidationResult(false, 
                    MultiLang.GetStringFormat(MultiLang.Instance.系统提示_长度不在范围内, MinLength, MaxLength))
                : ValidationResult.ValidResult;
        }
    }
}
