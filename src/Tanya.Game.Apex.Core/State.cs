using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Tanya.Driver.Interfaces;
using Tanya.Game.Apex.Core.Interfaces;
using Tanya.Game.Apex.Core.Models;

namespace Tanya.Game.Apex.Core
{
    public class State : Context, IUpdatable
    {
        private readonly ButtonList _buttons;
        private readonly EntityList _entities;
        private readonly LevelName _levelName;
        private readonly LocalPlayer _localPlayer;
        private readonly NpcList _npcs;
        private readonly PlayerList _players;
        private readonly SignifierList _signifiers;

        #region Constructors

        public State(IDriver driver, ILogger logger, IOffsets offsets, ulong address) : base(driver, logger)
        {
            _buttons = new ButtonList(driver, offsets, address);
            _levelName = new LevelName(driver, offsets, address);
            _localPlayer = new LocalPlayer(driver, offsets, address);
            _signifiers = new SignifierList(driver);
            _entities = new EntityList(driver, offsets, _levelName, address);
            _npcs = new NpcList(driver, offsets, _entities, _signifiers);
            _players = new PlayerList(driver, offsets, _entities, _signifiers);
        }

        #endregion

        #region Properties

        [JsonPropertyName("buttons")]
        public ButtonList Buttons => _buttons;

        [JsonPropertyName("levelName")]
        public string LevelName => _levelName.Value;

        [JsonPropertyName("localPlayer")]
        public ulong LocalPlayer => _localPlayer.Value;

        [JsonPropertyName("npcs")]
        public IReadOnlyDictionary<ulong, Npc> Npcs => _npcs.Value;

        [JsonPropertyName("players")]
        public IReadOnlyDictionary<ulong, Player> Players => _players.Value;

        #endregion

        #region Implementation of IUpdatable

        public void Update(DateTime frameTime)
        {
            _buttons.Update(frameTime);
            _levelName.Update(frameTime);
            _localPlayer.Update(frameTime);
            _entities.Update(frameTime);
            _npcs.Update(frameTime);
            _players.Update(frameTime);
            _signifiers.Update(frameTime);
        }

        #endregion
    }
}