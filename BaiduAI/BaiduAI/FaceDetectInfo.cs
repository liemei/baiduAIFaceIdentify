using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiduAI
{
    /// <summary>
    /// 人脸检测接口返回结构
    /// </summary>
    [Serializable]
    public class FaceDetectInfo
    {
        /// <summary>
        /// 人脸数目ID
        /// </summary>
        public int result_num { get; set; }

        public List<FaceDetectInfoResult> result { get; set; }

        /// <summary>
        /// 日志ID
        /// </summary>
        public long log_id { get; set; }
    }
    /// <summary>
    /// 人脸检测返回数据
    /// </summary>
    [Serializable]
    public class FaceDetectInfoResult
    {
        /// <summary>
        /// 年龄 face_fields包含age时返回
        /// </summary>
        public double age { get; set; }
        /// <summary>
        /// 美丑打分，范围0-100，越大表示越美。face_fields包含beauty时返回
        /// </summary>
        public double beauty { get; set; }
        /// <summary>
        /// 人脸在图像中的位置
        /// </summary>
        public FaceLocation location { get; set; }
        /// <summary>
        /// 人脸置信度，范围0-1
        /// </summary>
        public double face_probability { get; set; }
        /// <summary>
        /// 人脸框相对于竖直方向的顺时针旋转角，[-180,180]
        /// </summary>
        public int rotation_angle { get; set; }
        /// <summary>
        /// 三维旋转之左右旋转角[-90(左), 90(右)]
        /// </summary>
        public double yaw { get; set; }
        /// <summary>
        /// 三维旋转之俯仰角度[-90(上), 90(下)]
        /// </summary>
        public double pitch { get; set; }
        /// <summary>
        /// 平面内旋转角[-180(逆时针), 180(顺时针)]
        /// </summary>
        public double roll { get; set; }
        /// <summary>
        /// 表情，0，不笑；1，微笑；2，大笑。face_fields包含expression时返回
        /// </summary>
        public int expression { get; set; }
        /// <summary>
        /// 表情置信度，范围0~1。face_fields包含expression时返回
        /// </summary>
        public double expression_probability { get; set; }
        /// <summary>
        /// 脸型
        /// </summary>
        public FaceshapeInfo faceshape { get; set; }

        /// <summary>
        /// male、female。face_fields包含gender时返回
        /// </summary>
        public string gender { get; set; }
        /// <summary>
        /// 性别置信度，范围0~1。face_fields包含gender时返回
        /// </summary>
        public double gender_probability { get; set; }
        /// <summary>
        /// 是否带眼镜，0-无眼镜，1-普通眼镜，2-墨镜。face_fields包含glasses时返回
        /// </summary>
        public int glasses { get; set; }
        /// <summary>
        /// 眼镜置信度，范围0~1。face_fields包含glasses时返回
        /// </summary>
        public double glasses_probability { get; set; }
        /// <summary>
        /// 4个关键点位置，左眼中心、右眼中心、鼻尖、嘴中心。face_fields包含landmark时返回
        /// </summary>
        public Mark[] landmark { get; set; }
        /// <summary>
        /// 72个特征点位置，示例图 。face_fields包含landmark时返回
        /// </summary>
        public Mark[] landmark72 { get; set; }
        /// <summary>
        /// yellow、white、black、arabs。face_fields包含race时返回
        /// </summary>
        public string race { get; set; }
        /// <summary>
        /// 人种置信度，范围0~1。face_fields包含race时返回
        /// </summary>
        public double race_probability { get; set; }
        /// <summary>
        /// 人脸质量信息。face_fields包含qualities时返回
        /// </summary>
        public Qualities qualities { get; set; }
    }
    /// <summary>
    /// 人脸在图像中的位置
    /// </summary>
    [Serializable]
    public class FaceLocation
    {
        public int left { get; set; }
        public int top { get; set; }

        public int width { get; set; }

        public int height { get; set; }
    }

    /// <summary>
    /// 脸型
    /// </summary>
    [Serializable]
    public class FaceshapeInfo
    {
        /// <summary>
        /// 脸型：square/triangle/oval/heart/round
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 脸型置信度0~1
        /// </summary>
        public double probability { get; set; }
    }

    /// <summary>
    /// 标记位置
    /// </summary>
    [Serializable]
    public class Mark
    {
        public int x { get; set; }
        public int y { get; set; }
    }

    /// <summary>
    /// 人脸质量信息。face_fields包含qualities时返回
    /// </summary>
    [Serializable]
    public class Qualities
    {
        public Occlusion occlusion { get; set; }
        /// <summary>
        /// 人脸模糊程度，[0, 1]。0表示清晰，1表示模糊
        /// </summary>
        public double blur { get; set; }
        /// <summary>
        /// 取值范围在[0,255],表示脸部区域的光照程度
        /// </summary>
        public double illumination { get; set; }
        /// <summary>
        /// 人脸完整度，[0, 1]。0表示完整，1表示不完整
        /// </summary>
        public double completeness { get; set; }
        public FaceType type { get; set; }
    }

    /// <summary>
    /// 人脸各部分遮挡的概率,[0, 1],0表示完整，1表示不完整
    /// </summary>
    [Serializable]
    public class Occlusion
    {
        /// <summary>
        /// 左眼
        /// </summary>
        public double left_eye { get; set; }
        /// <summary>
        /// 右眼
        /// </summary>
        public double right_eye { get; set; }
        /// <summary>
        /// 鼻子
        /// </summary>
        public double nose { get; set; }
        /// <summary>
        /// 嘴
        /// </summary>
        public double mouth { get; set; }
        /// <summary>
        /// 左脸颊
        /// </summary>
        public double left_cheek { get; set; }
        /// <summary>
        /// 右脸颊
        /// </summary>
        public double right_cheek { get; set; }
        /// <summary>
        /// 下巴
        /// </summary>
        public double chin { get; set; }
    }

    /// <summary>
    /// 真实人脸/卡通人脸置信度
    /// </summary>
    [Serializable]
    public class FaceType
    {
        /// <summary>
        /// 真实人脸置信度，[0, 1]
        /// </summary>
        public double human { get; set; }

        /// <summary>
        /// 卡通人脸置信度，[0, 1]
        /// </summary>
        public double cartoon { get; set; }
    }
}
