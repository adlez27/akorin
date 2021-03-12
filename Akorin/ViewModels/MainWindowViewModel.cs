using System;
using System.Collections.Generic;
using System.Text;

namespace Akorin.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public void Exit()
        {
            Environment.Exit(0);
        }
    }
}
