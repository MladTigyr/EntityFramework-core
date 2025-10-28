namespace MusicHub
{
    using System;
    using System.Text;
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            string result = ExportSongsAboveDuration(context, 4);
            Console.WriteLine(result);
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            StringBuilder stringBuilder = new StringBuilder();

            var albums = context.Albums
                .Where(a => a.ProducerId == producerId)
                .Select(a => new
                {
                    AlbumName = a.Name,
                    a.ReleaseDate,
                    ProducerName = a.Producer!.Name,
                    Songs = a.Songs.Select(s => new
                    {
                        SongName = s.Name,
                        SongPrice = s.Price,
                        WriterName = s.Writer.Name
                    })
                    .OrderByDescending(s => s.SongName)
                    .ThenBy(s => s.WriterName)
                    .ToArray(),
                    TotalAlbumPrice = a.Price
                })
                .ToArray()
                .OrderByDescending(a => a.TotalAlbumPrice)
                .ToArray();

            foreach (var album in albums)
            {
                stringBuilder.AppendLine($"-AlbumName: {album.AlbumName}");
                stringBuilder.AppendLine($"-ReleaseDate: {album.ReleaseDate.ToString("MM/dd/yyyy")}");
                stringBuilder.AppendLine($"-ProducerName: {album.ProducerName}");
                stringBuilder.AppendLine("-Songs:");

                int songCounter = 1;

                foreach(var song in album.Songs)
                {
                    stringBuilder.AppendLine($"---#{songCounter++}");
                    stringBuilder.AppendLine($"---SongName: {song.SongName}");
                    stringBuilder.AppendLine($"---Price: {song.SongPrice:f2}");
                    stringBuilder.AppendLine($"---Writer: {song.WriterName}");
                }

                stringBuilder.AppendLine($"-AlbumPrice: {album.TotalAlbumPrice:f2}");
            }

            return stringBuilder.ToString().Trim();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            StringBuilder sb = new StringBuilder();

            var songs = context.Songs
                .Select(s => new
                {
                    SongName = s.Name,
                    PerformerFullName = s.SongPerformers.Select(sp => new
                    {
                        sp.Performer.FirstName,
                        sp.Performer.LastName,
                    })
                    .OrderBy(sp => sp.FirstName)
                    .ThenBy(sp => sp.LastName)
                    .ToArray(),
                    WriterName = s.Writer.Name,
                    AlbumProducer = s.Album != null ?
                    (s.Album.Producer != null ? s.Album.Producer.Name : null) : null,
                    s.Duration,
                })
                .ToArray()
                .OrderBy(s => s.SongName)
                .ThenBy(s => s.WriterName)
                .Where(s => s.Duration.TotalSeconds > duration)
                .ToArray();

            int songCount = 1;
            foreach (var s in songs)
            {
                sb.AppendLine($"-Song #{songCount++}");
                sb.AppendLine($"---SongName: {s.SongName}");
                sb.AppendLine($"---Writer: {s.WriterName}");

                foreach(var p in s.PerformerFullName)
                {
                    sb.AppendLine($"---Performer: {p.FirstName} {p.LastName}");
                }

                sb.AppendLine($"---AlbumProducer: {s.AlbumProducer}");
                sb.AppendLine($"---Duration: {s.Duration.ToString("c")}");
            }

            return sb.ToString().Trim();
        }
    }
}
