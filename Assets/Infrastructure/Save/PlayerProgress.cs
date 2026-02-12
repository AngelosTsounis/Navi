using Navi.Core.Interfaces;
using UnityEngine;

namespace Navi.Infrastructure.Save
{
    public sealed class PlayerProgress : IPlayerProgress
    {
        private const string HasSeenIntroKey = "navi_has_seen_intro";

        public bool HasSeenIntro { get; set; }


        public void Load()
        {
            HasSeenIntro = PlayerPrefs.GetInt(HasSeenIntroKey, 0) == 1;
        }

        public void Save()
        {
            PlayerPrefs.SetInt(HasSeenIntroKey, HasSeenIntro ? 1 : 0);
            PlayerPrefs.Save();
        }

        // For testing purposes, allows resetting the intro seen state.
        public void ResetIntro()
        {
            HasSeenIntro = false;
            Save();
        }
    }
}
