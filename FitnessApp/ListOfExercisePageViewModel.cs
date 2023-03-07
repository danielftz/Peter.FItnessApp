using FitnessApp.Models;
using FitnessApp.Tool;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace FitnessApp
{
    public class ListOfExercisePageViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Exercise> _allExercise;
        public ObservableCollection<Exercise> AllExercise 
        {
            get => _allExercise;
            set
            {
                if (value != _allExercise)
                {
                    _allExercise = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllExercise)));
                }
            }
        }

        private ObservableCollection<object> _selectedExercise = new ();
        public ObservableCollection<object> SelectedExercise
        {
            get => _selectedExercise;
            set
            {
                if (value != _selectedExercise)
                {
                    _selectedExercise = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedExercise)));
                }
            }
        }

        public ICommand AddToWorkoutCommand { get; set; }

        public ICommand RefreshCommand { get; set; }

        private string _nameOfTheWorkout;

        private DatabaseService _service = new();

        private ObservableCollection<Exercise> _currentSelectedExercise;

        public ListOfExercisePageViewModel()
        {

            RefreshCommand = new Command<RefreshView>(async(r) =>
            {
                List<Exercise> allExercise = await _service.ReadAllExerciseAsync();
                AllExercise = new ObservableCollection<Exercise>(allExercise);

                r.IsRefreshing = false;
            });

            AddToWorkoutCommand = new Command(() =>
            {
                //remove all exercise originally a part of destinationExercise, but not any more
                for (int i = _currentSelectedExercise.Count - 1; i >= 0; i--)
                {
                    if (!SelectedExercise.Contains(_currentSelectedExercise[i]))
                    {
                        _currentSelectedExercise.RemoveAt(i);
                    }
                }

                //Add all exercise that is part of selectedExercise but not destinationExercise
                foreach (Exercise ex in SelectedExercise)
                {
                    if (!_currentSelectedExercise.Contains(ex))
                    {
                        _currentSelectedExercise.Add(ex);
                    }

                    if (!ex.PartOfWorkout.Contains(_nameOfTheWorkout))
                    {
                        ex.PartOfWorkout.Add(_nameOfTheWorkout);
                    }
                }

                Shell.Current.GoToAsync("..");

            });
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            _nameOfTheWorkout = query["nameOfTheWorkout"] as string;
            _currentSelectedExercise = query["currentlySelectedExercise"] as ObservableCollection<Exercise>;

            await _service.CreateExerciseIfNotExistAsync
            (
                new Exercise()
                {
                    Name = "Dumbbell Fly",
                    IsWeighted = true,
                    IsTimed = false,
                    Repetitions = 8,
                    Sets = 4,
                    RestPeriod = TimeSpan.FromMinutes(1.5),
                    TargetingParts =
                    {
                        TargetableParts.Chest,
                        TargetableParts.Shoulders
                    }
                },

                new Exercise()
                {
                    Name = "Squat",
                    IsWeighted = true,
                    IsTimed = false,
                    Repetitions = 8,
                    Sets = 4,
                    RestPeriod = TimeSpan.FromMinutes(2),
                    TargetingParts =
                    {
                        TargetableParts.Quads,
                        TargetableParts.Glutes,
                        TargetableParts.Calves,
                    }
                },
                new Exercise()
                {
                    Name = "Bicep Curl",
                    IsWeighted = true,
                    IsTimed = false,
                    Repetitions = 6,
                    Sets = 3,
                    RestPeriod = TimeSpan.FromMinutes(0.5),
                    TargetingParts =
                    {
                        TargetableParts.Biceps,
                        TargetableParts.Forearms
                    }
                },
                new Exercise()
                {
                    Name = "Pull Up",
                    IsWeighted = false,
                    IsTimed = false,
                    Repetitions = 8,
                    Sets = 4,
                    RestPeriod = TimeSpan.FromMinutes(1),
                    TargetingParts =
                    {
                        TargetableParts.UpperBack,
                        TargetableParts.Biceps,
                    }
                },

                new Exercise()
                {
                    Name = "Sit Up",
                    IsWeighted = false,
                    IsTimed = true,
                    Repetitions = 0,
                    Sets = 4,
                    TimePerSet = TimeSpan.FromMinutes(1),
                    RestPeriod = TimeSpan.FromMinutes(1),
                    TargetingParts =
                    {
                        TargetableParts.UpperBack,
                        TargetableParts.Biceps,
                    }
                },

                new Exercise()
                {
                    Name = "Deadlift",
                    IsWeighted = true,
                    IsTimed = false,
                    Repetitions = 8,
                    Sets = 4,
                    RestPeriod = TimeSpan.FromMinutes(1.5),
                    TargetingParts =
                    {
                        TargetableParts.LowerBack,
                        TargetableParts.Glutes
                    }
                }
            );


            List<Exercise> allExercise = await _service.ReadAllExerciseAsync();
            AllExercise = new ObservableCollection<Exercise>(allExercise);


            List<Exercise> selected = new();
            foreach (Exercise ex in _currentSelectedExercise)
            {
                Exercise e = AllExercise.Where(item => item.Name == ex.Name).FirstOrDefault();
                if (e is not null)
                {
                    selected.Add(e);
                }
            }

            SelectedExercise = new ObservableCollection<object>(selected);
        }
    }
}
