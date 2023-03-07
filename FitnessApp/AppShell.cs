namespace FitnessApp
{
    public class AppShell : Shell
    {
        public AppShell()
        {
            Shell.SetBackgroundColor(this, Palette.Primary);
            Shell.SetTabBarBackgroundColor(this, Palette.Primary);
            Shell.SetTabBarTitleColor(this, Palette.Secondary);
            Shell.SetTabBarUnselectedColor(this, Colors.White);
            Items.Add(new TabBar()
            {
                Items =
                {
                    new Tab
                    {
                        Title = "Home",
                        Icon = "icon_home",
                        Items =
                        {
                            new ShellContent
                            {
                                Title = "Home",
                                Route = nameof(HomePage),
                                ContentTemplate = new DataTemplate(() =>
                                {
                                    return new HomePage();
                                })
                            }
                        }
                    },

                    new Tab
                    {
                        Title = "WorkOut",
                        Icon = "icon_workout",
                        Items=
                        {
                            new ShellContent
                            {
                                Title = "WorkOut",
                                Route = nameof(WorkOutPage),
                                ContentTemplate = new DataTemplate(() =>
                                {
                                    return new WorkOutPage();
                                })
                            }
                        }
                    }
                }
            });

            Routing.RegisterRoute(nameof(CreateAWorkoutPlanPage), typeof(CreateAWorkoutPlanPage));
            Routing.RegisterRoute(nameof(CreateAnExercisePage), typeof(CreateAnExercisePage));
            Routing.RegisterRoute(nameof(ListOfExercisePage), typeof(ListOfExercisePage));
            Routing.RegisterRoute(nameof(StartWorkOutPage), typeof(StartWorkOutPage));
        }
    }
}
