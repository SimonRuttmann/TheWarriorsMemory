using System;
using Scripts.Enums;
using Scripts.Pieces.Enums;
using UnityEngine;

//Path to create the object --> Create --> Scriptable Object --> Playground --> Piece Deployment
namespace Scripts.PieceDeployment
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Playground/PieceDeployment")]
    public class PieceDeploymentObject : ScriptableObject
    {
    
        [Serializable] private class Field
        {
            public Vector2Int position;
            public PieceType pieceType;
            public Team team;
        }

        [SerializeField] private Field[] playground;
        
        public int GetAmountOfPieces()
        {
            return playground.Length;
        }
        
        public Vector2Int GetPositionOfPiece(int index)
        {
            return new Vector2Int(
                playground[index].position.x - 1, 
                playground[index].position.y - 1);
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

