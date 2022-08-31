using Tanya.Core.Models;
using Tanya.Game.Apex.Core.Models;

namespace Tanya.Game.Apex.Feature.Aim.Interfaces
{
    public interface ITarget
    {
        #region Methods

        bool IsSameTeam(Player localPlayer);

        bool IsValid(Player localPlayer);

        #endregion

        #region Properties

        byte BleedoutState { get; }

        byte DuckState { get; }

        Vector LocalOrigin { get; }

        bool Visible { get; }

        #endregion
    }
}