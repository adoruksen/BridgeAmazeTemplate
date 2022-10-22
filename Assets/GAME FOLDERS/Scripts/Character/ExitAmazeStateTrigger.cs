using InteractionSystem;
using LevelSystem;
using Managers;
using AmazeSystem;
using UnityEngine;
using CharacterController = Character.CharacterController;

public class ExitAmazeStateTrigger : MonoBehaviour, IBeginInteract
{
    private GameAreaManager _newArea;
    public bool IsInteractable { get; } = true;

    private void Awake()
    {
        _newArea = GetComponentInParent<GameAreaManager>();
    }
    public void OnInteractBegin(IInteractor interactor)
    {
        var amaze= (AmazeController)interactor;
        var character = CharacterManager.Instance.GetCharacterByTeam(amaze.Team);
        character.SetState(character.ExitAmazeState);
        character.ExitAmazeState.OnStateExited += EnterGameArea;
    }

    private void EnterGameArea(CharacterController character)
    {
        character.ExitAmazeState.OnStateExited -= EnterGameArea;
        _newArea.OnCharacterEntered(character);
    }
}
