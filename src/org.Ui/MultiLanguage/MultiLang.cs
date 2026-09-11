using CommunityToolkit.Mvvm.ComponentModel;
using org.Ui.ViewModels;
using org.Utils.Global;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;

namespace org.Ui.MultiLanguage
{
    public partial class MultiLang
    {
        public static Dictionary<string, ResourceDictionary> LangDictionaries { get; private set; }

        public static MultiLangResourceViewModel CurrentLanguage { get; private set; }

        public static event OnLanguageChangedHandler OnLanguageChanged;

        public delegate void OnLanguageChangedHandler(string currLang, string nextLang);
        public static void Initialize()
        {
            var langVMs = ConfigContext.GenericConfig.MultipleLang.Supports
                .Select(s => new MultiLangResourceViewModel(s));
            Instance.LangItems = [.. langVMs];
            Instance.SelectedLang = Instance.LangItems
                .FirstOrDefault(f => f.Code == ConfigContext.GenericConfig.MultipleLang.DefaultCode);

            var list = langVMs
                .AsParallel().WithDegreeOfParallelism(4)
                .Select(s => LoadJsonToDict(s.Uri))
                .ToList();
            LangDictionaries = list
                .ToDictionary(d => d["Code"]?.ToString() ?? "");

            SwitchLanguage();
        }

        public static void SwitchLanguage()
        {
            var nextLang = Instance.SelectedLang;
            if (nextLang?.Code == CurrentLanguage?.Code ||
                !LangDictionaries.TryGetValue(nextLang?.Code, out var nextDict))
            {
                return;
            }

            if (Application.Current.Resources.MergedDictionaries.Contains(Instance.CurrentDictionary))
            {
                var index = Application.Current.Resources.MergedDictionaries.IndexOf(Instance.CurrentDictionary);
                Application.Current.Resources.MergedDictionaries[index] = nextDict;
            }
            else
            {
                Application.Current.Resources.MergedDictionaries.Add(nextDict);
            }

            SetCurrentLang(CurrentLanguage?.Code, nextLang, nextDict);

        }

        private static void SetCurrentLang(string currLang, MultiLangResourceViewModel nextLang, ResourceDictionary nextDict)
        {
            CurrentLanguage = nextLang;
            Instance.CurrentDictionary = nextDict;

            try
            {
                //SynchronizationContext.Current?.Post(new SendOrPostCallback(obj =>
                //{
                //        OnLanguageChanged?.Invoke(currLang, nextLang.Code);
                //}), null);

                Task.Factory.StartNew(
                    () =>
                    {
                        OnLanguageChanged?.Invoke(currLang, nextLang.Code);
                    },
                    CancellationToken.None,
                    TaskCreationOptions.RunContinuationsAsynchronously,
                    TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, $"SwitchLanguaging : Current {currLang},Next {nextLang}");
            }
        }

        public static string GetString(string text, string defText = null)
        {
            if (string.IsNullOrEmpty(text))
            {
                // 此处用于防止错误资源导致多语言切换崩溃
                return string.Empty;
            }

            if (string.IsNullOrEmpty(defText))
            {
                defText = text;
            }

            if (Instance.CurrentDictionary?.Contains(text) != true)
            {
                return defText;
            }

            return Instance.CurrentDictionary[text].ToString();
        }

        public static string GetStringFormat(string text, params object[] args)
        {
            return string.Format(GetString(text), args);
        }

        public static ResourceDictionary LoadJsonToDict(string uri)
        {
            ResourceDictionary dict = new ResourceDictionary();
            try
            {
                var filePath = Path.Combine(FilePathConst.ResourcesDirectory, uri);
                if (File.Exists(filePath))
                {
                    Debug.WriteLine($"Read Language File:{filePath}");
                    var json = File.ReadAllText(filePath, Encoding.UTF8);
                    var tmp = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                    foreach (var item in tmp)
                    {
                        dict[item.Key] = item.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, "MultiLang.LoadJsonToDict{uri}", uri);
            }

            return dict;
        }

        #region languageKeys
        public static MultiLang Instance { get; private set; }

        [ObservableProperty]
        private ObservableCollection<MultiLangResourceViewModel> _langItems;

        [ObservableProperty]
        private MultiLangResourceViewModel _selectedLang;

        static MultiLang()
        {
            Instance = new MultiLang();
        }

        private string GetString([CallerMemberName] string propertyName = null)
        {
            if (CurrentDictionary.Contains(propertyName))
            {
                return CurrentDictionary[propertyName].ToString();
            }

            return CurrentDictionary["未知"]?.ToString();
        }

        private ResourceDictionary _currentDictionary;
        public ResourceDictionary CurrentDictionary
        {
            get => _currentDictionary;
            set
            {
                SetProperty(ref _currentDictionary, value, "");
            }
        }
        #endregion
    }
}
