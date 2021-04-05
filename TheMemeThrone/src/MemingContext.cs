using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
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

    [Index(nameof(Guild), IsUnique = true)]
    public class GameState
    {
        public int GameStateId { get; set; }
        public ulong Guild { get; set; }
        public ulong Channel { get; set; }

        [Column(TypeName = "nvarchar(24)")]
        public GameStateEnum State { get; set; }

        public List<PlayerState> Players { get; set; } = new List<PlayerState>();

        internal bool IsJoinable(ulong id, out string msg)
        {
            if (Players.FirstOrDefault(p => p.User == id) != null)
            {
                msg = "Player already in here.";
                return false;
            }
            msg = "Joined.";
            return true;
        }
    }

    public class PlayerState
    {
        public int PlayerStateId { get; set; }
        public ulong User { get; set; }
    }
}