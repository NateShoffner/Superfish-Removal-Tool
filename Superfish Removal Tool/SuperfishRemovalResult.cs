#region

using System;

#endregion

internal class SuperfishRemovalResult
{
    public SuperfishRemovalResult(bool succeeded, Exception error = null)
    {
        Succeeded = succeeded;
        Error = error;
    }

    public bool Succeeded { get; private set; }
    public Exception Error { get; private set; }
}