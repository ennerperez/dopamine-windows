namespace Amphetamine.Core.Interfaces
{
    public interface IPlayerFactory
    {
       IPlayer Create(bool hasMediaFoundationSupport);
    }
}