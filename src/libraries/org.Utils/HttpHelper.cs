using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using org.Models.Athena;
using org.Utils.Global;
using Flurl;
using Flurl.Http;

namespace org.Utils
{
    public class HttpHelper
    {
        public static async Task<ApiResponse<List<AppReleaseNote>>> CheckUpdateAsync(string url)
        {
            try
            {
                var result = await url.WithTimeout(TimeSpan.FromSeconds(10))
                    .GetJsonAsync<ApiResponse<List<AppReleaseNote>>>();
                return result;
            }
            catch (Exception ex)
            {
                LogFactory.Error(ex, "HttpHelper.CheckUpdateAsync");
            }

            return default;
        }

        public static async Task<string> DownloadNewerAppAsync(string url, string fileName, int timeout)
        {
            return await url.WithTimeout(TimeSpan.FromSeconds(timeout))
                .DownloadFileAsync(FilePathConst.NewAppDirectory, fileName);
        }
    }
}
