using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] Text dialogText;
    [SerializeField] Color highlightedColor;
    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject moveSelector;
    [SerializeField] GameObject moveDetails;

    [SerializeField] List<Text> actionTexts;
    [SerializeField] List<Text> moveTexts;

    [SerializeField] Text ppText;
    [SerializeField] Text typeText;

    //Définit un texte au dialogText
    public void SetDialog(string dialog)
    {
        dialogText.text = dialog;
    }

    //Permet de faire apparaitre les lettres une par une dans la boite de dialog
    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";
        foreach (var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            //Affiche 30 lettre par seconde
            yield return new WaitForSeconds(1f/30);
        }
    }

    //Active ou désactive le dialogText
    public void EnableDialogText(bool enabled)
    {
        dialogText.enabled = enabled;
    }

    //actionSelector est un game object donc on appelle la méthode SetActive pour l'activer ou le désactiver
    //Affiche les actions disponibles
    public void EnableActionSelector(bool enabled)
    {
        actionSelector.SetActive(enabled);
    }

    //moveSelector et moveDetails sont des games objects donc on appelle la méthode SetActive pour les activer ou les désactiver
    //Affiche les move disponible
    public void EnableMoveSelector(bool enabled)
    {
        moveSelector.SetActive(enabled);
        moveDetails.SetActive(enabled);
    }

    //Change la couleur du texte
    public void UpdatActionSelection(int selectedAction)
    {
        for (int i = 0; i<actionTexts.Count; ++i)
        {
            //Si l'index de texte correspond à l'action selectionner la couleur du texte change
            if(i == selectedAction)
            {
                actionTexts[i].color = highlightedColor;
            }
            else
            {
                //Le texte qui n'est pas selectionné est en noir
                actionTexts[i].color = Color.black;
            }
        }
    }

    //Change la couleur du texte
    //Montre les détails du move dans le panel move details
    public void UpdateMoveSelection(int selectedMove, Move move)
    {
        for (int i = 0; i<moveTexts.Count; ++i)
        {
            //Si l'index de texte correspond à l'action selectionner la couleur du texte change
            if(i == selectedMove)
            {
                moveTexts[i].color = highlightedColor;
            }
            else
            {
                //Le texte qui n'est pas selectionné est en noir
                moveTexts[i].color = Color.black;
            }
        }
        //Affiche les PP total du move et son coût
        ppText.text = $"PP {move.PP}/{move.Base.PP}";
        //Affiche de quel type est le move (comme Base est un enum on a besoin de convertir en string)
        typeText.text = move.Base.Type.ToString();
    }

    //Prend la liste des moves des pokemons du joueur
    public void SetMoveNames(List<Move> moves)
    {
        for (int i = 0; i<moveTexts.Count; ++i)
        {
            if (i < moves.Count)
            {
                moveTexts[i].text = moves[i].Base.Name;
            }
            else
            {
                moveTexts[i].text = "-";
            }
        }
    }
}
