using FitnessApp.Models;
using FitnessApp.Tool;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace FitnessApp
{
    public class WorkOutPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Workout> _allWorkout;
        public ObservableCollection<Workout> AllWorkout
        {
            get => _allWorkout;
            set
            {
                if (value != _allWorkout)
                {
                    _allWorkout = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllWorkout)));
                }
            }
        }

        public ICommand RefreshAllWorkoutCommand { get; set; }

        public ICommand StartNowCommand { get; set; }

        public ICommand RemoveWorkoutCommand { get; set; }

        private readonly DatabaseService _service = new();
        public WorkOutPageViewModel()
        {
            LoadAlWorkout();

            RefreshAllWorkoutCommand = new Command<RefreshView>(async ( r ) =>
            {
                await LoadAlWorkout(); 
                r.IsRefreshing = false;
            });

            StartNowCommand = new Command<Workout>(async (w) =>
            {
                Dictionary<string, object> param = new()
                {
                    { "workout", w},
                };

                await Shell.Current.GoToAsync(nameof(StartWorkOutPage), param);
            });

            RemoveWorkoutCommand = new Command<Workout>(async (w) =>
            {
                bool result = await App.Current.MainPage.DisplayAlert("Confirm", "Are you sure you want to remove this workout?", "Ok", "Cancel");

                if (result is true)
                {
                    await _service.DeleteWorkoutAsync(w.Name);
                    AllWorkout.Remove(w);
                }

            });
        }

        public async Task LoadAlWorkout()
        {
            List<Workout> list = await _service.ReadAllWorkoutAsync();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                AllWorkout = new ObservableCollection<Workout>(list);
            });
        }
    }
}
