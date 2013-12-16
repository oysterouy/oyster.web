using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace oyster.web
{
    public class TimBlock
    {
        public TimTemplate Template { get; set; }
        public string CallID { get; set; }
        public object[] Parameters { get; set; }

        internal TimProcess Process { get; set; }

        internal ManualResetEvent waitHandle { get; set; }
        internal string Render()
        {
            if (waitHandle != null)
            {
                if (waitHandle.WaitOne(Process.Theme.LoadingTimeout))
                {
                    return Process.Response.BodyString == null ? "" : Process.Response.BodyString.ToString();
                }
                else
                {
                    return string.Format("<block id=\"This Block Md5\" class=\"block-rendering\">加载中...</block>");
                }
            }
            throw new Exception(string.Format("Block:{0},CallID:{1} Had not Invoke yet.", Template.GetType().FullName,
                CallID));
        }

        internal void Invoke()
        {
            waitHandle = new ManualResetEvent(false);
            ThreadPool.QueueUserWorkItem(new WaitCallback((p) =>
            {
                var block = p as TimBlock;
                try
                {
                    block.Template.Init(block.Process, block.Parameters);
                    block.Process.ProcessRequest();
                }
                catch (Exception ex)
                {
                    //todo:Block Exception.
                }
                finally
                {
                    block.waitHandle.Set();
                }
            }), this);
        }
    }
}
