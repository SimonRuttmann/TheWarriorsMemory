using System;
using Scripts.Enums;
using Scripts.Pieces.Enums;
using UnityEngine;

//Path to create the object --> Create --> Scriptable Object --> GameConfiguration --> Piece Deployment
namespace Scripts.PieceDeployment
{
    [CreateAssetMenu(menuName = "Scriptable Objects/GameConfiguration/PieceDeployment")]
    public class PieceDeploymentConfiguration : ScriptableObject
    {
    
        [Serializable] private class Field
        {
            public Vector2Int logicalPosition;
            public PieceType pieceType;
            public Team team;
        }

        [SerializeField] 
        private Field[] playground;
        
        public int GetAmountOfPieces()
        {
            return playground.Length;
        }
        
        public Vector2Int GetPositionOfPiece(int index)
        {
            return new Vector2Int(
                playground[index].logicalPosition.x - 1, 
                playground[index].logicalPosition.y - 1);
        }
        
        public PieceType GetTypeOfPiece(int index)
        {
            return playground[index].pieceType;
        }
        
        public Team GetTeamOfPiece(int index)
        {
            return playground[index].team;
        }
    }
}

