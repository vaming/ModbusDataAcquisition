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
using INIFILE;
using System.Runtime.InteropServices;
using Modbus.Device;    //for modbus master

namespace ModbusDataAcquisition
{
    
    public partial class ModbusDataAcquisition : Form
    {
        SerialPort serialPort = new SerialPort();
        ModbusSerialMaster master;
        public ModbusDataAcquisition()
        {
            InitializeComponent();
        }

        private void ModbusDataAcquisition_Load(object sender, EventArgs e)
        {
            #region 串口的参数载入            
            INIFILE.Profile.LoadProfile();//加载所有
            // 预置波特率
            switch (Profile.G_BAUDRATE)
            {
                case "4800":
                    cbBaudRate.SelectedIndex = 0;
                    break;
                case "9600":
                    cbBaudRate.SelectedIndex = 1;
                    break;
                case "19200":
                    cbBaudRate.SelectedIndex = 2;
                    break;
                case "38400":
                    cbBaudRate.SelectedIndex = 3;
                    break;
                case "115200":
                    cbBaudRate.SelectedIndex = 4;
                    break;
                default:
                    {
                        MessageBox.Show("波特率预置参数错误。");
                        return;
                    }
            }

            //预置数据位
            switch (Profile.G_DATABITS)
            {
                case "5":
                    cbDataBits.SelectedIndex = 0;
                    break;
                case "6":
                    cbDataBits.SelectedIndex = 1;
                    break;
                case "7":
                    cbDataBits.SelectedIndex = 2;
                    break;
                case "8":
                    cbDataBits.SelectedIndex = 3;
                    break;
                default:
                    {
                        MessageBox.Show("数据位预置参数错误。");
                        return;
                    }

            }
            //预置停止位
            switch (Profile.G_STOP)
            {
                case "1":
                    cbStop.SelectedIndex = 0;
                    break;
                case "1.5":
                    cbStop.SelectedIndex = 1;
                    break;
                case "2":
                    cbStop.SelectedIndex = 2;
                    break;
                default:
                    {
                        MessageBox.Show("停止位预置参数错误。");
                        return;
                    }
            }

            //预置校验位
            switch (Profile.G_PARITY)
            {
                case "NONE":
                    cbParity.SelectedIndex = 0;
                    break;
                case "ODD":
                    cbParity.SelectedIndex = 1;
                    break;
                case "EVEN":
                    cbParity.SelectedIndex = 2;
                    break;
                default:
                    {
                        MessageBox.Show("校验位预置参数错误。");
                        return;
                    }
            }

            //检查是否含有串口
            string[] str = SerialPort.GetPortNames();
            if (str == null)
            {
                MessageBox.Show("本机没有串口！", "Error");
                return;
            }

            //添加串口项目
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {//获取有多少个COM口
                //System.Diagnostics.Debug.WriteLine(s);
                cbSerial.Items.Add(s);
            }

            //串口设置默认选择项
            cbSerial.SelectedIndex = 0;         //note：获得COM9口，但别忘修改

            serialPort.BaudRate = 9600;

            Control.CheckForIllegalCrossThreadCalls = false;    //这个类中我们不检查跨线程的调用是否合法(因为.net 2.0以后加强了安全机制,，不允许在winform中直接跨线程访问控件的属性)
            //serialPort.DataReceived += new SerialDataReceivedEventHandler(scom_DataReceived);
            //serialPort.ReceivedBytesThreshold = 128;//事件发生前内部输入缓冲区的字节数，每当缓冲区的字节达到此设定的值，就会触发对象的数据接收事件
            //准备就绪              
            serialPort.DtrEnable = true;
            serialPort.RtsEnable = true;
            //设置数据读取超时为0.5秒
            serialPort.ReadTimeout = 300;
            serialPort.WriteTimeout = 1000;
            serialPort.ReadBufferSize = 1024 * 1024 * 30;//设置串口缓存大小为30M
            serialPort.WriteBufferSize = 1024 * 1024 * 30;
            serialPort.Close();
            #endregion

        }

        #region 保存串口参数
        private void btnSave_Click(object sender, EventArgs e)
        {
            //设置各“串口设置”
            string strBaudRate = cbBaudRate.Text;
            string strDateBits = cbDataBits.Text;
            string strStopBits = cbStop.Text;
            Int32 iBaudRate = Convert.ToInt32(strBaudRate);
            Int32 iDateBits = Convert.ToInt32(strDateBits);

            Profile.G_BAUDRATE = iBaudRate + "";       //波特率
            Profile.G_DATABITS = iDateBits + "";       //数据位
            switch (cbStop.Text)            //停止位
            {
                case "1":
                    Profile.G_STOP = "1";
                    break;
                case "1.5":
                    Profile.G_STOP = "1.5";
                    break;
                case "2":
                    Profile.G_STOP = "2";
                    break;
                default:
                    MessageBox.Show("Error：参数不正确!", "Error");
                    break;
            }
            switch (cbParity.Text)             //校验位
            {
                case "无":
                    Profile.G_PARITY = "NONE";
                    break;
                case "奇校验":
                    Profile.G_PARITY = "ODD";
                    break;
                case "偶校验":
                    Profile.G_PARITY = "EVEN";
                    break;
                default:
                    MessageBox.Show("Error：参数不正确!", "Error");
                    break;
            }


            Profile.SaveProfile();
            Console.WriteLine(DateTime.Now.ToString() + " =>Save profile sucessfully!");
        }
        #endregion

        #region 串口的打开
        private void btnOpenCloseSCom_Click(object sender, EventArgs e)
        {
            if (!serialPort.IsOpen)
            {
                try
                {
                    //设置串口号
                    string serialName = cbSerial.SelectedItem.ToString();
                    serialPort.PortName = serialName;

                    //设置各“串口设置”
                    string strBaudRate = cbBaudRate.Text;
                    string strDateBits = cbDataBits.Text;
                    string strStopBits = cbStop.Text;
                    Int32 iBaudRate = Convert.ToInt32(strBaudRate);
                    Int32 iDateBits = Convert.ToInt32(strDateBits);

                    serialPort.BaudRate = iBaudRate;       //波特率
                    serialPort.DataBits = iDateBits;       //数据位
                    switch (cbStop.Text)            //停止位
                    {
                        case "1":
                            serialPort.StopBits = StopBits.One;
                            break;
                        case "1.5":
                            serialPort.StopBits = StopBits.OnePointFive;
                            break;
                        case "2":
                            serialPort.StopBits = StopBits.Two;
                            break;
                        default:
                            MessageBox.Show("Error：参数不正确!", "Error");
                            break;
                    }
                    switch (cbParity.Text)             //校验位
                    {
                        case "无":
                            serialPort.Parity = Parity.None;
                            break;
                        case "奇校验":
                            serialPort.Parity = Parity.Odd;
                            break;
                        case "偶校验":
                            serialPort.Parity = Parity.Even;
                            break;
                        default:
                            MessageBox.Show("Error：参数不正确!", "Error");
                            break;
                    }

                    if (serialPort.IsOpen == true)//如果打开状态，则先关闭一下
                    {
                        serialPort.Close();
                    }
                    //状态栏设置
                    //tsSpNum.Text = "串口号：" + serialPort.PortName + "|";
                    //tsBaudRate.Text = "波特率：" + serialPort.BaudRate + "|";
                    //tsDataBits.Text = "数据位：" + serialPort.DataBits + "|";
                    //tsStopBits.Text = "停止位：" + serialPort.StopBits + "|";
                    //tsParity.Text = "校验位：" + serialPort.Parity + "|";

                    //设置必要控件不可用
                    cbSerial.Enabled = false;
                    cbBaudRate.Enabled = false;
                    cbDataBits.Enabled = false;
                    cbStop.Enabled = false;
                    cbParity.Enabled = false;


                    serialPort.Open();     //打开串口

                    master = ModbusSerialMaster.CreateRtu(serialPort);//创建RTU通信
                    master.Transport.Retries = 0;   //don't have to do retries
                    master.Transport.ReadTimeout = 300; //milliseconds
                    Console.WriteLine(DateTime.Now.ToString() + " =>Open " + serialPort.PortName + " sucessfully!");


                    btnOpenCloseSCom.Text = "关闭串口";
                    //打开定时器
                    //  taskTimer.Enabled = true;
                    //tsTips.Text = "串口打开成功";

                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Error:" + ex.Message, "Error");
                    //tmSend.Enabled = false;
                    return;
                }
            }
            else
            {

                //恢复控件功能
                //设置必要控件不可用
                cbSerial.Enabled = true;
                cbBaudRate.Enabled = true;
                cbDataBits.Enabled = true;
                cbStop.Enabled = true;
                cbParity.Enabled = true;

                serialPort.Close();                    //关闭串口
                btnOpenCloseSCom.Text = "打开串口";
                // tmSend.Enabled = false;         //关闭计时器
                // taskTimer.Enabled = false;//关闭计时器
                //tsTips.Text = "串口已关闭";
                Console.WriteLine(DateTime.Now.ToString() + " =>Disconnect " + serialPort.PortName);
            }
        }
        #endregion
    }
}
