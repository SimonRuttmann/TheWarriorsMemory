using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public enum Animationtrigger { Nichts, Sterben, Angriff, Idle, Loeschen, Bewegen, Drehen}
    private Piece angreifendeFig;
    private Piece sterbendeFig;

    private Animationtrigger animationFigAngreifend = Animationtrigger.Nichts;
    private Animationtrigger animationFigSterbend = Animationtrigger.Nichts;

    private Piece _bewegtePiece;
    private Vector2Int koordinaten;
    private Animationtrigger animationBewegteFigur = Animationtrigger.Nichts;


    private Piece gedrehteFigur1;
    private float drehung1;
    private Animationtrigger animationDreheFigur1 = Animationtrigger.Nichts;

    private Piece gedrehteFigur2;
    private float drehung2;
    private Animationtrigger animationDreheFigur2 = Animationtrigger.Nichts;

    public void SauberesLoeschen(float time, Piece piece)
    {
        StartCoroutine(Loeschenverwalter(time, piece));
    }

    IEnumerator Loeschenverwalter(float time, Piece piece)
    {
       
        yield return new WaitForSeconds(time);

        Destroy(piece.gameObject);

    }

    public void BewegeFigur(float time, Piece piece, Vector2Int koordianten)
    {
        StartCoroutine(Bewegungsverwalter(time, piece, koordianten));
    }

    IEnumerator Bewegungsverwalter(float time, Piece piece, Vector2Int koord)
    {
        yield return new WaitForSeconds(time);
        this.koordinaten = koord;
        this._bewegtePiece = piece;
        this.animationBewegteFigur = Animationtrigger.Bewegen;

    }



    public void StartAnimation(float time, Piece angreifendePiece, Piece sterbendePiece, Animationtrigger animationtrigger)
    {
        StartCoroutine(Animationsverwalter(time, angreifendePiece, sterbendePiece, animationtrigger));
    }

    
    public void DreheFigur1(float time, Piece gedrehtePiece, float drehung)
    {
        StartCoroutine(Drehungsverwalter(time, gedrehtePiece, drehung, true));
    }

    public void DreheFigur2(float time, Piece gedrehtePiece, float drehung)
    {
        StartCoroutine(Drehungsverwalter(time, gedrehtePiece, drehung, false));
    }

    IEnumerator Drehungsverwalter(float time, Piece gedrehtePiece, float drehung, bool isFirst)
    {
        yield return new WaitForSeconds(time);
        if (isFirst)
        {
            this.drehung1 = drehung;
            this.gedrehteFigur1 = gedrehtePiece;
            this.animationDreheFigur1 = Animationtrigger.Drehen;
        }
        else
        {
            this.drehung2 = drehung;
            this.gedrehteFigur2 = gedrehtePiece;
            this.animationDreheFigur2 = Animationtrigger.Drehen;
        }
    }


    IEnumerator Animationsverwalter(float time, Piece angreifendePiece, Piece sterbendePiece, Animationtrigger animationtrigger)
    {
        yield return new WaitForSeconds(time);

        if (angreifendePiece != null)
        {
            this.angreifendeFig = angreifendePiece;
            this.animationFigAngreifend = animationtrigger;
        }
        if (sterbendePiece != null)
        {
            this.sterbendeFig = sterbendePiece;
            this.animationFigSterbend = animationtrigger;
        }
       

    }


    public void Update()
    {
        
        switch (this.animationFigAngreifend)
        {
            case Animationtrigger.Nichts:   break;
            case Animationtrigger.Angriff:  this.animationFigAngreifend = Animationtrigger.Nichts; this.angreifendeFig.AttackAnimation(); break;
            case Animationtrigger.Idle:     this.animationFigAngreifend = Animationtrigger.Nichts; this.angreifendeFig.IdleAnimation(); break;
            case Animationtrigger.Sterben:  this.animationFigAngreifend = Animationtrigger.Nichts; this.angreifendeFig.DyingAnimation(); break;
            case Animationtrigger.Loeschen: this.animationFigAngreifend = Animationtrigger.Nichts; Destroy(this.angreifendeFig.gameObject); break;
        }
  
        switch (this.animationFigSterbend)
        {
            case Animationtrigger.Nichts:   break;
            case Animationtrigger.Angriff:  this.animationFigSterbend = Animationtrigger.Nichts; this.sterbendeFig.AttackAnimation(); break;
            case Animationtrigger.Idle:     this.animationFigSterbend = Animationtrigger.Nichts; this.sterbendeFig.IdleAnimation(); break;
            case Animationtrigger.Sterben:  this.animationFigSterbend = Animationtrigger.Nichts; this.sterbendeFig.DyingAnimation(); break;
            case Animationtrigger.Loeschen: this.animationFigSterbend = Animationtrigger.Nichts; Destroy(this.sterbendeFig.gameObject); break;
        }

        switch(this.animationBewegteFigur)
        {
            case Animationtrigger.Nichts:   break;
            case Animationtrigger.Bewegen:  this.animationBewegteFigur = Animationtrigger.Nichts; this._bewegtePiece.MoveToCoord(this.koordinaten); break;
        }
        switch (this.animationDreheFigur1)
        {
            case Animationtrigger.Nichts:   break;
            case Animationtrigger.Drehen:   this.animationDreheFigur1 = Animationtrigger.Nichts; this.gedrehteFigur1.RotatePiece(drehung1);            break;
        }
        switch (this.animationDreheFigur2)
        {
            case Animationtrigger.Nichts: break;
            case Animationtrigger.Drehen: this.animationDreheFigur2 = Animationtrigger.Nichts; this.gedrehteFigur2.RotatePiece(drehung2); break;
        }

    }

    public void StartEndAnimation(float time, Piece sterbendePiece)
    {
        StartCoroutine(EndAnimationsVerwalter(time, sterbendePiece));
    }



    IEnumerator EndAnimationsVerwalter(float time, Piece sterbendePiece)
    {
        sterbendePiece.SterbeOhneSound();
        yield return new WaitForSeconds(time);

        Destroy(sterbendePiece.gameObject);

    }




}
