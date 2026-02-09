using Navi.Core.Domain;
using Navi.Presentation.Controllers;
using VContainer;
using VContainer.Unity;

public class PuzzlePrototypeLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        // Core (pure logic)
        builder.Register<PuzzleFactory>(Lifetime.Singleton);

        // Presentation
        builder.RegisterEntryPoint<TutorialPuzzleController>();
        builder.RegisterComponentInHierarchy<PuzzleView>(); 
    }
}
