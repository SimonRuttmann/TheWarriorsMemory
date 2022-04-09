using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts.Enums;
using Scripts.InGameLogic;
using Scripts.Marker;
using Scripts.Pieces;
using Scripts.Pieces.Animation;
using UnityEngine;

public class Playground : MonoBehaviour
{
    
    /// <summary>
    /// This attribute needs to be matched within the center of the bottom left hexagon,
    /// based on it, the positions of the click events and the position of marker, pieces can be calculated
    /// </summary>
    [SerializeField] private Transform anchorPointBottomLeft;
    
    /// <summary>
    /// This attribute needs to be set to the exact size of the hexagon
    /// based on it, the positions of the click events and the position of marker, pieces can be calculated
    /// </summary>
    [SerializeField] private float hexagonSize;

    public const int GesFeldGroesse = 8;

    private Piece[,] grid;  //Start bei 1, 1
    private Piece _gewaehltePiece;
    private InGameManager _inGameManager;

    public MarkerCreator markerCreator; 
    private AnimationScheduler _animationScheduler;

    public Vector3 RelativePositionZumSchachbrettfeld(Vector2Int position)
    {
        return anchorPointBottomLeft.position + new Vector3(position.x * hexagonSize, 0f, position.y * hexagonSize);
    }

    protected virtual void Awake()
    {
        _animationScheduler = GetComponent<AnimationScheduler>();
        markerCreator = GetComponent<MarkerCreator>();

        CreateGrid();
    }

    private void CreateGrid()
    {
        grid = new Piece[GesFeldGroesse, GesFeldGroesse];
    }

    public void SetzeAbhaengigkeiten(InGameManager inGameManager)
    {
        this._inGameManager = inGameManager;
    }

    public Vector3 KalkulierePosVonCoords(Vector2Int coords)
    {
        return anchorPointBottomLeft.position + new Vector3(coords.x * hexagonSize, 0f, coords.y * hexagonSize);
    }

    private Vector2Int KalkuliereCoordsVonPos(Vector3 inputPosition)
    {
        int x = Mathf.FloorToInt(transform.InverseTransformPoint(inputPosition).x / hexagonSize) + GesFeldGroesse / 2;
        int y = Mathf.FloorToInt(transform.InverseTransformPoint(inputPosition).z / hexagonSize) + GesFeldGroesse / 2;
        return new Vector2Int(x, y);
    }

    public void OnFeldAuswahl(Vector3 inputPosition)
    {
        if (this.blocker) return;
        Vector2Int coords = KalkuliereCoordsVonPos(inputPosition);
        Piece piece = GetFigurOnFeld(coords);

        if (_gewaehltePiece)
        {
            // Figur nochnamal anwählen
            if (piece != null && _gewaehltePiece == piece)
            {
                DeselectFigur();
            }
            else if (piece != null && _gewaehltePiece != piece && _inGameManager.IstTeamzug(piece.Team))
            {
                piece.IdleAnimation();
                WahleFigur(piece);
            }

            //Figur ist ausgewählt und "Klick" auf bewegbares feld
            else if (_gewaehltePiece.IsMovePossibleTo(coords))
            {
                OnAusgewaehlteFigurBewegt(coords, _gewaehltePiece);
            }
        }
        else
        {
            if (piece != null && _inGameManager.IstTeamzug(piece.Team))
            {
                piece.IdleAnimation();
                WahleFigur(piece);
            }
        }
    }

    private void WahleFigur(Piece piece)
    {
      //  schachManager.EntferneAngriffsMoeglichkeitenAufFigur<Koenig>(piece);
        _gewaehltePiece = piece;
        List<Vector2Int> auswahl = _gewaehltePiece.GetPossibleMoves().ToList();
        ZeigeAusgewaehlteFelder(auswahl);
    }

    private void ZeigeAusgewaehlteFelder(List<Vector2Int> auswahl)
    {
        Dictionary<Vector3, bool> squaresData = new Dictionary<Vector3, bool>();
        for (int i = 0; i < auswahl.Count; i++)
        {
            Vector3 position = KalkulierePosVonCoords(auswahl[i]);
            bool isSquareFree = GetFigurOnFeld(auswahl[i]) == null;
            squaresData.Add(position, isSquareFree);
        }
        markerCreator.CreateAndShowMarkers(squaresData);
    }

    private void DeselectFigur()
    {
        _gewaehltePiece = null;
        markerCreator.DestroyMarkers();
    }

    private void OnAusgewaehlteFigurBewegt(Vector2Int kooridanten, Piece piece)
    {
        Vector2Int pos = piece.Position;
        VersucheGegnerischeFigurZuSchlagen(kooridanten);
        UpdateSchachbrettOnFigurBewegt(kooridanten, pos, piece, null);
        DeselectFigur();
        BeendeZug();
    }
    private void BeendeZug()
    {
        _inGameManager.BeendeZug();
    }

    public void UpdateSchachbrettOnFigurBewegt(Vector2Int newCoords, Vector2Int oldCoords, Piece neuFig, Piece altFig)
    {
        grid[oldCoords.x, oldCoords.y] = altFig;
        grid[newCoords.x, newCoords.y] = neuFig;
    }

    public Piece GetFigurOnFeld(Vector2Int coords)
    {
        if (CheckObCoordsAufFeld(coords)) return grid[coords.x, coords.y];
        return null;
    }

    public bool CheckObCoordsAufFeld(Vector2Int coords)
    {
        if (coords.x < 0 || coords.y < 0 || coords.x >= GesFeldGroesse || coords.y >= GesFeldGroesse) return false;
        return true;
    }

    public bool ContainsPiece(Piece piece)
    {
        for (int i = 0; i < GesFeldGroesse; i++)
        {
            for (int j = 0; j < GesFeldGroesse; j++)
            {
                if (grid[i, j] == piece) return true;
            }
        }
        return false;
    }

    public void SetzeFigurAufsFeld(Vector2Int coords, Piece piece)
    {
        if (CheckObCoordsAufFeld(coords))
            grid[coords.x, coords.y] = piece;
    }
    private void VersucheGegnerischeFigurZuSchlagen(Vector2Int coords)
    {
        //Gegnerische Figur
        Piece piece = GetFigurOnFeld(coords);
        if (piece && !_gewaehltePiece.IsSameTeam(piece))
        {
            StartKonflikt(_gewaehltePiece, piece, coords);
            SchlageFigur(piece);
        }
        else
        {
            _gewaehltePiece.MoveToCoord(coords);
        }

    }


    private void StartKonflikt(Piece angreifendePiece, Piece geschlagenePiece, Vector2Int kooridanten)
    {
        Piece figWeiß, figSchwarz;

        if (angreifendePiece.Team == Team.Player)
        {
            figWeiß = angreifendePiece;
            figSchwarz = geschlagenePiece;
        }
        else
        {
            figWeiß = geschlagenePiece;
            figSchwarz = angreifendePiece;
        }

        double RotationspunktAngreifer;
        double RotationspunktVerteidiger;

        if (geschlagenePiece.Position.y - angreifendePiece.Position.y == 0)
        {

            if (angreifendePiece.Team == Team.Enemy)
            {
                //Dame schwarz greif an und es passt sgoar
                if (geschlagenePiece.Position.x > angreifendePiece.Position.x) RotationspunktAngreifer = 270;
                else RotationspunktAngreifer = 90;
            }
            //Dame weiss greift an -> Beide figuren in die verkehrte richtung
            else
            {
                if (geschlagenePiece.Position.x > angreifendePiece.Position.x) RotationspunktAngreifer = 90;
                else RotationspunktAngreifer = 270;
            }
        }
        else
        {
            float gegenkathete = geschlagenePiece.Position.x - angreifendePiece.Position.x;
            float ankathete = geschlagenePiece.Position.y - angreifendePiece.Position.y;
            if (figWeiß.Position.y > figSchwarz.Position.y)
            {
                RotationspunktAngreifer = 180 + (180 / Math.PI) * Math.Atan((gegenkathete) / (ankathete));
            }
            else RotationspunktAngreifer = (180 / Math.PI) * Math.Atan((gegenkathete) / (ankathete));
        }

        RotationspunktVerteidiger = RotationspunktAngreifer;

        if (angreifendePiece.Team == Team.Player) { RotationspunktAngreifer = RotationspunktAngreifer - 180; }
        if (geschlagenePiece.Team == Team.Player) { RotationspunktVerteidiger = RotationspunktVerteidiger - 180; }


        //Bewegung Angreifer
        _animationScheduler.RotatePiece(0f, angreifendePiece, (float)RotationspunktAngreifer);

        //Bewegung Verteidiger
        _animationScheduler.RotatePiece(0f, geschlagenePiece, (float)RotationspunktVerteidiger);

        //Angriff
        _animationScheduler.StartAnimation(1f, angreifendePiece, AnimationStatus.Attack);

        //Sterben
        _animationScheduler.StartAnimation(1.5f, geschlagenePiece, AnimationStatus.Die);

        //Loeschen
        _animationScheduler.StartAnimation(4f, geschlagenePiece, AnimationStatus.Delete);

        //Bewege Figur 1 auf Figur 2
        _animationScheduler.MovePiece(5f, angreifendePiece, kooridanten);

        //Drehe Figur zurück
        int back = 0;
        if (angreifendePiece.Team == Team.Player) back = 180;

        _animationScheduler.RotatePiece(6f, angreifendePiece, (float)back);
        this.BlockEingabe(7f);
    }

    //Schlage Figur -> Übergebene Figur wird sterben
    private void SchlageFigur(Piece piece)
    {
        if (piece)
        {
            grid[piece.Position.x, piece.Position.y] = null;
            _inGameManager.OnFigurRemoved(piece);
        }
    }

    public void BlockEingabe(float time)
    {
        this.blocker = true;
        StartCoroutine(EingabeblockerManager(time));
    }

    IEnumerator EingabeblockerManager(float time)
    {
        yield return new WaitForSeconds(time);
        this.blocker = false;

    }
    private bool blocker = false;

    public void BefoerdereFigur(Piece piece)
    {
        string modell;
        if (piece.Team == Team.Player) modell = "DameWeiss";
        else
        {
            modell = "DameSchwarz";
        }
        Vector2Int pos = piece.Position;
        Team team = piece.Team;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        SchlageFigur(piece);
        _animationScheduler.CleanDelete(1, piece);

    }

    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    internal void OnSpielNeustart()
    {
        _gewaehltePiece = null;
        CreateGrid();
    }
    public void SterbenUndLoeschen(Piece geschlagenePiece)
    {
        _animationScheduler.StartEndAnimation(1.5f, geschlagenePiece);
        //animationManager.StartAnimation(1f, null, geschlageneFigur, AnimationManager.Animationtrigger.Loeschen);
    }
}