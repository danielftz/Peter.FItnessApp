using System.ComponentModel;
using System.Windows.Input;

namespace FitnessApp
{
    public class HomePageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _dayCounter;
        public int DayCounter
        {
            get
            {
                return _dayCounter;
            }

            set
            {
                if (value != _dayCounter)
                {
                    _dayCounter = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DayCounter)));
                }
            }
        }

        private DateTime _lastCheckedIn;

        public DateTime LastCheckedIn
        {
            get => _lastCheckedIn;
            set
            {
                if (value != _lastCheckedIn)
                {
                    _lastCheckedIn = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LastCheckedIn)));
                }
            }
        }


        public ICommand DayIncreaseCommand { get; set; }

        public HomePageViewModel()
        {

            LastCheckedIn = Preferences.Default.Get("LastCheckedInTime", DateTime.MinValue);
            if ((DateTime.Now - LastCheckedIn) > TimeSpan.FromHours(24))
            {
                DayCounter = 0;
                Preferences.Default.Set("DayCounter", 0);
            }
            else
            {
                DayCounter = Preferences.Default.Get("DayCounter", 0);
            }


            DayIncreaseCommand = new Command(async () =>
            {
                if (DateTime.Now.Date == LastCheckedIn.Date)
                {
                    await App.Current.MainPage.DisplayAlert("You have already checked in today", "", "ok");
                }
                else if (DateTime.Now.Date > LastCheckedIn.Date)
                {
                    DayCounter += 1;
                    LastCheckedIn = DateTime.Now;
                    Preferences.Default.Set("DayCounter", DayCounter);
                    Preferences.Default.Set("LastCheckedInTime", LastCheckedIn);
                }
            });


        }
    }
}

