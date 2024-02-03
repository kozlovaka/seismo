using SilencePauseApp.Model.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilencePauseApp.Model
{
    public class DataModel
    {
        public ChangeDB ChangeDB;

        public ObservableCollection<Pauses> Pauses { get; set; }

        public DataModel()
        {
            ChangeDB = new ChangeDB();
            Pauses = ChangeDB.GetPauses();
        }

    }
}
