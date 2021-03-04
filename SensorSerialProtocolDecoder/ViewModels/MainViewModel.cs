using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SensorSerialProtocolDecoder.Base;
using SensorSerialProtocolDecoder.Model;
using SensorSerialProtocolDecoder.Tools;

namespace SensorSerialProtocolDecoder.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        new SensorModel sensorModel = new SensorModel();
        int i = 0;

        public MainViewModel()
        {
            ComPorts = new ObservableCollection<COMPortModel>()
            {
                new COMPortModel(){PortId = 1, PortBaudRate = "COM1"}
            };
        }

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
                return _startListening;
            }
            set
            {
                SetValue(ref _startListening, value);
            }
        }

        private ICommand _saveToFile;
        public ICommand SaveToFile
        {
            get
            {
                return _saveToFile;
            }
            set
            {
                SetValue(ref _saveToFile, value);
            }
        }

        private ICommand _openPort;
        public ICommand OpenPort
        {
            get
            {
                return _openPort;
            }
            set
            {
                SetValue(ref _openPort, value);
            }
        }

        private ICommand _closePort;
        public ICommand ClosePort
        {
            get
            {
                return _closePort;
            }
            set
            {
                SetValue(ref _closePort, value);
            }
        }
        #endregion Buttons

        #region ComPorts
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
        private COMPortModel _selectedComPort;
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

        #endregion ComPorts

        public void test()
        {
            int i = 1;
        }
    }
}
