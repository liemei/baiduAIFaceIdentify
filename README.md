> baiduAIFaceIdentify项目是C#语言，集成百度AI的SDK利用AForge开发的实时人脸识别的小demo，里边包含了人脸检测识别，人脸注册，人脸登录等功能

#### 人脸实时检测识别功能
思路是利用AForge打开摄像头，通过摄像头获取到的图像显示在winform窗体中AForge的控件中，利用AForge控件中的NewFrame事件获取要显示的每一帧的图像，获取图像传输到百度AI平台进行人脸检测，并且将检测结果反馈到界面显示的图像中。在这个过程中有两个问题，获取图像上传到百度AI平台进行分析需要时间，这个时间跟网络有关，所以需要单独一个线程进行人脸识别，第二个问题，百度人脸识别接口开发者一秒内只能掉用2次接口，所以需要控制不是每一帧的图像都要上传。所以基于以上思路

首先页面初始化的时候获取视频设备、启动一个单独线程控制1秒内人脸检测的次数：
```
 private void Form1_Load(object sender, EventArgs e)
        {
            /// 获取电脑已经安装的视频设备
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices!=null && videoDevices.Count>0)
            {
                foreach (FilterInfo device in videoDevices)
                {
                    comboBox1.Items.Add(device.Name);
                }
                comboBox1.SelectedIndex = 0;
            }
            videoSourcePlayer1.NewFrame += VideoSourcePlayer1_NewFrame;

            // 开发者在百度AI平台人脸识别接口只能1秒中调用2次，所以需要做 定时开始检测，每个一秒检测2次
            ThreadPool.QueueUserWorkItem(new WaitCallback(p => {
                while (true)
                {
                    IsStart = true;
                    Thread.Sleep(500);
                }
            }));
        }
```

其次，在NewFrame的回调方法中，根据IsStart判断是否要开始人脸识别，并且另外启动一个线程进行人脸识别操作，判断如果已经有识别过的结构，根据返回的人脸的位置，在当前的一帧图像中绘制方框指示出识别出的人脸位置
```
private void VideoSourcePlayer1_NewFrame(object sender, ref Bitmap image)
        {
            try
            {
                if (IsStart)
                {
                    IsStart = false;
                    // 在线程池中另起一个线程进行人脸检测,这样不会造成界面视频卡顿现象
                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.Detect), image.Clone());
                }
                if (location != null)
                {
                    try
                    {
                        // 绘制方框套住人脸
                        Graphics g = Graphics.FromImage(image);
                        g.DrawLine(new Pen(Color.Black), new System.Drawing.Point(location.left, location.top), new System.Drawing.Point(location.left + location.width, location.top));
                        g.DrawLine(new Pen(Color.Black), new System.Drawing.Point(location.left, location.top), new System.Drawing.Point(location.left, location.top + location.height));
                        g.DrawLine(new Pen(Color.Black), new System.Drawing.Point(location.left, location.top + location.height), new System.Drawing.Point(location.left + location.width, location.top + location.height));
                        g.DrawLine(new Pen(Color.Black), new System.Drawing.Point(location.left + location.width, location.top), new System.Drawing.Point(location.left + location.width, location.top + location.height));
                        g.Dispose();

                    }
                    catch (Exception ex)
                    {
                        ClassLoger.Error("VideoSourcePlayer1_NewFrame", ex);
                    }
                }
            } catch (Exception ex)
            {
                ClassLoger.Error("VideoSourcePlayer1_NewFrame1", ex);
            }

        }
```

#### 人脸注册。

在一些类似刷脸签到、刷脸登录的应用场景中，根据人脸获取人物信息，前提就是人脸注册，人脸注册就是获取当前摄像头的一帧图像，调用百度AI的人脸注册接口进行注册

```
 // 用户ID
            string uid = "1";
            // 用户资料，长度限制256B
            string userInfo = textBox6.Text.Trim();
            // 用户组ID
            string groupId = textBox5.Text.Trim();

            if (comboBox1.Items.Count <= 0)
            {
                MessageBox.Show("请插入视频设备");
                return;
            }
            try
            {
                if (videoSourcePlayer1.IsRunning)
                {
                    BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                                    videoSourcePlayer1.GetCurrentVideoFrame().GetHbitmap(),
                                    IntPtr.Zero,
                                     Int32Rect.Empty,
                                    BitmapSizeOptions.FromEmptyOptions());
                    var img = BitmapSource2Byte(bitmapSource);
                    var options = new Dictionary<string, object>{
                        {"action_type", "replace"}
                    };
                    var result = client.UserAdd(uid, userInfo, groupId, img, options);
                    if (result.ToString().Contains("error_code"))
                    {
                        MessageBox.Show("注册失败:" + result.ToString());
                    }
                    else
                    {
                        MessageBox.Show("注册成功");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("摄像头异常：" + ex.Message);
            }
```

#### 人脸登录

人脸登录和人脸注册的方式一样，只不过调用的是百度AI的人脸登录接口
```
// 用户ID
            string uid = "1";
            // 用户资料，长度限制256B
            string userInfo = textBox6.Text.Trim();
            // 用户组ID
            string groupId = textBox5.Text.Trim();

            if (comboBox1.Items.Count <= 0)
            {
                MessageBox.Show("请插入视频设备");
                return;
            }
            try
            {
                if (videoSourcePlayer1.IsRunning)
                {
                    BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                                    videoSourcePlayer1.GetCurrentVideoFrame().GetHbitmap(),
                                    IntPtr.Zero,
                                     Int32Rect.Empty,
                                    BitmapSizeOptions.FromEmptyOptions());
                    var img = BitmapSource2Byte(bitmapSource);

                    // 如果有可选参数
                    //var options = new Dictionary<string, object>{
                    //    {"ext_fields", "faceliveness"},
                    //    {"user_top_num", 3}
                    //};

                    var result = client.Identify(groupId, img);
                    FaceIdentifyInfo info = JsonHelper.DeserializeObject<FaceIdentifyInfo>(result.ToString());
                    if (info!=null && info.result!=null && info.result.Length>0)
                    {
                        textBox7.Text = info.result[0].user_info;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("摄像头异常：" + ex.Message);
            }
```
