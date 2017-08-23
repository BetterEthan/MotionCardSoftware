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


namespace MotionCardSoftware
{
    public partial class SpeedShow : Form
    {



        const float ZOOM = 0.06f;

        //坐标原点偏移量
        static protected int offsetX;
        static protected int offsetY;

        //图像放大系数
        static protected float zoom = ZOOM;
        static protected float zoomL =0.15f;

        private int tempOffsetX;
        private int tempOffsetY;


        //速度更新
        private string VelInf = Directory.GetCurrentDirectory() + "\\VelInformation.txt";

        //更新速度txt文件
        private string SpeedReplanInfo = Directory.GetCurrentDirectory() + "\\speedReplan.txt";


        private float pathMax = 0;



        public SpeedShow()
        {
            InitializeComponent();
        }

        private void VelDisplay_Paint(object sender, PaintEventArgs e)
        {
            //this.AutoScrollMinSize = new Size(ClientRectangle.Width, ClientRectangle.Height);
            //绘制虚线表格
            Graphics myGra = VelDisplay.CreateGraphics();

            Pen myPen = new Pen(Color.Black, 1);
            myPen.EndCap = LineCap.ArrowAnchor;

            //绘画栅格
            ControlPaint.DrawGrid(e.Graphics, this.ClientRectangle, new Size(20, 20), Color.White);

            //绘制坐标系中心
            AdjustableArrowCap myLineCap = new AdjustableArrowCap(3, 3, true);
            myPen.CustomEndCap = myLineCap;
            myGra.DrawLine(myPen, 10, VelDisplay.Bottom,10, 5);
            myGra.DrawLine(myPen, 0, VelDisplay.Bottom / 2, VelDisplay.Right - 10, VelDisplay.Bottom / 2);

            switch (VelDisplayBox.Text)
            {
                case "1号轮速度":
                case "2号轮速度":
                case "3号轮速度":
                case "全部速度":
                    WheelVelCurveDrawing();
                    单位.Text = "mm/s";
                    break;

                case "合速度":
                case "合速度方向":
                case "旋转速度":
                    ResultantVelCurveDrawing();
                    
                    break;

                case "位置误差":
                    ErrCurveDrawing();
                    ResultantVelCurveDrawing();
                    单位.Text = "mm";
                    break;

                default:
                    break;

            }
            
            
            //ErrCurveDrawing();
        }

        private void SpeedShow_Load(object sender, EventArgs e)
        {
            //SetRoomAndHScrollBar();
            //设置坐标系偏移量
            offsetX = tempOffsetX = 10;
            offsetY = tempOffsetY = VelDisplay.Bottom / 2;
            
        }

        //将像素坐标转换为机器人场地坐标
        static public PointF GetRealAxes(float x, float y)
        {
            PointF realAxes = new PointF();
            realAxes.X = (x - offsetX) / zoomL;
            realAxes.Y = (-y + offsetY) / zoom;
            return realAxes;
        }


        //将机器人场地坐标转换为像素坐标。有放大系数zoom！！！当鼠标滚轮滚动时坐标会变化
        //例如,填入(0,0),实际像素点坐标确是(280,200)
        static public PointF GetPanelAxes(float x, float y)
        {
            PointF realAxes = new PointF();
            realAxes.X = x * zoomL + offsetX;
            realAxes.Y = -y * zoom + offsetY;
            return realAxes;
        }

        //将panel坐标系y轴反向，坐标系原点移动。返回其pannel的坐标。无放大系数zoom！！！当鼠标滚轮滚动时坐标不会变化
        //例如,填入(0,0),实际像素点坐标确是(280,200)


        private void VelDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            //显示实际坐标
            Vel.Text = String.Format("{0:F2}", GetRealAxes(e.X, e.Y).Y);
            Length.Text = String.Format("{0:F2}", GetRealAxes(e.X, e.Y).X) + "mm";

        }



        //绘画三个轮速度图像
        private void WheelVelCurveDrawing()
        {
            //数据读取
            StreamReader pathFile = File.OpenText(VelInf);

            List<string> tempString = new List<string>();

            while (!pathFile.EndOfStream)
            {
                tempString.Add(pathFile.ReadLine());
            }

            pathFile.Close();


            //如果点数小于2，不能规划出轨迹
            if (tempString.Count() < 2) return;

            Graphics myGra = VelDisplay.CreateGraphics();

            Pen myPen = new Pen(Color.Red, 1);

            PointF[] velWheel1 = new PointF[tempString.Count()];
            PointF[] velWheel2 = new PointF[tempString.Count()];
            PointF[] velWheel3 = new PointF[tempString.Count()];
            PointF[] targetVelWheel1 = new PointF[tempString.Count()];
            PointF[] targetVelWheel2 = new PointF[tempString.Count()];
            PointF[] targetVelWheel3 = new PointF[tempString.Count()];
            int i = 0;



            //设置zoom值
            foreach (string str in tempString)
            {
                string[] sArray = str.Split(',');

                velWheel1[i].X = velWheel2[i].X = velWheel3[i].X = float.Parse(sArray[0]);
                velWheel1[i].Y = float.Parse(sArray[4]);
                velWheel2[i].Y = float.Parse(sArray[5]);
                velWheel3[i].Y = float.Parse(sArray[6]);
                i++;
            }

            //计算速度最大值，用于调整画面比例
            float[] vel = new float[6];
            vel[0] = FindMaxAbs(velWheel1).Y;
            vel[1] = FindMaxAbs(velWheel2).Y;
            vel[2] = FindMaxAbs(velWheel3).Y;

            zoom = (float)(VelDisplay.Bottom / 2) / vel.Max();
            pathMax = FindMaxAbs(velWheel1).X;

            //设置滚动条最大值
            hScrollBar1.Maximum = (int)(PERCENT * pathMax / (VelDisplay.Right / zoomL)) + 1;





            i = 0;
            //读取文件
            foreach (string str in tempString)
            {
                string[] sArray = str.Split(',');
                velWheel1[i] = GetPanelAxes(float.Parse(sArray[0]), float.Parse(sArray[1]));
                velWheel2[i] = GetPanelAxes(float.Parse(sArray[0]), float.Parse(sArray[2]));
                velWheel3[i] = GetPanelAxes(float.Parse(sArray[0]), float.Parse(sArray[3]));
                targetVelWheel1[i] = GetPanelAxes(float.Parse(sArray[0]), float.Parse(sArray[4]));
                targetVelWheel2[i] = GetPanelAxes(float.Parse(sArray[0]), float.Parse(sArray[5]));
                targetVelWheel3[i] = GetPanelAxes(float.Parse(sArray[0]), float.Parse(sArray[6]));
                i++;
            }



            //进行滤波处理
            for (int n = 0; n < 5; n++)
            {
                for (int j = 1; j < targetVelWheel1.Count() - 1; j++)
                {
                    velWheel1[j].Y = (velWheel1[j - 1].Y + velWheel1[j + 1].Y) / 2;
                    velWheel2[j].Y = (velWheel2[j - 1].Y + velWheel2[j + 1].Y) / 2;
                    velWheel3[j].Y = (velWheel3[j - 1].Y + velWheel3[j + 1].Y) / 2;
                    targetVelWheel1[j].Y = (targetVelWheel1[j - 1].Y + targetVelWheel1[j + 1].Y) / 2;
                    targetVelWheel2[j].Y = (targetVelWheel2[j - 1].Y + targetVelWheel2[j + 1].Y) / 2;
                    targetVelWheel3[j].Y = (targetVelWheel3[j - 1].Y + targetVelWheel3[j + 1].Y) / 2;
                }
            }



            switch (VelDisplayBox.Text)
            {
                case "1号轮速度":
                        myPen.Color = Color.Red;
                        myGra.DrawLines(myPen, velWheel1);
                        myPen.Color = Color.Blue;
                        myGra.DrawLines(myPen, targetVelWheel1);
                    break;

                case "2号轮速度":
                        myPen.Color = Color.Red;
                        myGra.DrawLines(myPen, velWheel2);
                        myPen.Color = Color.Blue;
                        myGra.DrawLines(myPen, targetVelWheel2);
                    break;

                case "3号轮速度":
                        myPen.Color = Color.Red;
                        myGra.DrawLines(myPen, velWheel3);
                        myPen.Color = Color.Blue;
                        myGra.DrawLines(myPen, targetVelWheel3);
                    break;


            }


        }




        //绘制合速度图像
        void ResultantVelCurveDrawing()
        {
            //数据读取
            StreamReader pathFile = File.OpenText(VelInf);

            List<string> tempString = new List<string>();

            while (!pathFile.EndOfStream)
            {
                tempString.Add(pathFile.ReadLine());
            }

            pathFile.Close();


            //如果点数小于2，不能规划出轨迹
            if (tempString.Count() < 2) return;

            Graphics myGra = VelDisplay.CreateGraphics();

            Pen myPen = new Pen(Color.Red, 1);

            PointF[] velWheel1 = new PointF[tempString.Count()];
            PointF[] velWheel2 = new PointF[tempString.Count()];
            PointF[] velWheel3 = new PointF[tempString.Count()];
            PointF[] targetVelWheel1 = new PointF[tempString.Count()];
            PointF[] targetVelWheel2 = new PointF[tempString.Count()];
            PointF[] targetVelWheel3 = new PointF[tempString.Count()];

            float[] zAngle = new float[tempString.Count()];
            int i = 0;
            foreach (string str in tempString)
            {
                string[] sArray = str.Split(',');

                velWheel1[i].X = velWheel2[i].X = velWheel3[i].X
                = targetVelWheel1[i].X = targetVelWheel2[i].X = targetVelWheel3[i].X
                = float.Parse(sArray[0]);

                velWheel1[i].Y = float.Parse(sArray[1]);
                velWheel2[i].Y = float.Parse(sArray[2]);
                velWheel3[i].Y = float.Parse(sArray[3]);

                targetVelWheel1[i].Y = float.Parse(sArray[4]);
                targetVelWheel2[i].Y = float.Parse(sArray[5]);
                targetVelWheel3[i].Y = float.Parse(sArray[6]);
                i++;
            }

            PointF[] trueVel = new PointF[tempString.Count()];
            PointF[] targetVel = new PointF[tempString.Count()];
            PointF[] trueDir = new PointF[tempString.Count()];
            PointF[] targetDir = new PointF[tempString.Count()];
            PointF[] trueRotateVel = new PointF[tempString.Count()];
            PointF[] targetRotateVel = new PointF[tempString.Count()];
            PointF[] DirectionErr = new PointF[tempString.Count()];
            PointF[] binaryzation = new PointF[tempString.Count()];

           






            //由三个轮子的速度计算合成速度
            for (int j = 0; j < targetVelWheel1.Count(); j++)
            {
                Calculate.TriWheelVel2_t tempVel;
                tempVel = Calculate.TriWheelVel2ResultantVel(velWheel1[j].Y, velWheel2[j].Y, velWheel3[j].Y, zAngle[j]);

                trueVel[j].Y = tempVel.speed;
                trueVel[j].X = velWheel1[j].X;
                trueDir[j].Y = tempVel.direction;
                trueDir[j].X = velWheel1[j].X;
                trueRotateVel[j].Y = tempVel.rotationVel;
                trueRotateVel[j].X = velWheel1[j].X;


                tempVel = Calculate.TriWheelVel2ResultantVel(targetVelWheel1[j].Y, targetVelWheel2[j].Y, targetVelWheel3[j].Y, zAngle[j]);
                targetVel[j].Y = tempVel.speed;
                targetVel[j].X = targetVelWheel1[j].X;
                targetDir[j].Y = tempVel.direction;
                targetDir[j].X = targetVelWheel1[j].X;
                targetRotateVel[j].Y = tempVel.rotationVel;
                targetRotateVel[j].X = targetVelWheel1[j].X;



            }


            pathMax = FindMaxAbs(targetDir).X;
            zoom = (float)(VelDisplay.Bottom / 2) / FindMaxAbs(targetDir).Y;


            //设置滚动条最大值
            hScrollBar1.Maximum = (int)(PERCENT * pathMax / (VelDisplay.Right / zoomL)) + 1;



            for (int j = 0; j < targetVelWheel1.Count(); j++)
            {
                DirectionErr[j].X = targetDir[j].X;
                DirectionErr[j].Y = Math.Abs(targetDir[j].Y - trueDir[j].Y);
                if (DirectionErr[j].Y > 180) DirectionErr[j].Y = 360 - DirectionErr[j].Y;
                else if (DirectionErr[j].Y < -180) DirectionErr[j].Y = 360 + DirectionErr[j].Y;

            }


            //进行滤波处理
            for (int m = 0; m < 4; m++)
            {
                for (int j = 1; j < DirectionErr.Count() - 1; j++)
                {
                    DirectionErr[j].Y = (DirectionErr[j - 1].Y + DirectionErr[j + 1].Y) / 2;
                }
            }







            //进行阈值判断，二值化
            for (int j = 0; j < DirectionErr.Count(); j++)
            {
                if (DirectionErr[j].Y < 8)
                {
                    binaryzation[j].Y = 0;
                    binaryzation[j].X = DirectionErr[j].X;
                }
                else 
                {
                    binaryzation[j].Y = DirectionErr[j].Y;
                    binaryzation[j].X = DirectionErr[j].X;
                }
                
            }




            //将二值化的文件存入txt文档中
            StreamWriter SpeedFile = new StreamWriter(SpeedReplanInfo, false);

            for (int m = 0; m < binaryzation.Count(); m++)
            {
                SpeedFile.WriteLine("{0:f0},{1:f0}", binaryzation[m].X,  binaryzation[m].Y);
            }

            SpeedFile.Close();


            //进行阈值判断，二值化
            for (int j = 0; j < DirectionErr.Count(); j++)
            {
                binaryzation[j] = GetPanelAxes(binaryzation[j].X, binaryzation[j].Y);
            }



            
            myPen.Color = Color.Black;
            myGra.DrawLines(myPen, binaryzation);



        }

        private void SpeedReplanning()
        {
            //将txt文档中的数据存入ringBuffer
            SpeedPlanning.MoveTxtToRingBuffer();


            int num  = PointsInfo.pnts.Count();
            float[] angleErr = new float[num];

            //数据读取
            StreamReader speedReplanFile = File.OpenText(SpeedReplanInfo);

            List<string> tempString = new List<string>();

            while (!speedReplanFile.EndOfStream)
            {
                tempString.Add(speedReplanFile.ReadLine());
            }

            speedReplanFile.Close();


            if(num < 3) return;



            //计算每一段的平均角度偏差
            //将一段里面的打滑量放在后端点数组
            for (int i = 1; i < num; i++)
            {
                float angleErrSum = 0;
                int count = 0;
                foreach (string str in tempString)
                {
                    string[] sArray = str.Split(',');
                    if (float.Parse(sArray[0]) > PointsInfo.pnts[i - 1].length && float.Parse(sArray[0]) < PointsInfo.pnts[i].length)
                    {
                        angleErrSum += float.Parse(sArray[1]);
                        count++;
                    }

                    if (float.Parse(sArray[0]) > PointsInfo.pnts[i].length)
                    {
                        break;
                    }

                }

                if (count != 0)
                {
                    angleErr[i] = angleErrSum / count;
                }
                else
                {
                    angleErr[i] = 0;
                }

            }


            //对第一点和第二点进行忽略
            for (int i = 1; i < num - 1; i++)
            {
                if(angleErr[i] > 0.1f )
                {
                    if(angleErr[i] > 40) angleErr[i] = 40;

                    PointsInfo.pnts[i-1].velMax *= (1 - angleErr[i] / 100);
                }

                
            }



            float tempVell = 0.0f;
	        //通过v2^2 - v1^2 = 2*a*s对速度再次规划
	        for (int i = 0; i < num - 1; i++)
	        {
		        if (PointsInfo.pnts[i + 1].velMax > PointsInfo.pnts[i].velMax)
		        {
			        tempVell = (float)Math.Sqrt(2 * (MotionCardParameter.GetAccMax()) * (PointsInfo.pnts[i + 1].length - PointsInfo.pnts[i].length) + PointsInfo.pnts[i].velMax * PointsInfo.pnts[i].velMax);
			        if (tempVell < PointsInfo.pnts[i+1].velMax)
			        {
				        PointsInfo.pnts[i+1].velMax = tempVell;
			        }
		        }
	        }

	        for (int i = num - 1; i > 0; i--)
	        {
		        if (PointsInfo.pnts[i-1].velMax > PointsInfo.pnts[i].velMax)
		        {
			        tempVell = (float)Math.Sqrt(2 * (MotionCardParameter.GetAccMax()) * (PointsInfo.pnts[i].length - PointsInfo.pnts[i-1].length) +PointsInfo.pnts[i].velMax *PointsInfo.pnts[i].velMax);
			        if (tempVell < PointsInfo.pnts[i-1].velMax)
			        {
				       PointsInfo.pnts[i-1].velMax = tempVell;
			        }
		        }
	        }

            for (int i = 0; i < num;i++ )
            {
                Console.WriteLine(PointsInfo.pnts[i].velMax);
            }

            SpeedPlanning.MoveListToTxt();

            //将速度小于最小速度的做处理
            for (int i = 0; i < num; i++)
            {
                if (PointsInfo.pnts[i].velMax < 70)
                {
                    PointsInfo.pnts[i].velMax = 70;
                }
            }

        }


        private void ErrCurveDrawing()
        {
            //数据读取
            StreamReader pathFile = File.OpenText(VelInf);

            List<string> tempString = new List<string>();

            while (!pathFile.EndOfStream)
            {
                tempString.Add(pathFile.ReadLine());
            }

            pathFile.Close();


            //如果点数小于2，不能规划出轨迹
            if (tempString.Count() < 2) return;

            Graphics myGra = VelDisplay.CreateGraphics();

            Pen myPen = new Pen(Color.Orange, 1);

            PointF[] Err = new PointF[tempString.Count()];

            int i = 0;

            foreach (string str in tempString)
            {
                string[] sArray = str.Split(',');
                Err[i].X = float.Parse(sArray[0]);
                Err[i].Y = float.Parse(sArray[7]);
                i++;
            }


            pathMax = FindMaxAbs(Err).X;

            float iii = FindMaxAbs(Err).Y;

            zoom = (float)(VelDisplay.Bottom / 2) / FindMaxAbs(Err).Y;


            //设置滚动条最大值
            hScrollBar1.Maximum = (int)(PERCENT * pathMax / (VelDisplay.Right / zoomL)) + 1;

            for (int j = 0; j < Err.Count();j++ )
            {
                Err[j] = GetPanelAxes(Err[j].X, Err[j].Y);
            }

            for (int m = 0; m < 4; m++)
            {
                for (int j = 1; j < Err.Count() - 1; j++)
                {
                    Err[j].Y = (Err[j - 1].Y + Err[j + 1].Y) / 2;
                }
            }

            myGra.DrawLines(myPen, Err);

        }

        private const int PERCENT=10;

        private void VelDisplayBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            VelDisplay.Refresh();
        }

        private PointF FindMaxAbs(PointF[] pnt)
        {
            int n = pnt.Count();
            PointF max;
            max = pnt[0];
            for (int i = 1; i < n; i++)
            {
                if (Math.Abs(pnt[i].Y) > max.Y) max.Y = Math.Abs(pnt[i].Y);
            }
            max.X = pnt[n - 1].X;
            return max;
        }

        private void SpeedShow_Resize(object sender, EventArgs e)
        {
            //设置坐标系偏移量
            offsetX = tempOffsetX = 10;
            offsetY = tempOffsetY = VelDisplay.Bottom / 2;
            //SetRoomAndHScrollBar();
            VelDisplay.Refresh();
        }


        //移动滚动条
        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            offsetX = -VelDisplay.Right / PERCENT * hScrollBar1.Value;
            VelDisplay.Refresh();
        }


        //比例调节滚动条
        private void ZoomLScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            zoomL = 0.15f + ZoomLScrollBar.Value * 0.01f;
            //SetRoomAndHScrollBar();
            VelDisplay.Refresh();
        }


        private void 速度更新_Click(object sender, EventArgs e)
        {
            ResultantVelCurveDrawing();
            SpeedReplanning();
        }





    }
}
