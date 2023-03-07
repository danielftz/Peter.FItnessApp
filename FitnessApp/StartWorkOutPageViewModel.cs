using CommunityToolkit.Maui.Views;
using FitnessApp.Components;
using FitnessApp.Models;
using FitnessApp.Tool;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace FitnessApp
{
    public class StartWorkOutPageViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }


        private ObservableCollection<Exercise> _exercise = new();
        public ObservableCollection<Exercise> Exercise
        {
            get => _exercise;
            set
            {
                if (value != _exercise)
                {
                    _exercise = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Exercise)));
                }
            }
        }

        private DatabaseService _service = new();

        private Page _page;

        public ICommand StartTimerCommand { get; }

        public ICommand AllDoneCommand { get; }



        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Workout workout = query["workout"] as Workout;

            Name = workout.Name;

            foreach (string exercise in workout.ExerciseList)
            {
                Exercise e = await _service.ReadExerciseAsync(exercise);

                Exercise.Add(e);
            }
        }

        public StartWorkOutPageViewModel(Page page)
        {
            _page = page;

            StartTimerCommand = new Command<TimeSpan>((TimeSpan timeSpan) =>
            {
                TimerPopUp timer = new()
                {
                    StartTime = timeSpan,
                    CanBeDismissedByTappingOutsideOfPopup = true
                };
                

                _page.ShowPopup(timer);

                timer.Closed += (s, e) =>
                {
                    Alarm.Instance.Stop();
                };

                timer.TimerView.Start();

            });
        }
    }
}
