using FitnessApp.Models;
using FitnessApp.Tool;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace FitnessApp
{
    public class CreateAnExercisePageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isTimed;
        public bool IsTimed
        {
            get => _isTimed;
            set
            {
                if (value != _isTimed)
                {
                    _isTimed = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsTimed)));
                }
            }
        }


        private Exercise _currentExercise;
        public Exercise CurrentExercise
        {
            get => _currentExercise;
            set
            {
                if (value != _currentExercise)
                {
                    _currentExercise = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentExercise)));
                }
            }
        }

        private string _timePerSetMinutes;
        public string TimePerSetMinutes
        {
            get => _timePerSetMinutes;
            set
            {
                if (value != _timePerSetMinutes)
                {
                    _timePerSetMinutes = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TimePerSetMinutes)));
                }
            }
        }

        private string _timePerSetSeconds;
        public string TimePerSetSeconds
        {
            get => _timePerSetSeconds;
            set
            {
                if (value != _timePerSetSeconds)
                {
                    _timePerSetSeconds = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TimePerSetSeconds)));
                }
            }
        }


        private string _restMinutes;
        public string RestMinutes
        {
            get => _restMinutes;
            set
            {
                if (value != _restMinutes)
                {
                    _restMinutes = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs( nameof(RestMinutes)));
                }
            }
        }

        private string _restSeconds;
        public string RestSeconds
        {
            get => _restSeconds;
            set
            {
                if (value != _restSeconds)
                {
                    _restSeconds = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RestSeconds)));
                }
            }
        }

        private ObservableCollection<object> _targetingParts;
        public ObservableCollection<object> TargetingParts
        {
            get => _targetingParts;
            set
            {
                if (value != _targetingParts)
                {
                    _targetingParts = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TargetingParts)));
                }
            }
        }

        public ICommand SaveCommand { get; set; }

        public ICommand CancelCommand { get; set; }
        public CreateAnExercisePageViewModel(Exercise exercise = null)
        {
            DatabaseService service = new();
            if (exercise == null)
            {
                CurrentExercise = new Exercise();
                RestMinutes = "0";
                RestSeconds = "0";
                TimePerSetMinutes = "0";
                TimePerSetSeconds = "0";
                TargetingParts = new ObservableCollection<object>();
            }
            else
            {
                CurrentExercise = exercise;

                IsTimed = exercise.IsTimed;
                RestMinutes = CurrentExercise.RestPeriod.Minutes.ToString(); //TimeSpan.FromMinutes(CurrentExercise.RestPeriod.Minutes).Minutes.ToString();
                RestSeconds = CurrentExercise.RestPeriod.Seconds.ToString(); //TimeSpan.FromSeconds(CurrentExercise.RestPeriod.Seconds).Seconds.ToString();

                TimePerSetMinutes = CurrentExercise.TimePerSet.Minutes.ToString(); //TimeSpan.FromMinutes(CurrentExercise.TimePerSet.Minutes).Minutes.ToString();
                TimePerSetSeconds = CurrentExercise.TimePerSet.Seconds.ToString(); //TimeSpan.FromSeconds(CurrentExercise.TimePerSet.Seconds).Seconds.ToString();

                TargetingParts = new ObservableCollection<object>();
                foreach(TargetableParts p in exercise.TargetingParts)
                {
                    TargetingParts.Add(p);
                }
            }

            SaveCommand = new Command(async() =>
            {
                bool restMinConvertResult = int.TryParse(RestMinutes, out int restMin);

                bool restSecConvertResult = int.TryParse(RestSeconds, out int restSec);

                bool timePerSetMinConvertResult = int.TryParse(TimePerSetMinutes, out int timePerSetMin);

                bool timePerSetSecConvertResult = int.TryParse(TimePerSetSeconds, out int timePerSetSec);

                if (restMinConvertResult is true && restSecConvertResult is true && timePerSetMinConvertResult is true && timePerSetSecConvertResult is true)
                {
                    if (0 <= restSec  && restSec < 60)
                    {
                        //prompt user to save

                        bool result = await App.Current.MainPage.DisplayAlert("Confirmation", "Do you want to save this exercise?", "OK", "Cancel");
                        if (result is true)
                        {
                            //save
                            CurrentExercise.RestPeriod = TimeSpan.FromMinutes(restMin) + TimeSpan.FromSeconds(restSec);
                            CurrentExercise.TimePerSet = TimeSpan.FromMinutes(timePerSetMin) + TimeSpan.FromSeconds(timePerSetSec);
                            CurrentExercise.IsTimed = IsTimed;

                            foreach(object p in TargetingParts)
                            {
                                CurrentExercise.TargetingParts.Add((TargetableParts)p);
                            }

                            //save to database
                            int r = await service.CreateExerciseIfNotExistAsync(CurrentExercise);
                            if (r == 0)
                            {
                                await App.Current.MainPage.DisplayAlert("Error creating exercise", "Name already exists", "Ok");
                            }
                            else
                            {
                                await Shell.Current.GoToAsync("..");
                            }

                           
                        }
                        
                        return;

                    }
                }
                //indicate to user that the time input is not correct
                await App.Current.MainPage.DisplayAlert("Wrong Input", "Please make sure the rest period is correct", "OK");
            });

            CancelCommand = new Command(async () =>
            {
                await Shell.Current.GoToAsync("..");
            });
        }
    }
}
