using System;
using UnityEngine;

namespace Scripts.Marker
{
    
    /// <summary>
    /// Within this class, the created markers can be configured
    /// </summary>
    [Serializable]
    public class MarkerConfiguration
    {
        /// <summary>
        /// The prefab of the movement marker needs to be set to 
        /// </summary>
        /// <value>Required to be set in the inspector</value> 
        [SerializeField] 
        private GameObject movementPrefab;
        
        /// <summary>
        /// The prefab of the attack marker
        /// </summary>
        /// <value>Required to be set in the inspector</value> 
        [SerializeField] 
        private GameObject attackPrefab;
        

        /// <summary>
        /// The prefab to instantiate for displaying fields to attack
        /// </summary>
        /// <seealso cref="attackPrefab"/>
        public GameObject AttackPrefab => attackPrefab;
        
        /// <summary>
        /// The prefab to instantiate for displaying fields to move
        /// </summary>
        /// <seealso cref="movementPrefab"/>
        public GameObject MovementPrefab => movementPrefab;
    }
}