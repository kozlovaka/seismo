using GalaSoft.MvvmLight.Command;
using SilencePauseApp.Model;
using SilencePauseApp.Model.DB;
using SilencePauseApp.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilencePauseApp.ViewModel
{
    internal class MainViewModel : BaseViewModel
    {
        private string _dateStart { get; set; }
        private string _dateEnd { get; set; }
        private double _magnitudeStart { get; set; }
        private double _magnitudeEnd { get; set; }
        private int _step { get; set; }
        private int _eventWindow { get; set; }
        private int _eventCount { get; set; }
        private int _magnitude { get; set; }

        private string _find;

        private ObservableCollection<Pauses> _pauses;
        private ObservableCollection<Pauses> _findPauses;
        private Pauses _selectPauses;
        private ObservableCollection<PointGraf> _graf;
        private ObservableCollection<Pauses> _dataFromGraf;


        public ObservableCollection<Pauses> Pauses
        {
            get => _pauses;
            set
            {
                _pauses = value;
                OnProperty("Pauses");
            }
        }

        public ObservableCollection<Pauses> FindPauses
        {
            get => _findPauses;
            set
            {
                _findPauses = value;
                OnProperty("FindPauses");
            }
        }

        public Pauses SelectPauses
        {
            get => _selectPauses;
            set
            {
                _selectPauses = value;
                OnProperty("SelectPauses");
            }
        }

        public ObservableCollection<PointGraf> Grafs
        {
            get => _graf;
            set
            {
                _graf = value;
                OnProperty("Grafs");
            }
        }

        public ObservableCollection<Pauses> DataFromGraf
        {
            get => _dataFromGraf;
            set
            {
                _dataFromGraf = value;
                OnProperty("DataFromGraf");
            }
        }

        public string DateStart 
        {
            get => _dateStart;
            set
            {
                _dateStart = value;
                Calculate();
                OnProperty("DateStart");
            }
        }
        public string DateEnd
        {
            get => _dateEnd;
            set
            {
                _dateEnd = value;
                Calculate();
                OnProperty("DateEnd");
            }
        }
        public double MagnitudeStart 
        {
            get => _magnitudeStart;
            set
            {
                _magnitudeStart = value;
                Calculate();
                OnProperty("MagnitudeStart");
            }
        }
        public double MagnitudeEnd 
        {
            get => _magnitudeEnd;
            set
            {
                _magnitudeEnd = value;  
                Calculate();
                OnProperty("MagnitudeEnd");
            }
        }
        public int Step 
        {
            get => _step;
            set
            {
                _step = value;
                Calculate();
                OnProperty("Step");
            }
        }
        public int EventWindow
        {
            get => _eventWindow;
            set
            {
                _eventWindow = value;
                Calculate();
                OnProperty("EventWindow");
            }
        }
        public int EventCount
        {
            get => _eventCount;
            set
            {
                _eventCount = value;
                Calculate();
                OnProperty("EventCount");
            }
        }
        public int Magnitude 
        {
            get => _magnitude;
            set
            {
                _magnitude = value; 
                Calculate();
                OnProperty("Magnitude");
            }
        }

        public string Find
        {
            get => _find;
            set
            {
                _find = value;
                Finding();
                OnProperty("Find");
            }
        }

        public MainViewModel()
        {
            Data = new DataModel();

            Pauses = Data.Pauses;
            FindPauses = Pauses;

            EventWindow = 90;
            Step = 30;
            MagnitudeStart = 3;
            MagnitudeEnd = 5;
            DateStart = "1.01.2015";
            DateEnd = "1.11.2015";
            EventCount = 5;
            Magnitude = 0;
        }

        private void Calculate()
        {
            DateTime start;
            DateTime end;

            try
            {
                start = DateTime.Parse(DateStart);
                end = DateTime.Parse(DateEnd);  
            }
            catch
            {
                return;
            }


            Grafs = new ObservableCollection<PointGraf>();
            DataFromGraf = new ObservableCollection<Pauses>();

            List<Pauses> temp = Pauses.OrderBy(x => DateTime.Parse(x.DateEvent)).ToList()
                .Where(x => DateTime.Parse(x.DateEvent) >= start && DateTime.Parse(x.DateEvent) <= end
                && x.Ms >= MagnitudeStart && x.Ms <= MagnitudeEnd).ToList();

            if(temp.Count == 0)
            {
                return;
            }

            DateTime currentStart = start;
            DateTime currentEnd = currentStart.AddDays(EventWindow);

            int i = 1;

            while (currentEnd.AddDays(Step) < end)
            {
                List<Pauses> currentPauses = temp.Where(x => DateTime.Parse(x.DateEvent) >= currentStart && DateTime.Parse(x.DateEvent) <= currentEnd).ToList();
                List<Pauses> maxValue = new List<Pauses>();

                currentPauses.Select(x => x.DateEvent).ToList().Distinct().ToList()
                    .ForEach(x => maxValue.Add(currentPauses.Where(y => y.DateEvent == x).OrderByDescending(y => y.Ms).First()));


                double countDay = 0;

                maxValue = maxValue.OrderByDescending(x => x.Ms).ToList().Take(EventCount).ToList().OrderBy(x => DateTime.Parse(x.DateEvent)).ToList();

                maxValue.ForEach(x => DataFromGraf.Add(x));

                if (maxValue.Count > 1) 
                {
                    for (int j = 0; j < maxValue.Count() - 1; j++)
                    {
                        double sum = (DateTime.Parse(maxValue[j + 1].DateEvent) - DateTime.Parse(maxValue[j].DateEvent)).TotalDays;
                        countDay += sum;
                    }
                }

                Grafs.Add(
                    new PointGraf()
                    {
                        X = currentEnd.DayOfYear,
                        Y = countDay
                    });

                currentStart = currentStart.AddDays(Step);
                currentEnd = currentEnd.AddDays(Step);
                i++;
            }

            OnProperty("Grafs");
            OnProperty("DataFromGraf");
        }

        private void Add()
        {
            PausesView add = new PausesView("Добавить", Data);
            add.ShowDialog();

            Pauses = Data.Pauses;
            Finding();
        }

        private void Update()
        {
            if(SelectPauses == null)
            {
                Message("Не выбраны данные для обновления");
                return;
            }

            PausesView update = new PausesView("Обновить", Data, SelectPauses);
            update.ShowDialog();    

            Pauses = Data.Pauses;
            SelectPauses = null;
            Finding();
        }

        private void Remove()
        {
            if (SelectPauses == null)
            {
                Message("Не выбраны данные для удаления");
                return;
            }

            Data.ChangeDB.RemoveAll(SelectPauses);
            Data.Pauses.Remove(SelectPauses);          

            Pauses = Data.Pauses;
            Finding();
        }

        public void Finding()
        {
            FindPauses = new ObservableCollection<Pauses>(
                Pauses.Where(x => 
                x.DateEvent.ToString().Contains(Find) ||
                x.Hour.ToString().Contains(Find) ||
                x.Min.ToString().Contains(Find) ||
                x.Sec.ToString().Contains(Find) ||
                x.Latitude.ToString().Contains(Find) ||
                x.Longitude.ToString().Contains(Find) ||
                x.Depth.ToString().Contains(Find) ||
                x.DDep.ToString().Contains(Find) ||
                x.Ms.ToString().Contains(Find) ||
                x.Kl.ToString().Contains(Find))
                .ToList());
        }



        public RelayCommand AddCommand => new RelayCommand(Add);
        public RelayCommand UpdateCommand => new RelayCommand(Update);
        public RelayCommand RemoveCommand => new RelayCommand(Remove);
    }
}
