using System.Collections.Generic;
using System.IO;
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

    [Index(nameof(Guild), IsUnique = true)]
    public class GameState
    {
        public int GameStateId { get; set; }
        public ulong Guild { get; set; }
        public ulong Channel { get; set; }
        public string State { get; set; }
    }
}