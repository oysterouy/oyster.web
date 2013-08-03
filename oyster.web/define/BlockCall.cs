﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oyster.web.define
{
    class BlockCall
    {
        public BlockCall(RequestHead head)
        {
            Header = head;
        }
        RequestHead Header { get; set; }

        KeyValueCollection<string, BlockCallInfo> blocks = new KeyValueCollection<string, BlockCallInfo>();

        string GetCallID<TTemplate>(string callId) where TTemplate : TemplateBase
        {
            var tp = typeof(TTemplate);
            return string.Format("{0}-{1}", tp.FullName, callId);
        }

        public string Block<TTemplate>(string callId, bool sync) where TTemplate : TemplateBase
        {
            string key = GetCallID<TTemplate>(callId);
            BlockCallInfo callData = null;
            if (blocks.TryGetValue(key, out callData))
            {
                return callData.Waiting(sync);
            }
            throw new Exception(string.Format("Block<{0}> Id:{1} No found!", typeof(TTemplate).FullName, callId));
        }

        public void InvorkBlock<TTemplate>(string callId) where TTemplate : TemplateBase
        {
            string key = GetCallID<TTemplate>(callId);
            blocks.Add(key, new BlockCallInfo(callId, TemplateManager.GetTemplateInstance(typeof(TTemplate)), Header));
        }

        public void BlockModel<TTemplate>(string callId, object[] reqParams) where TTemplate : TemplateBase
        {
            string key = GetCallID<TTemplate>(callId);
            BlockCallInfo callData = null;
            if (blocks.TryGetValue(key, out callData))
            {
                callData.Paramsters = reqParams;
                callData.Invork();
            }
        }

    }
    class BlockCallInfo
    {
        public BlockCallInfo(string id, TemplateBase temp, RequestHead head)
        {
            Id = id;
            Template = temp;
            Request = new Request(head) { Template = Template };
        }
        public string Id { get; set; }
        public TemplateBase Template { get; set; }
        public object[] Paramsters { get; set; }
        public Request Request { get; set; }
        public Response Response { get; set; }

        bool hadInvork = false;

        public void Invork()
        {
            Request.Body.Paramters = Paramsters;
            Response = TemplateManager.GetSetting(Template.GetType().Assembly).RequestInternal(Request);
            hadInvork = true;
        }
        public string Waiting(bool alwaysBack)
        {
            if (!hadInvork)
                throw new Exception(string.Format("Block<{0}> Id:{1}  Had No Set Request Paramsters Yet!", Template.GetType().FullName, Id));

            if (alwaysBack)
                Response.Waiting(0);
            else
                Response.Waiting();
            return Response.GetOutPut();
        }
    }
}
