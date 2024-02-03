using GalaSoft.MvvmLight.Command;
using SilencePauseApp.Model;
using SilencePauseApp.Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace SilencePauseApp.ViewModel
{
    internal class PausesViewModel : BaseViewModel
    {
        private Pauses _pauses;
        private string _action;

        private string _date = DateTime.Now.ToShortDateString();

        public Pauses Pauses
        {
            get => _pauses;
            set
            {
                _pauses = value;
                OnProperty("Pauses");
            }
        }

        public string Action
        {
            get => _action;
            set
            {
                _action = value;
                OnProperty("");
            }
        }

        public string Date
        {
            get => _date;
            set
            {
                _date = value;
                OnProperty("Start");
            }
        }

        public PausesViewModel(string action, DataModel data, Pauses pauses = null)
        {
            Action = action; 
            Data = data;

            if(pauses == null)
            {
                Pauses = new Pauses();
            }
            else
            {
                Pauses = pauses;
                Date = pauses.DateEvent;
            }
        }

        private void Execute()
        {

            try
            {
                DateTime.Parse(Date);
            }
            catch
            {
                Message("Некорректная дат");
                return;
            }

            if(Action == "Добавить")
            {
                Pauses.Id = Data.Pauses.Count() == 0 ? 1 : Data.Pauses.Last().Id + 1;
                Pauses.DateEvent = Date;

                Data.Pauses.Add(Pauses);
                Data.ChangeDB.AddAll(Pauses);

                Close();
            }    
            else
            {
                Pauses.DateEvent = Date;

                Data.ChangeDB.UpdateAll(Pauses);
                Close();
            }
        }

        public RelayCommand ExecuteCommand => new RelayCommand(Execute);
    }
}
