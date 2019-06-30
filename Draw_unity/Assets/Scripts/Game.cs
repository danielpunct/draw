using System;
using System.Collections;
using DG.Tweening;
using Gamelogic.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Game : Singleton<Game>
{
    public TMP_Text levelText;
    public Transform pieceHolder;
    public Transform frameHolder;
    public Transform frameParent;

    public Level[] levels;

    int _currentLevel;
    int _currentPiece;
    Piece _loadedPiece;

    public AnimationCurve frameAppear;
    Sequence _seq;

    public void ShowLevel(int level)
    {
        frameParent.rotation = Quaternion.Euler(0, 0, -4.8f);
        frameParent.localPosition = new Vector3(-7.5f, 3.8f, 0);
        _seq?.Kill();
        _seq = DOTween.Sequence()
            .Insert(0.5f, frameParent.DOLocalMove(new Vector3(0, 3.8f, 0), 0.5f).SetEase(Ease.OutExpo))
            .Insert(0.5f, frameParent.DOLocalRotate(Vector3.zero, 0.8f).SetEase(frameAppear));

        levelText.text = $"Level {level + 1}";
        _currentLevel = level;
        _currentPiece = 0;
        LoadPiece();
    }

    public IEnumerator ContourPassed()
    {
        _loadedPiece.ShowPassed();

        yield return new WaitForSeconds(2);
        DrawBoard.Instance.ClearBoard();
        
        GameManager.Instance.OnContourPassed();

    }

    public bool LoadNextPiece()
    {
        if (levels[_currentLevel].pieces.Length == _currentPiece + 1)
        {
            return false;
        }

        _currentPiece++;
        LoadPiece();
        return true;
    }

    public bool LoadNextLevel()
    {
        if (levels.Length <= _currentLevel + 1)
        {
            return false;
        }
        
        ShowLevel(_currentLevel + 1);
        return true;
    }

    public IEnumerator ContourFailed()
    {
        yield return new WaitForSeconds(1);
        DrawBoard.Instance.ClearBoard();
    }

    void LoadPiece()
    {
        pieceHolder.DestroyChildren();

        _loadedPiece = Instantiate(levels[_currentLevel].pieces[_currentPiece], pieceHolder.position,
            Quaternion.identity,
            pieceHolder).GetComponent<Piece>();

        _loadedPiece.ShowInit();
    }
}