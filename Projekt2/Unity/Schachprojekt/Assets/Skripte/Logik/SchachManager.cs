
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
//using Valve.VR;


//[RequireComponent(typeof(FigurErsteller))]
public class SchachManager : MonoBehaviour
{
    private enum Spielzustand
    {
        Start, Spiel, Fertig
    }

    // Hier wird das Skriptobjekt im Editor hinzugefügt
    [SerializeField] private SchachbrettAufstellung Startkonfiguration;

    [SerializeField] private Text teamanzeigeText1;
    [SerializeField] private Text teamanzeigeText2;


    [SerializeField] private GameObject[] teammarker;
    [SerializeField] private Material weissMarker;
    [SerializeField] private Material schwarzMarker;

    //Schachfeld hinzufügen
    [SerializeField] private Playground playground;

    [SerializeField] private SchachUIManager SchachUIManager;

    [SerializeField] private VrSchachMenu VR_UIManager;
    //public SteamVR_Input_Sources Hand;
    private FigurErsteller FigurErsteller;
    private Spieler WeisserSpieler;
    private Spieler SchwarzerSpieler;
    private Spieler AktiverSpieler;
    private Spielzustand spielzustand;


    //FigurErsteller ist ein Singleton -> Objekt kann über GetComponent erhalten werden
    private void Awake()
    {
        //Abhängigkeiten
        this.FigurErsteller = GetComponent<FigurErsteller>();
        ErstelleSpieler();
    }

    private void ErstelleSpieler()
    {
        this.WeisserSpieler = new Spieler(Team.Player, playground);
        this.SchwarzerSpieler = new Spieler(Team.Enemy, playground);
    }

    private void StarteNeuesSpiel(bool firstGame)
    {
        playground.feldAuswahlErsteller.entferneAuswaehler();
        this.spielzustand = Spielzustand.Start;
        teamanzeigeText1.text = "Am Zug: Team    Weiss";
        teamanzeigeText2.text = "Am Zug: Team    Weiss";
        foreach (var marker in teammarker)
        {
            marker.GetComponent<MeshRenderer>().material = weissMarker;
        }

        if (firstGame) this.SchachUIManager.startUI();
        else this.SchachUIManager.SpielStarten();

        playground.SetzeAbhaengigkeiten(this);

        this.ErstelleFigurenVonAufstellung(Startkonfiguration);
        AktiverSpieler = WeisserSpieler;
        this.SchachUIManager.SetTeamanzeige(Team.Player);
        ErstelleAlleSpielerZuege(AktiverSpieler);
        this.spielzustand = Spielzustand.Spiel;
    }

    private void Update()
    {
        /*
        //VR
        bool menueOeffnen = SteamVR_Actions._default.VRMenue.GetStateDown(this.Hand);

        if (menueOeffnen)
        {
            this.VR_UIManager.setupUI();
            menueOeffnen = false;
        }
        */
    }
    //neues Spiel
    private void Start()
    {
        StarteNeuesSpiel(true);
    }

    private void ErstelleFigurenVonAufstellung(SchachbrettAufstellung schachbrettAufstellung)
    {
        for (int i = 0; i < schachbrettAufstellung.GetFigurenAnzahl(); i++)
        {
            //Daten der Figur abrufen
            Vector2Int xyPosition = schachbrettAufstellung.Get_XY_VonAufstellungsFigur(i);
            Team figurfarbe = schachbrettAufstellung.Get_Farbe_VonAufstellungsFigur(i);
            string figurtypS = schachbrettAufstellung.Get_Name_VonAufstellungsFigur(i);

            //Figur erstellen, instanziieren und dem Spieler hinzufügen 
            ErstelleFigurUndInitialisiere(xyPosition, figurfarbe, figurtypS);
        }
    }

    public void ErstelleFigurUndInitialisiere(Vector2Int xyPosition, Team figurfarbe, string figurtypS)
    {
        Piece neuePiece = this.FigurErsteller.ErstelleFigur(figurtypS).GetComponent<Piece>();
        neuePiece.InitializePiece(xyPosition, figurfarbe, playground);
        
        //Setzt die Figur auf das Schachfeld
        this.playground.SetzeFigurAufsFeld(xyPosition, neuePiece);

        //Figur dem Spieler hinzufügen
        //Spieler aktuellerSpieler;
        
        if (figurfarbe == Team.Player) { this.AktiverSpieler = WeisserSpieler; }
        else { this.AktiverSpieler = SchwarzerSpieler; }
        this.AktiverSpieler.AddFigur(neuePiece);
    }

    private void ErstelleAlleSpielerZuege(Spieler spieler)
    {
        spieler.GeneriereAlleMoeglichenZuege();
    }

    public bool IstTeamzug(Team farbe)
    {
        return AktiverSpieler.Farbe == farbe;
    }
    
    public void BeendeZug()
    {
        //Spielerzüge vom aktuellen Spieler ermitteln
        ErstelleAlleSpielerZuege(AktiverSpieler);
        
        //Spielerzüge vom Gegner ermitteln
        ErstelleAlleSpielerZuege(GegnerVonSpieler(AktiverSpieler));

        if (IstSpielVorbei()) { BeendeSpiel(); }
        else { WechlseAktivesTeam(); }
    }


    internal bool LaueftSpiel()
    {
        return spielzustand == Spielzustand.Spiel;
    }

    private bool IstSpielVorbei()
    {
        Piece[] koenigsbedroher = AktiverSpieler.GetPieceAtackingOppositePiceOfType<Koenig>();
        if (koenigsbedroher.Length > 0)
        {
            Spieler gegnerischerSpieler = GegnerVonSpieler(AktiverSpieler);
            Piece angegriffenerKoenig = gegnerischerSpieler.GetFigurenVomTyp<Koenig>().FirstOrDefault();
            gegnerischerSpieler.EntferneAngriffsMoeglichkeitenAufFigur<Koenig>(AktiverSpieler, angegriffenerKoenig);

            int moeglicheKoenigszuege = angegriffenerKoenig._possibleMoves.Count;
            if (moeglicheKoenigszuege == 0)
            {
                bool koenigDeckbar = gegnerischerSpieler.KannFigurVorAngriffRetten<Koenig>(AktiverSpieler);
                if (!koenigDeckbar)
                    return true;
            }
        }
        return false;
    }

    private void BeendeSpiel()
    {
        this.SchachUIManager.OnGameFinished(AktiverSpieler.Farbe.ToString());
        if (AktiverSpieler.Farbe == Team.Player)
        {
            teamanzeigeText1.text = "Team Weiss      gewinnt";
            teamanzeigeText2.text = "Team Weiss      gewinnt";
            SchwarzerSpieler.AktiveFiguren.ForEach(
                (p => playground.SterbenUndLoeschen(p)));
        }
        else
        {
            teamanzeigeText1.text = "Team Schwarz    gewinnt";
            teamanzeigeText2.text = "Team Schwarz    gewinnt";
            WeisserSpieler.AktiveFiguren.ForEach(
                (p => playground.SterbenUndLoeschen(p)));
        }


        spielzustand = Spielzustand.Fertig;

    }



    private void ZerstoereFiguren()
    {
        WeisserSpieler.AktiveFiguren.ForEach(
            p => {
                if(p!=null && p.gameObject !=null) Destroy(p.gameObject);
            }
        );
        SchwarzerSpieler.AktiveFiguren.ForEach(p => {
            if (p != null && p.gameObject != null) Destroy(p.gameObject);
        }
        );
    }

    private void WechlseAktivesTeam()
    {
        
        if (AktiverSpieler == WeisserSpieler) { 
            AktiverSpieler = SchwarzerSpieler; 
            this.SchachUIManager.SetTeamanzeige(Team.Enemy); 
            foreach(var marker in teammarker)
            {
                marker.GetComponent<MeshRenderer>().material = schwarzMarker;
            }
            teamanzeigeText1.text= "Am Zug: Team Schwarz";
            teamanzeigeText2.text = "Am Zug: Team Schwarz";
        }
        else {
            AktiverSpieler = WeisserSpieler;   
            this.SchachUIManager.SetTeamanzeige(Team.Player);
            foreach (var marker in teammarker)
            {
                marker.GetComponent<MeshRenderer>().material = weissMarker;
            }
            teamanzeigeText1.text = "Am Zug: Team    Weiss";
            teamanzeigeText2.text = "Am Zug: Team    Weiss";
        }
    }

    private Spieler GegnerVonSpieler(Spieler spieler)
    {
        Spieler aktuellerGegner;
        if (spieler == WeisserSpieler) { aktuellerGegner = SchwarzerSpieler; }
        else { aktuellerGegner = WeisserSpieler; }

        return aktuellerGegner;
    }

    internal void OnFigurRemoved(Piece piece)
    {
        Spieler figurBesitzer = (piece.Team == Team.Player) ? WeisserSpieler : SchwarzerSpieler;
        figurBesitzer.RemoveFigur(piece);
    }

    internal void EntferneAngriffsMoeglichkeitenAufFigur<T>(Piece piece) where T : Piece
    {
        AktiverSpieler.EntferneAngriffsMoeglichkeitenAufFigur<T>(GegnerVonSpieler(AktiverSpieler), piece);
    }

    public void RestartGame()
    {
        ZerstoereFiguren();
        playground.OnSpielNeustart();
        WeisserSpieler.OnSpielNeustart();
        SchwarzerSpieler.OnSpielNeustart();
        StarteNeuesSpiel(false);

    }


    public void BefoerdernErstellung(Vector2Int xyPosition, Team figFarbe, string modelltyp)
    {
        StartCoroutine(WarteBefoerdern(xyPosition, figFarbe, modelltyp));
    }

    IEnumerator WarteBefoerdern(Vector2Int xyPos, Team figFarbe, string modelltyp)
    {
        yield return new WaitForSeconds(0.25f);
        this.ErstelleFigurUndInitialisiere(xyPos, figFarbe, modelltyp);
        //  ErstelleAlleSpielerZuege(AktiverSpieler);
        this.BeendeZug();
    }
}

