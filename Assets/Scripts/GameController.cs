using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Permet de montrer dans quel état est le jeu en premier lorsqu'on commence
public enum GameState { FreeRoam, Battle }

//Ce script gère le BattleSystem, le PlayerController et la WorldCamera
public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;

    GameState state;

    //Commence et fini la battle grace à l'event onEncountered et OnBattleOver
    private void Start()
    {
        playerController.onEncountered += StartBattle;
        battleSystem.OnBattleOver += EndBattle;
    }

    void StartBattle()
    {
        //Si une battle commence, le BattleSystem initialment désactiver, s'affiche et la worldCamera disparait
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);
        
        var playerParty = playerController.GetComponent<PokemonParty>();
        var wildPokemon = FindObjectOfType<MapArea>().GetComponent<MapArea>().GetRandomWildPokemon();
        battleSystem.StartBattle(playerParty, wildPokemon);
    }

    //Dit si la battle est gagné ou pas
    void EndBattle(bool won)
    {
        //Désactive le BattleSystem et remet la WorldCamera
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
    }

    private void Update()
    {
        if(state == GameState.FreeRoam)
        {
            //L'état du jeu est libre alors le controle est au PlayerController
            playerController.HandleUpdate();
        }

        else if (state == GameState.Battle)
        {
            //Une battle a démarrer alors c'est le BattleSystem qui controle
            battleSystem.HandleUpdate();
        }
    }
}
