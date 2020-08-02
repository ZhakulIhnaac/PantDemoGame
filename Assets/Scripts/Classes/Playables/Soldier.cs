using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Classes.GamePlay;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Classes
{
    public class Soldier : Unit, ISoldier, IInteractable
    {

        void Awake()
        {
            AudioSource = GetComponent<AudioSource>();
            AudioSource.clip = BuildSound;
            AudioSource.PlayOneShot(BuildSound);
            name = "Soldier";
        }
    }
}
