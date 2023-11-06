using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using discord_bot.Core.Managers;
using Discord_bot.Core.Managers;
using Friends.Commands;
using Microsoft.Extensions.DependencyInjection;
using discord_bot.Core;
using System.IO;

namespace Friends
{
    class Program
    {
        static void Main(string[] args)
        {
            new Bot().MainAsync().GetAwaiter().GetResult();
            Console.ReadKey();
        }

    }
}