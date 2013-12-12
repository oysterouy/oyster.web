using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using oyster.web.define;

namespace oyster.web.A.manage
{
    class StaticResourceManager
    {
        const string NoFoundFileUrl = "/nofound";
        static readonly KeyValueCollection<string, string> realFileDic = new KeyValueCollection<string, string>();
        static readonly KeyValueCollection<string, ResourceUrlInfo> fileUrlDic = new KeyValueCollection<string, ResourceUrlInfo>();

        static readonly List<FileSystemWatcher> fileWatcherList = new List<FileSystemWatcher>();
        static void AddWatch(string relativedir)
        {
            if (string.IsNullOrWhiteSpace(relativedir))
                return;
            string dir = relativedir.StartsWith("/") ? relativedir.Substring(1) : relativedir;
            if (!System.IO.Directory.Exists(dir))
            {
                dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dir);
                if (!System.IO.Directory.Exists(dir))
                    throw new Exception(string.Format("Static Resource FileWatch Dir:{0} Not Exists!", dir));
            }
            //InitWatchDir(dir);

            var watcher = new FileSystemWatcher();
            watcher.Path = dir;
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;

            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Deleted += new FileSystemEventHandler(OnChanged);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);

            watcher.EnableRaisingEvents = true;
            fileWatcherList.Add(watcher);
        }
        static void InitWatchDir(string dir)
        {
            var fs = Directory.GetFiles(dir, "*", SearchOption.AllDirectories);
            foreach (var f in fs)
            {
                string path = f.Substring(AppDomain.CurrentDomain.BaseDirectory.Length);
                var fileInfo = new FileInfo(f);
                realFileDic.Add(fileInfo.FullName, path);
                fileUrlDic.Add(path, new ResourceUrlInfo
                {
                    Path = path,
                    RealPath = fileInfo.FullName,
                    ETag = GetFileInfoETAG(fileInfo)
                });
            }
        }

        private static string GetFileInfoETAG(FileInfo fileInfo)
        {
            return fileInfo.LastWriteTime.ToString("yyMMddHHmmss");
        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            OnChanged(source, e);
        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            string path = null;
            if (realFileDic.TryGetValue(e.FullPath, out path))
            {
                fileUrlDic.Remove(path);
                realFileDic.Remove(e.FullPath);
            }
        }

        public static ResourceUrlInfo GetResourceUrlInfo(string path)
        {
            string filePath = path.Trim().ToLower();
            ResourceUrlInfo urlInfo = null;
            if (!fileUrlDic.TryGetValue(filePath, out urlInfo))
            {
                var fileInfo = GetFileInfo(filePath);
                if (fileInfo != null)
                {
                    urlInfo = new ResourceUrlInfo
                    {
                        Path = filePath,
                        RealPath = fileInfo.FullName,
                        ETag = GetFileInfoETAG(fileInfo)
                    };

                    realFileDic.Add(fileInfo.FullName, filePath);
                    fileUrlDic.Add(filePath, urlInfo);
                }
            }
            return urlInfo;
        }
        public static string GetResourceUrl(string path)
        {
            var urlInfo = GetResourceUrlInfo(path);
            return urlInfo == null ? "" : urlInfo.Url;
        }

        static FileInfo GetFileInfo(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return null;

            string fullFilePath = filePath;
            var host = RequestContext.GetHost();
            if (host != null)
            {
                AddWatch(host.TemplateStaticResourceDir);
                fullFilePath = Path.Combine(host.TemplateStaticResourceDir,
                    fullFilePath.StartsWith("/") ? fullFilePath.Substring(1) : fullFilePath);
            }

            string realFullFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                fullFilePath.StartsWith("/") ? fullFilePath.Substring(1) : fullFilePath);
            if (File.Exists(realFullFilePath))
            {
                return new FileInfo(realFullFilePath);
            }
            else
            {
                var realCommonFullFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    filePath.StartsWith("/") ? filePath.Substring(1) : filePath);
                if (File.Exists(realCommonFullFilePath))
                    return new FileInfo(realCommonFullFilePath);
            }

            return null;
        }
    }

    [Serializable]
    public class ResourceUrlInfo
    {
        public string Path { get; set; }
        public string RealPath { get; set; }
        public string Url { get { return string.Format("{0}?etag={1}", Path, ETag); } }
        public string ETag { get; set; }

        public string ContentType
        {
            get
            {
                string ext = System.IO.Path.GetExtension(Path).ToLower();
                switch (ext)
                {
                    case ".htm":
                    case ".html":
                        return "text/html";
                    case ".js":
                        if (Path.EndsWith(".json.js"))
                            return "application/json";
                        return "application/x-javascript";
                    case ".css":
                        return "text/css";
                    case ".gif":
                        return "image/gif";
                    case ".jpg":
                    case ".jpeg":
                        return "image/jpg";
                    case ".png":
                        return "image/png";
                    default:
                        return "application/octet-stream";
                }
                return "";
            }
        }
    }
}
