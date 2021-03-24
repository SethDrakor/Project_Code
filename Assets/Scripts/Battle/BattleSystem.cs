using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

//Définit dans l'ordre les actions pour une battle
public enum BattleState {Start, PlayerAction, PlayerMove, EnemyMove, Busy, PartyScreen}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] PartyScreen partyScreen;

    public event Action<bool> OnBattleOver;

    BattleState state;
    int currentAction;
    int currentMove;
    int currentMember;

    PokemonParty playerParty;
    Pokemon wildPokemon;

    public void StartBattle(PokemonParty playerParty, Pokemon wildPokemon)
    {
        //Assigne les paramètres aux variables
        this.playerParty = playerParty;
        this.wildPokemon = wildPokemon;
        //Appelle la coroutine SetupBattle et démarre la battle
        StartCoroutine(SetupBattle());
    }

    //Lance les fonctions de la bataille
    public IEnumerator SetupBattle()
    {
        //Configure le playerUnit et l'enemyUnit
        playerUnit.Setup(playerParty.GetHealthyPokemon());
        enemyUnit.Setup(wildPokemon);

        //Définit les données du playerHud et de l'enemyHud
        playerHud.SetData(playerUnit.Pokemon);
        enemyHud.SetData(enemyUnit.Pokemon);

        partyScreen.Init();

        //Affiche les moves des pokemons du joueur
        dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);
        
        //Stop la coroutine et affiche une phrase
        yield return dialogBox.TypeDialog($"A wild {enemyUnit.Pokemon.Base.Name} appeared.");

        PlayerAction();
    }

    //Propose des actions au joueur et affiche celle qu'il a choisi
    void PlayerAction()
    {
        state = BattleState.PlayerAction;
        dialogBox.SetDialog("Choose an action");
        dialogBox.EnableActionSelector(true);
    }

    //Ouvre la selection des pokemon
    void OpenPartyScreen()
    {
        state = BattleState.PartyScreen;
        partyScreen.SetPartyData(playerParty.Pokemons);
        partyScreen.gameObject.SetActive(true);
    }

    //Désactive l'actionSelector et le DialogText et affiche les moves que peut choisir le joueur
    void PlayerMove()
    {
        state = BattleState.PlayerMove;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
    }

    //L'effet du move du pokemon du joueur sur le pokemon adverse
    IEnumerator PerformPlayerMove()
    {
        //Le pokemon du joueur lance son move
        state = BattleState.Busy;
        var move = playerUnit.Pokemon.Moves[currentMove];
        yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name} used {move.Base.Name}");
        yield return new WaitForSeconds(1f);

        //Le pokemon adverse prend les dégats
        bool isFainted = enemyUnit.Pokemon.TakeDamage(move, playerUnit.Pokemon);
        yield return enemyHud.UpdateHP();

        if(isFainted)
        {
            //Le pokemon adverse est mort et la battle est fini
            yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name} Fainted");
            yield return new WaitForSeconds(2f);
            OnBattleOver(true);
        }

        //Le pokemon adverse fait un move
        else
        {
            StartCoroutine(EnemyMove());
        }
    }

    //Ce que le pokemon ennemi va faire
    IEnumerator EnemyMove()
    {
        //Lorsque c'est le tour du pokemon adverse il lance une attaque aléatoire et une phrase s'affiche
        state = BattleState.EnemyMove;
        var move = enemyUnit.Pokemon.GetRandomMove();
        yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name} used {move.Base.Name}");
        yield return new WaitForSeconds(1f);

        //Le pokemon du joueur prend les damages et sa vie baisse
        bool isFainted = playerUnit.Pokemon.TakeDamage(move, playerUnit.Pokemon);
        yield return playerHud.UpdateHP();

        if(isFainted)
        {
            yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name} Fainted");

            yield return new WaitForSeconds(2f);

            //Ouvre le PartyScreen pour choisir un pokemon vivant, sinon la battle se termine
            var nextPokemon = playerParty.GetHealthyPokemon();
            if(nextPokemon != null)
            {
                //La selection de pokemon s'ouvre et le joueur choisi quel pokemon il veut envoyer
                OpenPartyScreen();
            }

            else
            {
                OnBattleOver(false);
            }
        }

        else
        {
            PlayerAction();
        }
    }

    //Permet de selectionner les actions durant la battle
    public void HandleUpdate()
    {
        if(state == BattleState.PlayerAction)
        {
            HandleActionSelection();
        }
        else if (state == BattleState.PlayerMove)
        {
            HandleMoveSelection();
        }

        else if (state == BattleState.PartyScreen)
        {
            HandlePartySelection();
        }
    }

    //Le joueur peut séléctionner l'action qu'il veut avec les flèches
    void HandleActionSelection()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            ++currentAction;
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            --currentAction;
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentAction += 2;
        }

        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentAction -= 2;
        }

        //Fixe l'index entre 0 et 3 grace à la méthode Clamp disponible dans Mathf
        currentAction = Mathf.Clamp(currentAction, 0, 3);

        //Appelle la méthode qui modifie la couleur du texte
        dialogBox.UpdatActionSelection(currentAction);

        //Si le joueur appuie sur Space
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //Et que l'index est à 0
            if (currentAction == 0)
            {
                //Le joueur combat et va selectionner le move qu'il veut faire
                PlayerMove();
            }

            //Et que l'index est à 1
            else if (currentAction == 1)
            {
                //Le joueur choisi un pokemon dans son équipe
                OpenPartyScreen();
            }

            //Et que l'index est à 2
            else if (currentAction == 2)
            {
                //Le joueur choisi de fuir
            }
        }
    }

    //Permet au joueur de choisir le move qu'il veut avec les flèches
    void HandleMoveSelection()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            ++currentMove;
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            --currentMove;
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentMove += 2;
        }

        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentMove -= 2;
        }
        
        //Fixe l'index entre 0 et -1 grace à la méthode Clamp disponible dans Mathf
        currentMove = Mathf.Clamp(currentMove, 0, playerUnit.Pokemon.Moves.Count - 1);

        //Affiche les moves que peut choisir le joueur ainsi que leurs PP et leurs types
        dialogBox.UpdateMoveSelection(currentMove, playerUnit.Pokemon.Moves[currentMove]);

        //Quand le joueur appuie sur Space un move se lance et une phrase apparait
        if(Input.GetKeyDown(KeyCode.Space))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PerformPlayerMove());
        }

        //Le joueur peut revenir en arrière dans le moveSelector
        else if (Input.GetKeyDown(KeyCode.R))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            PlayerAction();
        }
    }
    
    //Permet au joueur de choisir le pokemon qu'il veut avec les flèches dans la selections des pokemon
    void HandlePartySelection()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            ++currentMember;
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            --currentMember;
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentMember += 2;
        }

        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentMember -= 2;
        }

        //Fixe l'index entre 0 et -1 grace à la méthode Clamp disponible dans Mathf
        currentMember = Mathf.Clamp(currentMember, 0, playerParty.Pokemons.Count - 1);

        partyScreen.UpdateMemberSelection(currentMember);

        //Vérifie si le pokemon que le joueur n'est pas mort ou si il  n'est pas deja au combat
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var selectedMember = playerParty.Pokemons[currentMember];
            if (selectedMember.HP <= 0)
            {
                partyScreen.SetMessageText("You can't send out a fainted pokemon");
                return;
            }
            if (selectedMember == playerUnit.Pokemon)
            {
                partyScreen.SetMessageText("You can't switch with the same pokemon");
                return;
            }

            //La selection de pokemon est désactivée et la méthode Sxitch est lancée
            partyScreen.gameObject.SetActive(false);
            state = BattleState.Busy;
            StartCoroutine(SwitchPokemon(selectedMember));
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            partyScreen.gameObject.SetActive(false);
            PlayerAction();
        }
    }

    //Change le pokemon du  joueur si son pokemon actuelle est mort ou si le joueur veut changer
    IEnumerator SwitchPokemon(Pokemon newPokemon)
    {
        if (playerUnit.Pokemon.HP > 0)
        {
            yield return dialogBox.TypeDialog($"Come back {playerUnit.Pokemon.Base.Name}");
            yield return new WaitForSeconds(2f);
        }

        playerUnit.Setup(newPokemon);
        playerHud.SetData(newPokemon);
        dialogBox.SetMoveNames(newPokemon.Moves);

        yield return dialogBox.TypeDialog($"Go {newPokemon.Base.Name}!");

        StartCoroutine(EnemyMove());
    }
}
