namespace InteractionSystem
{
    public interface IFillObject
    {
        bool IsInteractable { get; }
        void OnInteractBegin(IInteractor interactor);
    }
}
