using Navi.Core.Interfaces;
using Navi.Presentation.Views.Debug;
using VContainer.Unity;

namespace Navi.Presentation.Controllers
{
    public sealed class DevDebugController : IStartable
    {
        private readonly DevDebugView _view;
        private readonly IPlayerProgress _progress;

        public DevDebugController(DevDebugView view, IPlayerProgress progress)
        {
            _view = view;
            _progress = progress;
        }

        public void Start()
        {
            _view.ResetIntroClicked += OnResetIntro;
        }

        private void OnResetIntro()
        {
            UnityEngine.Debug.Log("Intro reset.");
            _progress.ResetIntro();
        }
    }
}
