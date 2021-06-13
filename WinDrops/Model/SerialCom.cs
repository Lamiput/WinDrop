﻿using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading;


namespace WinDrops.Model
{
    public class SerialCom
    {
        private const int _BaudRate = 9600;
        private SerialPort _ComPort = new SerialPort();

        public enum BasicCMD
        {
            Info,
            Reset,
            Cancel
        }

        public int OpenPort(string PortName)
        {
            if (!PortName.StartsWith("COM"))
                return 1;
            if (_ComPort.IsOpen)
                return 2;
            _ComPort = new SerialPort(PortName, _BaudRate);
            _ComPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            _ComPort.Open();
            return 0;
        }

        public int ClosePort()
        {
            if (_ComPort.IsOpen)
                _ComPort.Close();
            return 0;
        }


        public int SendBasicCmd(BasicCMD basicCMD)
        {
            switch (basicCMD)
            {
                case BasicCMD.Info:
                    return WriteToPort("I\n");
                case BasicCMD.Reset:
                    return WriteToPort("X\n");
                case BasicCMD.Cancel:
                    return WriteToPort("C\n");
                default:
                    return -1;
            }
        }

        public int SendRunCmd(int passes = 0, int delay = 0)
        {
            string _cmd = "R";
            if (passes > 0)
            {
                _cmd += ";" + passes.ToString();
                if (delay > 0)
                    _cmd += ";" + (delay * 1000).ToString();
            }
            _cmd += "\n";
            return WriteToPort(_cmd);
        }

        public int SendTaskList(Device.EDevType devType, int pin, List<DropTask> dropTasks)
        {
            if (pin < 0)
                return 1;
            if (dropTasks is null)
                return 2;
            if (dropTasks.Count < 1)
                return 3;
            string _cmd = "S;" + pin.ToString();
            switch (devType)
            {
                case Device.EDevType.undef:
                    return 4;
                case Device.EDevType.Valve:
                    _cmd += ";V";
                    break;
                case Device.EDevType.Flash:
                    _cmd += ";F";
                    break;
                case Device.EDevType.Camera:
                    _cmd += ";C";
                    break;
                default:
                    return 5;
            }
            int _chksum = 0;
            foreach (var dropTask in dropTasks)
            {
                if (dropTask.StartTime < 0)
                    return 6;
                if (dropTask.Duration <= 0)
                    return 7;
                _chksum += dropTask.StartTime + dropTask.Duration;
                _cmd += ";" + dropTask.StartTime.ToString() + "|" + dropTask.Duration.ToString();
            }
            _cmd += "^" + _chksum.ToString() + "\n";
            int _retval = WriteToPort(_cmd);
            return _retval == 0 ? _retval : _retval + 10 ;
        }

        private int WriteToPort(string data)
        {
            if (!_ComPort.IsOpen)
                return 1;
            while (_ComPort.CtsHolding)
            {
                Thread.Sleep(100);
            }
            _ComPort.Write(data);
            return 0;
        }

        public event NewSerialReadingHandler NewSerialReading;
        public delegate void NewSerialReadingHandler(object sender, SerialReadingEventArgs e);

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string dataread = sp.ReadExisting();
            NewSerialReading?.Invoke(this, new SerialReadingEventArgs(dataread));
        }

        public static List<string> ListSerialPorts()
        {
            List<string> _PortList;
            try
            {
                _PortList = SerialPort.GetPortNames().ToList();
                if (_PortList.Count == 0)
                    _PortList.Add("no Ports found");
            }
            catch (Exception)
            {
                _PortList = new List<string> { "something went wrong while scanning" };
            }
            return _PortList;
        }
    }

    public class SerialReadingEventArgs
    {
        public SerialReadingEventArgs(string reading) { Reading = reading; }
        public string Reading { get; }
    }
}
