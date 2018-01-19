using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiduAI
{
    /// <summary>
    /// 人脸识别接口返回数据
    /// </summary>
    [Serializable]
    public class FaceIdentifyInfo
    {
        public long log_id { get; set; }
        /// <summary>
        /// 返回人脸的数量
        /// </summary>
        public int result_num { get; set; }

        public Ext_info[] ext_info { get; set; }

        public ResultData[] result { get; set; }
    }
    /// <summary>
    /// 对应参数中的ext_fields
    /// </summary>
    [Serializable]
    public class Ext_info
    {
        /// <summary>
        /// 活体分数，如0.49999。单帧活体检测参考阈值0.393241，超过此分值以上则可认为是活体。
        /// </summary>
        public string faceliveness { get; set; }
    }
    [Serializable]
    public class ResultData
    {
        public string group_id { get; set; }
        public string uid { get; set; }

        public string user_info { get; set; }

        public double[] scores { get; set; }
    }
}
