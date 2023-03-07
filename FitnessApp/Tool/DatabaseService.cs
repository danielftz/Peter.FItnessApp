using FitnessApp.Models;
using SQLite;

namespace FitnessApp.Tool
{
    public class DatabaseService
    {
        private static SQLiteAsyncConnection _dbConnection;

        private static readonly string _dbPath = Path.Combine(FileSystem.AppDataDirectory, "MyFitnessAppDatabase.db");

        public static async Task OpenConnectionAsync()
        {
            if (_dbConnection == null)
            {
                _dbConnection = new SQLiteAsyncConnection(_dbPath);
               
            }
            await _dbConnection.CreateTableAsync<ExerciseDataObj>();
            await _dbConnection.CreateTableAsync<WorkoutDataObj>();

            
        }

        public static async Task CloseConnectionAsync()
        {
            if (_dbConnection != null)
            {
                await _dbConnection.CloseAsync();
            }
        }

        public DatabaseService()
        {
            if (_dbConnection == null)
            {
                _dbConnection = new SQLiteAsyncConnection(_dbPath);
                Task.Run(async () =>
                {
                    await _dbConnection.CreateTableAsync<ExerciseDataObj>();
                    await _dbConnection.CreateTableAsync<WorkoutDataObj>();
                });
            }
        }

        //CRUD for Exercise  (Create, Read, Update, Delete)
        public async Task<int> CreateExerciseIfNotExistAsync(params Exercise[] exercise)
        {
            int totalAdded = 0;
            foreach (Exercise e in exercise)
            {
                //make sure an exercise with the same name doesn't already exist
                if (await ReadExerciseAsync(e.Name) == null)
                {
                    await CreateExerciseAsync(e);
                    totalAdded += 1;
                }
            }

            return totalAdded;
        }

        public async Task<int> CreateExerciseAsync(Exercise exercise)
        {
            return await _dbConnection.InsertAsync(new ExerciseDataObj(exercise));
        }

        public async Task<Exercise> ReadExerciseAsync(string name)
        {
            ExerciseDataObj data = await _dbConnection.FindAsync<ExerciseDataObj>(name);
            if (data != null)
            {
                return new Exercise(data);
            }
            else
            {
                return null;
            }
        }

        public async Task<List<Exercise>> ReadAllExerciseAsync()
        {
            List<ExerciseDataObj> data = await _dbConnection.Table<ExerciseDataObj>().ToListAsync();
            
            List<Exercise> result= new List<Exercise>();
            foreach(ExerciseDataObj ex in data)
            {
                result.Add(new Exercise(ex));
            }
            return result;
        }

        public async Task<int> UpdateOrInsertExerciseAsync(Exercise exercise)
        {
            ExerciseDataObj data = await _dbConnection.FindAsync<ExerciseDataObj>(exercise.Name);
            if (data == null)
            {
                return await CreateExerciseAsync(exercise);
            }
            else
            {
                return await _dbConnection.UpdateAsync(new ExerciseDataObj(exercise));
            }
        }

        public async Task<int> DeleteExerciseAsync(string name)
        {
            return await _dbConnection.DeleteAsync<ExerciseDataObj>(name);
        }

        
        public async Task<int> CreateWorkOutIfNotExistAsync(params Workout[] workout)
        {
            int totalAdded = 0;
            foreach (Workout e in workout)
            {
                //make sure an exercise with the same name doesn't already exist
                if (await ReadWorkoutAsync(e.Name) == null)
                {
                    await CreateWorkoutAsync(e);
                    totalAdded += 1;
                }
            }

            return totalAdded;
        }

        public async Task<int> CreateWorkoutAsync(Workout workout)
        {
            return await _dbConnection.InsertAsync(new WorkoutDataObj(workout));
        }

        public async Task<Workout> ReadWorkoutAsync(string name)
        {
            WorkoutDataObj data = await _dbConnection.FindAsync<WorkoutDataObj>(name);
            if (data != null)
            {
                return new Workout(data);
            }
            else
            {
                return null;
            }
        }

        public async Task<List<Workout>> ReadAllWorkoutAsync()
        {
            List<WorkoutDataObj> data = await _dbConnection.Table<WorkoutDataObj>().ToListAsync();

            List<Workout> result = new List<Workout>();
            foreach (WorkoutDataObj ex in data)
            {
                result.Add(new Workout(ex));
            }
            return result;
        }

        public async Task<int> UpdateOrInsertWorkoutAsync(Workout workout)
        {
            WorkoutDataObj data = await _dbConnection.FindAsync<WorkoutDataObj>(workout.Name);
            if (data == null)
            {
                return await CreateWorkoutAsync(workout);
            }
            else
            {
                return await _dbConnection.UpdateAsync(new WorkoutDataObj(workout));
            }
        }

        public async Task<int> DeleteWorkoutAsync(string name)
        {
            return await _dbConnection.DeleteAsync<WorkoutDataObj>(name);
        }

    }
}
