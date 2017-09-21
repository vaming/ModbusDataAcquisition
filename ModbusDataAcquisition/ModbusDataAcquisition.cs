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
using System.Timers;
using ModbusRTU;



namespace ModbusDataAcquisition
{

    public partial class ModbusDataAcquisition : Form
    {
        SerialPort serialPort = new SerialPort();
        ModbusSerialMaster master;
         
        System.Timers.Timer taskTimer = new System.Timers.Timer(1000);//定义一个1000ms的定时器


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

            tbAddr.Text = "1";//预置地址
            tbTime.Text = "1000";//预置时间间隔

            
            taskTimer.Elapsed += new System.Timers.ElapsedEventHandler(timerReadSCom);//定义定时执行的任务
            taskTimer.AutoReset = true;//不断执行
            taskTimer.Enabled = false;//关定时

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

        #region 串口的打开与关闭
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
                    master.Transport.ReadTimeout = 30; //milliseconds
                    Console.WriteLine(DateTime.Now.ToString() + " =>Open " + serialPort.PortName + " sucessfully!");


                    ///代码测试区
                    ///
                     ModbusRTU.ModbusRTU.ConvertHexToSingle();
                   float x1= ModbusRTU.ModbusRTU.StringToFloat("3FCC81BF");
                    string x2 = ModbusRTU.ModbusRTU.FloatToIntString(123.456f);
                    byte[] y = new byte[] { 0x3F, 0xCC, 0x81, 0xBF };
                    float x3 = ModbusRTU.ModbusRTU.ByteToFloat(y);

                    //创建定时任务
                    taskTimer.Interval = Convert.ToInt16(tbTime.Text);//重新设置间隔时间
                    taskTimer.Enabled = true;//启用定时


                    btnOpenCloseSCom.Text = "关闭串口";
                    //打开定时器
                    //  taskTimer.Enabled = true;
                    //tsTips.Text = "串口打开成功";


                    

                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Error:" + ex.Message, "Error");
                    //tmSend.Enabled = false;
                    cbSerial.Enabled = true;
                    cbBaudRate.Enabled = true;
                    cbDataBits.Enabled = true;
                    cbStop.Enabled = true;
                    cbParity.Enabled = true;
                    taskTimer.Enabled = false;//关定时
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
                taskTimer.Enabled = false;//先不启用定时

                serialPort.Close();                    //关闭串口
                btnOpenCloseSCom.Text = "打开串口";
                // tmSend.Enabled = false;         //关闭计时器
                // taskTimer.Enabled = false;//关闭计时器
                //tsTips.Text = "串口已关闭";
                Console.WriteLine(DateTime.Now.ToString() + " =>Disconnect " + serialPort.PortName);
            }
        }
        #endregion


        #region RTU数据的读取
        void timerReadSCom(object sender, ElapsedEventArgs e)
        {
            //Console.WriteLine(DateTime.Now.ToString() + " => 定时器在运行" );
            try
            {
                byte slaveID = Convert.ToByte(tbAddr.Text);
                ushort startAddress = 20;
                ushort numofPoints = 12;
                List<ushort> listAI = new List<ushort>();//定义字符串list
                //read AI(3xxxx)                        
                ushort[] register = master.ReadInputRegisters(slaveID, startAddress, numofPoints);
                Boolean[] re1 = master.ReadInputs(slaveID, startAddress, numofPoints);

                for (int i = 0; i < numofPoints; i++)
                {
                    listAI[i] = register[i];
                    Console.WriteLine(DateTime.Now.ToString() + " => AI " + i + "=" + listAI[i]);
                    //If you need to show the value with other unit, you have to caculate the gain and offset
                    //eq. 0 to 0kg, 32767 to 1000kg
                    //0 (kg) = gain * 0 + offset
                    //1000 (kg) = gain *32767 + offset
                    //=> gain=1000/32767, offset=0
                    //double value = (double)register[i] * 10.0 / 32767;
                    //listAI[i].Text = value.ToString("0.00");
                }
            }
            catch (Exception exception)
            {
                //Connection exception
                //No response from server.
                //The server maybe close the com port, or response timeout.
                if (exception.Source.Equals("System"))
                {
                    Console.WriteLine(DateTime.Now.ToString() + " " + exception.Message);
                }
                //The server return error code.
                //You can get the function code and exception code.
                if (exception.Source.Equals("nModbusPC"))
                {
                    string str = exception.Message;
                    int FunctionCode;
                    string ExceptionCode;

                    str = str.Remove(0, str.IndexOf("\r\n") + 17);
                    FunctionCode = Convert.ToInt16(str.Remove(str.IndexOf("\r\n")));
                    Console.WriteLine("Function Code: " + FunctionCode.ToString("X"));

                    str = str.Remove(0, str.IndexOf("\r\n") + 17);
                    ExceptionCode = str.Remove(str.IndexOf("-"));
                    switch (ExceptionCode.Trim())
                    {
                        case "1":
                            Console.WriteLine("Exception Code: " + ExceptionCode.Trim() + "----> Illegal function!");
                            break;
                        case "2":
                            Console.WriteLine("Exception Code: " + ExceptionCode.Trim() + "----> Illegal data address!");
                            break;
                        case "3":
                            Console.WriteLine("Exception Code: " + ExceptionCode.Trim() + "----> Illegal data value!");
                            break;
                        case "4":
                            Console.WriteLine("Exception Code: " + ExceptionCode.Trim() + "----> Slave device failure!");
                            break;
                    }
                    /*
                       //Modbus exception codes definition                            
                       * Code   * Name                                      * Meaning
                         01       ILLEGAL FUNCTION                            The function code received in the query is not an allowable action for the server.
                         
                         02       ILLEGAL DATA ADDRESS                        The data addrdss received in the query is not an allowable address for the server.
                         
                         03       ILLEGAL DATA VALUE                          A value contained in the query data field is not an allowable value for the server.
                           
                         04       SLAVE DEVICE FAILURE                        An unrecoverable error occurred while the server attempting to perform the requested action.
                             
                         05       ACKNOWLEDGE                                 This response is returned to prevent a timeout error from occurring in the client (or master)
                                                                              when the server (or slave) needs a long duration of time to process accepted request.
                          
                         06       SLAVE DEVICE BUSY                           The server (or slave) is engaged in processing a long–duration program command , and the
                                                                              client (or master) should retransmit the message later when the server (or slave) is free.
                             
                         08       MEMORY PARITY ERROR                         The server (or slave) attempted to read record file, but detected a parity error in the memory.
                             
                         0A       GATEWAY PATH UNAVAILABLE                    The gateway is misconfigured or overloaded.
                             
                         0B       GATEWAY TARGET DEVICE FAILED TO RESPOND     No response was obtained from the target device. Usually means that the device is not present on the network.
                     */
                }
            }



            #endregion
        }

    }
}