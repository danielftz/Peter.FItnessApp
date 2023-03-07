using Newtonsoft.Json;
using SQLite;
using System.Collections.ObjectModel;

namespace FitnessApp.Models
{
    public enum TargetableParts
    {
        Chest,
        Shoulders,
        Biceps,
        Triceps,
        Forearms,
        UpperBack,
        LowerBack,
        Core,
        Quads,
        Glutes,
        Calves
    }


    public class Exercise //Domain class
    {
        public string Name { get; set; }
        public bool IsWeighted { get; set; }
        public bool IsTimed { get; set; }
        public int Repetitions { get; set; } = 1;
        public int Sets { get; set; } = 1;
        public TimeSpan TimePerSet { get; set; }
        public TimeSpan RestPeriod { get; set; }

        public List<string> PartOfWorkout { get; set; } = new();

        public List<TargetableParts> TargetingParts { get; set; } = new();

        public Exercise()
        {
        }

        public Exercise(ExerciseDataObj data)
        {
            Name= data.Name;
            IsWeighted= data.IsWeighted;
            IsTimed= data.IsTimed;
            Repetitions= data.Repetitions;
            Sets = data.Sets;
            TimePerSet = data.TimePerSet;
            RestPeriod = data.RestPeriod;
            PartOfWorkout = JsonConvert.DeserializeObject<List<string>>(data.PartOfWorkoutJson);
            TargetingParts = JsonConvert.DeserializeObject<List<TargetableParts>>(data.TargetingPartsJson);
        }


        public static readonly ReadOnlyCollection<string> AllTargetableParts = new(new List<string>
        {
            "Chest",
            "Shoulders",
            "Biceps",
            "Triceps",
            "Forearms",
            "Upper back",
            "Lower back",
            "Core",
            "Quads",
            "Glutes",
            "Calves"
        });
    }


    public class ExerciseDataObj
    {
        [PrimaryKey, Column("_id")]
        public string Name { get; set; }
        public bool IsWeighted { get; set; }
        public bool IsTimed { get; set; }
        public int Repetitions { get; set; }
        public int Sets { get; set; }
        public TimeSpan TimePerSet { get; set; }
        public TimeSpan RestPeriod { get; set; }

        public string PartOfWorkoutJson { get; set; }

        public string TargetingPartsJson { get; set; }

        public ExerciseDataObj()
        {
        }

        public ExerciseDataObj(Exercise exercise)
        {
            Name = exercise.Name;
            IsWeighted = exercise.IsWeighted;
            IsTimed = exercise.IsTimed;
            Repetitions= exercise.Repetitions;
            Sets= exercise.Sets;
            TimePerSet= exercise.TimePerSet;
            RestPeriod = exercise.RestPeriod;
            PartOfWorkoutJson = JsonConvert.SerializeObject(exercise.PartOfWorkout);
            TargetingPartsJson = JsonConvert.SerializeObject(exercise.TargetingParts);
        }
    }
}
