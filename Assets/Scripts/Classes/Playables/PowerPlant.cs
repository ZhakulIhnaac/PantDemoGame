using System;
using System.Collections;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Classes.Playables
{
    public class PowerPlant : Building, IPowerPlant
    {
        /* Events */
        public static event Action<float> ProducePower;

        private float _powerProductionInterval;
        private float _powerAmountToSupply;

        private void Awake()
        {
            AudioSource = gameObject.GetComponent<AudioSource>();
            AudioSource.clip = CreatedSound;
            AudioSource.PlayOneShot(CreatedSound);
            HealthPoint = 150;
        }

        private void Start()
        {
            _powerProductionInterval = 5f;
            _powerAmountToSupply = 20f;
            name = "Power Plant";
            StartCoroutine(SupplyPower());
        }

        /*
         SupplyPower coroutine produces an amount of power for the player in every t seconds.
         */
        public IEnumerator SupplyPower()
        {
            while (true) // While loop ensures to continue the production after each t seconds.
            {
                yield return new WaitForSeconds(_powerProductionInterval);
                ProducePower?.Invoke(_powerAmountToSupply);
            }
        }
    }
}
