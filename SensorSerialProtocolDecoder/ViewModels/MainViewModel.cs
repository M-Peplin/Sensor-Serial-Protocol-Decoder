using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SensorSerialProtocolDecoder.Base;
using SensorSerialProtocolDecoder.Model;
using SensorSerialProtocolDecoder.Services;
using SensorSerialProtocolDecoder.Interfaces;
using System.IO.Ports;

namespace SensorSerialProtocolDecoder.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly ICOMPortService _comPortService;
        private readonly IDecodeService _decodeService;
        private readonly ISendStatusService _sendStatusService;
        new SensorModel sensorModel = new SensorModel();                

        public MainViewModel(ICOMPortService comPortService,
            IDecodeService decodeService,
            ISendStatusService sendStatusService)
        {
            this._comPortService = comPortService;
            this._decodeService = decodeService;
            this._sendStatusService = sendStatusService;

            //Get avalible port names and add to ComPort list of strings
            ComPorts = new ObservableCollection<String>();            
            foreach(var port in SerialPort.GetPortNames())
            {                
                ComPorts.Add(port); 
            }
            //Auto-select first avalible port on list
            SelectedComPort = ComPorts.FirstOrDefault();
            SelectedComPort2 = ComPorts.FirstOrDefault();
            //add avalible baudrate options
            BaudRates = new ObservableCollection<string>()
            {
                "115200",
                "9600",
                "38400",
                "4800",
                "921600",
            };
            //auto-select first of baud rates
            SelectedBaudRate = BaudRates.First();
            SelectedBaudRate2 = BaudRates.First();
        }

        private SerialPort mySerialPort;
        private SerialPort mySerialPort2;
        //Func<SerialPort, string> setPortStatus; 
        //Action<string> setPortStatus;

        #region Buttons
        private ICommand _echoRangeOn;
        public ICommand EchoRangeOn
        {
            get
            {
                if(_echoRangeOn == null)
                {
                    _echoRangeOn = new RelayCommand(
                        param => sensorModel.test(),
                        param => true); 
                }
                return _echoRangeOn;                
            }
            set
            {
                SetValue(ref _echoRangeOn, value);
            }
        }

        private ICommand _echoRangeOff;
        public ICommand EchoRangeOff
        {
            get
            {
                return _echoRangeOff;
            }
            set
            {
                SetValue(ref _echoRangeOff, value);
            }
        }

        private ICommand _displaySettings;
        public ICommand DisplaySettings
        {
            get
            {
                return _displaySettings;
            }
            set
            {
                SetValue(ref _displaySettings, value);
            }
        }

        private ICommand _setRange;
        public ICommand SetRange
        {
            get
            {
                return _setRange;
            }
            set
            {
                SetValue(ref _setRange, value);
            }
        }
        
        private ICommand _sendBtn;
        public ICommand SendBtn
        {
            get
            {
                return _sendBtn;
            }
            set
            {
                SetValue(ref _sendBtn, value);
            }
        }

        private ICommand _startListening;
        public ICommand StartListening
        {
            get
            {
                if(_startListening == null)
                {
                    _startListening = new RelayCommand(
                        //param => _comPortService.ReadMessage(mySerialPort2, value => PortMessage = value),
                        param => _comPortService.ReadMessages(mySerialPort, mySerialPort2, value => Port1Message = value, value => Port2Message = value),
                        param => true);
                }
                return _startListening;
            }            
        }

        private ICommand _saveToFile;
        public ICommand SaveToFile
        {
            get
            {
                if(_saveToFile == null)
                {
                    _saveToFile = new RelayCommand(
                        param => _decodeService.saveDataToFile(PortMessage),
                        param => true);
                }
                return _saveToFile;
            }           
        }

        private ICommand _openPort;
        public ICommand OpenPort
        {
            get
            {
                if(_openPort == null)
                {
                    _openPort = new RelayCommand(
                        param => mySerialPort = _comPortService.createSerialPort(SelectedBaudRate, SelectedComPort, value => PortStatus = value),
                        param => true);                        
                }
                return _openPort;
            }            
        }       

        private ICommand _closePort;
        public ICommand ClosePort
        {
            get
            {
                if (_closePort == null)
                {
                    _closePort = new RelayCommand(
                        param => _comPortService.closeSerialPort(mySerialPort, value => PortStatus = value),
                        param => true);
                }
                return _closePort;
            }
            set
            {
                SetValue(ref _closePort, value);
            }
        }

        // port 2 

        private ICommand _openPort2;
        public ICommand OpenPort2
        {
            get
            {
                if (_openPort2 == null)
                {
                    _openPort2 = new RelayCommand(
                        param => mySerialPort2 = _comPortService.createSerialPort(SelectedBaudRate2, SelectedComPort2, value => PortStatus2 = value),
                        param => true);
                }
                return _openPort2;
            }
        }

        private ICommand _closePort2;
        public ICommand ClosePort2
        {
            get
            {
                if (_closePort2 == null)
                {
                    _closePort2 = new RelayCommand(
                        param => _comPortService.closeSerialPort(mySerialPort2, value => PortStatus2 = value),
                        param => true);
                }
                return _closePort2;
            }
            set
            {
                SetValue(ref _closePort2, value);
            }
        }

        private ICommand _combinedMessageBtn;
        public ICommand CombinedMessageBtn
        {
            get
            {
                if (_combinedMessageBtn == null)
                {
                    _combinedMessageBtn = new RelayCommand(
                        param => PortMessage = _decodeService.showMessage(3, Port1Message, Port2Message),
                        param => true);
                }
                return _combinedMessageBtn;
            }
        }


        // sending test 
        /*
        private ICommand _testSend;
        public ICommand TestSend
        {
            get
            {
                if (_testSend == null)
                {
                    _testSend = new RelayCommand(
                        param => _comPortService.testSendMessage(mySerialPort),
                        param => true);
                }
                return _testSend;
            }
        }
        */

        #endregion Buttons

        #region ComPorts
        /*
        private ObservableCollection<COMPortModel> _comPorts;
        public ObservableCollection<COMPortModel> ComPorts
        {
            get
            {
                return _comPorts;
            }
            set
            {
                _comPorts = value;
            }
        }
        */

        private ObservableCollection<String> _comPorts;
        public ObservableCollection<String> ComPorts
        {
            get
            {
                return _comPorts;
            }
            set
            {
                _comPorts = value;
            }
        }
        /*
        private COMPortModel _selectedComPort = new COMPortModel() { PortId = 1, PortName = "COM1" };
        public COMPortModel SelectedComPort
        {
            get
            {
                return _selectedComPort;
            }
            set
            {
                _selectedComPort = value;
            }
        }
        */

        private string _selectedComPort;
        public string SelectedComPort
        {
            get
            {
                return _selectedComPort;
            }
            set
            {
                _selectedComPort = value;
            }
        }

        private string _selectedComPort2;
        public string SelectedComPort2
        {
            get
            {
                return _selectedComPort2;
            }
            set
            {
                _selectedComPort2 = value;
            }
        }

        private string _portStatus = "";

        public string PortStatus
        {
            get
            {
                return _portStatus;
            }
            set
            {
                SetValue(ref _portStatus, value);
            }
        }

        // second port
        private string _portStatus2 = "";

        public string PortStatus2
        {
            get
            {
                return _portStatus2;
            }
            set
            {
                SetValue(ref _portStatus2, value);
            }
        }


        private string _portMessage;
        public string PortMessage
        {
            get
            {
                return _portMessage;
            }
            set
            {
                SetValue(ref _portMessage, value);
            }
        }

        private string _port1Message;
        public string Port1Message
        {
            get
            {
                return _port1Message;
            }
            set
            {
                SetValue(ref _port1Message, value);
            }
        }

        private string _port2Message;
        public string Port2Message
        {
            get
            {
                return _port2Message;
            }
            set
            {
                SetValue(ref _port2Message, value);
            }
        }

        #endregion ComPorts

        #region BaudRates
        private ObservableCollection<string> _baudRates;

        public ObservableCollection<string> BaudRates
        {
            get
            {
                return _baudRates;
            }
            set
            {
                _baudRates = value;
            }
        }

        private string _selectedBaudRate;

        public string SelectedBaudRate
        {
            get
            {
                return _selectedBaudRate;
            }
            set
            {
                _selectedBaudRate = value;
            }
        }

        // port 2 
        private string _selectedBaudRate2;

        public string SelectedBaudRate2
        {
            get
            {
                return _selectedBaudRate2;
            }
            set
            {
                _selectedBaudRate2 = value;
            }
        }
        #endregion BaudRates

        private string _serialProtocolMessage = "test";

        public string SerialProtocolMessage
        {
            get
            {
                return _serialProtocolMessage;
            }
            set
            {
                SetValue(ref _serialProtocolMessage, value);
            }
        }
    }
}
