using Dalamud.Game;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Capybara
{
    internal unsafe class Game
    {
        public delegate void DoEmoteDelegate(nint agent, uint emoteID, long a3, bool a4, bool a5);
        public static DoEmoteDelegate DoEmote;
        public static nint emoteAgent = nint.Zero;
        public static void Init(ISigScanner sigg)
        {
            try
            {
                var agentModule = Framework.Instance()->GetUIModule()->GetAgentModule();



                try
                {
                    DoEmote = Marshal.GetDelegateForFunctionPointer<DoEmoteDelegate>(sigg.ScanText("E8 ?? ?? ?? ?? E9 ?? ?? ?? ?? B8 0A 00 00 00"));
                    emoteAgent = (nint)agentModule->GetAgentByInternalId(AgentId.Emote);
                }
                catch { Plugin.PrintError("Failed to load /doemote"); }
            } catch { Plugin.PrintError("failed to get agent module"); }
        }
    }
}
