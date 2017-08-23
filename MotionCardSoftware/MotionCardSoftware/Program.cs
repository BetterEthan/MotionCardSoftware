using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MotionCardSoftware
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Calculate.TriWheelVel2_t trueVell;
            //trueVell = Calculate.TriWheelVel2ResultantVel(100,0 ,-100, 0);

            //Console.WriteLine(trueVell.speed);
            //Console.WriteLine(trueVell.direction);
            //Console.WriteLine(trueVell.rotationVel);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MotionControlPlatform());

        }
    }
}
