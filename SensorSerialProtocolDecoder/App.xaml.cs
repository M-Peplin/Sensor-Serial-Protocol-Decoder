using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Ninject;
using SensorSerialProtocolDecoder.Interfaces;
using SensorSerialProtocolDecoder.Services;
using SensorSerialProtocolDecoder.Views;
using SensorSerialProtocolDecoder.ViewModels;

namespace SensorSerialProtocolDecoder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_Startup(object sender, StartupEventArgs e)
        {
            IKernel kernel = new StandardKernel();
            kernel.Bind<ICOMPortService>().To<COMPortService>();
            kernel.Bind<IDecodeService>().To<DecodeService>();
            kernel.Bind<ISendStatusService>().To<SendStatusService>();

            var applicationViewmodel = kernel.Get<MainViewModel>();


            MainView mainView = new MainView();
            mainView.DataContext = applicationViewmodel;
            mainView.Show();
        }
    }
}
