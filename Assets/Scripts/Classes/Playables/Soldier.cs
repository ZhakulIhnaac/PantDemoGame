using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Classes.Playables
{
    public class Soldier : Unit, ISoldier, IInteractable
    {

        private void Awake()
        {
            AudioSource = GetComponent<AudioSource>();
            AudioSource.clip = CreatedSound;
            AudioSource.PlayOneShot(CreatedSound);
            name = "Soldier";
        }
    }
}
