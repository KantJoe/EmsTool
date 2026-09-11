using org.Ui.MultiLanguage;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace org.Ui.ValidationRules
{
    public class OnlyAlphabetAndNumbersValidRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return Regex.IsMatch(value?.ToString() ?? string.Empty, "[a-zA-Z0-9]+")
                ? ValidationResult.ValidResult
                : new ValidationResult(false, MultiLang.GetStringFormat(
                    MultiLang.Instance.系统提示_仅允许英文字母和数字));
        }
    }
}
