using System.Linq;
using Discord;
using Discord.Commands;
using Discord_bot.Core.Managers;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Diagnostics;
using System.Text;

namespace Friends.Commands
{
    [Name("Test")]
    public class Command : ModuleBase<ICommandContext>
    {
        private static CommandService _service = ServiceManager.GetService<CommandService>();


        [Command("getMyId")]
        public async Task GetMyIdAsync()
        {
            var user = Context.User;
            await ReplyAsync($"{user.Username}, ваш ID: {user.Id}");
        }

        [Command("help")]
        [Summary("Выводит все команды")]
        public async Task Help()
        {
            string commands = string.Empty;

            foreach (var module in _service.Modules)
            {
                foreach (var command in module.Commands)
                {
                    commands += $"`!{command.Name}`: {command.Summary ?? "-"}\n";
                }
            }
            await ReplyAsync(commands);
        }

    }



    class PermissionsSearch3
    {
        public List<string> SListTag { get; set; }
        public List<string> ETag { get; set; }
        public int Index { get; set; }
    }

    class PermissionsSearch2
    {
        public string Start { get; set; }
        public string End { get; set; }

        public List<PermissionsSearch3> Manutag { get; set; }
    }
    class PermissionsSearch
    {
        public List<PermissionsSearch2> Generic { get; set; }
        public RequestPermission Request { get; set; }
        public int Index { get; set; }
    }
    class GenPermissions
    {
        public Dictionary<string, PermissionsSearch> GeneralName { get; set; }
    }
    public class RequestPermission
    {
        public string Address { get; set; }
        public string Method { get; set; }
        public string Data { get; set; }
        public string ECode { get; set; } = "UTF-8";
        public Dictionary<string, string> Head { get; set; }

    }


    public class GetRequest : ModuleBase<ICommandContext>
    {
        private static GenPermissions outs = new GenPermissions()
        {
            GeneralName = new Dictionary<string, PermissionsSearch>()
                {
                    {"Steam",new PermissionsSearch()
                        {
                        Index =0,
                        Request=new RequestPermission()
                        {
                            Method = "GET",
                            Head = new Dictionary<string, string>()
                            {
                                {"Accept","application/xml;q=0.9"},
                            },
                        },
                        Generic = new List<PermissionsSearch2>()
                        {
                            new PermissionsSearch2()
                            {
                                Start="<a href=\"https://store.steampowered.com/app",
                                End="</a>",

                                Manutag=new List<PermissionsSearch3>()
                                {

                                    new PermissionsSearch3()
                                    {
                                        Index =1,

                                        SListTag=new List<string>()
                                        {
                                            "\"",
                                        },

                                        ETag=new List<string>()
                                        {
                                            "\"",
                                        },
                                    },

                                    new PermissionsSearch3()
                                    {
                                        Index =0,

                                        SListTag=new List<string>()
                                        {
                                            "<span class=\"title\">"
                                        },
                                        ETag=new List<string>()
                                        {
                                            "</span>",
                                        },
                                    }
                                },
                            }
                        }
                        }
                    },


                }

        };
        [Command("getList")]
        [Summary("Получает информацию обо всех наименования из файла")]

        public async Task getInfoList()
        {
            foreach (string line in System.IO.File.ReadLines(@"check.txt"))
            {
                await getInfo(line);
            }
        }
        class BackBu
        {
            public string NameTitle { get; set; }
            public string MainText { get; set; }
        }
        public static Regex expr = new Regex($"[^\\w a-zA-Z0-9 # ~ ` ! @ \" # $ : № % ^:&?*-_+=<>?,.\\ / | () \\[\\] {{}}]", RegexOptions.CultureInvariant);
        public static Regex AllSpec = new Regex($"[^\\w ]", RegexOptions.CultureInvariant);
        [Command("get")]
        [Summary("Получает информацию об введённом наименовании")]
        public async Task getInfo([Remainder] string UserGameMessage)
        {
            Stopwatch sw = new Stopwatch();
            Stopwatch Gam = new Stopwatch();

            sw.Start();


            List<EmbedFieldBuilder> tags = new List<EmbedFieldBuilder>();
            string ProcessedMessage = new Regex(" ").Replace(expr.Replace(UserGameMessage, ""), "+");
            outs.GeneralName["Steam"].Request.Address = $"https://store.steampowered.com/search/results?term={ProcessedMessage}&ignore_preferences=1&force_infinite=1&category1=998%2C21&os=win&snr=1_7_7_230_7";


            foreach (var GeneralName in outs.GeneralName)
            {
                Thread newThread = new Thread(() =>
                {
                    string CollectedResponse = null;
                    HttpClientRequest(UserGameMessage, GeneralName.Value.Request, ref CollectedResponse);
                    if (CollectedResponse != "")
                        if (tags.Count() >= GeneralName.Value.Index)
                            tags.Insert(GeneralName.Value.Index, new EmbedFieldBuilder { Name = GeneralName.Key, Value = CollectedResponse, IsInline = false });
                        else
                            tags.Add(new EmbedFieldBuilder { Name = GeneralName.Key, Value = CollectedResponse, IsInline = false });
                });
                newThread.Start();
                newThread.Join();
            }

            var builder = new EmbedBuilder().WithColor(new Color(255, 255, 255));
            if (tags.Count() != 0)
            {
                builder.WithTitle(UserGameMessage);
                builder.Fields = tags;
                await ReplyAsync(null, false, builder.Build());
            }
            else
                await ReplyAsync($"**Не могу найти:**: {UserGameMessage}");



            //'Field name length must be less than or equal to 256.' Название не выше 256
            //'Основа to 1024.'
            // Объект 6000 включая title

            sw.Stop();
            Console.WriteLine($"Общие: {sw.Elapsed.TotalMilliseconds}");


        }

        static void HttpClientRequest(string UserGameMessage, RequestPermission Request, ref string CollectedResponse)
        {


            string Response = null;


            HttpWebRequest _request = (HttpWebRequest)WebRequest.Create(Request.Address);
            _request.Proxy = null;
            ServicePointManager.Expect100Continue = false;


            _request.Method = Request.Method;



            foreach (var Head in Request.Head)
            {
                _request.Headers.Add(Head.Key, Head.Value);

            }


            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding encoding = System.Text.Encoding.GetEncoding(Request.ECode);


            if (Request.Data != null)
            {
                byte[] sentData = Encoding.Default.GetBytes(Request.Data);

                _request.ContentLength = sentData.Length;


                using (Stream requestStream = _request.GetRequestStream())
                {
                    requestStream.Write(sentData, 0, sentData.Length);
                    requestStream.Close();
                }
            }


            // using (BufferedStream buffer = new BufferedStream(stream))
            // {
            using (StreamReader streamReader = new StreamReader(((HttpWebResponse)_request.GetResponse()).GetResponseStream(), encoding))
            {
                Response = streamReader.ReadToEnd();
            }
            // }
            Stopwatch sw = new Stopwatch();
            sw.Start();
            processingSearch(Response, UserGameMessage, ref CollectedResponse);
            sw.Stop();
            Console.WriteLine($"{Request.Address}\n{UserGameMessage}\n{sw.Elapsed.TotalMilliseconds}");

        }

        static void processingSearch(string Response, string UserGameMessage, ref string CollectedResponse)
        {

            var CCollectedResponse = "";
            foreach (var GeneralName in outs.GeneralName)
            {
                Thread newThread = new Thread(() =>
                {

                    foreach (var Generic in GeneralName.Value.Generic)
                    {

                        int StartTag = 0;
                        int EndTag = 0;

                    While:
                        while (StartTag != -1)
                        {

                            List<string> SearchDo1p = new List<string>();

                            StartTag = Response.IndexOf(Generic.Start, StartTag);
                            if (StartTag == -1) goto While;
                            EndTag = Response.IndexOf(Generic.End, StartTag);
                            string FindResponse = Response.Substring(StartTag, EndTag - StartTag);
                            StartTag += FindResponse.Length;
                            int SLine = 0;
                            int ELine = 0;

                            for (int Mi = 0; Mi < Generic.Manutag.Count(); Mi++)
                            {

                                int CheckTag = 0; ;
                                for (int i = 0; i < Generic.Manutag[Mi].SListTag.Count(); i++)
                                {
                                    CheckTag = FindResponse.IndexOf(Generic.Manutag[Mi].SListTag[i], SLine);
                                    if (CheckTag != -1)
                                    {
                                        if (Generic.Manutag[Mi].SListTag.Count() < 2) CheckTag += Generic.Manutag[Mi].SListTag[i].Length;
                                        SLine = CheckTag;
                                        break;
                                    }

                                }
                                if (CheckTag == -1) goto While;

                                //ToDo сделать EndTag так что бы варианты верхнего и нижнего регистра без игнорирования 
                                for (int i = 0; i < Generic.Manutag[Mi].ETag.Count(); i++)
                                {
                                    ELine = FindResponse.IndexOf(Generic.Manutag[Mi].ETag[i], SLine);
                                    if (ELine != -1)
                                        break;


                                }
                                if (ELine == -1) goto While;

                                string Tag = FindResponse.Substring(SLine, ELine - SLine);

                                if (Generic.Manutag[Mi].Index == 0)
                                {

                                    string ReplaceTag = AllSpec.Replace(Tag, "").ToLowerInvariant();
                                    string ReplaceUserMessage = AllSpec.Replace(UserGameMessage, "").ToLowerInvariant();


                                    if (!ReplaceTag.Contains(ReplaceUserMessage))
                                    { goto While; }



                                }


                                if (SearchDo1p.Count() >= Generic.Manutag[Mi].Index)
                                    SearchDo1p.Insert(Generic.Manutag[Mi].Index, Tag);
                                else
                                    SearchDo1p.Add(Tag);

                            }
                            string CheckingForOverflow = string.Join("\n", SearchDo1p);
                            if (CheckingForOverflow.Length + CCollectedResponse.Length > 1024) return;
                            if (CCollectedResponse != "") CCollectedResponse += "\n";
                            CCollectedResponse += CheckingForOverflow;





                        }

                    }

                });
                newThread.Start();
                newThread.Join();
            }
            CollectedResponse = CCollectedResponse;
        }

    }
}