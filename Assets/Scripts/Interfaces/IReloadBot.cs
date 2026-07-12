public interface IReloadBot
{
    int CurrentAmmo { get; set; }
    int MaxAmmo { get; }
    void Reload();
    void PlayReloadAnimation();
}