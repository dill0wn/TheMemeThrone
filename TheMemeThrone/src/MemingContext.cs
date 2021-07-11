using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using Discord;
using Microsoft.EntityFrameworkCore;

namespace MemeThroneBot
{
    public class MemingContext : DbContext
    {
        public DbSet<MemeCard> MemeCards { get; set; }
        public DbSet<CaptionCard> CaptionCards { get; set; }
        public DbSet<GameState> Games { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            string path = Path.GetFullPath(Path.Combine("db", "memes.db"));
            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            options.UseSqlite($"Data Source={path}");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // base.OnModelCreating(modelBuilder);
        }
    }

    public class MemeCard
    {
        public int MemeCardId { get; set; }
        public string Url { get; set; }
        public string Label { get; set; }
    }

    public class CaptionCard
    {
        public int CaptionCardId { get; set; }
        public string Text { get; set; }
    }

    public enum GameStateEnum
    {
        Lobby,
        Started,
        Ended
    }

    [Index(nameof(GuildId), IsUnique = true)]
    public class GameState
    {
        public const int MIN_PLAYER_COUNT = 2;
        public int GameStateId { get; set; }

        public ulong GuildId { get; set; }
        public ulong ChannelId { get; set; }
        public ulong MessageId { get; set; }

        public MessageReference MessageReference { get => new MessageReference(MessageId, ChannelId, GuildId); }

        [Column(TypeName = "nvarchar(24)")]
        public GameStateEnum State { get; set; }

        public List<PlayerState> Players { get; set; } = new List<PlayerState>();
        public PlayerState CurrentPlayer { get => Players[0]; }

        internal bool JoinGame(ulong userId, out string msg)
        {
            if (Players.FirstOrDefault(p => p.UserId == userId) != null)
            {
                msg = "Player already in here.";
                return false;
            }

            Players.Add(new PlayerState
            {
                UserId = userId,
            });
            msg = "Joined.";
            return true;
        }

        internal bool StartGame(out string msg)
        {
            if (State != GameStateEnum.Lobby)
            {
                msg = "Game already started";
                return false;
            }

            if (Players.Count < MIN_PLAYER_COUNT)
            {
                msg = $"Not enough players. You need at least {MIN_PLAYER_COUNT} to start a game.";
                return false;
            }

            State = GameStateEnum.Started;
            msg = "Started";
            return true;
        }
    }

    public class PlayerState
    {
        public int PlayerStateId { get; set; }
        public ulong UserId { get; set; }

        public GameState GameState { get; set; }
        public int GameStateId { get; set; }
    }
}