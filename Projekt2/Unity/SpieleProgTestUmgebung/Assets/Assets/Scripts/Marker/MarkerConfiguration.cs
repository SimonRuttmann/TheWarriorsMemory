﻿using System;
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
        /// The prefab of the marker for the currently selected piece
        /// </summary>
        /// <value>Required to be set in the inspector</value> 
        [SerializeField] 
        private GameObject selectionPrefab;
        
        /// <summary>
        /// The prefab of the marker for the enemy selected piece
        /// </summary>
        /// <value>Required to be set in the inspector</value> 
        [SerializeField] 
        private GameObject enemySelectionPrefab;
        
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
        
        /// <summary>
        /// The prefab to instantiate for displaying the field of
        /// the currently selected piece
        /// </summary>
        /// <seealso cref="selectionPrefab"/>
        public GameObject SelectionPrefab => selectionPrefab;

        /// <summary>
        /// The prefab to instantiate for displaying the field of
        /// the enemy selected piece
        /// </summary>
        /// <seealso cref="enemySelectionPrefab"/>
        public GameObject EnemySelectionPrefab => enemySelectionPrefab;
    }
}