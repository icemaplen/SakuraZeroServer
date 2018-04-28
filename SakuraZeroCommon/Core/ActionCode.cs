namespace SakuraZeroCommon.Core
{
    /// <summary>
    /// 二级协议名，Action，用于查找执行方法.
    /// </summary>
    public enum EActionCode
    {
        None,

        // System
        HeartBeat,

        // User
        Login,
        Register,
        KickOff,

        // Player
        GetRoles,
        CreateRole,
        Logout
    }
}
