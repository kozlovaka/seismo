using SilencePauseApp.Model;
using SilencePauseApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SilencePauseApp.View
{
    /// <summary>
    /// Логика взаимодействия для PausesView.xaml
    /// </summary>
    public partial class PausesView : Window
    {
        public PausesView(string action, DataModel data, Pauses pauses = null)
        {
            InitializeComponent();
            PausesViewModel model = new PausesViewModel(action, data, pauses);
            this.DataContext = model;

            if (model.Close == null)
                model.Close = new Action(this.Close);
        }
    }
}
