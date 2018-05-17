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
        UserLogin,
        UserLogout,
        Register,
        KickOff,

        // Player
        PlayerLogin,
        PlayerLogout,
        GetRoles,
        CreateRole,
        DeleteRole,

        // Inventory
        GetAllItems,
        UpdateItem,
        UpdateEquipmentStatus
    }
}
