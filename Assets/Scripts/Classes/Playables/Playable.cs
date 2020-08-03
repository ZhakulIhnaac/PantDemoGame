using System;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Classes.Playables
{
    public abstract class Playable : MonoBehaviour, IPlayable
    {
        public static event Action<string> GiveWarning;
        public int HealthPoint;
        public AudioClip CreatedSound;
        protected AudioSource AudioSource;
        public float PowerCost;
        public Vector2 PlayableSize => (Vector2)gameObject.GetComponent<SpriteRenderer>().bounds.size;

        /*
         Selected, LeftMouseClick and RightMouseClick methods will be overriden by its children,
         Unit and Building classes. They are located in Playable class, because Selected,
         LeftMouseClick and RightMouseClick will be called from Getcomponent<Playable>().
         */
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

        /*
         SendWarningMessage publishes the received message to the subscribers.
         */
        public void SendWarningMessage(string message)
        {
            GiveWarning?.Invoke(message);
        }

    }
}
