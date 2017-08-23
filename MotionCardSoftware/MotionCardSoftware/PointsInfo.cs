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
    
    class KeyPointInf 
    {
        public PointF point;
        public float direction;
        public float posAngle;
        public float length;
        public float curvatureR;
        public float velMax;

        public KeyPointInf(PointF pnt)
        {
            point = pnt;
        }

        public KeyPointInf(PointF pnt, float posAn)
        {
            point = pnt;
            posAngle = posAn;
        }

        public KeyPointInf(float x , float y, float posAn)
        {
            point.X = x;
            point.Y = y;
            posAngle = posAn;
        }


        public KeyPointInf(PointF pnt, float dir,float posAn)
        {
            point = pnt;
            direction = dir;
            posAngle = posAn;
        }

        public KeyPointInf(PointF pnt, float dir, float posAn,float len)
        {
            point = pnt;
            direction = dir;
            posAngle = posAn;
            length = len;
        }

        public KeyPointInf(PointF pnt, float dir, float posAn, float len, float cur)
        {
            point = pnt;
            direction = dir;
            posAngle = posAn;
            length = len;
            curvatureR = cur;
        }

        public KeyPointInf(PointF pnt, float dir, float posAn, float len, float cur, float velM)
        {
            point = pnt;
            direction = dir;
            posAngle = posAn;
            length = len;
            curvatureR = cur;
            velMax = velM;
        }

        public KeyPointInf(float x, float y, float dir, float posAn, float len, float cur, float velM)
        {
            point.X = x;
            point.Y = y;
            direction = dir;
            posAngle = posAn;
            length = len;
            curvatureR = cur;
            velMax = velM;
        }
    }

    class PointsInfo
    {
          static public  List<KeyPointInf> pnts = new List<KeyPointInf>();


    }
}
