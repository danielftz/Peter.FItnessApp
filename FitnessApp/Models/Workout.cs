using Newtonsoft.Json;
using SQLite;

namespace FitnessApp.Models
{

    public class Workout //Domain class
    {
        public string Name { get; set; }

        public IList<string> ExerciseList { get; set; }

        public IList<TargetableParts> TargetingParts { get; set; }

        public Workout()
        {
        }

        public Workout(WorkoutDataObj data)
        {
            Name = data.Name;
            ExerciseList = JsonConvert.DeserializeObject<List<string>>(data.ExerciseListJson);
            TargetingParts = JsonConvert.DeserializeObject<List<TargetableParts>>(data.TargetingPartsJson);
        }
    }

    public class WorkoutDataObj
    {
        [PrimaryKey, Column("_id")]
        public string Name { get; set; }
        public string ExerciseListJson { get; set; }
        public string TargetingPartsJson { get; set; }
        public WorkoutDataObj() 
        { 
        }

        public WorkoutDataObj(Workout workout)
        {
            Name = workout.Name;
            ExerciseListJson = JsonConvert.SerializeObject(workout.ExerciseList);
            TargetingPartsJson = JsonConvert.SerializeObject(workout.TargetingParts);
        }
    }


}
