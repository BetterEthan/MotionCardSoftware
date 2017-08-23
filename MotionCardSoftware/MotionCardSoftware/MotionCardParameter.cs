using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionCardSoftware
{
    class MotionCardParameter
    {
        static private float accMax  = 630.0f;
        static private float velMax = 5000.0f;
        static private float robotR = 291.0f;

        static public void SetMotionCardParameters(float acc,float vel,float r)
        {
            accMax = acc;
            velMax = vel;
            robotR = r;
        }

        static public float GetAccMax()
        {
            return accMax;
        }

        static public float GetVelMax()
        {
            return velMax;
        }

        static public float GetRobotR()
        {
            return robotR;
        }


    }
}
