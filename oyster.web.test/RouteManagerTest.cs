using oyster.web.hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using oyster.web;
using System.Text.RegularExpressions;

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


        /// <summary>
        ///Match 的测试
        ///</summary>
        [TestMethod()]
        public void MatchTest()
        {
            Request request = new Request();
            request.Head.Path = "/ttt/adds-123/5dadf.html";
            RouteManager.Route<T1>("/ttt/", "/ttt/{0}-{1}/{2}.html", "n", "q", "s");
            TemplateBase actual = RouteManager.Match(request);
            Assert.AreNotEqual(actual, null);
        }

        class T1 : TemplateBase<T1>
        {

            public override object[] Init(Request request)
            {
                throw new NotImplementedException();
            }

            public override void Request(Request request, Response response)
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
    }
}
