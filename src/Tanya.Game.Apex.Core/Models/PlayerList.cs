using Tanya.Driver.Interfaces;
using Tanya.Game.Apex.Core.Interfaces;

namespace Tanya.Game.Apex.Core.Models
{
    public class PlayerList : EntityListFilter<Player>
    {
        private readonly IOffsets _offsets;

        #region Constructors

        public PlayerList(IDriver driver, IOffsets offsets, EntityList entities, SignifierList signifiers) : base(driver, entities, signifiers, "player")
        {
            _offsets = offsets;
        }

        #endregion

        #region Overrides of EntityListFilter<Player>

        protected override Player Create(IDriver driver, ulong address)
        {
            return new Player(driver, _offsets, address);
        }

        #endregion
    }
}