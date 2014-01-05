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

        internal bool Wait()
        {
            var loadingTimeout = Process.LoadingTimeout;

            if (loadingTimeout > 0)
                return waitHandle.WaitOne(loadingTimeout);
            else
                return waitHandle.WaitOne();
        }
        internal TimBlock Render()
        {
            if (waitHandle != null)
            {
                if (!Wait())
                {
                    Process.Response.Body.Append("<block id=\"This Block Md5\" class=\"block-rendering\">加载中...</block>");
                }
                Process.ProcessRender();
                return this;
            }
            throw new Exception(string.Format("Block:{0},CallID:{1} Had not Invoke yet.", Template.GetType().FullName,
                CallID));
        }

        internal TimBlock Invoke()
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
            return this;
        }
    }
}
