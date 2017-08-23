using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace MotionCardSoftware
{
    class Calculate
    {

        const float CHANGE_TO_ANGLE = 57.2958f;
        const float CHANGE_TO_RADIAN = 0.017453f;
        static float robotR = MotionCardParameter.GetRobotR();//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        public struct TriWheelVel2_t {
            public float speed;
            public float direction;
            public float rotationVel;
        };

        public struct TriWheelVel1_t
        {
            public float v1;
            public float v2;
            public float v3;
        };

        /*********************************************************************************
        * @name 	CalculateAngleSub
        * @brief	对-180,180交界处作处理
        * @param	minuend: 被减数;
                    subtrahend: 减数 A - B,A为被减数，B为减数;
        * @retval	返回计算后的角度值 -180~180
        *********************************************************************************/
        static public float CalculateAngleSub(float minuend, float subtrahend)
        {
            float result = 0.0f;
            result = minuend - subtrahend;
            if (result > 180.0f) result -= 360.0f;
            if (result < -180.0f) result += 360.0f;
            return result;
        }


        /*************************************************************
         * @name Matrix
         * @brief 追赶法求解线性方程组
         * @constantTerm 线性方程组等号右边的列矩阵
         * @solution 解
         * @num 行数或列数 
         * @m 系数矩阵对角数组
         * @n 对角上方数组
         * @k 对角下方数组
         * ***********************************************************/

        public void Matrix(float[] constantTerm, int num, ref float[] m, ref float[] n, ref float[] k, ref float[] solution)
        {
            //b为分解后的下三角矩阵的对角数组
            float[] a = new float[num];
            //a为分解后的单位上三角矩阵的对角上方数组
            float[] b = new float[num - 1];
            //c为分解后的单位上三角矩阵的对角上方数组
            float[] c = new float[num];
            //x为求解过程中的间接解
            float[] x = new float[num];
            int i;

            a[0] = m[0];
            b[0] = n[0] / a[0];

            //给分解后下三角矩阵的对角下方数组c赋值
            for (i = 1; i < num; i++)
            {
                c[i] = k[i];
            }


            //给分解后的单位上三角矩阵的对角上方数组a和分解后的单位上三角矩阵的对角上方数组b赋值
            for (i = 1; i < num - 1; i++)
            {
                a[i] = m[i] - k[i] * b[i - 1];
                b[i] = n[i] / a[i];

            }

            a[num - 1] = m[num - 1] - k[num - 1] * b[num - 2];
            //中间解x的初始值
            x[0] = constantTerm[0] / a[0];

            //给中间解赋值
            for (i = 1; i < num; i++)
            {
                x[i] = (constantTerm[i] - k[i] * x[i - 1]) / a[i];
            }

            //解出最终解
            solution[num - 1] = x[num - 1];

            for (i = num - 1; i > 0; i--)
            {
                solution[i - 1] = x[i - 1] - solution[i] * b[i - 1];
            }
        }

        /// <summary>
        /// 把输入格式为**x**y的字符串转化为相应的数值并存入,并描出小圆点
        /// @pnt为相对原点
        /// </summary>
        /// <param name="str"></param>



        //由三轮速度计算合成的速度
        //v1，v2,v3为三轮速度，单位为mm/s
        //zAngle 单位为度每秒
        public static TriWheelVel2_t TriWheelVel2ResultantVel(float v1, float v2, float v3, float zAngle)
        {
            TriWheelVel2_t trueVel;
            double[][] M = new double[3][]{
                new double[3],
                new double[3],
                new double[3] 
            };
            double[][] B = new double[3][]{
                new double[3],
                new double[3],
                new double[3] 
            };

            M[0][0] = -Math.Cos((60 + zAngle) * CHANGE_TO_RADIAN);
            M[0][1] = -Math.Sin((60 + zAngle) * CHANGE_TO_RADIAN);
            M[0][2] = robotR;
            M[1][0] = Math.Cos(zAngle * CHANGE_TO_RADIAN);
            M[1][1] = Math.Sin(zAngle * CHANGE_TO_RADIAN);
            M[1][2] = robotR;
            M[2][0] = -Math.Cos((60 - zAngle) * CHANGE_TO_RADIAN);
            M[2][1] = Math.Sin((60 - zAngle) * CHANGE_TO_RADIAN);
            M[2][2] = robotR;

            B = MatrixInv.InverseMatrix(M);

            float xVell = (float)(B[0][0] * v1 + B[0][1] * v2 + B[0][2] * v3);
            float yVell = (float)(B[1][0] * v1 + B[1][1] * v2 + B[1][2] * v3);
            trueVel.rotationVel = (float)(B[2][0] * v1 + B[2][1] * v2 + B[2][2] * v3);

            trueVel.rotationVel *= (CHANGE_TO_ANGLE);
            trueVel.speed = (float)Math.Sqrt((double)(xVell * xVell + yVell * yVell));
            trueVel.direction = (float)Math.Atan2(yVell, xVell) * CHANGE_TO_ANGLE;

            return trueVel;
        }



        //返回三个轮子轮速
        //speed       单位mm/s
        //direction 速度的方向  单位 度
        //rotationVell 旋转速度 单位 度/s 
        //posAngle 机器人的姿态  单位 度
        static public TriWheelVel1_t CaculateThreeWheelVel(float speed, float direction, float rotationVell, float angleZ)
        {
            TriWheelVel1_t vell;
            float Vx, Vy;
            float theta;
            float robotR = 0.0f;
            const float ALPHA = 60.0f; 

            robotR = MotionCardParameter.GetRobotR();
            rotationVell = rotationVell / CHANGE_TO_ANGLE;
            Vx = speed * (float)Math.Cos(direction * CHANGE_TO_RADIAN);
            Vy = speed * (float)Math.Sin(direction * CHANGE_TO_RADIAN);

            theta = angleZ;

            vell.v1 = (float)(-(float)Math.Cos((ALPHA + theta) * CHANGE_TO_RADIAN) * Vx - (float)Math.Sin((theta + ALPHA) * CHANGE_TO_RADIAN) * Vy + rotationVell * robotR);
            vell.v2 = (float)((float)Math.Cos(theta * CHANGE_TO_RADIAN) * Vx + (float)Math.Sin(theta * CHANGE_TO_RADIAN) * Vy + rotationVell * robotR);
            vell.v3 = (float)(-(float)Math.Cos((ALPHA - theta) * CHANGE_TO_RADIAN) * Vx + (float)Math.Sin((ALPHA - theta) * CHANGE_TO_RADIAN) * Vy + rotationVell * robotR);

            return vell;
        }

    }
}
