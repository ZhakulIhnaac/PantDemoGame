using System;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Classes
{
    public abstract class Playable : MonoBehaviour, IPlayable
    {
        public static event Action<string> GiveWarning;
        public int HealthPoint;
        public AudioClip BuildSound;
        protected AudioSource AudioSource;
        public float BuildingCost;
        public Vector2 PlayableSize => (Vector2)gameObject.GetComponent<SpriteRenderer>().bounds.size;

        public virtual void Selected()
        {
            // Will be overriden
        }

        public virtual void LeftMouseClick()
        {
            // Will be overriden
        }

        public virtual void RightMouseClick()
        {
            // Will be overriden
        }

        public void SendWarningMessage(string message)
        {
            GiveWarning?.Invoke(message);
        }

    }
}
