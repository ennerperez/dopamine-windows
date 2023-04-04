namespace Amphetamine.Core.Audio
{
    public interface IPlayerFactory
    {
       IPlayer Create(bool hasMediaFoundationSupport);
    }
}