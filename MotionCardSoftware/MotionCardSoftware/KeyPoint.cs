using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;



namespace MotionCardSoftware
{
    class KeyPoint
    {
        public PointF point;
        public float direction;

        public KeyPoint(PointF point, float direction)
        {
            this.point = point;
            this.direction = direction;
        }


        public KeyPoint(float x,float y, float direction)
        {
            this.point.X = x;
            this.point.Y = y;
            this.direction = direction;
        }

    }
}
