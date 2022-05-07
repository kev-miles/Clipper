using UnityEngine;

namespace ScreenComponents.Jukebox
{
    public struct Song
    {
        public string name;
        public string artist;
        public AudioClip clip;
        public float length;
    }
}