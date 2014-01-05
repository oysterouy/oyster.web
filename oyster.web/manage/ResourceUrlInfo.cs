using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace oyster.web.manage
{
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
            }
        }

        internal ResourceUrlInfo FreshETag()
        {
            if (File.Exists(RealPath))
            {
                var f = new FileInfo(RealPath);
                ETag = GetFileInfoETAG(f);
            }
            return this;
        }
        string GetFileInfoETAG(FileInfo fileInfo)
        {
            return fileInfo.LastWriteTime.ToString("yyMMddHHmmss");
        }
    }
}
