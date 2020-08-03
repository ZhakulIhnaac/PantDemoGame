using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Classes.GamePlay;
using Assets.Scripts.Classes.Playables;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Classes
{
    public class Soldier : Unit, ISoldier, IInteractable
    {

        void Awake()
        {
            AudioSource = GetComponent<AudioSource>();
            AudioSource.clip = CreatedSound;
            AudioSource.PlayOneShot(CreatedSound);
            name = "Soldier";
        }
    }
}
