using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Reflection;
using oyster.web;

namespace Demo.Host
{
    public class SitesManager
    {
        public static string[] binPath = new string[] { 
            "plugins",
            "libs",
            "bin",
        };
        public static Dictionary<AppDomainDTO, AppDomain> LoadSiteTemplate(string tempRoot, string siteRoot)
        {
            var ApplicationDic = new Dictionary<AppDomainDTO, AppDomain>();
            var dllFs = Directory.GetFiles(tempRoot, "*.dll", SearchOption.AllDirectories);

            for (int i = 0; i < dllFs.Length; i++)
            {
                var fpath = dllFs[i];

                AppDomain app = AppDomain.CreateDomain(fpath, null, new AppDomainSetup
                {
                    ApplicationBase = AppDomain.CurrentDomain.BaseDirectory, //siteRoot,
                    PrivateBinPath = string.Join(";", binPath),
                    LoaderOptimization = LoaderOptimization.MultiDomain
                });
                string nn = app.FriendlyName;
                app.DoCallBack(() =>
                {
                    AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler((o, e) =>
                    {
                        return null;
                    });
                });
                var dtoTp = typeof(AppDomainDTO);
                app.Load(File.ReadAllBytes(dtoTp.Assembly.Location));

                var loader = (AppDomainDTO)app.CreateInstanceAndUnwrap(dtoTp.Assembly.FullName, dtoTp.FullName);
                loader.DllPath = fpath;
                app.DoCallBack(loader.Load);

                ApplicationDic.Add(loader, app);
            }

            return ApplicationDic;
        }
    }
}