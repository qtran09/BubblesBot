using Discord;
using Discord.Audio;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YoutubeExtractor;

namespace BubblesBot
{
    public class AudioService
    {
        private readonly ConcurrentDictionary<ulong, IAudioClient> ConnectedChannels = new ConcurrentDictionary<ulong, IAudioClient>();
        private IGuild guild;
        private IVoiceChannel target;

        public async Task JoinAudio(IGuild guild, IVoiceChannel target)
        {
                IAudioClient client;
                if (ConnectedChannels.TryGetValue(guild.Id, out client))
                {
                    return;
                }
                if (target.Guild.Id != guild.Id)
                {
                    return;
                }

                var audioClient = await target.ConnectAsync();

                if (ConnectedChannels.TryAdd(guild.Id, audioClient))
                {
                    this.guild = guild;
                    this.target = target;
                    Console.Write($"Connected to voice on {guild.Name}.");
                }
        }

        public async Task LeaveAudio(IGuild guild)
        {
            IAudioClient client;
            if (ConnectedChannels.TryRemove(guild.Id, out client))
            {
                await client.StopAsync();
                Console.Write($"Disconnected from voice on {guild.Name}.");
            }
        }
        
        public async Task PlayAudio(IGuild guild, IMessageChannel channel, string url)
        {
            //Check if bot is present
            try
            {
                IAudioClient client;
                IEnumerable<VideoInfo> s = DownloadUrlResolver.GetDownloadUrls(url, false);
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
                string filePath = DownloadAudio(s);
                if (!File.Exists(filePath))
                {
                    await channel.SendMessageAsync("File does not exist!");
                    return;
                }
                if (ConnectedChannels.TryGetValue(guild.Id, out client))
                {
                    var output = CreateStream(filePath).StandardOutput.BaseStream;
                    var stream = client.CreatePCMStream(AudioApplication.Music);
                    await output.CopyToAsync(stream);
                    await stream.FlushAsync().ConfigureAwait(false);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
                

        }

        private static string DownloadAudio(IEnumerable<VideoInfo> videoInfos)
        {
            try
            {
                VideoInfo vidInfo = videoInfos
                    .Where(info => info.CanExtractAudio)
                    .OrderByDescending(info => info.AudioBitrate)
                    .First();
                if (vidInfo.RequiresDecryption) DownloadUrlResolver.DecryptDownloadUrl(vidInfo);
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), RemoveIllegalPathCharacters(vidInfo.Title) + vidInfo.AudioExtension);
                var audioDownloader = new AudioDownloader(vidInfo, filePath);
                audioDownloader.DownloadProgressChanged += (sender, args) => Console.WriteLine(args.ProgressPercentage * 0.85);
                audioDownloader.AudioExtractionProgressChanged += (sender, args) => Console.WriteLine(85 + args.ProgressPercentage * 0.15);
                audioDownloader.Execute();
                return filePath;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            return null;
        }
        private static string RemoveIllegalPathCharacters(string path)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(path, "");
        }

        private Process CreateStream(string path)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg.exe",
                Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true
            });
        }
    }
}
