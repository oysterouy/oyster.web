﻿using System;
using System.Threading;
using oyster.web.hosting;
using oyster.web.define;

namespace oyster.web
{
    [Serializable]
    public class Request
    {
        public Request()
            : this(new RequestHead())
        { }
        public Request(RequestHead header)
        {
            Head = header;
            Body = new RequestBody();
            BlockCall = new BlockCall(Head);
        }

        internal HostBase Host { get; set; }
        public TemplateBase Template { get; internal set; }
        public RequestHead Head { get; internal set; }
        public RequestBody Body { get; internal set; }

        public Response Execute()
        {
            Response resp = new Response { Host = this.Host, Template = Template, Request = this };
            if (Host != null)
            {
                if (!Host.BeforeRequestFilter(this, resp))
                    return resp;
            }
            if (LayoutRequest != null)
            {
                resp.LayoutResponse = LayoutRequest.Execute();
            }
            //todo:check cache
            ResponseInfo respInfo = null;//get from cache;
            if (respInfo != null)
            {
                resp.Head = respInfo.Header;
                resp.Body = respInfo.Body;
            }
            else
            {
                resp.waitHandle = new AutoResetEvent(false);
                ThreadPool.QueueUserWorkItem((state) =>
                {
                    try
                    {
                        var it = state as TemplateBase;
                        it.Request(this, resp);
                    }
                    catch (Exception ex)
                    {
                        resp.Exception = ex;
                    }
                    finally
                    {
                        resp.waitHandle.Set();
                    }
                }, Template);
            }

            return resp;
        }

        public Request LayoutRequest { get; set; }
        public void Layout<T>(RequestBody body = null) where T : TemplateBase
        {
            LayoutRequest = new Request
            {
                Host = this.Host,
                Head = Head,
                Template = TemplateManager.GetTemplateInstance(typeof(T))
            };
            if (body == null)
            {
                var parms = LayoutRequest.Template.Init(LayoutRequest);
                LayoutRequest.Body.Paramters = parms;
            }
            else
                LayoutRequest.Body = body;
        }

        [NonSerialized]
        BlockCall BlockCall = null;
        public string BlockRander<TTemplate>(string callId, bool sync) where TTemplate : TemplateBase
        {
            return BlockCall.BlockRander<TTemplate>(callId, sync);
        }

        public void BlockInvork<TTemplate>(string callId, object[] reqParams) where TTemplate : TemplateBase
        {
            BlockCall.BlockInvork<TTemplate>(callId, reqParams);
        }

        public void BlockRegister<TTemplate>(string callId) where TTemplate : TemplateBase
        {
            BlockCall.BlockRegister<TTemplate>(callId);
        }
    }
}
