using System;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using oyster.web.host;

namespace oyster.web.test
{


    /// <summary>
    ///这是 RouteManagerTest 的测试类，旨在
    ///包含所有 RouteManagerTest 单元测试
    ///</summary>
    [TestClass()]
    public class RouteManagerTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        // 
        //编写测试时，还可使用以下特性:
        //
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //使用 TestInitialize 在运行每个测试前先运行代码
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //使用 TestCleanup 在运行完每个测试后运行代码
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        class TestTheme : TimTheme
        {
            public override string ThemeName
            {
                get { throw new NotImplementedException(); }
            }

            public override int LoadingTimeout
            {
                get { throw new NotImplementedException(); }
            }

            public override string ThemeRelactivePath
            {
                get { throw new NotImplementedException(); }
            }

            public override void Init()
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        ///Match 的测试
        ///</summary>
        [TestMethod()]
        public void MatchTest()
        {
            var theme = new TestTheme();

            Request request = new Request();
            request.RequestUrl = new Uri("http://xxxx/ttt/adds-123/5dadf.html");
            theme.Route.Add<T1>("/ttt/", "/ttt/{0}-{1}/{2}.html", "n", "q", "s");
            var actual = theme.Route.Match(request);
            Assert.AreNotEqual(actual, null);
        }

        class T1 : TimTemplate<T1>
        {
            public override object[] Init(Request request)
            {
                throw new NotImplementedException();
            }

            public override void Request(TimProcess timProcess)
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void TestRouteRegex()
        {
            Regex regRoute = new Regex("TemplateHelper\\.Route(?'open'\\()((?'open'\\()+[^\\(\\)]*(?'-open'\\))[^\\(\\)]*)+(?(open)\\)|(?!))", RegexOptions.Singleline);

            Regex regRouteUrl = new Regex("TemplateHelper\\.Route[^<]*<([^>]+)>[^\\(]*\\(([^\\)]+)\\)", RegexOptions.Singleline);


            string code = @"
@TemplateHelper.Config((int _loadingTimeout) => 200)
@(TemplateHelper.Route<timsitedemo.Index>(""/"", ""/""))
@(TemplateHelper.Route<timsitedemo.Index>(""/index"", ""/index/{0}-_-{1}/"", ""name"", ""age""))
@TemplateHelper.Route((request) =>
{
    return InstanceHelper<timsitedemo.Index>.Instance;
})
@(TemplateHelper.Route<timsitedemo.Index>(""/index"", ""/idx/{0}"", ""n""))
";


            var ms = regRoute.Matches(code);
            var msurl = regRouteUrl.Matches(code);
        }
        class TestHost : TimHost
        {

            public override TimTheme GetTheme(Request request)
            {
                return new TestTheme();
            }

            public override TimTheme GetTheme(string themeName)
            {
                return new TestTheme();
            }
        }

        [TestMethod]
        public void TestProcess()
        {
            var host = new TestHost();
            host.Execute(new Request
            {
                RequestUrl = new Uri("")
            });
        }
    }
}
