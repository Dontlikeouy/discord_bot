using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_bot.Core.Managers;
using Friends.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace discord_bot.Core.Managers
{
    public class EventManager : ModuleBase<ICommandContext>
    {
        private static DiscordSocketClient _client = ServiceManager.GetService<DiscordSocketClient>();
        private static CommandService _commandService = ServiceManager.GetService<CommandService>();
        public static Task LoadCommands()
        {
            // _client.Log += message =>
            //   {
            //       Console.WriteLine($"[{DateTime.Now}\t({message.Source})\t{message.Message}");
            //       return Task.CompletedTask;
            //   };
            // _commandService.Log += message =>
            //   {
            //       Console.WriteLine($"[{DateTime.Now}\t({message.Source})\t{message.Message}");
            //       return Task.CompletedTask;
            //   };

            //ПОДКЛЮЧЕНИЕ EVENT

            _client.Ready += OnReady;
            _client.MessageReceived += OnMessageReceived;
            return Task.CompletedTask;

        }

        private static async Task OnMessageReceived(SocketMessage arg)
        {

            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);
            // if (message.Author.IsBot || message.Channel is IDMChannel)

            var argPos = 0;
            // Реагирует на !(prefix) или @(упоминание)
            if (!(message.HasStringPrefix(ConfigManager.Config.Prefix, ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))) return;
            var result = await _commandService.ExecuteAsync(context, argPos, ServiceManager.Provider);

            if (!result.IsSuccess)
            {
                if (result.Error == CommandError.UnknownCommand)
                {
                    await message.Channel.SendMessageAsync($"**Неизвестная команда: {message.Content}**");
                    return;
                }
            }
            //  await message.DeleteAsync();
        }


        private static async Task OnReady()
        {

            Console.WriteLine($"[{DateTime.Now}\t(READY)\t Я РОДИЛСЯ!");
            await _client.SetStatusAsync(UserStatus.AFK);
            await _client.SetGameAsync($"Prefix : [{ConfigManager.Config.Prefix}]", null, ActivityType.Listening);
        }
    }
}