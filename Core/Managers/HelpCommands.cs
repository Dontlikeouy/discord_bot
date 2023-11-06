using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord_bot.Core.Managers;

namespace discord_bot.Core.Managers
{

    public class HelpCommands 
    {
        private static CommandService _service = ServiceManager.GetService<CommandService>();

        // [Command("Help")]
        // public async Task HelpCommand()
        // {

        //     // var Commands = new List<OutputPermission>();
        //     // foreach (var module in _service.Modules)
        //     // {
        //     //     string TempCommands = string.Empty;
        //     //     foreach (var command in module.Commands)
        //     //     {
        //     //         TempCommands += $"`!{command.Name}`: {command.Summary ?? "-" }\n";
        //     //     }
        //     //     Commands.Add(new OutputPermission() { Title = $"{module.Name}", Information = TempCommands });
        //     // }
        //     // await SendBox(Commands, null);
        // }

    }
}