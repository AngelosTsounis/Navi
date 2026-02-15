using Navi.Core.Domain;
using Navi.Core.Interfaces;
using Navi.Infrastructure.Save;
using Navi.Infrastructure.Content;
using Navi.Presentation.Controllers;
using Navi.Presentation.Navigation;
using Navi.Presentation.Views.Debug;
using Navi.Presentation.Views.Intro;
using Navi.Presentation.Views.MainMenu;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        // Core
        builder.Register<PuzzleFactory>(Lifetime.Singleton);

        // Save
        builder.Register<IPlayerProgress, PlayerProgress>(Lifetime.Singleton);
        builder.Register<IPuzzleCatalog, InMemoryPuzzleCatalog>(Lifetime.Singleton);

        // Scene components
        builder.RegisterComponentInHierarchy<ScreenRegistry>();
        builder.RegisterComponentInHierarchy<IntroView>();
        builder.RegisterComponentInHierarchy<MainMenuView>();
        builder.RegisterComponentInHierarchy<PuzzleView>();
#if UNITY_EDITOR
        builder.RegisterComponentInHierarchy<DevDebugView>();
#endif
        // Navigation
        builder.Register<ScreenNavigator>(Lifetime.Singleton);

        // Entry points
        builder.RegisterEntryPoint<AppStartController>();
        builder.RegisterEntryPoint<IntroController>();
        builder.RegisterEntryPoint<TutorialPuzzleController>();
#if UNITY_EDITOR
        builder.RegisterEntryPoint<DevDebugController>();
#endif
    }
}
