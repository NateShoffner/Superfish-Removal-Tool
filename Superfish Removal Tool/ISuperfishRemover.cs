namespace Superfish_Removal_Tool
{
    internal interface ISuperfishRemover
    {
        string Name { get; }
        bool NeedsRemoved();
        SuperfishRemovalResult Remove();
    }
}