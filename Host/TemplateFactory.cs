using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Host
{
    public class TemplateFactory : oyster.web.TemplateFactory
    {
        public override oyster.web.ITemplate MapTemplate(HttpContext context)
        {
            return base.MapTemplate(context);
        }
    }

    public interface ITaskSpider
    {
        /// <summary>
        /// 组合任务
        /// </summary>
        /// <param name="taskJsonData">
        /// 任务格式：
        /// {Name:"任务名",参数1名:"参数1值",参数2名:"参数2值",参数3名:"参数3值",参数4名:"参数4值",参数5名:"参数5值",参数6名:"参数6值",参数7名:"参数7值",参数8名:"参数8值",参数9名:"参数9值",参数10名:"参数10值"}
        /// </param>
        /// <returns>string[0]:taskJsonData,string[1]:url</returns>
        string[] Compose(string taskJsonData);
        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="taskData">string[0]:taskJsonData,string[1]:url</param>
        /// <param name="pageText">抓取到的页面内容</param>
        /// <returns>{Name:"任务名",Data:[{字段1:字段1值,字段2:字段2值,字段3:字段3值,...}]}</returns>
        string Analysis(string[] taskData, string pageText);
    }
}