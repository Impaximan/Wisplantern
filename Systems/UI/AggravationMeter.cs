using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.UI;
using Wisplantern.UI.AggravationMeter;
using System.Collections.Generic;

namespace Wisplantern.Systems.UI
{
    public class AggravationMeter : ModSystem
    {
        internal UserInterface uInterface;
        internal AggravationMeterUI meterUI;

        public override void OnModLoad()
        {
            if (!Main.dedServ)
            {
                uInterface = new UserInterface();

                meterUI = new AggravationMeterUI();
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
            int index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Entity Health Bars"));

            if (index != -1)
            {
                layers.Insert(index, new LegacyGameInterfaceLayer(
                    "Wisplantern: Aggravation Meter",
                    delegate
                    {
                        if (_lastUpdateUiGameTime != null && uInterface?.CurrentState != null)
                        {
                            uInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
                        }
                        return true;
                    },
                       InterfaceScaleType.Game));
            }

        }

    }
}
