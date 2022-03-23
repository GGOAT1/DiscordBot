using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class CommandHandler : ModuleBase
    {
        /// <summary>
        /// [ping]というコメントが来た際の処理
        /// </summary>
        /// <returns>Botのコメント</returns>
        [Command("ping")]
        public async Task ping()
        {
            await ReplyAsync("pong!");
        }
    }
}
