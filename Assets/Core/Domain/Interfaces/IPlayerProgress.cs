namespace Navi.Core.Interfaces
{
    public interface IPlayerProgress
    {
        bool HasSeenIntro { get; set; }
        void ResetIntro();
        void Save();
        void Load();
    }
}
