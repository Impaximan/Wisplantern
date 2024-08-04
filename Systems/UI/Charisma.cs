using Terraria.UI;
using Wisplantern.UI.Charisma;
using System.Collections.Generic;

namespace Wisplantern.Systems.UI
{
    public class Charisma : ModSystem
    {
        internal UserInterface uInterface;
        internal CharismaUI meterUI;

        public override void OnModLoad()
        {
            if (!Main.dedServ)
            {
                uInterface = new UserInterface();

                meterUI = new CharismaUI();
                meterUI.Activate();

                uInterface?.SetState(meterUI);
            }
        }

        public override void OnModUnload()
        {
            uInterface?.SetState(null);
        }

        private GameTime _lastUpdateUiGameTime;
        public override void UpdateUI(GameTime gameTime)
        {
            _lastUpdateUiGameTime = gameTime;
            if (uInterface?.CurrentState != null)
            {
                uInterface.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

            if (index != -1)
            {
                layers.Insert(index, new LegacyGameInterfaceLayer(
                    "Wisplantern: Charisma UI",
                    delegate
                    {
                        if (_lastUpdateUiGameTime != null && uInterface?.CurrentState != null)
                        {
                            uInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
                        }
                        return true;
                    },
                       InterfaceScaleType.UI));
            }

        }

    }
}
