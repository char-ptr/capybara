using System.IO;
using Dalamud.Game;
using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace Capybara;

public sealed class Plugin : IDalamudPlugin
{
    [PluginService]
    internal static IGameGui GameGui { get; private set; } = null!;
    [PluginService]
    internal static ISigScanner SigScanner { get; private set; } = null!;
    [PluginService]
    internal static IChatGui ChatGui { get; private set; } = null!;
    [PluginService]
    internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;

    [PluginService]
    internal static ITextureProvider TextureProvider { get; private set; } = null!;

    [PluginService]
    internal static ICommandManager CommandManager { get; private set; } = null!;

    private const string CommandName = "/capybara";

    public Plugin()
    {

        CommandManager.AddHandler(
            CommandName,
            new CommandInfo(OnCommand) { HelpMessage = "capybara does emotes give first arg as id" }
        );
        Game.Init(SigScanner);
    }
    public static void PrintError(string message) => ChatGui.PrintError($"[capybara] {message}");

    public void Dispose()
    {

        CommandManager.RemoveHandler(CommandName);
    }

    private void OnCommand(string command, string args)
    {
        Game.emoteAgent = (Game.emoteAgent != nint.Zero) ? Game.emoteAgent : GameGui.FindAgentInterface("Emote");
        if (Game.emoteAgent == nint.Zero) { PrintError("Failed to get emote agent, please open the emote window and then use this command to initialize it."); return; }

        if (uint.TryParse(args, out var emote))
            Game.DoEmote(Game.emoteAgent, emote, 0, true, true);
        else
            PrintError("Emote must be specified by a number.");

        // in response to the slash command, just toggle the display status of our main ui
    }

}
