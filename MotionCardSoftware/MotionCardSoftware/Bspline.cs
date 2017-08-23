using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using DxfViewExample;

namespace MotionCardSoftware
{
    class Bspline
    {

        /// <summary>
        /// @name DrowBspline1
        /// @brief 首末端点为自由条件的B样条曲线绘制
        /// @inPoint 示教点
        /// </summary>
        static public void DrawBspline1(int num, Graphics gra, Pen pen, PointF[] inPoint)
        {
            //系数矩阵对角列
            float[] a = new float[num];
            //系数矩阵对角上列
            float[] b = new float[num];
            //系数矩阵对角下列
            float[] c = new float[num];

            //定义soluctionX、soluctionY为线性方程的解
            float[] soluctionX = new float[num];
            float[] soluctionY = new float[num];
            //定义dataX和dataY,用来存放inPoint里的X和Y坐标
            float[] dataX = new float[num];
            float[] dataY = new float[num];
            //定义controlPoint用来存放控制点
            PointF[] controlPoint = new PointF[num + 4];
            //存放画线的两个使用点
            PointF[] lines = new PointF[2];

            //初始化 a,b,c
            a[0] = 18;
            a[num - 1] = 18;

            for (int i = 1; i < num - 1; i++)
            {
                a[i] = 4;
            }

            for (int i = 1; i < num - 1; i++)
            {
                b[i] = 1;
                c[i] = 1;
            }

            c[num - 1] = -9;
            b[0] = -9;



            for (int i = 0; i < num; i++)
            {
                dataX[i] = 6.0f * inPoint[i].X;
                dataY[i] = 6.0f * inPoint[i].Y;
            }

            dataX[0] *= 1.5f;
            dataY[0] *= 1.5f;
            dataX[num - 1] *= 1.5f;
            dataY[num - 1] *= 1.5f;






            //计算outdataX,outdataY;

            Calculate math = new Calculate();

            //调用Matrix用追赶法求解线性方程
            math.Matrix(dataX, num, ref a, ref b, ref c, ref soluctionX);
            math.Matrix(dataY, num, ref a, ref b, ref c, ref soluctionY);



            controlPoint[num + 3].X = dataX[num - 1] / 9;
            controlPoint[num + 2].X = dataX[num - 1] / 9;
            controlPoint[0].X = dataX[0] / 9;
            controlPoint[1].X = dataX[0] / 9;


            for (int i = 0; i < num; i++)
            {
                controlPoint[i + 2].X = soluctionX[i];

            }

            controlPoint[num + 3].Y = dataY[num - 1] / 9;
            controlPoint[num + 2].Y = dataY[num - 1] / 9;
            controlPoint[0].Y = dataY[0] / 9;
            controlPoint[1].Y = dataY[0] / 9;


            for (int i = 0; i < num; i++)
            {
                controlPoint[i + 2].Y = soluctionY[i];

            }



            //计算型值点，画出曲线
            //从初始点开始
            lines[0].X = (int)controlPoint[0].X;
            lines[0].Y = (int)controlPoint[0].Y;

            
            for (int i = 0; i < num + 1; i++)
            {

                for (float u = 0.01f; u <= 1; u += 0.01f)
                {
                    float b0 = 1.0f / 6 * (1 - u) * (1 - u) * (1 - u);
                    float b1 = 1.0f / 6 * (3 * u * u * u - 6 * u * u + 4);
                    float b2 = 1.0f / 6 * (-3 * u * u * u + 3 * u * u + 3 * u + 1);
                    float b3 = 1.0f / 6 * u * u * u;

                    lines[1].X = (b0 * controlPoint[i].X + b1 * controlPoint[i + 1].X + b2 * controlPoint[i + 2].X + b3 * controlPoint[i + 3].X);
                    lines[1].Y = (b0 * controlPoint[i].Y + b1 * controlPoint[i + 1].Y + b2 * controlPoint[i + 2].Y + b3 * controlPoint[i + 3].Y);
                    PointF pnt1 = new PointF();
                    PointF pnt2 = new PointF();


                    //转换到panel像素坐标系
                    pnt1 = ViewControl.GetPanelAxes(lines[0].X, lines[0].Y);
                    pnt2 = ViewControl.GetPanelAxes(lines[1].X, lines[1].Y);

                    gra.DrawLine(Pens.Yellow, pnt1, pnt2);


                    lines[0] = lines[1];
                }
            }


        }


        /// <summary>
        /// 首末端点有方向的B样条曲线绘制
        /// </summary>
        /// <param name="gra">外部声明调用GDI</param>
        /// <param name="point1">点1</param>
        /// <param name="point2">点2</param>
        /// <param name="angle1">点1的方向</param>
        /// <param name="angle2">点2的方向</param>
        static public void DrawBspline2(Graphics gra, PointF point1, PointF point2, float angle1, float angle2)
        {
            PointF[] finalDataPoint = new PointF[6];
            PointF[] dataPoint = new PointF[2];
            float length = 0.0f;
            const float CHANGE_TO_RADIAN = (3.1415927f / 180.0f);
            PointF[] lines = new PointF[2];

            length = 0.6f * (float)Math.Sqrt((point1.X - point2.X) * (point1.X - point2.X) + (point1.Y - point2.Y) * (point1.Y - point2.Y));
            dataPoint[0].X = (int)((3 * point1.X + length * Math.Cos((angle1) * CHANGE_TO_RADIAN)) / 3.0f);
            dataPoint[0].Y = (int)((3 * point1.Y + length * Math.Sin((angle1) * CHANGE_TO_RADIAN)) / 3.0f);
            dataPoint[1].X = (int)((3 * point2.X - length * Math.Cos((angle2) * CHANGE_TO_RADIAN)) / 3.0f);
            dataPoint[1].Y = (int)((3 * point2.Y - length * Math.Sin((angle2) * CHANGE_TO_RADIAN)) / 3.0f);

            //复制最终控制点坐标
            finalDataPoint[0] = point1;
            finalDataPoint[1] = point1;
            finalDataPoint[2] = dataPoint[0];
            finalDataPoint[3] = dataPoint[1];
            finalDataPoint[4] = point2;
            finalDataPoint[5] = point2;
            //从初始点开始
            lines[0].X = (int)finalDataPoint[0].X;
            lines[0].Y = (int)finalDataPoint[0].Y;

            PointF pnt1 = new PointF();
            PointF pnt2 = new PointF();
            for (int i = 0; i < 3; i++)
            {
                for (float u = 0.01f; u <= 1; u += 0.01f)
                {
                    float b0 = 1.0f / 6 * (1 - u) * (1 - u) * (1 - u);
                    float b1 = 1.0f / 6 * (3 * u * u * u - 6 * u * u + 4);
                    float b2 = 1.0f / 6 * (-3 * u * u * u + 3 * u * u + 3 * u + 1);
                    float b3 = 1.0f / 6 * u * u * u;

                    lines[1].X = (b0 * finalDataPoint[i].X + b1 * finalDataPoint[i + 1].X + b2 * finalDataPoint[i + 2].X + b3 * finalDataPoint[i + 3].X);
                    lines[1].Y = (b0 * finalDataPoint[i].Y + b1 * finalDataPoint[i + 1].Y + b2 * finalDataPoint[i + 2].Y + b3 * finalDataPoint[i + 3].Y);
                    //转换到panel像素坐标系
                    pnt1 = ViewControl.GetPanelAxes(lines[0].X, lines[0].Y);
                    pnt2 = ViewControl.GetPanelAxes(lines[1].X, lines[1].Y);


                    gra.DrawLine(Pens.Yellow, pnt1, pnt2);
                    lines[0] = lines[1];
                }
            }
            pnt2 = ViewControl.GetPanelAxes(point2.X, point2.Y);
            gra.DrawLine(Pens.Red, pnt1, pnt2);

        }


        //将自由端点的b样条曲线分割为每段15cm的曲线
        static public void BsplineSegment(string BsplineWithoutDirTxt, string BsplineWithDirTxt)
        {
            //将端点长度写入文件，便于姿态角度拟合
            Bspline.CalculateBsplineInfo(BsplineWithoutDirTxt);
            //数据读取
            StreamReader pathFile = File.OpenText(BsplineWithoutDirTxt);

            List<string> tempString = new List<string>();

            while (!pathFile.EndOfStream)
            {
                tempString.Add(pathFile.ReadLine());
            }

            pathFile.Close();

            List<PointF> tempPnts = new List<PointF>();
            List<PointF> posAnglePoints = new List<PointF>();

            foreach (string str in tempString)
            {
                string[] sArray = str.Split(',');
                tempPnts.Add(new PointF(float.Parse(sArray[0]), float.Parse(sArray[1])));
                posAnglePoints.Add(new PointF(float.Parse(sArray[3]), float.Parse(sArray[2])));
            }

            System.Drawing.PointF[] tempPoint = tempPnts.ToArray();

            System.Drawing.PointF[] posAnglePoint = posAnglePoints.ToArray();

            //创建临时文件
            StreamWriter posAngleFile = new StreamWriter(Directory.GetCurrentDirectory() + "\\23423.txt", false);
            //创建临时文件
            StreamWriter lengthFile = new StreamWriter(Directory.GetCurrentDirectory() + "\\325.txt", false);




            //创建临时文件
            StreamWriter lengthSegmentFile = new StreamWriter(Directory.GetCurrentDirectory() + "\\3424ds.txt", false);
            //每15cm分一段，计算端点切线方向 ,以无方向b样条的长度15cm一段进行切分

            Segment(tempPoint, BsplineWithDirTxt,lengthSegmentFile);

            lengthSegmentFile.Close();

            //数据读取
            StreamReader lengthSegmentFileR = File.OpenText(Directory.GetCurrentDirectory() + "\\3424ds.txt");


            //每15cm分一段，计算端点姿态方向
            Segment2(posAnglePoint, posAngleFile,  lengthSegmentFileR);

            lengthSegmentFileR.Close();
            System.IO.File.Delete(Directory.GetCurrentDirectory() + "\\3424ds.txt");


            

            
            //计算每一段的长度
            Segment3(BsplineWithDirTxt, lengthFile);


            lengthFile.Close();
            posAngleFile.Close();

  //将三个文件合在一起///////////////////////////////////////////////////////////////////
            //数据读取
            StreamReader file1 = File.OpenText(BsplineWithDirTxt);

            List<string> tempString1 = new List<string>();

            while (!file1.EndOfStream)
            {
                tempString1.Add(file1.ReadLine());
            }

            file1.Close();

            //数据读取  posAngleFile
            StreamReader file2 = File.OpenText(Directory.GetCurrentDirectory() + "\\23423.txt");

            List<string> tempString2 = new List<string>();

            while (!file2.EndOfStream)
            {
                tempString2.Add(file2.ReadLine());
            }

            file2.Close();


            //数据读取  lengthFile
            StreamReader file3 = File.OpenText(Directory.GetCurrentDirectory() + "\\325.txt");

            List<string> tempString3 = new List<string>();

            while (!file3.EndOfStream)
            {
                tempString3.Add(file3.ReadLine());
            }

            file3.Close();



            Console.WriteLine(tempString1.Count());
            Console.WriteLine(tempString2.Count());
            Console.WriteLine(tempString3.Count());

            //创建临时文件
            StreamWriter tempAllInfoFile = new StreamWriter(Directory.GetCurrentDirectory() + "\\asdgfas.txt", true);
            for (int i = 0; i < tempString2.Count(); i++)
            {
                tempAllInfoFile.WriteLine(tempString1[i] + "," + tempString2[i] + "," + tempString3[i]);
            }


            tempAllInfoFile.Close();
            FileTxt.FileTxtCopy(Directory.GetCurrentDirectory() + "\\asdgfas.txt", BsplineWithDirTxt);

            
            //删除文件
            System.IO.File.Delete(Directory.GetCurrentDirectory() + "\\asdgfas.txt");

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



            lengthFile.Close();
            posAngleFile.Close();
            System.IO.File.Delete(Directory.GetCurrentDirectory() + "\\23423.txt");
            System.IO.File.Delete(Directory.GetCurrentDirectory() + "\\325.txt");
            
        }



        //进行整体分段处理，仅用于BsplineSegment,计算每段15cm处切线方向
        static public float Segment(PointF[] inPoint, string BsplineWithDirTxt, StreamWriter lengthSegmentFile)
        {
            StreamWriter file = new StreamWriter(BsplineWithDirTxt, false);

            int num = inPoint.Count();
            //系数矩阵对角列
            float[] a = new float[num];
            //系数矩阵对角上列
            float[] b = new float[num];
            //系数矩阵对角下列
            float[] c = new float[num];

            //定义soluctionX、soluctionY为线性方程的解
            float[] soluctionX = new float[num];
            float[] soluctionY = new float[num];
            //定义dataX和dataY,用来存放inPoint里的X和Y坐标
            float[] dataX = new float[num];
            float[] dataY = new float[num];
            //定义controlPoint用来存放控制点
            PointF[] controlPoint = new PointF[num + 4];
            //存放画线的两个使用点
            PointF[] lines = new PointF[2];

            //初始化 a,b,c
            a[0] = 18;
            a[num - 1] = 18;

            for (int i = 1; i < num - 1; i++)
            {
                a[i] = 4;
            }

            for (int i = 1; i < num - 1; i++)
            {
                b[i] = 1;
                c[i] = 1;
            }

            c[num - 1] = -9;
            b[0] = -9;



            for (int i = 0; i < num; i++)
            {
                dataX[i] = 6.0f * inPoint[i].X;
                dataY[i] = 6.0f * inPoint[i].Y;
            }

            dataX[0] *= 1.5f;
            dataY[0] *= 1.5f;
            dataX[num - 1] *= 1.5f;
            dataY[num - 1] *= 1.5f;






            //计算outdataX,outdataY;

            Calculate math = new Calculate();

            //调用Matrix用追赶法求解线性方程
            math.Matrix(dataX, num, ref a, ref b, ref c, ref soluctionX);
            math.Matrix(dataY, num, ref a, ref b, ref c, ref soluctionY);



            controlPoint[num + 3].X = dataX[num - 1] / 9;
            controlPoint[num + 2].X = dataX[num - 1] / 9;
            controlPoint[0].X = dataX[0] / 9;
            controlPoint[1].X = dataX[0] / 9;


            for (int i = 0; i < num; i++)
            {
                controlPoint[i + 2].X = soluctionX[i];

            }

            controlPoint[num + 3].Y = dataY[num - 1] / 9;
            controlPoint[num + 2].Y = dataY[num - 1] / 9;
            controlPoint[0].Y = dataY[0] / 9;
            controlPoint[1].Y = dataY[0] / 9;


            for (int i = 0; i < num; i++)
            {
                controlPoint[i + 2].Y = soluctionY[i];

            }



            //计算型值点，画出曲线
            ////从初始点开始
            //lines[0].X = controlPoint[0].X;
            //lines[0].Y = controlPoint[0].Y;

            
            float length = 0.0f;
            
            float lengthOrigin = 0.0f;

            const float CHANGE_TO_ANGLE =  (180.0f/3.1415927f);

            int count = 0;
            for (int i = 0; i < num + 1; i++)
            {

                for (float u = 0.01f; u <= 1; u += 0.01f)
                {
                    float b0 = 1.0f / 6 * (1 - u) * (1 - u) * (1 - u);
                    float b1 = 1.0f / 6 * (3 * u * u * u - 6 * u * u + 4);
                    float b2 = 1.0f / 6 * (-3 * u * u * u + 3 * u * u + 3 * u + 1);
                    float b3 = 1.0f / 6 * u * u * u;

                    lines[1].X = (b0 * controlPoint[i].X + b1 * controlPoint[i + 1].X + b2 * controlPoint[i + 2].X + b3 * controlPoint[i + 3].X);
                    lines[1].Y = (b0 * controlPoint[i].Y + b1 * controlPoint[i + 1].Y + b2 * controlPoint[i + 2].Y + b3 * controlPoint[i + 3].Y);

                    lengthOrigin += Bspline.CalculatePnt2Pnt(lines[0], lines[1]);
                    length += Bspline.CalculatePnt2Pnt(lines[0], lines[1]);


			        if (length > 300.0)
			        {
				        length = 0;
				        float Db0 = -1.0f / 2 * (u - 1) * (u - 1);
				        float Db1 = 1.0f / 2 * (3 * u * u - 4 * u);
				        float Db2 = 1.0f / 2 * (-3 * u * u + u * 2 + 1);
				        float Db3 = 1.0f / 2 * u * u;

				        float angle = CHANGE_TO_ANGLE * (float)Math.Atan2((Db0 * controlPoint[i].Y + Db1 * controlPoint[i + 1].Y 
                            + Db2 * controlPoint[i + 2].Y + Db3 * controlPoint[i + 3].Y),
					        (Db0 * controlPoint[i].X + Db1 * controlPoint[i + 1].X 
                            + Db2 * controlPoint[i + 2].X + Db3 * controlPoint[i + 3].X));

                        if (count == 0)
                        {

                            file.Write(inPoint[0].X.ToString() + "," + inPoint[0].Y.ToString() + "," + (Math.Atan2(lines[1].Y-inPoint[0].Y,lines[1].X-inPoint[0].X)*180.0f/3.14159f).ToString()  + "\r\n");

                            file.Write(lines[1].X.ToString() + "," + lines[1].Y.ToString() + "," + angle.ToString() + "\r\n");
                            
                        }
                        else 
                        {
                            file.Write(lines[1].X.ToString() + "," + lines[1].Y.ToString() + "," + angle.ToString() + "\r\n");
                        }
                        count++;

                        //并没有记录第一点和最后一点
                        lengthSegmentFile.WriteLine(lengthOrigin);
                        
			        }

                    lines[0] = lines[1];
                }
            }

            lengthOrigin += Bspline.CalculatePnt2Pnt(inPoint[num-1], lines[1]);

            file.Write(inPoint[num - 1].X.ToString() + "," + inPoint[num - 1].Y.ToString() + "," + (Math.Atan2(inPoint[num - 1].Y - lines[1].Y, inPoint[num - 1].X - lines[1].X) * 180.0f / 3.14159f).ToString() + "\r\n");

            file.Close();

            return lengthOrigin;
        }


        //将姿态角度没15cm计算一个值
        static public void Segment2(PointF[] inPoint, StreamWriter file, StreamReader lengthSegmentFileR)
        {

            int num = inPoint.Count();
            //系数矩阵对角列
            float[] a = new float[num];
            //系数矩阵对角上列
            float[] b = new float[num];
            //系数矩阵对角下列
            float[] c = new float[num];

            //定义soluctionX、soluctionY为线性方程的解
            float[] soluctionX = new float[num];
            float[] soluctionY = new float[num];
            //定义dataX和dataY,用来存放inPoint里的X和Y坐标
            float[] dataX = new float[num];
            float[] dataY = new float[num];
            //定义controlPoint用来存放控制点
            PointF[] controlPoint = new PointF[num + 4];
            //存放画线的两个使用点
            PointF[] lines = new PointF[2];

            //初始化 a,b,c
            a[0] = 18;
            a[num - 1] = 18;

            for (int i = 1; i < num - 1; i++)
            {
                a[i] = 4;
            }

            for (int i = 1; i < num - 1; i++)
            {
                b[i] = 1;
                c[i] = 1;
            }

            c[num - 1] = -9;
            b[0] = -9;



            for (int i = 0; i < num; i++)
            {
                dataX[i] = 6.0f * inPoint[i].X;
                dataY[i] = 6.0f * inPoint[i].Y;
            }

            dataX[0] *= 1.5f;
            dataY[0] *= 1.5f;
            dataX[num - 1] *= 1.5f;
            dataY[num - 1] *= 1.5f;






            //计算outdataX,outdataY;

            Calculate math = new Calculate();

            //调用Matrix用追赶法求解线性方程
            math.Matrix(dataX, num, ref a, ref b, ref c, ref soluctionX);
            math.Matrix(dataY, num, ref a, ref b, ref c, ref soluctionY);



            controlPoint[num + 3].X = dataX[num - 1] / 9;
            controlPoint[num + 2].X = dataX[num - 1] / 9;
            controlPoint[0].X = dataX[0] / 9;
            controlPoint[1].X = dataX[0] / 9;


            for (int i = 0; i < num; i++)
            {
                controlPoint[i + 2].X = soluctionX[i];
            }

            controlPoint[num + 3].Y = dataY[num - 1] / 9;
            controlPoint[num + 2].Y = dataY[num - 1] / 9;
            controlPoint[0].Y = dataY[0] / 9;
            controlPoint[1].Y = dataY[0] / 9;


            for (int i = 0; i < num; i++)
            {
                controlPoint[i + 2].Y = soluctionY[i];
            }


            List<string> segmentStr = new List<string>();

            while (!lengthSegmentFileR.EndOfStream)
            {
                segmentStr.Add(lengthSegmentFileR.ReadLine());
            }


            List<float> lenthSegments = new List<float>();

            foreach (string str in segmentStr)
            {
                string[] sArray = str.Split(',');
                lenthSegments.Add(float.Parse(sArray[0]));
            }


            int count = 0;

            file.WriteLine(inPoint[0].Y);

            for (int i = 0; i < num + 1; i++)
            {
                for (float u = 0.00001f; u <= 1; u += 0.00001f)
                {
                    float b0 = 1.0f / 6 * (1 - u) * (1 - u) * (1 - u);
                    float b1 = 1.0f / 6 * (3 * u * u * u - 6 * u * u + 4);
                    float b2 = 1.0f / 6 * (-3 * u * u * u + 3 * u * u + 3 * u + 1);
                    float b3 = 1.0f / 6 * u * u * u;

                    lines[1].X = (b0 * controlPoint[i].X + b1 * controlPoint[i + 1].X + b2 * controlPoint[i + 2].X + b3 * controlPoint[i + 3].X);
                    lines[1].Y = (b0 * controlPoint[i].Y + b1 * controlPoint[i + 1].Y + b2 * controlPoint[i + 2].Y + b3 * controlPoint[i + 3].Y);


                    float temp = 0.0f;

                    if (lines[1].X > lenthSegments[count])
                    {
                        temp = lines[1].Y;
                        if (lines[1].Y < 0.0f)
                        {
                            int tempI = -1;
                            while (true)
                            {
                                tempI++;
                                if (temp > tempI * (-360) - 180)
                                {
                                    break;
                                }
                            }
                            temp += tempI * 360;
                            file.WriteLine(temp);
                        }
                        else if (lines[1].Y > 0.0f)
                        {
                            int tempI = -1;
                            while (true)
                            {
                                tempI++;
                                if (temp < tempI * (360) + 180)
                                {
                                    break;
                                }
                            }
                            temp -= tempI * 360;
                            file.WriteLine(temp);
                        }
                        else
                        {
                            file.WriteLine(lines[1].Y);
                        }

                        count++;

                        if (count == lenthSegments.Count())
                        {
                            break;
                        }

                    }
                }

                if (count == lenthSegments.Count())
                {
                    break;
                }
            }


            
            //对最后一点进行判断
            if (inPoint[num - 1].Y < 0.0f)
            {
                float temp1 = inPoint[num - 1].Y;
                int tempI = -1;
                while (true)
                {
                    tempI++;
                    if (temp1 > tempI * (-360) - 180)
                    {
                        break;
                    }
                }
                temp1 += tempI * 360;
                file.WriteLine(temp1);
            }
            else if (lines[1].Y > 0.0f)
            {
                float temp1 = inPoint[num - 1].Y;
                int tempI = -1;
                while (true)
                {
                    tempI++;
                    if (temp1 < tempI * (360) + 180)
                    {
                        break;
                    }
                }
                temp1 -= tempI * 360;
                file.WriteLine(temp1);
            }
            else
            {
                file.WriteLine(lines[1].Y);
            }

        }

        //计算每一段的长度，并存入文件。
        static public int Segment3(string BsplineWithDirTxt, StreamWriter file)
        {
            //数据读取
            StreamReader pathFile = File.OpenText(BsplineWithDirTxt);

            List<string> tempString = new List<string>();

            while (!pathFile.EndOfStream)
            {
                tempString.Add(pathFile.ReadLine());
            }

            pathFile.Close();

            List<PointF> tempPnts = new List<PointF>();
           
            foreach (string str in tempString)
            {
                string[] sArray = str.Split(',');
                tempPnts.Add(new PointF(float.Parse(sArray[0]), float.Parse(sArray[1])));
            }

            System.Drawing.PointF[] tempPoint = tempPnts.ToArray();

            float[] direction = new float[tempPoint.Count()];

            int i = 0;
            foreach (string str in tempString)
            {
                string[] sArray = str.Split(',');
                direction[i++] = float.Parse(sArray[2]);
            }

            float length = 0.0f;
            file.WriteLine(0);
            for (int j = 1; j < tempPoint.Count(); j++)
            {
                length += CalculateBsplineLen(tempPoint[j - 1], tempPoint[j], direction[j - 1], direction[j]);
                file.WriteLine(length);
            }

            file.Close();
            return tempPoint.Count();
        }



        //计算所绘制B样条曲线关键点路径长度，并存在txt文件中,用于切分姿态角度所用
        static public void CalculateBsplineInfo(string txtAdress)
        {
            //创建临时文件
            StreamWriter tempFile = new StreamWriter(Directory.GetCurrentDirectory() + "\\eeww.txt", true);

            //////////////////////////////////从TXT文档读取数据点/////////////////////////////////////
            //数据读取
            StreamReader pathFile = File.OpenText(txtAdress);
            List<string> tempString = new List<string>();

            while (!pathFile.EndOfStream)
            {
                tempString.Add(pathFile.ReadLine());
            }

            pathFile.Close();

            //如果点数小于2，不能规划出轨迹
            if (tempString.Count() < 2) return;

            List<PointF> tempPnts = new List<PointF>();

            Pen myPen = new Pen(Color.Black, 1);

            foreach (string str in tempString)
            {
                string[] sArray = str.Split(',');
                tempPnts.Add(new PointF(float.Parse(sArray[0]), float.Parse(sArray[1])));
            }

                System.Drawing.PointF[] tempPoint = tempPnts.ToArray();
            //////////////////////////////////////////////////////////////////////////////////////////////


             int  num = tempPoint.Count();

            float[] pntLen = new float[num];
            //系数矩阵对角列
            float[] a = new float[num];
            //系数矩阵对角上列
            float[] b = new float[num];
            //系数矩阵对角下列
            float[] c = new float[num];

            //定义soluctionX、soluctionY为线性方程的解
            float[] soluctionX = new float[num];
            float[] soluctionY = new float[num];
            //定义dataX和dataY,用来存放inPoint里的X和Y坐标
            float[] dataX = new float[num];
            float[] dataY = new float[num];
            //定义controlPoint用来存放控制点
            PointF[] controlPoint = new PointF[num + 4];
            //存放画线的两个使用点
            PointF[] lines = new PointF[2];

            //初始化 a,b,c
            a[0] = 18;
            a[num - 1] = 18;

            for (int i = 1; i < num - 1; i++)
            {
                a[i] = 4;
            }

            for (int i = 1; i < num - 1; i++)
            {
                b[i] = 1;
                c[i] = 1;
            }

            c[num - 1] = -9;
            b[0] = -9;



            for (int i = 0; i < num; i++)
            {
                dataX[i] = 6.0f * tempPnts[i].X;
                dataY[i] = 6.0f * tempPnts[i].Y;
            }

            dataX[0] *= 1.5f;
            dataY[0] *= 1.5f;
            dataX[num - 1] *= 1.5f;
            dataY[num - 1] *= 1.5f;






            //计算outdataX,outdataY;

            Calculate math = new Calculate();

            //调用Matrix用追赶法求解线性方程
            math.Matrix(dataX, num, ref a, ref b, ref c, ref soluctionX);
            math.Matrix(dataY, num, ref a, ref b, ref c, ref soluctionY);



            controlPoint[num + 3].X = dataX[num - 1] / 9;
            controlPoint[num + 2].X = dataX[num - 1] / 9;
            controlPoint[0].X = dataX[0] / 9;
            controlPoint[1].X = dataX[0] / 9;


            for (int i = 0; i < num; i++)
            {
                controlPoint[i + 2].X = soluctionX[i];

            }

            controlPoint[num + 3].Y = dataY[num - 1] / 9;
            controlPoint[num + 2].Y = dataY[num - 1] / 9;
            controlPoint[0].Y = dataY[0] / 9;
            controlPoint[1].Y = dataY[0] / 9;


            for (int i = 0; i < num; i++)
            {
                controlPoint[i + 2].Y = soluctionY[i];

            }



            
                float threshold = 1.0f;
                while(true)
                {
                    //计算型值点，画出曲线
                    //从初始点开始
                    lines[0] = tempPoint[0];

                    pntLen[0] = 0.0f;

                    int count = 1;

                    float length = 0.0f;
                    for (int i = 0; i < num + 1; i++)
                    {

                        for (float u = 0.01f; u <= 1; u += 0.01f)
                        {
                            float b0 = 1.0f / 6 * (1 - u) * (1 - u) * (1 - u);
                            float b1 = 1.0f / 6 * (3 * u * u * u - 6 * u * u + 4);
                            float b2 = 1.0f / 6 * (-3 * u * u * u + 3 * u * u + 3 * u + 1);
                            float b3 = 1.0f / 6 * u * u * u;

                            lines[1].X = (b0 * controlPoint[i].X + b1 * controlPoint[i + 1].X + b2 * controlPoint[i + 2].X + b3 * controlPoint[i + 3].X);
                            lines[1].Y = (b0 * controlPoint[i].Y + b1 * controlPoint[i + 1].Y + b2 * controlPoint[i + 2].Y + b3 * controlPoint[i + 3].Y);

                            length += Bspline.CalculatePnt2Pnt(lines[0], lines[1]);

                            if (Bspline.CalculatePnt2Pnt(lines[1], tempPnts[count]) < threshold)
                            {
                                pntLen[count++] = length;

                            }



                            lines[0] = lines[1];
                        }
                    }
                    if (count == num - 1)
                    {
                        length += Bspline.CalculatePnt2Pnt(lines[0], tempPoint[num - 1]);

                        pntLen[num - 1] = length;

                        break;

                    }
                    else
                    {
                        threshold *= 2.0f;
                    }
                        
                }






                int ii = 0;
                foreach (string str in tempString)
                {
                    tempFile.WriteLine(str + "," + pntLen[ii++].ToString());
                }

                tempFile.Close();

                //清空txt文档
                FileStream fs = new FileStream(txtAdress, FileMode.Create, FileAccess.Write);
                fs.Close();

                FileTxt.FileTxtCopy(Directory.GetCurrentDirectory() + "\\eeww.txt", txtAdress);

            //删除临时文件
            System.IO.File.Delete(Directory.GetCurrentDirectory() + "\\eeww.txt");
        }


        //计算两点之间距离
        static public float CalculatePnt2Pnt(PointF point1, PointF point2)
        {
            float dis;
            dis = (float)Math.Sqrt((point1.X - point2.X) * (point1.X - point2.X) + (point1.Y - point2.Y) * (point1.Y - point2.Y));
            return dis;
        }



        //计算两点所形成B样条的长度
        static public float CalculateBsplineLen(PointF point1, PointF point2, float angle1, float angle2)
        {
            PointF[] finalDataPoint = new PointF[6];
            PointF[] dataPoint = new PointF[2];
            float length = 0.0f;
            const float CHANGE_TO_RADIAN = (3.1415927f / 180.0f);
            PointF[] lines = new PointF[2];

            length = 0.6f * (float)Math.Sqrt((point1.X - point2.X) * (point1.X - point2.X) + (point1.Y - point2.Y) * (point1.Y - point2.Y));
            dataPoint[0].X = (int)((3 * point1.X + length * Math.Cos((angle1) * CHANGE_TO_RADIAN)) / 3.0f);
            dataPoint[0].Y = (int)((3 * point1.Y + length * Math.Sin((angle1) * CHANGE_TO_RADIAN)) / 3.0f);
            dataPoint[1].X = (int)((3 * point2.X - length * Math.Cos((angle2) * CHANGE_TO_RADIAN)) / 3.0f);
            dataPoint[1].Y = (int)((3 * point2.Y - length * Math.Sin((angle2) * CHANGE_TO_RADIAN)) / 3.0f);

            //复制最终控制点坐标
            finalDataPoint[0] = point1;
            finalDataPoint[1] = point1;
            finalDataPoint[2] = dataPoint[0];
            finalDataPoint[3] = dataPoint[1];
            finalDataPoint[4] = point2;
            finalDataPoint[5] = point2;

            //从初始点开始
            lines[0].X = (int)finalDataPoint[0].X;
            lines[0].Y = (int)finalDataPoint[0].Y;

            length = 0.0f;
            for (int i = 0; i < 3; i++)
            {
                for (float u = 0.01f; u <= 1; u += 0.01f)
                {
                    float b0 = 1.0f / 6 * (1 - u) * (1 - u) * (1 - u);
                    float b1 = 1.0f / 6 * (3 * u * u * u - 6 * u * u + 4);
                    float b2 = 1.0f / 6 * (-3 * u * u * u + 3 * u * u + 3 * u + 1);
                    float b3 = 1.0f / 6 * u * u * u;

                    lines[1].X = (b0 * finalDataPoint[i].X + b1 * finalDataPoint[i + 1].X + b2 * finalDataPoint[i + 2].X + b3 * finalDataPoint[i + 3].X);
                    lines[1].Y = (b0 * finalDataPoint[i].Y + b1 * finalDataPoint[i + 1].Y + b2 * finalDataPoint[i + 2].Y + b3 * finalDataPoint[i + 3].Y);

                    length += Bspline.CalculatePnt2Pnt(lines[0], lines[1]);

                    lines[0] = lines[1];
                }
            }
            length += Bspline.CalculatePnt2Pnt(lines[0], point2);

            return length;

        }



        //计算b样条长度，端点条件为自由端点条件 
        static public float CalculateBsplineLenWithoutDir(PointF[] inPoint)
        {
            int num = inPoint.Count();
            //系数矩阵对角列
            float[] a = new float[num];
            //系数矩阵对角上列
            float[] b = new float[num];
            //系数矩阵对角下列
            float[] c = new float[num];

            //定义soluctionX、soluctionY为线性方程的解
            float[] soluctionX = new float[num];
            float[] soluctionY = new float[num];
            //定义dataX和dataY,用来存放inPoint里的X和Y坐标
            float[] dataX = new float[num];
            float[] dataY = new float[num];
            //定义controlPoint用来存放控制点
            PointF[] controlPoint = new PointF[num + 4];
            //存放画线的两个使用点
            PointF[] lines = new PointF[2];

            //初始化 a,b,c
            a[0] = 18;
            a[num - 1] = 18;

            for (int i = 1; i < num - 1; i++)
            {
                a[i] = 4;
            }

            for (int i = 1; i < num - 1; i++)
            {
                b[i] = 1;
                c[i] = 1;
            }

            c[num - 1] = -9;
            b[0] = -9;



            for (int i = 0; i < num; i++)
            {
                dataX[i] = 6.0f * inPoint[i].X;
                dataY[i] = 6.0f * inPoint[i].Y;
            }

            dataX[0] *= 1.5f;
            dataY[0] *= 1.5f;
            dataX[num - 1] *= 1.5f;
            dataY[num - 1] *= 1.5f;






            //计算outdataX,outdataY;

            Calculate math = new Calculate();

            //调用Matrix用追赶法求解线性方程
            math.Matrix(dataX, num, ref a, ref b, ref c, ref soluctionX);
            math.Matrix(dataY, num, ref a, ref b, ref c, ref soluctionY);



            controlPoint[num + 3].X = dataX[num - 1] / 9;
            controlPoint[num + 2].X = dataX[num - 1] / 9;
            controlPoint[0].X = dataX[0] / 9;
            controlPoint[1].X = dataX[0] / 9;


            for (int i = 0; i < num; i++)
            {
                controlPoint[i + 2].X = soluctionX[i];

            }

            controlPoint[num + 3].Y = dataY[num - 1] / 9;
            controlPoint[num + 2].Y = dataY[num - 1] / 9;
            controlPoint[0].Y = dataY[0] / 9;
            controlPoint[1].Y = dataY[0] / 9;


            for (int i = 0; i < num; i++)
            {
                controlPoint[i + 2].Y = soluctionY[i];

            }



            //计算型值点，画出曲线
            //从初始点开始
            lines[0] = inPoint[0];

            float length = 0.0f;
            const float CHANGE_TO_ANGLE = (180.0f / 3.1415927f);
            for (int i = 0; i < num + 1; i++)
            {

                for (float u = 0.01f; u <= 1; u += 0.01f)
                {
                    float b0 = 1.0f / 6 * (1 - u) * (1 - u) * (1 - u);
                    float b1 = 1.0f / 6 * (3 * u * u * u - 6 * u * u + 4);
                    float b2 = 1.0f / 6 * (-3 * u * u * u + 3 * u * u + 3 * u + 1);
                    float b3 = 1.0f / 6 * u * u * u;

                    lines[1].X = (b0 * controlPoint[i].X + b1 * controlPoint[i + 1].X + b2 * controlPoint[i + 2].X + b3 * controlPoint[i + 3].X);
                    lines[1].Y = (b0 * controlPoint[i].Y + b1 * controlPoint[i + 1].Y + b2 * controlPoint[i + 2].Y + b3 * controlPoint[i + 3].Y);
                    length += Bspline.CalculatePnt2Pnt(lines[0], lines[1]);

                        float Db0 = -1.0f / 2 * (u - 1) * (u - 1);
                        float Db1 = 1.0f / 2 * (3 * u * u - 4 * u);
                        float Db2 = 1.0f / 2 * (-3 * u * u + u * 2 + 1);
                        float Db3 = 1.0f / 2 * u * u;

                        float angle = CHANGE_TO_ANGLE * (float)Math.Atan2((Db0 * controlPoint[i].Y + Db1 * controlPoint[i + 1].Y
                            + Db2 * controlPoint[i + 2].Y + Db3 * controlPoint[i + 3].Y),
                            (Db0 * controlPoint[i].X + Db1 * controlPoint[i + 1].X
                            + Db2 * controlPoint[i + 2].X + Db3 * controlPoint[i + 3].X));

                    lines[0] = lines[1];
                }
            }

            length += Bspline.CalculatePnt2Pnt(lines[0], inPoint[num-1]);

            return length;
        }



        static public float test1(string BsplineWithoutDirTxt)
        {
            float lelen = 0.0f;
            //数据读取
            StreamReader pathFile = File.OpenText(BsplineWithoutDirTxt);

            List<string> tempString = new List<string>();

            while (!pathFile.EndOfStream)
            {
                tempString.Add(pathFile.ReadLine());
            }

            pathFile.Close();


            //如果点数小于2，不能规划出轨迹
            if (tempString.Count() < 2) return 0.0f;

            List<PointF> tempPnts = new List<PointF>();

            foreach (string str in tempString)
            {
                string[] sArray = str.Split(',');
                tempPnts.Add(new PointF(float.Parse(sArray[0]), float.Parse(sArray[1])));
            }

            System.Drawing.PointF[] tempPoint = tempPnts.ToArray();


            pathFile.Close();


            return CalculateBsplineLenWithoutDir(tempPoint);

        }

        static public void test2(string textAddress)
        {
            //数据读取
            StreamReader pathFile = File.OpenText(textAddress);
            List<string> tempString = new List<string>();

            while (!pathFile.EndOfStream)
            {
                tempString.Add(pathFile.ReadLine());
            }

            pathFile.Close();

            //如果点数小于2，不能规划出轨迹
            if (tempString.Count() < 2) return;

            int i = 0;
            PointF[] pnt = new PointF[2];
            float[] angle = new float[2];
            float length = 0.0f;
            foreach (string str in tempString)
            {
                string[] sArray = str.Split(',');
                if (i == 0)
                {
                    pnt[1].X = float.Parse(sArray[0]);
                    pnt[1].Y = float.Parse(sArray[1]);
                    angle[1] = float.Parse(sArray[2]);
                }
                else
                {
                    pnt[1].X = float.Parse(sArray[0]);
                    pnt[1].Y = float.Parse(sArray[1]);
                    angle[1] = float.Parse(sArray[2]);

                    length += Bspline.CalculateBsplineLen(pnt[0], pnt[1], angle[0], angle[1]);
                }
                i++;
                pnt[0] = pnt[1];
                angle[0] = angle[1];
                //tempPnts.Add(new PointF(float.Parse(sArray[0]), float.Parse(sArray[1])));

            }


        }



    }
}
