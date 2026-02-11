using Navi.Core.Domain;
using Navi.Presentation.Controllers;
using Navi.Presentation.Navigation;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        // Core
        builder.Register<PuzzleFactory>(Lifetime.Singleton);

        // Scene components
        builder.RegisterComponentInHierarchy<ScreenRegistry>();
        builder.RegisterComponentInHierarchy<PuzzleView>();

        // Navigation
        builder.Register<ScreenNavigator>(Lifetime.Singleton);

        // Entry points
        //builder.RegisterEntryPoint<AppStartController>();
        builder.RegisterEntryPoint<AppStartController>();
        builder.RegisterEntryPoint<TutorialPuzzleController>();
    }
}
