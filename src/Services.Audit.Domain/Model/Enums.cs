namespace Services.Audit.Domain.Model
{
    public enum Events
    {
        Lock_Open,
        Lock_Close,
        Lock_AddUser,
        Lock_RemoveUser,
        Site_AddLock,
        Site_AddUser,
        Site_RemoveLock,
        Site_RemoveUser
    }
}
