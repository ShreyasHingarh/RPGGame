using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game
{
    public class ScreenManager
    {
        Dictionary<ScreenStates, Screen> screenMap = new Dictionary<ScreenStates, Screen>();
       
        public ScreenStates CurrentScreenState = ScreenStates.StartingScreen;
        public Screen CurrentScreen => screenMap[CurrentScreenState];
        public void SetScreen(ScreenStates screenState)
        {
            CurrentScreenState = screenState;
        }

        public void AddScreen(ScreenStates newState, Screen newScreen)
        {
            screenMap.Add(newState, newScreen);
        }
    }
}
