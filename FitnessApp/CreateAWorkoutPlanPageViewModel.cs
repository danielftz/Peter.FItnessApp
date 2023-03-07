using FitnessApp.Models;
using FitnessApp.Tool;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace FitnessApp
{
    public class CreateAWorkoutPlanPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _workoutName;
        public string WorkoutName
        {
            get => _workoutName;
            set
            {
                if (value != _workoutName)
                {
                    _workoutName= value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WorkoutName)));
                }
            }
        }

        private ObservableCollection<Exercise> _selectedExercise;
        public ObservableCollection<Exercise> SelectedExercise
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

        public ICommand SelectExerciseCommand { get; set; }
        public ICommand SaveWorkoutCommand { get; set; }

        public CreateAWorkoutPlanPageViewModel()
        {
            DatabaseService service= new DatabaseService();
            SelectedExercise = new();
            
            SelectExerciseCommand = new Command(async () =>
            {
                //Validate that the workout name does not exist in the database
                if (String.IsNullOrEmpty(WorkoutName))
                {
                    await App.Current.MainPage.DisplayAlert("Error", "You must enter a name", "OK");
                    return;
                }

                Workout e = await service.ReadWorkoutAsync(WorkoutName);
                if (e is not null)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "This exercise already exists. Please try another name", "OK");
                    return;
                }



                Dictionary<string, object> param = new()
                {
                    { "nameOfTheWorkout", WorkoutName },
                    { "currentlySelectedExercise", SelectedExercise }
                };
                await Shell.Current.GoToAsync(nameof(ListOfExercisePage), param);
            });


            SaveWorkoutCommand = new Command(async() =>
            {
                List<string> list = new List<string>();
                List<TargetableParts> targetingParts = new();
                foreach (Exercise e in SelectedExercise)
                {
                    //TODO: modify PartOfWorkout
                    foreach (TargetableParts t in e.TargetingParts)
                    {
                        if (targetingParts.Contains(t) is not true)
                        {
                            targetingParts.Add(t);
                        }
                    }

                    
                    list.Add(e.Name);
                }

                


                Workout w = new Workout
                {
                    Name = WorkoutName,
                    ExerciseList = list,
                    TargetingParts = targetingParts
                };


                await service.CreateWorkoutAsync(w);

                await Shell.Current.GoToAsync("..");
            });
        }

    }
}
