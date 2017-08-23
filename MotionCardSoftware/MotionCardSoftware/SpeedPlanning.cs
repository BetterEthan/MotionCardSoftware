using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace MotionCardSoftware
{
    class SpeedPlanning
    {
        //计算所得关键点信息
        static public string PathAllInf = Directory.GetCurrentDirectory() + "\\PathAllInf.txt";

        public const float CHANGE_TO_RADIAN = 0.017453f;

        //计算每一段的曲率半径并存入文件
        static public void CalculateCurvature()
        {

            //清空ringbuffer
            PointsInfo.pnts.Clear();
            //数据读取
            StreamReader pathFile = File.OpenText(PathAllInf);
            List<string> tempString = new List<string>();

            while (!pathFile.EndOfStream)
            {
                tempString.Add(pathFile.ReadLine());
            }

            pathFile.Close();

            //如果点数小于2，不能规划出轨迹
            if (tempString.Count() < 2) return;

            float directionNow = 0.0f;

            float lengthNow = 0.0f;

            float directionLast = 0.0f;

            float lengthLast = 0.0f;

            float curvatureR = 0.0f;

            foreach (string str in tempString)
            {
                string[] sArray = str.Split(',');

                directionNow = float.Parse(sArray[2]);

                lengthNow = float.Parse(sArray[4]);

                if (Math.Abs(Calculate.CalculateAngleSub(directionLast, directionNow)) < 0.01f)
                {
                    curvatureR = (lengthNow - lengthLast) / 0.0001f;
                }
                else
                {
                    curvatureR = Math.Abs((lengthNow - lengthLast) / (Calculate.CalculateAngleSub(directionLast, directionNow) * CHANGE_TO_RADIAN));
                }

                PointsInfo.pnts.Add(new KeyPointInf( float.Parse(sArray[0]), float.Parse(sArray[1]), float.Parse(sArray[2]), float.Parse(sArray[3]),
                    float.Parse(sArray[4]), curvatureR,0));

                directionLast = directionNow;

                lengthLast = lengthNow;

            }
            //Console.WriteLine(PointsInfo.pnts.Count());
            SpeedPlan();

            MoveListToTxt();

        }



        //将ringbuffer里的数据存入txt文档中
        static public void MoveListToTxt()
        {
            StreamWriter PathAllInfFile = new StreamWriter(PathAllInf, false);

            for (int i = 0; i < PointsInfo.pnts.Count(); i++)
            {
                PathAllInfFile.WriteLine("{0:f0},{1:f0},{2:f1},{3:f1},{4:f0},{5:f0},{6:f0}", PointsInfo.pnts[i].point.X,
                    PointsInfo.pnts[i].point.Y, PointsInfo.pnts[i].direction, PointsInfo.pnts[i].posAngle,
                    PointsInfo.pnts[i].length, PointsInfo.pnts[i].curvatureR, PointsInfo.pnts[i].velMax);
            }

            PathAllInfFile.Close();
           
        }

        static public void MoveTxtToRingBuffer()
        {
            PointsInfo.pnts.Clear();
            //数据读取
            StreamReader file = File.OpenText(PathAllInf);

            List<string> tempString = new List<string>();

            while (!file.EndOfStream)
            {
                tempString.Add(file.ReadLine());
            }

            file.Close();

            foreach (string str in tempString)
            {
                string[] sArray = str.Split(',');
                PointsInfo.pnts.Add(new KeyPointInf(float.Parse(sArray[0]), float.Parse(sArray[1]), float.Parse(sArray[2]), float.Parse(sArray[3])
                    , float.Parse(sArray[4]), float.Parse(sArray[5]), float.Parse(sArray[6])));
            }
            
        }


        static public void SpeedPlan()
        {
            int num = PointsInfo.pnts.Count();

            //将曲率半径左移一个单位，便于机器提前反应
            for (int i = 0; i < num - 1; i++)
            {
                PointsInfo.pnts[i].curvatureR = PointsInfo.pnts[i+1].curvatureR;
            }

            //电机最大速度能够满足的最小曲率半径
            float curvatureMaxVell = MotionCardParameter.GetVelMax() * MotionCardParameter.GetVelMax() / (2 * MotionCardParameter.GetAccMax());

            //将过大的曲率半径全设为能最大速度能满足的最小曲率半径
            for (int i = 0; i < num; i++)
            {
                if (PointsInfo.pnts[i].curvatureR > curvatureMaxVell)
                {
                    PointsInfo.pnts[i].curvatureR = curvatureMaxVell;
                }
            }

            //curvature[0] = curvature[1];
            //curvature[n - 1] = curvature[n - 2];

            //通过曲率半径计算该段能满足的最大速度                                         
            for (int i = 0; i < num; i++)
            {
                PointsInfo.pnts[i].velMax = (float)Math.Sqrt((2 * MotionCardParameter.GetAccMax()) * PointsInfo.pnts[i].curvatureR);
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




            float[] wheelOne = new float[PointsInfo.pnts.Count()];
            float[] wheelTwo = new float[PointsInfo.pnts.Count()];
            float[] wheelThree = new float[PointsInfo.pnts.Count()];

            //计算此时三个轮的速度
	        CalThreeWheelVel(wheelOne, wheelTwo, wheelThree);


	        //动态的对速度进行平衡
	        while (true)
	        {
		        int ipoint = 0;

                for (ipoint = 3; ipoint < num; ipoint++)
		        {
			        float time = 0.0f;

			        float lll;
			        float vvv;
			        lll = (PointsInfo.pnts[ipoint - 1].length - PointsInfo.pnts[ipoint - 2].length);
			        vvv = (PointsInfo.pnts[ipoint - 1].velMax + PointsInfo.pnts[ipoint - 2].velMax) / 2;
			        time = lll / vvv;

			        float a1, a2, a3;
			        //如果判断某一个轮子加速度大于最大加速度时，进行调节

			        a1 = (wheelOne[ipoint - 1] - wheelOne[ipoint - 2]) / time;
			        a2 = (wheelTwo[ipoint - 1] - wheelTwo[ipoint - 2]) / time;
			        a3 = (wheelThree[ipoint - 1] - wheelThree[ipoint - 2]) / time;

			        if (((a1 > MotionCardParameter.GetAccMax()) && (wheelOne[ipoint - 1] * wheelOne[ipoint - 2] > 0)) 
				        || ((a2 > MotionCardParameter.GetAccMax()) && (wheelTwo[ipoint - 1] * wheelTwo[ipoint - 2] > 0))
				        || ((a3 > MotionCardParameter.GetAccMax()) && (wheelThree[ipoint - 1] * wheelThree[ipoint - 2] > 0)))
			        {
				        //平衡法规划速度
				        DynamicalAjusting(wheelOne, wheelTwo, wheelThree);

				        break;
			        }
		        }




                if (ipoint == num)
                {


                    for (int i = 1; i < num-1; i++)
                    {
                        Calculate.TriWheelVel1_t tempTrueVell;
                        tempTrueVell.v1 = wheelOne[i];
                        tempTrueVell.v2 = wheelTwo[i];
                        tempTrueVell.v3 = wheelThree[i];

                        float vellCar1, vellCar2, vellCar3, vellCar;
                        float angErr = PointsInfo.pnts[i + 1].posAngle - PointsInfo.pnts[i].posAngle;

                        angErr = angErr > 180 ? angErr - 360 : angErr;
                        angErr = angErr < -180 ? 360 + angErr : angErr;

                        //粗略计算每两示教点之间的运动的时间
                        float time = (PointsInfo.pnts[i + 1].length - PointsInfo.pnts[i].length) / (PointsInfo.pnts[i + 1].velMax + PointsInfo.pnts[i].velMax) * 2;

                        vellCar1 = DecreseVellByOneWheel(PointsInfo.pnts[i].velMax, PointsInfo.pnts[i].direction, angErr / time, PointsInfo.pnts[i].posAngle, 1, tempTrueVell.v1);

                        vellCar2 = DecreseVellByOneWheel(PointsInfo.pnts[i].velMax, PointsInfo.pnts[i].direction, angErr / time, PointsInfo.pnts[i].posAngle, 2, tempTrueVell.v2);

                        vellCar3 = DecreseVellByOneWheel(PointsInfo.pnts[i].velMax, PointsInfo.pnts[i].direction, angErr / time, PointsInfo.pnts[i].posAngle, 3, tempTrueVell.v3);

                        if (Math.Abs(vellCar1) >= Math.Abs(vellCar2) && Math.Abs(vellCar1) >= Math.Abs(vellCar3))
                        {
                            vellCar = vellCar1;
                            //将计算的最新合速度放入缓存池中
                            PointsInfo.pnts[i].velMax = vellCar;
                        }
                        else if (Math.Abs(vellCar2) >= Math.Abs(vellCar1) && Math.Abs(vellCar2) >= Math.Abs(vellCar3))
                        {
                            vellCar = vellCar2;
                            //将计算的最新合速度放入缓存池中
                            PointsInfo.pnts[i].velMax = vellCar;
                        }
                        else if (Math.Abs(vellCar3) >= Math.Abs(vellCar2) && Math.Abs(vellCar3) >= Math.Abs(vellCar1))
                        {
                            vellCar = vellCar3;
                            //将计算的最新合速度放入缓存池中
                            PointsInfo.pnts[i].velMax = vellCar;
                        }





                    }
                }

                if (ipoint == num)
                {



                    PointsInfo.pnts[0].velMax = 100;
                    PointsInfo.pnts[num - 1].velMax = 100; 


                    //通过v2^2 - v1^2 = 2*a*s对速度再次规划
                    for (int i = 0; i < num - 1; i++)
                    {
                        if (PointsInfo.pnts[i + 1].velMax > PointsInfo.pnts[i].velMax)
                        {
                            tempVell = (float)Math.Sqrt(2 * (MotionCardParameter.GetAccMax()) * (PointsInfo.pnts[i + 1].length - PointsInfo.pnts[i].length) + PointsInfo.pnts[i].velMax * PointsInfo.pnts[i].velMax);
                            if (tempVell < PointsInfo.pnts[i + 1].velMax)
                            {
                                PointsInfo.pnts[i + 1].velMax = tempVell;
                            }
                        }
                    }




                    for (int i = num - 1; i > 0; i--)
                    {
                        if (PointsInfo.pnts[i - 1].velMax > PointsInfo.pnts[i].velMax)
                        {
                            tempVell = (float)Math.Sqrt(2 * (MotionCardParameter.GetAccMax()) * (PointsInfo.pnts[i].length - PointsInfo.pnts[i - 1].length) + PointsInfo.pnts[i].velMax * PointsInfo.pnts[i].velMax);
                            if (tempVell < PointsInfo.pnts[i - 1].velMax)
                            {
                                PointsInfo.pnts[i - 1].velMax = tempVell;
                            }
                        }
                    }








                    //将速度小于最小速度的做处理
                    for (int i = 0; i < num; i++)
                    {
                        if (PointsInfo.pnts[i].velMax < 70)
                        {
                            PointsInfo.pnts[i].velMax = 70;
                        }
                    }


                    break;
                }


            }

        }




        //通过ringBuffer里的数据计算每一点处三个轮子的速度
        //目的更新wheelOne wheelTwo wheelThree这三个数组里的三轮速度，便于下一次的速度削减
        //wheelOne 一号轮速度数组首地址
        //wheelTwo 	二号轮速度数组首地址
        //wheelThree 三号轮速度数组首地址
        static public void CalThreeWheelVel(float[] wheelOne, float[] wheelTwo, float[] wheelThree)
        {
            //分解到三个轮对全局速度进行规划
            
            float n = PointsInfo.pnts.Count();

            Calculate.TriWheelVel1_t threeVell;
            for (int i = 2; i < n + 1; i++)
            {
                float angErr = PointsInfo.pnts[i - 1].posAngle - PointsInfo.pnts[i - 2].posAngle;
                angErr = angErr > 180 ? angErr - 360 : angErr;
                angErr = angErr < -180 ? 360 + angErr : angErr;

                float time = 0.0f;

                time = (PointsInfo.pnts[i - 1].length - PointsInfo.pnts[i - 2].length) / (PointsInfo.pnts[i - 1].velMax + PointsInfo.pnts[i - 2].velMax) * 2;

                float rotationVell = angErr / time;


                threeVell = Calculate.CaculateThreeWheelVel(PointsInfo.pnts[i - 1].velMax, PointsInfo.pnts[i - 1].direction, rotationVell, PointsInfo.pnts[i - 1].posAngle);


                wheelOne[i - 1] = threeVell.v1;

                wheelTwo[i - 1] = threeVell.v2;

                wheelThree[i - 1] = threeVell.v3;

            }
            wheelOne[0] = wheelOne[1];
            wheelTwo[0] = wheelTwo[1];
            wheelThree[0] = wheelThree[1];

        }


        //通过配置的轮子最大加速度进行降速
        //适当比例的降速，算完后记得把最新速度数据放在ringbuffer里
        //wheelOne 一号轮速度数组首地址
        //wheelTwo 	二号轮速度数组首地址
        //wheelThree 三号轮速度数组首地址
        static public void DynamicalAjusting(float[] wheelOne, float[] wheelTwo, float[] wheelThree)
        {
            float time = 0.0f;
            int n = PointsInfo.pnts.Count();
            float tempAcc = 0.0f;

            //每次加速度降低至上次的百分值
            float percent = 0.9f;

            //先正向削减速度
            for (int i = 2; i < n + 1; i++)
            {

                //粗略计算每两示教点之间的运动的时间
                time = (PointsInfo.pnts[i - 1].length - PointsInfo.pnts[i - 2].length) / (PointsInfo.pnts[i - 1].velMax + PointsInfo.pnts[i - 2].velMax) * 2;
                //轮1
                //只处理速度同向的情况
                if (wheelOne[i - 1] * wheelOne[i - 2] > 0)
                {
                    tempAcc = (Math.Abs(wheelOne[i - 1]) - Math.Abs(wheelOne[i - 2])) / time;
                }
                else
                {
                    tempAcc = 0.0f;
                }

                if (tempAcc > MotionCardParameter.GetAccMax())
                {
                    //每次削减0.05的加速度
                    wheelOne[i - 1] = wheelOne[i - 1] > 0 ? wheelOne[i - 2] + tempAcc * percent * time : wheelOne[i - 2] - tempAcc * percent * time;
                }
                //轮2
                //只处理速度同向的情况
                if (wheelTwo[i - 1] * wheelTwo[i - 2] > 0)
                {
                    tempAcc = (Math.Abs(wheelTwo[i - 1]) - Math.Abs(wheelTwo[i - 2])) / time;
                }
                else
                {
                    tempAcc = 0.0f;
                }

                if (tempAcc > MotionCardParameter.GetAccMax())
                {
                    //每次削减0.05的加速度
                    wheelTwo[i - 1] = wheelTwo[i - 1] > 0 ? wheelTwo[i - 2] + tempAcc * percent * time : wheelTwo[i - 2] - tempAcc * percent * time;
                }

                //轮3
                //只处理速度同向的情况
                if (wheelThree[i - 1] * wheelThree[i - 2] > 0)
                {
                    tempAcc = (Math.Abs(wheelThree[i - 1]) - Math.Abs(wheelThree[i - 2])) / time;
                }
                else
                {
                    tempAcc = 0.0f;
                }

                if (tempAcc > MotionCardParameter.GetAccMax())
                {
                    //每次削减0.05的加速度
                    wheelThree[i - 1] = wheelThree[i - 1] > 0 ? wheelThree[i - 2] + tempAcc * percent * time : wheelThree[i - 2] - tempAcc * percent * time;
                }
            }

            //反向削减速度
            for (int i = n; i > 1; i--)
            {

                //粗略计算每两示教点之间的运动的时间
                time = (PointsInfo.pnts[i - 1].length - PointsInfo.pnts[i - 2].length) / (PointsInfo.pnts[i - 1].velMax + PointsInfo.pnts[i - 2].velMax) * 2;
                //轮1
                //只处理速度同向的情况
                if (wheelOne[i - 1] * wheelOne[i - 2] > 0)
                {
                    tempAcc = (Math.Abs(wheelOne[i - 1]) - Math.Abs(wheelOne[i - 2])) / time;
                }
                else
                {
                    tempAcc = 0.0f;
                }

                if (tempAcc < -MotionCardParameter.GetAccMax())
                {
                    //每次削减0.05的加速度
                    wheelOne[i - 2] = wheelOne[i - 2] > 0 ? wheelOne[i - 1] - tempAcc * percent * time : wheelOne[i - 1] + tempAcc * percent * time;
                }


                //轮2
                //只处理速度同向的情况
                if (wheelTwo[i - 1] * wheelTwo[i - 2] > 0)
                {
                    tempAcc = (Math.Abs(wheelTwo[i - 1]) - Math.Abs(wheelTwo[i - 2])) / time;
                }
                else
                {
                    tempAcc = 0.0f;
                }

                if (tempAcc < -MotionCardParameter.GetAccMax())
                {
                    //每次削减0.05的加速度
                    wheelTwo[i - 2] = wheelTwo[i - 2] > 0 ? wheelTwo[i - 1] - tempAcc * percent * time : wheelTwo[i - 1] + tempAcc * percent * time;
                }


                //轮3
                //只处理速度同向的情况
                if (wheelThree[i - 1] * wheelThree[i - 2] > 0)
                {
                    tempAcc = (Math.Abs(wheelThree[i - 1]) - Math.Abs(wheelThree[i - 2])) / time;
                }
                else
                {
                    tempAcc = 0.0f;
                }

                if (tempAcc < -MotionCardParameter.GetAccMax())
                {
                    //每次削减0.05的加速度
                    wheelThree[i - 2] = wheelThree[i - 2] > 0 ? wheelThree[i - 1] - tempAcc * percent * time : wheelThree[i - 1] + tempAcc * percent * time;
                }
            }



            for (int i = 0; i < n - 1; i++)
            {
                Calculate.TriWheelVel2_t tempVel2;

                tempVel2 = Calculate.TriWheelVel2ResultantVel(wheelOne[i], wheelTwo[i], wheelThree[i],PointsInfo.pnts[i].posAngle);

                //存入新计算的速度
                PointsInfo.pnts[i].velMax = tempVel2.speed;
            }

        }




        //通过降低合速度保证某轮的速度要求
        //vellCar 降速前的前进合速度 单位 mm/s
        //orientation 速度朝向 单位 度
        //rotationalVell 旋转速度 单位 度每秒
        //wheelNum  所降速的轮号 
        // targetWheelVell   所降速的目标
        // 返回所降低后的合速度
        static public float DecreseVellByOneWheel(float vellCar, float orientation, float rotationalVell, float zAngle, int wheelNum, float targetWheelVell)
        {
            Calculate.TriWheelVel1_t vell;
            int i;
            switch (wheelNum)
            {
                case 1:
                    //每次合速度乘0.9,直到满足一号轮速度降低至目标速度。对于一些不能满足的，循环10次后自动退出
                    for (i = 0; i < 10; i++)
                    {

                        vellCar *= 0.9f;
                        vell = Calculate.CaculateThreeWheelVel(vellCar, orientation, rotationalVell, zAngle);
                        if (Math.Abs(vell.v1) < Math.Abs(targetWheelVell))
                        {
                            break;
                        }
                    }
                    break;

                case 2:
                    for (i = 0; i < 10; i++)
                    {

                        vellCar *= 0.9f;
                        vell = Calculate.CaculateThreeWheelVel(vellCar, orientation, rotationalVell, zAngle);
                        if (Math.Abs(vell.v2) < Math.Abs(targetWheelVell))
                        {
                            break;
                        }
                    }
                    break;

                case 3:
                    for (i = 0; i < 10; i++)
                    {

                        vellCar *= 0.9f;
                        vell = Calculate.CaculateThreeWheelVel(vellCar, orientation, rotationalVell, zAngle);
                        if (Math.Abs(vell.v3) < Math.Abs(targetWheelVell))
                        {
                            break;
                        }
                    }
                    break;
            }
            return vellCar;
        }
    }
}
