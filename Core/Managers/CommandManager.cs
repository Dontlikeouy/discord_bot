using System.Reflection;
using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord_bot.Core.Managers;

namespace discord_bot.Core.Managers
{
    public static class CommandManager
    {
        public static CommandService _commandService = ServiceManager.GetService<CommandService>();
        public static async Task LoadCommandAsync()
        {
            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), ServiceManager.Provider);
            foreach (var command in _commandService.Commands)
            {
                Console.WriteLine($"Команда: {command.Name} загружена.");
            }
        }
    }
}