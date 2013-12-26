using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using oyster.web;
using oyster.web.host;

namespace oyster.web.manage
{
    public class StaticResourceManager
    {
        static string _defaultNoFoundFileUrl = "/nofound";
        public static string NoFoundFileUrl { get { return _defaultNoFoundFileUrl; } protected set { _defaultNoFoundFileUrl = value; } }

        static string _defaultResourceUrlStart = "/resource/";
        public static string ResourceUrlStart { get { return _defaultResourceUrlStart; } protected set { _defaultResourceUrlStart = value; } }

        static readonly KeyValueCollection<string, string> realFileDic = new KeyValueCollection<string, string>();
        static readonly KeyValueCollection<string, ResourceUrlInfo> fileUrlDic = new KeyValueCollection<string, ResourceUrlInfo>();

        public static ResourceUrlInfo GetResourceUrlInfo(Uri url)
        {
            var path = url.LocalPath;
            string filePath = path.Trim().ToLower();
            ResourceUrlInfo urlInfo = null;
            if (!fileUrlDic.TryGetValue(filePath, out urlInfo))
            {
                TimTheme theme = null;
                string themeStart = Path.Combine(ResourceUrlStart, "theme-").ToLower();
                if (filePath.StartsWith(themeStart))
                {
                    var ct = filePath.IndexOf(Path.DirectorySeparatorChar, themeStart.Length) - themeStart.Length;
                    var themeName = filePath.Substring(themeStart.Length, ct);
                    if (!string.IsNullOrWhiteSpace(themeName))
                    {
                        theme = TimHost.Instance.GetTheme(themeName);
                        if (theme != null && !string.IsNullOrWhiteSpace(theme.ThemeRelactivePath))
                        {
                            string fpath = Path.Combine(ResourceUrlStart, filePath.Substring(ct + 1));
                            var realfilePath = GetFileInfo(fpath, theme.ThemeRelactivePath);
                            if (!string.IsNullOrWhiteSpace(realfilePath))
                            {
                                urlInfo = new ResourceUrlInfo
                                {
                                    Path = filePath,
                                    RealPath = realfilePath,
                                }.FreshETag();
                                realFileDic.Add(realfilePath, filePath);
                                fileUrlDic.Add(filePath, urlInfo);
                            }
                        }
                    }
                }
                if (urlInfo == null)
                {
                    if (theme == null)
                    {
                        var process = TimProcessContext.GetProcess();
                        if (process != null && process.Theme != null)
                            theme = process.Theme;
                    }
                    if (theme != null)
                    {
                        var realfilePath = GetFileInfo(filePath, theme.ThemeRelactivePath);
                        if (!string.IsNullOrWhiteSpace(realfilePath))
                        {
                            urlInfo = new ResourceUrlInfo
                            {
                                Path = Path.Combine(ResourceUrlStart, "theme-" + theme.ThemeName, filePath.Substring(ResourceUrlStart.Length)).ToLower(),
                                RealPath = realfilePath,
                            }.FreshETag();
                            realFileDic.Add(realfilePath, filePath);
                            fileUrlDic.Add(filePath, urlInfo);
                        }
                    }
                }
                if (urlInfo == null)
                {
                    var realfilePath = GetFileInfo(filePath, "");
                    if (!string.IsNullOrWhiteSpace(realfilePath))
                    {
                        urlInfo = new ResourceUrlInfo
                        {
                            Path = filePath,
                            RealPath = realfilePath,
                        }.FreshETag();

                        realFileDic.Add(realfilePath, filePath);
                        fileUrlDic.Add(filePath, urlInfo);
                    }
                }
            }
            if (url.Query.Contains("__refresh--"))
            {

            }
            return urlInfo;
        }

        public static string GetResourceUrl(string path)
        {
            var urlInfo = GetResourceUrlInfo(new Uri(path));
            return urlInfo == null ? NoFoundFileUrl : urlInfo.Url;
        }

        static string GetFileInfo(string filePath, string searchDir)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return null;

            string realFullFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                filePath.StartsWith("/") ? filePath.Substring(1) : filePath);
            if (File.Exists(realFullFilePath))
            {
                return realFullFilePath;
            }
            else
            {
                var realCommonFullFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    filePath.StartsWith("/") ? filePath.Substring(1) : filePath);
                if (File.Exists(realCommonFullFilePath))
                    return realCommonFullFilePath;
            }

            return null;
        }
    }
}
