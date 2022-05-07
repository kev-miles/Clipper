namespace Host.Delivery.GenericInterfaces
{
    public interface ILocalizable
    {
        string GetTranslationID();
        void SetText(string newText);
    }
}