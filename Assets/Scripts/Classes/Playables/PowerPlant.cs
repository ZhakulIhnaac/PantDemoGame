using System;
using System.CodeDom.Compiler;
using System.Collections;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Classes
{
    public class PowerPlant : Building, IPowerPlant
    {
        private float _powerProductionInterval;
        private float _powerAmountToSupply;
        public static event Action<float> ProducePower;

        void Awake()
        {
            AudioSource = gameObject.GetComponent<AudioSource>();
            AudioSource.clip = BuildSound;
            AudioSource.PlayOneShot(BuildSound);
            HealthPoint = 150;
        }

        void Start()
        {
            _powerProductionInterval = 5f;
            _powerAmountToSupply = 20f;
            name = "Power Plant";
            StartCoroutine(SupplyPower());
        }

        public IEnumerator SupplyPower() // This coroutine will be producing an amount of power for the player in every t seconds.
        {
            while (true) // Continue the production after 5 second.
            {
                yield return new WaitForSeconds(_powerProductionInterval);
                ProducePower?.Invoke(_powerAmountToSupply);
            }
        }
    }
}
