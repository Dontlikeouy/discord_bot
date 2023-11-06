using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using discord_bot.Core.Managers;
using Discord_bot.Core.Managers;
using Microsoft.Extensions.DependencyInjection;

//Реализация prefix и токен
namespace discord_bot.Core
{
    public class Bot
    {
        private DiscordSocketClient _client;
        private CommandService _commandService;

        public Bot()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Debug,
                GatewayIntents = GatewayIntents.All
            });

            _commandService = new CommandService(new CommandServiceConfig()
            {
                LogLevel = LogSeverity.Debug,
                CaseSensitiveCommands = false,//учитывает регистр при вводе команд
                DefaultRunMode = RunMode.Async, // по дефолту асинхронная
                IgnoreExtraArgs = true // лишние аргументы

            });
            var collection = new ServiceCollection();
            collection.AddSingleton(_client);
            collection.AddSingleton(_commandService);

            ServiceManager.SetProvider(collection);


        }
        public async Task MainAsync()
        {
            //защита json
            if (string.IsNullOrWhiteSpace(ConfigManager.Config.Token))
            {
                Console.WriteLine("Ошибка: не введён token");
                return;
            }

            //Запускает методы из CommandManager и EventManager
            await CommandManager.LoadCommandAsync();
            await EventManager.LoadCommands();

            await _client.LoginAsync(TokenType.Bot, ConfigManager.Config.Token);
            await _client.StartAsync();
            await Task.Delay(-1);
        }
    }
}