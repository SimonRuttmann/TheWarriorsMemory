using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Scripts.Enums;
using Scripts.Pieces;

/*
 * 
 * The main reason to use System.Serializable for most users is so that your class and variables will show up in the inspector.
    If you added a public List of BaseTest to a component, you would be able to add and configure instances of BaseTest right in the inspector, with all that configured data available in the list at runtime. Otherwise you don't need to make your data classes Serializable.

 */
[System.Serializable] //Vermutung: Entweder zur Speicherung der Klasse oder Inspektor
public class Spieler
{
    public Team Farbe;
    public Playground playground;
    //Alle Figuren eines Spieler, welche sich auf dem Spielfeld befinden
    public List<Piece> AktiveFiguren;
    // Start is called before the first frame update

    //VORSICHT KONSTRUKTOR!
    public Spieler(Team farbe, Playground playground)
    {
        AktiveFiguren = new List<Piece>();
        this.playground = playground;
        this.Farbe = farbe;
    }

    public void AddFigur(Piece piece)
    {
        if (!AktiveFiguren.Contains(piece))
        {
            AktiveFiguren.Add(piece);
        }
    }

    public void RemoveFigur(Piece piece)
    {
        if (AktiveFiguren.Contains(piece))
        {
            AktiveFiguren.Remove(piece);
        }
    }

      public void GeneriereAlleMoeglichenZuege()
      {
          foreach(var figur in AktiveFiguren)
          {
              if (playground.HatFigur(figur))
              {
                figur.GeneratePossibleMoves();
              }
          }
      }

	public Piece[] GetPieceAtackingOppositePiceOfType<T>() where T : Piece
	{
		return AktiveFiguren.Where(p => p.IsAttackingPieceOfType<T>()).ToArray();
	}

	public Piece[] GetFigurenVomTyp<T>() where T : Piece
	{
		return AktiveFiguren.Where(p => p is T).ToArray();
	}

	public void EntferneAngriffsMoeglichkeitenAufFigur<T>(Spieler gegner, Piece gewaehltePiece) where T : Piece
	{
		List<Vector2Int> coordsZumEntfernen = new List<Vector2Int>();

		coordsZumEntfernen.Clear();
		foreach (var coords in gewaehltePiece.PossibleMoves)
		{
			Piece pieceOnCoords = this.playground.GetFigurOnFeld(coords);
			playground.UpdateSchachbrettOnFigurBewegt(coords, gewaehltePiece.Position, gewaehltePiece, null);
			gegner.GeneriereAlleMoeglichenZuege();
			if (gegner.CheckObEsFigurAngreift<T>())
				coordsZumEntfernen.Add(coords);
			playground.UpdateSchachbrettOnFigurBewegt(gewaehltePiece.Position, coords, gewaehltePiece, pieceOnCoords);
		}
		foreach (var coords in coordsZumEntfernen)
		{
			gewaehltePiece.PossibleMoves.Remove(coords);
		}

	}

	internal bool CheckObEsFigurAngreift<T>() where T : Piece
	{
		foreach (var piece in AktiveFiguren)
		{
			if (playground.HatFigur(piece) && piece.IsAttackingPieceOfType<T>())
				return true;
		}
		return false;
	}

	public bool KannFigurVorAngriffRetten<T>(Spieler opponent) where T : Piece
	{
		foreach (var piece in AktiveFiguren)
		{
			foreach (var coords in piece.PossibleMoves)
			{
				Piece pieceOnCoords = playground.GetFigurOnFeld(coords);
				playground.UpdateSchachbrettOnFigurBewegt(coords, piece.Position, piece, null);
				opponent.GeneriereAlleMoeglichenZuege();
				if (!opponent.CheckObEsFigurAngreift<T>())
				{
					playground.UpdateSchachbrettOnFigurBewegt(piece.Position, coords, piece, pieceOnCoords);
					return true;
				}
				playground.UpdateSchachbrettOnFigurBewegt(piece.Position, coords, piece, pieceOnCoords);
			}
		}
		return false;
	}

	internal void OnSpielNeustart()
	{
		AktiveFiguren.Clear();
	}




}

