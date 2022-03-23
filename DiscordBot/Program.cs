using Discord;
using Discord.WebSocket;
using System;
using System.Threading;
using System.Configuration;
using System.Threading.Tasks;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AllBotApp
{
    class Program
    {
        // public string token = ConfigurationManager.AppSettings["Token"]; // .net開発で用いる App.configを使用
        public string token = "OTM4NDk1NDA2MzUzMDU1Nzc0.YfrH9Q.C_VESB80_2-zx7qG1NNCcGuJfZA";
        public readonly DiscordSocketClient _client;
        public static CommandService commands;
        public static IServiceProvider services;

        static async Task Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public Program()
        {
            _client = new DiscordSocketClient();
            _client.Log += LogAsync;
            _client.Ready += onReady;
        }

        public async Task MainAsync()
        {
            commands = new CommandService();
            services = new ServiceCollection().BuildServiceProvider();
            _client.MessageReceived += CommandRecieved;
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(Timeout.Infinite);
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        private Task onReady()
        {
            Console.WriteLine($"{_client.CurrentUser} is Running!!");
            return Task.CompletedTask;
        }

        private async Task CommandRecieved(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            Console.WriteLine("{0} {1}:{2}", message.Channel.Name, message.Author.Username, message);
            // コメントがユーザーかBotかの判定
            if (message.Author.IsBot)
            {
                return;
            }
            if (message == null)
            {
                return;
            }

            int argPos = 0;
            var context = new SocketCommandContext(_client, message);
            // コマンドかどうか判定（今回は、「!」で判定）
            if ((message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos)))
            {

                // 実行
                var result = await commands.ExecuteAsync(context, argPos, services);

                //実行できなかった場合
                if (!result.IsSuccess)
                {
                    await context.Channel.SendMessageAsync(result.ErrorReason);
                }
            }

        }
    }
}