using System;
using System.ComponentModel;

namespace hnzheng.FunctionHelper.Models
{
    public class ExecuteResult
    {
        [Description("执行结果")]
        public Boolean IsSuccess { get; set; }
        [Description("执行状态")]
        public ExcuteStatus ExcuteStatus { get; set; }
        [Description("返回参数")]
        public Object ReturnData { get; set; }
    }
    public enum ExcuteStatus
    {
        [Description("将要执行")]
        ToExecute = 1,
        [Description("执行中")]
        Executing = 2,
        [Description("执行结束")]
        Executed = 3
    }
}
