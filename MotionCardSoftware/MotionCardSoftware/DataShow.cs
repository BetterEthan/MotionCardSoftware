using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;



namespace MotionCardSoftware
{
    public partial class DataShow : Form
    {

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

        private string SpeedReplanInfo = Directory.GetCurrentDirectory() + "\\speedReplan.txt";


        public DataShow()
        {
            InitializeComponent();
        }

        private void DataShow_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.Text)
            {
                case "PathAllInf":
                    //数据读取
                    StreamReader pathFile1 = File.OpenText(PathAllInf);
                    List<string> tempString1 = new List<string>();

                    while (!pathFile1.EndOfStream)
                    {
                        tempString1.Add(pathFile1.ReadLine());
                    }

                    listBox1.DataSource = tempString1;

                    pathFile1.Close();
                    break;

                case "BsplineDrawPathInf":
                    //数据读取
                    StreamReader pathFile2 = File.OpenText(BsplineDrawPathInf);
                    List<string> tempString2 = new List<string>();

                    while (!pathFile2.EndOfStream)
                    {
                        tempString2.Add(pathFile2.ReadLine());
                    }

                    listBox1.DataSource = tempString2;

                    pathFile2.Close();
                    break;

                case "SpeedReplanInfo":
                    //数据读取
                    StreamReader pathFile3 = File.OpenText(SpeedReplanInfo);
                    List<string> tempString3 = new List<string>();

                    while (!pathFile3.EndOfStream)
                    {
                        tempString3.Add(pathFile3.ReadLine());
                    }

                    listBox1.DataSource = tempString3;

                    pathFile3.Close();
                    break;
            }

        }


    }
}
