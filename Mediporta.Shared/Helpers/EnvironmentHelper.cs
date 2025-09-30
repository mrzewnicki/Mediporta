namespace Mediporta.Shared.Helpers;

public static class EnvironmentHelper
{
    public static bool IsDebug
    {
        get
        {
            bool isDebug = false;
#if DEBUG
            isDebug = true;
#endif
            return isDebug;
        }
    }
}