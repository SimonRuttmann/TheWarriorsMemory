using System.Linq;
using Scripts.Enums;
using Scripts.PieceDeployment;
using Scripts.Pieces;
using Scripts.Pieces.Enums;
using UnityEngine;
using UnityEngine.UI;

//using Valve.VR;


//[RequireComponent(typeof(FigurErsteller))]
namespace Scripts.InGameLogic
{
    public class InGameManager : MonoBehaviour
    {
        private enum Spielzustand
        {
            Start, Spiel, Fertig
        }

        // Hier wird das Skriptobjekt im Editor hinzugefügt
        [SerializeField] private PieceDeploymentObject Startkonfiguration;

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
        private PieceCreator _pieceCreator;
        private Player _weisserPlayer;
        private Player _schwarzerPlayer;
        private Player _aktiverPlayer;
        private Spielzustand spielzustand;


        //FigurErsteller ist ein Singleton -> Objekt kann über GetComponent erhalten werden
        private void Awake()
        {
            //Abhängigkeiten
            this._pieceCreator = GetComponent<PieceCreator>();
            ErstelleSpieler();
        }

        private void ErstelleSpieler()
        {
            this._weisserPlayer = new Player(Team.Player, playground);
            this._schwarzerPlayer = new Player(Team.Enemy, playground);
        }

        private void StarteNeuesSpiel(bool firstGame)
        {
            playground.markerCreator.DestroyMarkers();
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
            _aktiverPlayer = _weisserPlayer;
            this.SchachUIManager.SetTeamanzeige(Team.Player);
            ErstelleAlleSpielerZuege(_aktiverPlayer);
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

        private void ErstelleFigurenVonAufstellung(PieceDeploymentObject pieceDeploymentObject)
        {
            for (int i = 0; i < pieceDeploymentObject.GetAmountOfPieces(); i++)
            {
                //Daten der Figur abrufen
                Vector2Int xyPosition = pieceDeploymentObject.GetPositionOfPiece(i);
                Team figurfarbe = pieceDeploymentObject.GetTeamOfPiece(i);
                var figurtypS = pieceDeploymentObject.GetTypeOfPiece(i);

                //Figur erstellen, instanziieren und dem Spieler hinzufügen 
                ErstelleFigurUndInitialisiere(xyPosition, figurfarbe, figurtypS);
            }
        }

        public void ErstelleFigurUndInitialisiere(Vector2Int xyPosition, Team figurfarbe, PieceType figurtypS)
        {
            Piece neuePiece = this._pieceCreator.CreatePiece(figurtypS, figurfarbe).GetComponent<Piece>();
            neuePiece.InitializePiece(xyPosition, figurfarbe, playground);
        
            //Setzt die Figur auf das Schachfeld
            this.playground.SetzeFigurAufsFeld(xyPosition, neuePiece);

            //Figur dem Spieler hinzufügen
            //Spieler aktuellerSpieler;
        
            if (figurfarbe == Team.Player) { this._aktiverPlayer = _weisserPlayer; }
            else { this._aktiverPlayer = _schwarzerPlayer; }
            this._aktiverPlayer.AddPiece(neuePiece);
        }

        private void ErstelleAlleSpielerZuege(Player player)
        {
            player.GenerateAllPossibleMoves();
        }

        public bool IstTeamzug(Team farbe)
        {
            return _aktiverPlayer.team == farbe;
        }
    
        public void BeendeZug()
        {
            //Spielerzüge vom aktuellen Spieler ermitteln
            ErstelleAlleSpielerZuege(_aktiverPlayer);
        
            //Spielerzüge vom Gegner ermitteln
            ErstelleAlleSpielerZuege(GegnerVonSpieler(_aktiverPlayer));

            if (IstSpielVorbei()) { BeendeSpiel(); }
            else { WechlseAktivesTeam(); }
        }


        internal bool LaueftSpiel()
        {
            return spielzustand == Spielzustand.Spiel;
        }

        private bool IstSpielVorbei()
        {
            if (!_aktiverPlayer.remainingPiecesOfPlayer.Any()) return true;
            return false;
        }

        private void BeendeSpiel()
        {
            this.SchachUIManager.OnGameFinished(_aktiverPlayer.team.ToString());
            if (_aktiverPlayer.team == Team.Player)
            {
                teamanzeigeText1.text = "Team Weiss      gewinnt";
                teamanzeigeText2.text = "Team Weiss      gewinnt";
                _schwarzerPlayer.remainingPiecesOfPlayer.ForEach(
                    (p => playground.SterbenUndLoeschen(p)));
            }
            else
            {
                teamanzeigeText1.text = "Team Schwarz    gewinnt";
                teamanzeigeText2.text = "Team Schwarz    gewinnt";
                _weisserPlayer.remainingPiecesOfPlayer.ForEach(
                    (p => playground.SterbenUndLoeschen(p)));
            }


            spielzustand = Spielzustand.Fertig;

        }



        private void ZerstoereFiguren()
        {
            _weisserPlayer.remainingPiecesOfPlayer.ForEach(
                p => {
                    if(p!=null && p.gameObject !=null) Destroy(p.gameObject);
                }
            );
            _schwarzerPlayer.remainingPiecesOfPlayer.ForEach(p => {
                    if (p != null && p.gameObject != null) Destroy(p.gameObject);
                }
            );
        }

        private void WechlseAktivesTeam()
        {
        
            if (_aktiverPlayer == _weisserPlayer) { 
                _aktiverPlayer = _schwarzerPlayer; 
                this.SchachUIManager.SetTeamanzeige(Team.Enemy); 
                foreach(var marker in teammarker)
                {
                    marker.GetComponent<MeshRenderer>().material = schwarzMarker;
                }
                teamanzeigeText1.text= "Am Zug: Team Schwarz";
                teamanzeigeText2.text = "Am Zug: Team Schwarz";
            }
            else {
                _aktiverPlayer = _weisserPlayer;   
                this.SchachUIManager.SetTeamanzeige(Team.Player);
                foreach (var marker in teammarker)
                {
                    marker.GetComponent<MeshRenderer>().material = weissMarker;
                }
                teamanzeigeText1.text = "Am Zug: Team    Weiss";
                teamanzeigeText2.text = "Am Zug: Team    Weiss";
            }
        }

        private Player GegnerVonSpieler(Player player)
        {
            Player aktuellerGegner;
            if (player == _weisserPlayer) { aktuellerGegner = _schwarzerPlayer; }
            else { aktuellerGegner = _weisserPlayer; }

            return aktuellerGegner;
        }

        internal void OnFigurRemoved(Piece piece)
        {
            Player figurBesitzer = (piece.Team == Team.Player) ? _weisserPlayer : _schwarzerPlayer;
            figurBesitzer.RemovePiece(piece);
        }



        public void RestartGame()
        {
            ZerstoereFiguren();
            playground.OnSpielNeustart();
            _weisserPlayer.OnRestartGame();
            _schwarzerPlayer.OnRestartGame();
            StarteNeuesSpiel(false);

        }





    }
}

