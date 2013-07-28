using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

namespace oyster.web.hosting
{
    public class HostAssermblyLoader : MarshalByRefObject
    {
        public AppDomain Domain { get; protected set; }
        public HostApplication Application { get; protected set; }

        public string Name { get; internal set; }
        public string ApplicationBase { get; internal set; }
        public string LibRoot { get; internal set; }
        public string MainModulePath { get; internal set; }

        public bool Init()
        {
            Domain = AppDomain.CurrentDomain;
            AssemblyManager.SetAssemblyLoadRoot(LibRoot);

            Application = new HostApplication(Name);
            LoadMainModule();
            return Application.HostSetting != null;
        }

        void LoadMainModule()
        {
            var bs = System.IO.File.ReadAllBytes(MainModulePath);
            var asm = Domain.Load(bs);
            var set = TemplateManager.GetSetting(asm);
            if (set != null)
            {
                Application.HostSetting.SetHosting(set);
            }
        }

        public static HostAssermblyLoader CreateLoader(string name, string libRoot, string mainPath, string applicationBaseDir = null)
        {
            var loaderTp = typeof(HostAssermblyLoader);
            var assemblyPath = Path.GetTempFileName();
            var webDllPath = loaderTp.Assembly.Location;
            System.IO.File.Copy(webDllPath, assemblyPath, true);

            var domain = AppDomain.CreateDomain(name, null,
                new AppDomainSetup
                {
                    LoaderOptimization = LoaderOptimization.MultiDomain,
                    ApplicationBase = applicationBaseDir,
                    ApplicationName = name,
                });
            var loader = domain.CreateInstanceFromAndUnwrap(assemblyPath, loaderTp.FullName) as HostAssermblyLoader;

            if (loader != null)
            {
                loader.Name = name;
                loader.LibRoot = libRoot;
                loader.MainModulePath = mainPath;
                loader.Init();
            }
            return loader;
        }


    }
}
