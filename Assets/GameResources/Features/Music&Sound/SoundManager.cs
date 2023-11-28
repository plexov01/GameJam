using System;
using System.Collections;
using System.Collections.Generic;
using GameJam.Features.CardSystem;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }    
    
    [SerializeField] public AudioClipRefsSO audioClipRefsSo;

    private void Awake()
    {
        Instance = this;
    }

    private void Start() {
        Meteor.OnMeteorExploded += MeteorOnOnMeteorExploded;
        Mine.OnMineExploded += Mine_OnMineExploded;
        AbstractCard.OnCardActivated += AbstractCard_OnCardActivated;
        GameHandler.OnStateChanged += GameHandler_OnStateChanged;
        BuildManager.OnConstructionPlaced += BuildManagerOnOnConstructionPlaced;
    }

    private void BuildManagerOnOnConstructionPlaced(object sender, BuildManager.OnConstructionPlacedEventArgs e) {
        switch (e.buildMode) {
            case BuildManager.BuildMode.Tower:
                if (GameHandler.Instance.IsFirstStageActive())
                {
                    if (Random.value < 0.5f)
                    {
                        PlaySound(audioClipRefsSo.thatsIt, Camera.main.transform.position);
                    }
                }
                else
                {
                    PlaySound(audioClipRefsSo.thatsIt, Camera.main.transform.position);
                }
                break;
            case BuildManager.BuildMode.Wall:
                if (GameHandler.Instance.IsFirstStageActive())
                {
                    if (Random.value < 0.5f)
                    {
                        PlaySound(audioClipRefsSo.thatsIt, Camera.main.transform.position);
                    }
                }
                else
                {
                    if (Random.value < 0.5f)
                    {
                        PlaySound(audioClipRefsSo.thatsIt, Camera.main.transform.position);
                    }
                    else
                    {
                        PlaySound(audioClipRefsSo.stopRats, Camera.main.transform.position);
                    }

                }
                break;
            case BuildManager.BuildMode.Mine:
                if (GameHandler.Instance.IsFirstStageActive())
                {
                    if (Random.value < 0.5f)
                    {
                        PlaySound(audioClipRefsSo.thatsIt, Camera.main.transform.position);
                    }
                }
                else
                {
                    PlaySound(audioClipRefsSo.stopRats, Camera.main.transform.position);
                }
                break;
            case BuildManager.BuildMode.Repair:
                PlaySound(audioClipRefsSo.Upgrade, Camera.main.transform.position);
                break;
            default:
                break;
        }
    }

    private void MeteorOnOnMeteorExploded(object sender, EventArgs e) {
        PlaySound(audioClipRefsSo.meteor, Camera.main.transform.position);
    }
    
    private void GameHandler_OnStateChanged(object sender, EventArgs e) {
        if (GameHandler.Instance.IsSecondStageActive()) {
            PlaySound(audioClipRefsSo.stealCheese,Camera.main.transform.position);
        }
    }

    private void AbstractCard_OnCardActivated(object sender, EventArgs e) {
        AbstractCard abstractCard = sender as AbstractCard;

        PlaySound(abstractCard.EventSoundList,Camera.main.transform.position);
    }

    private void Mine_OnMineExploded(object sender, EventArgs e) {
        PlaySound(audioClipRefsSo.Bomb, Camera.main.transform.position);
    }

    public void PlaySound(AudioClip audioClip, Vector3 position, float volume = .15f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }
    
    public void PlaySound(List<AudioClip> audioClipList, Vector3 position, float volume = .15f)
    {
        if (audioClipList.Count == 0) return;
        
        int rndm = Random.Range(0, audioClipList.Count);
        
        AudioSource.PlayClipAtPoint(audioClipList[rndm], position, volume);
    }

    private void OnDisable() {
        Meteor.OnMeteorExploded -= MeteorOnOnMeteorExploded;
        Mine.OnMineExploded -= Mine_OnMineExploded;
        AbstractCard.OnCardActivated -= AbstractCard_OnCardActivated;
        GameHandler.OnStateChanged -= GameHandler_OnStateChanged;
        BuildManager.OnConstructionPlaced -= BuildManagerOnOnConstructionPlaced;
    }
}
