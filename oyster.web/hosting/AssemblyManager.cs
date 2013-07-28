using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace oyster.web.hosting
{
    class AssemblyManager
    {
        static AssemblyManager()
        {
            if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory))
                SetAssemblyLoadRoot(AppDomain.CurrentDomain.BaseDirectory);
        }
        static string assemblyRoot { get; set; }

        public static Dictionary<string, Assembly> HadAssemblys = new Dictionary<string, Assembly>();
        public static Dictionary<string, List<string>> AssemblyPath = new Dictionary<string, List<string>>();
        public static void SetAssemblyLoadRoot(string root)
        {
            assemblyRoot = root;
            foreach (var f in Directory.GetFiles(assemblyRoot, "*.dll", SearchOption.AllDirectories))
            {
                List<string> ls = null;
                string fname = Path.GetFileNameWithoutExtension(f);
                if (!AssemblyPath.TryGetValue(fname, out ls))
                {
                    ls = new List<string>();
                    AssemblyPath.Add(fname, ls);
                }

                ls.Add(f);
            }
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
        }

        public static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                if (HadAssemblys.ContainsKey(args.Name))
                {
                    return HadAssemblys[args.Name];
                }
                string[] names = args.Name.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (names != null && names.Length > 0)
                {
                    string fname = names[0];
                    List<string> assemblyFs = null;
                    if (AssemblyManager.AssemblyPath.TryGetValue(fname, out assemblyFs))
                    {
                        if (assemblyFs.Count == 1)
                        {
                            return Assembly.Load(File.ReadAllBytes(assemblyFs[0]));
                        }
                        else if (assemblyFs.Count > 1)
                        {
                            foreach (var f in assemblyFs)
                            {
                                var domain = AppDomain.CreateDomain("tempDomain" + Guid.NewGuid().ToString("N"));
                                try
                                {
                                    var tpMbr = typeof(Mbr);
                                    //避免引用自身 造成死循环
                                    if (tpMbr.Assembly.FullName.Equals(args.Name))
                                        return tpMbr.Assembly;

                                    var mbr = (Mbr)domain.CreateInstanceFromAndUnwrap(tpMbr.Assembly.Location, tpMbr.FullName);
                                    mbr.Name = args.Name;
                                    mbr.FilePath = f;
                                    domain.DoCallBack(mbr.Check);

                                    if (mbr.IsTrue)
                                        return Assembly.Load(File.ReadAllBytes(f));
                                }
                                catch (Exception ex)
                                {

                                }
                                finally
                                {
                                    AppDomain.Unload(domain);
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                var asmls = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var asm in asmls)
                {
                    if (!HadAssemblys.ContainsKey(asm.FullName))
                        HadAssemblys.Add(asm.FullName, asm);
                }
            }
            return null;
        }
    }

    public class Mbr : MarshalByRefObject
    {
        public string Name { get; set; }
        public string FilePath { get; set; }
        public bool IsTrue = false;
        public void Check()
        {
            string nnn = AppDomain.CurrentDomain.FriendlyName;
            var asm = Assembly.Load(File.ReadAllBytes(FilePath));
            IsTrue = asm != null && asm.FullName == Name;
        }
    }
}
