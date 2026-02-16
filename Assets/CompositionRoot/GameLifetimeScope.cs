using Navi.Core.Domain;
using Navi.Core.Interfaces;
using Navi.Infrastructure.DataDefinitions;
using Navi.Infrastructure.Save;
using Navi.Presentation.Controllers;
using Navi.Presentation.Navigation;
using Navi.Presentation.Views.Debug;
using Navi.Presentation.Views.Intro;
using Navi.Presentation.Views.MainMenu;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] 
    private PuzzleCatalogSO puzzleCatalog;
     
    protected override void Configure(IContainerBuilder builder)
    {
        Debug.Log("GameLifetimeScope.Configure called");

        // Core
        builder.Register<PuzzleFactory>(Lifetime.Singleton);
        builder.Register<GameSession>(Lifetime.Singleton);


        // Save
        builder.Register<IPlayerProgress, PlayerProgress>(Lifetime.Singleton);
        if (puzzleCatalog == null)
            throw new InvalidOperationException("GameLifetimeScope: Puzzle Catalog is NOT assigned in the Inspector.");
        builder.RegisterInstance(puzzleCatalog);

        // Content
        builder.Register<IPuzzleCatalog, PuzzleCatalog>(Lifetime.Singleton);
        

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
        builder.Register<TutorialPuzzleController>(Lifetime.Singleton);
        builder.RegisterEntryPoint<TutorialPuzzleController>();
        
#if UNITY_EDITOR
        builder.RegisterEntryPoint<DevDebugController>();
#endif
    }
}
