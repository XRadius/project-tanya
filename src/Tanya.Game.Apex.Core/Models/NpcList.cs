using Tanya.Driver.Interfaces;
using Tanya.Game.Apex.Core.Interfaces;

namespace Tanya.Game.Apex.Core.Models
{
    public class NpcList : EntityListFilter<Npc>
    {
        private readonly IOffsets _offsets;

        #region Constructors

        public NpcList(IDriver driver, IOffsets offsets, EntityList entities, SignifierList signifiers) : base(driver, entities, signifiers, "npc_dummie")
        {
            _offsets = offsets;
        }

        #endregion

        #region Overrides of EntityListFilter<Npc>

        protected override Npc Create(IDriver driver, ulong address)
        {
            return new Npc(driver, _offsets, address);
        }

        #endregion
    }
}