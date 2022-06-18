using System.Collections.Generic;
using Scripts.Pieces.Enums;
using UnityEngine;

namespace Scripts.PieceDeployment
{
    /// <summary>
    /// Responsible for creating pieces defined in the PieceDeploymentObject
    /// </summary>
    public class PieceCreator : MonoBehaviour
    {
        
        /// <summary>
        /// Maps all prefabs to the corresponding name
        /// <example> E.g. PrefabX --> Key: PrefabX, Value GameObject of PrefabX </example>
        /// </summary>
        private readonly Dictionary<string, GameObject> _pieceTypeModels = new Dictionary<string, GameObject>();
    
        /// <summary>
        /// Initialized the piece creator with all prefabs,
        /// required to create the pieces
        /// </summary>
        /// <param name="pieceModels"></param>
        public void Initialize(GameObject[] pieceModels)
        {
            AddModelsToDictionary(pieceModels);
        }
    
        private void AddModelsToDictionary(GameObject[] pieceModels)
        {
            foreach (var model in pieceModels)
            {
                Debug.Log(model.ToString());
                var modelName = model.ToString();
                //Split the first part, to receive its actual name
                //E.g. ArcherEnemy (Scripts.Pieces.Archer) -> ArcherEnemy
                modelName = modelName.Split(' ')[0];
                _pieceTypeModels.Add(modelName, model);
            }
        
        }

        /// <summary>
        /// Searches a prefab based on the pieceType and instantiates the prefab
        /// </summary>
        /// <param name="pieceType"></param>
        /// <returns></returns>
        public GameObject CreatePiece(PieceType pieceType)
        {

            var prefab = _pieceTypeModels[pieceType.ToString()];
            if (!prefab) return null;
        
            var instantiatedPiece = Instantiate(prefab);          
            return instantiatedPiece;
        }

    }
}