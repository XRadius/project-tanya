using Tanya.Game.Apex.Core;
using Tanya.Game.Apex.Core.Interfaces;

namespace Tanya.Game.Apex.Feature.Sense
{
    public class Feature : IFeature
    {
        private readonly Config _config;

        #region Constructors

        public Feature(Config config)
        {
            _config = config;
        }

        #endregion

        #region Implementation of IFeature

        public void Tick(DateTime frameTime, State state)
        {
            if (state.Players.TryGetValue(state.LocalPlayer, out var localPlayer))
            {
                foreach (var (_, player) in state.Players)
                {
                    if (player.IsValid() && !player.IsSameTeam(localPlayer))
                    {
                        if (localPlayer.LocalOrigin.Distance(player.LocalOrigin) * Constants.UnitToMeter < _config.Distance)
                        {
                            player.GlowEnable = (byte)(player.Visible ? 5 : 7);
                            player.GlowThroughWalls = (byte)(player.Visible ? 1 : 2);
                        }
                        else if (player.GlowEnable is 5 or 7)
                        {
                            player.GlowEnable = 2;
                            player.GlowThroughWalls = 5;
                        }
                    }
                }
            }
        }

        #endregion
    }
}