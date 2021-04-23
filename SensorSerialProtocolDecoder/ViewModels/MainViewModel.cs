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
            string data = "TS,   3008,    0,1,03,02,150, b0,05,22,31,15,38,11,3c,11,41,11,44,OFF0, 00,ff,ec,ae,a7,a2,b0,92,69,71,71,85,5f,57,6c,64,53,3f,60,5d,56,60,47,40,3d,4a,42,4b,3e,30,3c,1b,43,2a,25,2c,17,1e,25,24,10,23,34,1a,0e,17,0e,0f,15,0a,22,19,0f,1a,0b,0b,06,15,04,03,03,11,06,00,0d,00,11,03,03,11,05,07,05,01,05,03,02,0e,00,02,02,01,00,0a,05,01,02,02,01,02,00,05,00,01,02,09,01,0b,03,00,03,01,00,02,03,00,01,01,00,09,00,01,05,01,00,00,00,02,00,02,03,08,00,05,06,00,01,00,04,06,00,00,02,00,08,07,01,06,04,00,00,08,09,00,00,05,00,05,00,00,01,03,01,01,03,03,00,01,00,01,00,00,00,0a,08,00,00,01,00,06,03,00,01,01,01,01,03,00,00,03,04,00,02,00,0d,03,00,07,00,02,00,02,00,01,00,00,00,00,00,03,09,03,00,00,01,01,00,00,00,01,01,00,03,0d,00,00,00,01,00,01,00,01,01,03,01,01,04,01,01,02,05,03,00,03,00,00,07,01,0c,01,02,00,00,00,04,04,07,00,00,09,07,00,08,07,00,00,02,01,00,05,00,08,03,00,05,02,00,03,04,05,0d,09,00,00,03,01,02,03,01,00,02,00,00,02,00,03,00,00,01,01,00,04,03,00,01,00,03,01,05,02,01,09,04,01,00,00,10,00,01,05,00,00,00,00,01,00,01,00,00,00,01,00,0b,04,02,06,01,00,00,00,02,07,0b,04,01,00,08,03,00,01,02,02,02,00,00,02,00,01,00,00,00,00,00,00,00,05,00,00,02,00,00,00,00,01,00,00,01,01,01,07,04,02,00,04,00,01,00,06,00,01,01,05,00,00,03,07,06,00,01,01,01,03,00,01,00,00,05,06,00,02,00,00,04,01,01,00,03,00,03,05,04,09,00,03,01,06,02,00,04,01,04,01,03,00,01,01,00,00,01,00,06,01,00,00,02,00,00,00,03,02,05,00,00,00,00,00,06,00,02,0b,06,00,01,00,03,00,00,02,00,00,01,05,00,02,0a,00,00,04,00,03,00,04,05,02,04,01,00,00,00,06,06,03,00,03,02,00,00,01,04,00,06,00,00,01,00,00,01,0b,01,01,00,01,00,0a,00,00,00,00,01,03,02,00,04,03,01,00,01,00,04,07,00,00,00,01,00,07,01,02,00,01,00,04,00,00,00,04,01,03,03,01,04,02,01,00,00,02,02,00,05,04,00,01,00,00,08,00,00,02,05,02,07,02,01,01,01,0c,05,01,08,00,05,00,01,00,04,00,03,00,02,00,00,02,02,00,00,00,00,02,0f,00,01,00,02,00,01,00,00,00,00,00,02,02,04,0c,02,02,01,04,03,06,00,0a,03,07,05,00,00,00,07,01,03,00,01,00,01,02,00,00,05,09,00,02,02,02,00,00,01,06,00,03,01,00,00,01,00,02,00,00,04,00,03,06,02,00,00,01,00,00,00,03,05,02,04,02,01,05,0f,01,00,0f,02,00,02,00,00,00,01,00,02,00,00,01,03,00,00,00,01,05,00,00,00,00,00,01,01,02,00,00,07,02,02,00,01,00,02,02,01,02,03,02,0b,00,01,00,02,00,00,04,08,02,01,01,00,00,01,01,01,00,0a,0b,01,0b,0a,01,00,00,06,00,00,04,00,00,00,01,00,00,01,00,02,00,01,04,02,00,01,00,02,00,00,06,00,03,00,00,01,00,00,03,00,00,00,04,00,10,00,00,05,00,00,04,00,00,00,01,00,01,05,00,00,08,00,02,00,01,04,00,01,01,00,01,01,00,01,00,02,05,07,08,00,08,05,00,02,06,04,01,02,01,03,03,01,03,02,01,11,03,01,00,00,00,00,01,00,03,02,00,00,00,05,02,00,01,03,00,01,0d,06,00,05,02,01,00,03,01,01,07,03,00,00,00,04,03,01,00,0a,00,00,03,01,00,04,00,02,02,00,02,00,02,03,00,00,05,00,00,02,00,01,01,0d,05,00,04,02,08,04,04,00,01,00,05,ES,   3008";
            decodeService.Decode(data);
        }

        private SerialPort mySerialPort;
        private SerialPort mySerialPort2;
        private bool listeningIsOn = false;
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
                        //param => _comPortService.ReadMessage(mySerialPort, value => PortMessage = value),
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

        private ICommand _recordFile;
        public ICommand RecordFile
        {
            get
            {
                if(_recordFile == null)
                {
                    _recordFile = new RelayCommand(
                        p => _decodeService.recordDataToFile(PortMessage),
                        p => true);
                }
                return _recordFile;
            }
        }

        private bool _recordToFile;
        public bool RecordToFile
        {
            get
            {
                return _recordToFile;
            }
            set
            {
                SetValue(ref _recordToFile, value);
                if(listeningIsOn == true)
                {
                    //to be done - execute CombinedMessageBtn
                    CombinedMessageBtn.Execute(null);
                }
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
                        //param => PortMessage = _decodeService.showMessage(3, Port1Message, Port2Message),
                        //param => _decodeService.showMessages(3, Port1Message, Port2Message, value => PortMessage = value),
                        //param => _comPortService.ReadCombinedMessage(mySerialPort, mySerialPort2, value => Port1Message = value, value => Port2Message = value, value => PortMessage = value),
                        param => _comPortService.ReadCombinedMessage(mySerialPort, mySerialPort2, value => Port1Message = value,
                        value => Port2Message = value, value => PortMessage = value, RecordToFile),
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
