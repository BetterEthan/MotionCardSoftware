using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using WW.Cad.IO;
using WW.Cad.Model;
using DxfViewExample;
using WW.Math;
namespace MotionCardSoftware
{
    

    public partial class MotionControlPlatform : Form
    {

        //用于鼠标按下后的不同模式选择
        private string MouseDownMode = "";

        //用于记录鼠标按下时的像素点坐标
        private Point MouseDownPixelPoint;

        //用于记录鼠标按下时的实际点坐标
        private PointF MouseDownRealPoint;

        //存取关键点的数组
        List<KeyPoint> keyPoints = new List<KeyPoint>();

        static public string PathClear = "路径已清空";

        private string DrawPointFlag = "已填入";

        private string usartMode = "";

        private string feedBackInfo = "";

        //声明串口
        private SerialPort Usart = new SerialPort();

        //上位机绘制关键点信息
        private string BsplineDrawPathInf = Directory.GetCurrentDirectory() + "\\BsplineDrawPathInf.txt";
        //串口接收示教点信息
        private string RecieveTeachingPathInf = Directory.GetCurrentDirectory() + "\\RecieveTeachingPathInf.txt";
        //串口接收实际跟随信息
        private string RecieveRealPointInf = Directory.GetCurrentDirectory() + "\\RecieveRealPointInf.txt";
        //速度信息
        private string VelInf = Directory.GetCurrentDirectory() + "\\VelInformation.txt";
        //计算所得关键点信息
        private string PathAllInf = Directory.GetCurrentDirectory() + "\\PathAllInf.txt";
        //更新速度txt文件
        private string SpeedReplanInfo = Directory.GetCurrentDirectory() + "\\speedReplan.txt";


        //MotionControlPlatform初始化函数
        public MotionControlPlatform()
        {
            InitializeComponent();
        }

        //控制平台Form装载函数
        private void MotionControlPlatform_Load(object sender, EventArgs e)
        {
            //串口参数初始化
            Usart.BaudRate = 115200;
            Usart.DataBits = 8;
            Usart.StopBits = System.IO.Ports.StopBits.One;
            Usart.Parity = System.IO.Ports.Parity.None;
            Usart.ReadBufferSize = 4096;
            KeyPointNumLabel.Text = "";
            NoticeInformation.Text = "";

            //初始化背景
            string filename = Directory.GetCurrentDirectory() + "\\blank.dwg";

                DxfModel model;
            string extension = Path.GetExtension(filename);
            if (string.Compare(extension, ".dwg", true) == 0)
            {
                model = DwgReader.Read(filename);
            }
            else
            {
                model = DxfReader.Read(filename);
            }

            viewControl.Model = model;
        }






        private void viewControl_MouseDown(object sender, MouseEventArgs e)
        {
            switch (MouseDownMode)
            {
                case "Bspline绘制":
                    if (DrawPointFlag == "未填入")
                    {
                        MessageBox.Show("请输入姿态(单位:度)", "注意", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        //记录鼠标点击时的像素点坐标
                        MouseDownPixelPoint.X = e.X;
                        MouseDownPixelPoint.Y = e.Y;
                        MouseDownRealPoint = ViewControl.GetRealAxes(e.X, e.Y);
                    }
                    break;

                case "":
                    //记录鼠标点击时的像素点坐标
                    MouseDownPixelPoint.X = e.X;
                    MouseDownPixelPoint.Y = e.Y;
                    MouseDownRealPoint = ViewControl.GetRealAxes(e.X, e.Y);
                    break;
            }

        }

        private void viewControl_MouseUp(object sender, MouseEventArgs e)
        {
            Graphics myGra = viewControl.CreateGraphics();
            Pen myPen = new Pen(System.Drawing.Color.Yellow, 1);
            switch (MouseDownMode)
            {
                case "Bspline绘制":

                    myGra.DrawEllipse(myPen, MouseDownPixelPoint.X, MouseDownPixelPoint.Y, 3, 3);
                    DrawPointFlag = "未填入";

                    break;

                case "":
                    break;
                default:
                    break;
            }
        }





        private void viewControl_MouseMove(object sender, MouseEventArgs e)
        {
            Point2D point = viewControl.GetModelSpaceCoordinates(new Point2D(e.X, e.Y));
            label1.Text = string.Format("{0:0.000}mm, {1:0.000}mm", point.X, point.Y);
        }



        private void 确定输入_Click(object sender, EventArgs e)
        {
            if (DrawPointFlag == "已填入")
            {
                MessageBox.Show("未采集任何新点！！", "注意", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            float posAngle = 0.0f;


            //注意输入单位为度，无范围限制
            try
            {
                posAngle = float.Parse(InputBox.Text);
            }
            catch
            {
                MessageBox.Show("输入有误", "注意", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            keyPoints.Add(new KeyPoint(MouseDownRealPoint, posAngle));

            KeyPointNumLabel.Text = keyPoints.Count.ToString();

            DrawPointFlag = "已填入";
        }



        static public string DRAW_INF = "";
        private void 路径绘制_Click(object sender, EventArgs e)
        {
            //
            if (MouseDownMode == "Bspline绘制")
            {
                if (DrawPointFlag == "未填入")
                {
                    MessageBox.Show("请输入姿态(单位:度)", "注意", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                //完成绘制结束工作
                MouseDownMode = "";

                路径绘制.Text = "路径绘制";

                DRAW_INF = "绘制完成";

                KeyPointNumLabel.Text = "";

                //单独一个点是不允许的
                if (keyPoints.Count() == 1) return;

                StreamWriter keyPointFile = new StreamWriter(BsplineDrawPathInf, false);

                Graphics myGra = viewControl.CreateGraphics();

                Pen myPen = new Pen(System.Drawing.Color.Red, 2);

                foreach (KeyPoint keyP in keyPoints)
                {
                    keyPointFile.WriteLine("{0:f2},{1:f2},{2:f2}", keyP.point.X, keyP.point.Y, keyP.direction);
                }

                keyPointFile.Close();

                //进行分段处理，便于速度规划和数据传输
                Bspline.BsplineSegment(BsplineDrawPathInf, PathAllInf);

                //曲率半径计算
                SpeedPlanning.CalculateCurvature();

                PathClear = "绘制路径";

                路径恢复.Enabled = true;

                路径清空.Enabled = true;

                viewControl.Refresh();
            }
            else
            {
                //开始绘制准备工作
                MessageBox.Show("默认第一点为(0,0),姿态为0度", "注意", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                keyPoints.Clear();

                keyPoints.Add(new KeyPoint(0.0f, 0.0f, 0.0f));

                KeyPointNumLabel.Text = keyPoints.Count.ToString();

                MouseDownMode = "Bspline绘制";

                路径绘制.Text = "完成绘制";

                DRAW_INF = "正在绘制";

                路径恢复.Enabled = false;

                路径清空.Enabled = false;
            }

        }

        private void 路径清空_Click(object sender, EventArgs e)
        {
            PathClear = "路径已清空";
            viewControl.Refresh();
        }

        private void 路径恢复_Click(object sender, EventArgs e)
        {
            PathClear = "绘制路径";
            viewControl.Refresh();
        }

        private void 示教路径恢复_Click(object sender, EventArgs e)
        {
            PathClear = "示教路径";
            viewControl.Refresh();
        }

        private void 实时路径恢复_Click(object sender, EventArgs e)
        {
            RealPathReview();
        }




        private void 串口开关_Click(object sender, EventArgs e)
        {
            if (串口开关.Text == "打开串口")
            {
                if (UsartBox.Text == "")
                {
                    MessageBox.Show("请选择串口！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                Usart.PortName = UsartBox.Text;
                if (Usart.IsOpen == false)
                {
                    Usart.Open();
                }
                //关闭串口选择功能
                UsartBox.Enabled = false;

                串口开关.Text = "关闭串口";
            }
            else if (串口开关.Text == "关闭串口")
            {
                Usart.Close();

                //关闭串口选择功能
                NoticeInformation.Text = "";

                UsartBox.Enabled = true;

                串口开关.Text = "打开串口";
            }
        }




        private void 示教路径_Click(object sender, EventArgs e)
        {
            if (示教路径.Text == "示教路径")
            {
                if (Usart.IsOpen == false)
                {
                    MessageBox.Show("请开启串口！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //清空txt文档
                FileStream fs = new FileStream(RecieveTeachingPathInf, FileMode.Create, FileAccess.Write);

                fs.Close();

                示教路径.Text = "完成接收";
                usartMode = "示教路径";

                实时路径.Enabled = false;
                TimeReadCom.Enabled = true;
            }
            else
            {
                PathClear = "示教路径";
                NoticeInformation.Text = "";
                示教路径.Text = "示教路径";
                usartMode = "";
                TimeReadCom.Enabled = false;
                实时路径.Enabled = true;

                FileTxt.FileTxtCopy(RecieveTeachingPathInf, PathAllInf);

                viewControl.Refresh();
            }

        }

        private void 实时路径_Click(object sender, EventArgs e)
        {
            if (实时路径.Text == "实时路径")
            {
                if (Usart.IsOpen == false)
                {
                    MessageBox.Show("请开启串口！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //准备开启
                实时路径.Text = "接受完成";
                usartMode = "实时路径";
                示教路径.Enabled = false;
                TimeReadCom.Enabled = true;
                //清空txt文档
                FileStream fs = new FileStream(RecieveRealPointInf, FileMode.Create, FileAccess.Write);
                fs.Close();
                FileStream ffs = new FileStream(VelInf, FileMode.Create, FileAccess.Write);
                ffs.Close();
            }
            else
            {
                NoticeInformation.Text = "";
                实时路径.Text = "实时路径";
                usartMode = "";
                TimeReadCom.Enabled = false;
                示教路径.Enabled = true;
            }
        }

        private void 开始运行_Click(object sender, EventArgs e)
        {
            if (Usart.IsOpen == false)
            {
                MessageBox.Show("请开启串口！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //开启串口接收中断
            TimeReadCom.Enabled = true;

            usartMode = "发送路径";

            SendLabel.Text = "发送中";

            while (true)
            {
                Usart.Write("a" + (1.2000).ToString() + "s");
                //延时100ms
                Delay(100);
                if (feedBackInfo == "y")
                {
                    feedBackInfo = "";
                    TimeReadCom.Enabled = false;
                    SendLabel.Text = "";
                    NoticeInformation.Text = "";


                    //开启实时 显示功能
                    //准备开启
                    实时路径.Text = "接受完成";
                    usartMode = "实时路径";
                    示教路径.Enabled = false;
                    TimeReadCom.Enabled = true;
                    //清空txt文档
                    FileStream fs = new FileStream(RecieveRealPointInf, FileMode.Create, FileAccess.Write);
                    fs.Close();
                    FileStream ffs = new FileStream(VelInf, FileMode.Create, FileAccess.Write);
                    ffs.Close();
                    break;
                }
            }
        }


        private void 停止运行_Click(object sender, EventArgs e)
        {
            if (Usart.IsOpen == false)
            {
                MessageBox.Show("请开启串口！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //开启串口接收中断
            TimeReadCom.Enabled = true;

            usartMode = "发送路径";

            SendLabel.Text = "发送中";

            while (true)
            {
                Usart.Write("a" + (1.3000).ToString() + "s");
                //延时100ms
                Delay(100);
                if (feedBackInfo == "y")
                {
                    feedBackInfo = "";
                    TimeReadCom.Enabled = false;
                    SendLabel.Text = "";
                    NoticeInformation.Text = "";
                    break;
                }
            }
        }

        private void 发送路径_Click(object sender, EventArgs e)
        {
            if (Usart.IsOpen == false)
            {
                MessageBox.Show("请开启串口！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //开启串口接收中断
            TimeReadCom.Enabled = true;

            usartMode = "发送路径";

            SendLabel.Text = "发送中";

            //数据读取
            StreamReader pathFile = File.OpenText(PathAllInf);

            List<string> tempString = new List<string>();

            while (!pathFile.EndOfStream)
            {
                tempString.Add(pathFile.ReadLine());
            }

            pathFile.Close();


            //如果点数小于2，不能规划出轨迹
            if (tempString.Count() < 2)
            {
                MessageBox.Show("请规划轨迹！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            int count = 0;


            while (true)
            {

                Usart.Write("b");
                string[] sArray = tempString[count].Split(',');
                float sum = 0.0f;
                for (int i = 0; i < sArray.Count(); i++)
                {
                    Usart.Write(sArray[i] + ",");
                    sum += float.Parse(sArray[i]);
                }


                Usart.Write(sum.ToString() + ",");
                Usart.Write((count + 1).ToString() + ",");
                Usart.Write("s");
                //延时100ms
                Delay(100);

                if (feedBackInfo == "y")
                {
                    count++;
                    feedBackInfo = "";
                }
                Console.WriteLine(count);

                if (count == tempString.Count())
                {
                    break;
                }
            }

            while (true)
            {
                Usart.Write("a" + (1.1).ToString() + "s");
                //延时100ms
                Delay(100);
                if (feedBackInfo == "y")
                {
                    feedBackInfo = "";
                    TimeReadCom.Enabled = false;
                    SendLabel.Text = "";
                    NoticeInformation.Text = "";
                    break;
                }
            }

        }

        //延时函数
        public static void Delay(int milliSecond)
        {
            int start = Environment.TickCount;
            while (Math.Abs(Environment.TickCount - start) < milliSecond)
            {
                Application.DoEvents();
            }
        }

        //串口选择框
        private void UsartBox_DropDown(object sender, EventArgs e)
        {
            string[] names = SerialPort.GetPortNames();
            UsartBox.Items.Clear();
            for (int i = 0; i < names.Length; i++)
            {
                UsartBox.Items.Add(names[i]);
            }
        }


        //串口接收定时器事件
        private void TimeReadCom_Tick(object sender, EventArgs e)
        {
            NoticeInformation.Text = "";
            if (Usart.IsOpen == false) return;
            try
            {
                //记录缓存数量
                int n = Usart.BytesToRead;

                if (n == 0) return;

                NoticeInformation.Text = "接收数据中";

                //声明一个临时数组来存储当前来的串口数据
                byte[] buf = new byte[n];



                //读取缓冲数据
                Usart.Read(buf, 0, n);

                //丢弃接受缓冲区数据
                Usart.DiscardInBuffer();

                string tempStr = " ";

                this.Invoke((EventHandler)(delegate
                {
                   
                    //将字节全部转换为字符串！
                    tempStr = (Encoding.ASCII.GetString(buf));
                    switch (usartMode)
                    { 
                        //实时显示实际轨迹
                        case "实时路径":
                            PointRecieve(tempStr, RecieveRealPointInf);
                            break;

                        case "示教路径":
                            PointRecieve(tempStr, RecieveTeachingPathInf);
                            break;

                        case "发送路径":
                            feedBackInfo = tempStr;
                            break;
                        default:
                            break;
                    
                    }

                   

                }));
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(Usart.BytesToRead.ToString() + "接收串口信息错误：" + ex.Message);
            }
        }

        //坐标点接收函数
        private void PointRecieve(string tempStr,string DataPath)
        {
            try
            {
                StreamWriter velFile = new StreamWriter(VelInf, true);
                StreamWriter keyPointFile = new StreamWriter(DataPath, true);

                //轨迹实时显示
                Graphics myGra = viewControl.CreateGraphics();

                Pen myPen = new Pen(System.Drawing.Color.Red, 1);


                string[] sArray = tempStr.Split('\n');


                for (int i = 0; i < sArray.Length - 1; i++)
                {
                    if (i == 0) continue;
                    string[] tempArray = sArray[i].Split('\t');
                    if (tempArray.Length < 2) return;


                    PointF pnt;
                    pnt = ViewControl.GetPanelAxes(float.Parse(tempArray[0]), float.Parse(tempArray[1]));
                    myGra.DrawEllipse(myPen, pnt.X, pnt.Y, 1, 1);


                    if (DataPath == RecieveRealPointInf)
                    {
                        keyPointFile.Write(tempArray[0] + "," + tempArray[1] + "\r\n");
                        velFile.WriteLine(tempArray[2] + "," + tempArray[3] + "," + tempArray[4] + "," + tempArray[5] + "," + tempArray[6] + "," +
                            tempArray[7] + "," + tempArray[8] + "," + tempArray[9] );
                    }
                    else //示教关键点信息
                    {
                        keyPointFile.Write(tempArray[0] + "," + tempArray[1] + "," + tempArray[2] + "\r\n");
                    }


                }

                keyPointFile.Close();
                velFile.Close();
            }
            catch
            {
                return;
            }
        }


        


        //跟随点显示
        private void RealPathReview()
        {
            //数据读取
            StreamReader pathFile = File.OpenText(RecieveRealPointInf);

            List<string> tempString = new List<string>();

            while (!pathFile.EndOfStream)
            {
                tempString.Add(pathFile.ReadLine());
            }

            pathFile.Close();


            //如果点数小于2，不能规划出轨迹
            if (tempString.Count() < 2) return;

            Graphics myGra = viewControl.CreateGraphics();

            Pen myPen = new Pen(System.Drawing.Color.Red, 1);

            foreach (string str in tempString)
            {
                string[] sArray = str.Split(',');
                PointF pnt;
                pnt = ViewControl.GetPanelAxes(float.Parse(sArray[0]), float.Parse(sArray[1]));
                myGra.DrawEllipse(myPen, pnt.X, pnt.Y, 1, 1);
            }

        }

        private void 速度图像_Click(object sender, EventArgs e)
        {
            new SpeedShow().Show();
        }



        private void button1_Click(object sender, EventArgs e)
        {
           
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //进行分段处理，便于速度规划和数据传输
            //Bspline.BsplineSegment(BsplineDrawPathInf, PathAllInf);

            //曲率半径计算
            //SpeedPlanning.CalculateCurvature();

            //Usart.Write("s");


            //Console.WriteLine(MotionCardParameter.GetAccMax());

            //PointsInfo.pnts.Add(new KeyPointInf(1,2,0));
            //Console.WriteLine("切分图像长度");

            //Bspline.test2(PathAllInf);

            //Console.WriteLine("原路径长度");

            //Console.WriteLine(Bspline.test1(BsplineDrawPathInf));

            //BsplineReviewWithDir(PathAllInf);
            //BsplineReview(BsplineDrawPathInf);
        }

        private void 关键点信息_Click(object sender, EventArgs e)
        {
            new DataShow().Show();
        }

        private void 打开文件_Click(object sender, EventArgs e)
        {
            string filename = null;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "AutoCad files (*.dxf, *.dwg)|*.dxf;*.dwg";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    filename = openFileDialog.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error occurred: " + ex.Message);
                    Environment.Exit(1);
                }
            }
            else
            {
                Environment.Exit(0);
            }

            DxfModel model;
            string extension = Path.GetExtension(filename);
            if (string.Compare(extension, ".dwg", true) == 0)
            {
                model = DwgReader.Read(filename);
            }
            else
            {
                model = DxfReader.Read(filename);
            }

            viewControl.Model = model;
        }












       
    }
}
//byte [] data =new UTF8Encoding().GetBytes(String);