//
// AIWolfGame.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using AIWolf.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AIWolf.Server
{
#if JHELP
    /// <summary>
    /// 人狼知能ゲームクラス
    /// </summary>    
#else
    /// <summary>
    /// AIWolf game class.
    /// </summary>
#endif
    public class AIWolfGame
    {
        GameSettingToSend gameSetting;
        IGameServer gameServer;
        GameData gameData;
        Dictionary<Agent, string> agentNameMap = new Dictionary<Agent, string>();

#if JHELP
        /// <summary>
        /// 乱数ジェネレーター
        /// </summary>
#else
        /// <summary>
        /// The random generator.
        /// </summary>
#endif
        public Random Rand { get; set; } = new Random();

#if JHELP
        /// <summary>
        /// コンソールにログを表示するか否か
        /// </summary>
#else
        /// <summary>
        /// Whether or not the console logs are shown.
        /// </summary>
#endif
        public bool ShowConsoleLog { get; set; } = true;

#if JHELP
        /// <summary>
        /// ロガー
        /// </summary>
#else
        /// <summary>
        /// The logger.
        /// </summary>
#endif
        public FileGameLogger GameLogger { get; set; }

        int Day => gameData.Day;
        List<Agent> AgentList => gameData.AgentList;
        List<Agent> OrderedAgentList => AgentList.OrderBy(x => x.AgentIdx).ToList();
        List<Agent> AliveAgentList => AgentList.Where(a => StatusOf(a) == Status.ALIVE).ToList();
        List<Agent> AliveHumanList => AliveAgentList.Where(a => RoleOf(a).GetSpecies() == Species.HUMAN).ToList();
        List<Agent> AliveWolfList => AliveAgentList.Where(a => RoleOf(a) == Role.WEREWOLF).ToList();
        bool GameFinished => GetWinner() != Team.UNC;

#if JHELP
        /// <summary>
        /// AIWolfGameクラスの新しいインスタンスを初期化する
        /// </summary>
        /// <param name="gameSetting">ゲーム設定</param>
        /// <param name="gameServer">ゲームサーバ</param>
#else
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="gameSetting">The setting of this game.</param>
        /// <param name="gameServer">The game server.</param>
#endif
        public AIWolfGame(GameSettingToSend gameSetting, IGameServer gameServer)
        {
            this.gameSetting = gameSetting;
            this.gameServer = gameServer;
        }

#if JHELP
        /// <summary>
        /// ゲームを初期化する
        /// </summary>
#else
        /// <summary>
        /// Initializes the game.
        /// </summary>
#endif
        void Init()
        {
            gameData = new GameData(gameSetting);
            gameServer.GameData = gameData;
            agentNameMap.Clear();

            var agentList = gameServer.ConnectedAgentList.Shuffle();

            if (agentList.Count != gameSetting.PlayerNum)
            {
                throw new Exception($"Player num is {gameSetting.PlayerNum} but connected agent is {agentList.Count}.");
            }

            // 希望役職を優先して配役する
            var vacantMap = gameSetting.RoleNumMap.Where(p => p.Value > 0).ToDictionary(p => p.Key, p => p.Value);
            var noRequestAgentQueue = new Queue<Agent>();
            foreach (var agent in agentList)
            {
                var requestedRole = gameServer.RequestRequestRole(agent);
                if (vacantMap.ContainsKey(requestedRole))
                {
                    if (vacantMap[requestedRole] > 0)
                    {
                        gameData.AddAgent(agent, Status.ALIVE, requestedRole);
                        vacantMap[requestedRole]--;
                    }
                    else
                    {
                        noRequestAgentQueue.Enqueue(agent);
                    }
                }
                else
                {
                    noRequestAgentQueue.Enqueue(agent);
                }
            }

            foreach (var pair in vacantMap.Where(p => p.Value > 0))
            {
                for (var i = 0; i < pair.Value; i++)
                {
                    gameData.AddAgent(noRequestAgentQueue.Dequeue(), Status.ALIVE, pair.Key);
                }
            }

            gameServer.GameSetting = gameSetting;
            foreach (var agent in agentList)
            {
                gameServer.Init(agent);
                agentNameMap[agent] = gameServer.RequestName(agent);
            }
        }

        /// <summary>
        /// Starts the game.
        /// </summary>
        public void Start()
        {
            Init();

            while (!GameFinished)
            {
                ConsoleLog();
                // Day phase.
                DayStart();
                if (Day == 0)
                {
                    if (gameSetting.TalkOnFirstDay)
                    {
                        Whisper();
                        Talk();
                    }
                }
                else // Day > 0
                {
                    Talk();
                }
                // Night phase.
                Night();
                if (GameLogger != null)
                {
                    GameLogger.Flush();
                }
            }
            ConsoleLog();
            Finish();

            if (ShowConsoleLog)
            {
                Console.WriteLine("Winner:" + GetWinner());
            }
        }

        /// <summary>
        /// Finishes the game.
        /// </summary>
        void Finish()
        {
            if (GameLogger != null)
            {
                foreach (var agent in OrderedAgentList)
                {
                    GameLogger.Log($"{Day},status,{agent.AgentIdx},{RoleOf(agent)},{StatusOf(agent)},{agentNameMap[agent]}");
                }
                GameLogger.Log($"{Day},result,{AliveHumanList.Count},{AliveWolfList.Count},{GetWinner()}");
                GameLogger.Close();
            }

            foreach (var agent in AgentList)
            {
                gameServer.Finish(agent);
            }
        }

        /// <summary>
        /// Returns the team which wins the game.
        /// </summary>
        /// <returns>The team which wins.</returns>
        Team GetWinner()
        {
            var humanSide = 0;
            var wolfSide = 0;
            var otherSide = 0;
            foreach (var agent in AliveAgentList)
            {
                switch (RoleOf(agent))
                {
                    case Role.FOX:
                        otherSide++;
                        break;
                    case Role.WEREWOLF:
                        wolfSide++;
                        break;
                    default:
                        humanSide++;
                        break;
                }
            }
            if (wolfSide == 0)
            {
                if (otherSide > 0)
                {
                    return Team.OTHERS;
                }
                return Team.VILLAGER;
            }
            else if (humanSide <= wolfSide)
            {
                if (otherSide > 0)
                {
                    return Team.OTHERS;
                }
                return Team.WEREWOLF;
            }
            return Team.UNC;
        }

        void ConsoleLog()
        {
            if (!ShowConsoleLog)
            {
                return;
            }

            var yesterday = gameData.DayBefore;

            Console.WriteLine("=============================================");
            if (yesterday != null)
            {
                Console.WriteLine($"Day {yesterday.Day:00}");
                Console.WriteLine("========talk========");
                foreach (var talk in yesterday.TalkList)
                {
                    Console.WriteLine(talk);
                }
                Console.WriteLine("========Whisper========");
                foreach (var whisper in yesterday.WhisperList)
                {
                    Console.WriteLine(whisper);
                }

                Console.WriteLine("========Actions========");
                foreach (var vote in yesterday.VoteList)
                {
                    Console.WriteLine($"Vote:{vote.Agent}->{vote.Target}");
                }

                foreach (var vote in yesterday.AttackVoteList)
                {
                    Console.WriteLine($"AttackVote:{vote.Agent}->{vote.Target}");
                }

                Console.WriteLine($"{yesterday.Executed} executed");

                var divine = yesterday.Divine;
                if (divine != null)
                {
                    Console.WriteLine($"{divine.Agent} divine {divine.Target}. Result is {divine.Result}");
                }

                if (yesterday.Guard != null)
                {
                    Console.WriteLine($"{yesterday.Guard} guarded");
                }

                if (yesterday.AttackedDead != null)
                {
                    Console.WriteLine($"{yesterday.AttackedDead} attacked");
                }

                if (yesterday.CursedFox != null)
                {
                    Console.WriteLine($"{yesterday.CursedFox} cursed");
                }
            }
            Console.WriteLine("======");
            foreach (var agent in OrderedAgentList)
            {
                Console.Write($"{agent}\t{agentNameMap[agent]}\t{StatusOf(agent)}\t{RoleOf(agent)}");
                if (yesterday != null)
                {
                    if (agent == yesterday.Executed)
                    {
                        Console.Write("\texecuted");
                    }
                    if (agent == yesterday.AttackedDead)
                    {
                        Console.Write("\tattacked");
                    }
                    var divine = yesterday.Divine;
                    if (divine != null && agent == divine.Target)
                    {
                        Console.Write("\tdivined");
                    }
                    var guard = yesterday.Guard;
                    if (guard != null && agent == guard.Target)
                    {
                        Console.Write("\tguarded");
                    }
                    if (agent == yesterday.CursedFox)
                    {
                        Console.Write("\tcursed");
                    }
                }
                Console.WriteLine();
            }
            Console.Write($"Human:{AliveHumanList.Count}\nWerewolf:{AliveWolfList.Count}\n");
            if (gameSetting.RoleNumMap[Role.FOX] != 0)
            {
                Console.Write($"Others:{AliveAgentList.Count(a => RoleOf(a).GetTeam() == Team.OTHERS)}\n");
            }
            Console.WriteLine("=============================================");
        }

        void Night()
        {
            foreach (var agent in AliveAgentList)
            {
                gameServer.DayFinish(agent);
            }

            if (!gameSetting.TalkOnFirstDay && Day == 0)
            {
                Whisper();
            }

            Agent executed = null;
            List<Agent> candidates = null;
            if (Day != 0)
            {
                for (var i = 0; i <= gameSetting.MaxRevote; i++)
                {
                    Vote();
                    candidates = GetVotedCandidates(gameData.VoteList);
                    if (candidates.Count == 1)
                    {
                        executed = candidates[0];
                        break;
                    }
                }

                // In case of multiple candidates.
                if (executed == null && !gameSetting.EnableNoExecution)
                {
                    executed = candidates.Shuffle().First();
                }

                if (executed != null)
                {
                    gameData.Executed = executed;
                    if (GameLogger != null)
                    {
                        GameLogger.Log($"{Day},execute,{executed.AgentIdx},{RoleOf(executed)}");
                    }
                }
            }

            Divine();

            if (Day != 0)
            {
                Whisper();
                Guard();

                Agent attacked = null;
                if (!(AliveWolfList.Count == 1 && RoleOf(gameData.Executed) == Role.WEREWOLF))
                {
                    for (var i = 0; i <= gameSetting.MaxAttackRevote; i++)
                    {
                        if (i > 0 && gameSetting.WhisperBeforeRevote)
                        {
                            Whisper();
                        }
                        AttackVote();
                        candidates = GetAttackVotedCandidates(gameData.AttackVoteList.Where(v => v.Agent != executed).ToList());
                        if (candidates.Count == 1)
                        {
                            attacked = candidates[0];
                            break;
                        }
                    }

                    // In case of multiple candidates.
                    if (attacked == null && !gameSetting.EnableNoAttack)
                    {
                        attacked = candidates.Shuffle().First();
                    }

                    gameData.Attacked = attacked;

                    var guarded = false;
                    if (gameData.Guard != null)
                    {
                        if (gameData.Guard.Target == attacked && attacked != null)
                        {
                            if (gameData.Executed == null || gameData.Executed != gameData.Guard.Agent)
                            {
                                guarded = true;
                            }
                        }
                    }
                    if (!guarded && attacked != null && RoleOf(attacked) != Role.FOX)
                    {
                        gameData.AttackedDead = attacked;
                        gameData.AddLastDeadAgent(attacked);
                        if (GameLogger != null)
                        {
                            GameLogger.Log($"{Day},attack,{attacked.AgentIdx},true");
                        }
                    }
                    else if (attacked != null)
                    {
                        if (GameLogger != null)
                        {
                            GameLogger.Log($"{Day},attack,{attacked.AgentIdx},false");
                        }
                    }
                    else
                    {
                        if (GameLogger != null)
                        {
                            GameLogger.Log($"{Day},attack,-1,false");
                        }
                    }
                }
            }
            gameData = gameData.NextDay;
            gameServer.GameData = gameData;
        }

        List<Agent> GetVotedCandidates(List<Vote> voteList)
        {
            var counter = new Dictionary<Agent, int>();
            foreach (var vote in voteList.Where(v => StatusOf(v.Target) == Status.ALIVE))
            {
                if (counter.ContainsKey(vote.Target))
                {
                    counter[vote.Target]++;
                }
                else
                {
                    counter[vote.Target] = 1;
                }
            }
            var max = counter.Count == 0 ? 0 : counter.Max(p => p.Value);
            return counter.Keys.Where(a => counter[a] == max).ToList();
        }

        List<Agent> GetAttackVotedCandidates(List<Vote> voteList)
        {
            var counter = new Dictionary<Agent, int>();
            foreach (var vote in voteList.Where(v => StatusOf(v.Target) == Status.ALIVE && RoleOf(v.Target) != Role.WEREWOLF))
            {
                if (counter.ContainsKey(vote.Target))
                {
                    counter[vote.Target]++;
                }
                else
                {
                    counter[vote.Target] = 1;
                }
            }
            if (!gameSetting.EnableNoAttack)
            {
                foreach (var agent in AliveHumanList)
                {
                    if (counter.ContainsKey(agent))
                    {
                        counter[agent]++;
                    }
                    else
                    {
                        counter[agent] = 1;
                    }
                }
            }
            var max = counter.Count == 0 ? 0 : counter.Max(p => p.Value);
            return counter.Keys.Where(a => counter[a] == max).ToList();
        }

        void DayStart()
        {
            foreach (var agent in OrderedAgentList)
            {
                gameServer.DayStart(agent);
                if (GameLogger != null)
                {
                    GameLogger.Log($"{Day},status,{agent.AgentIdx},{RoleOf(agent)},{StatusOf(agent)},{agentNameMap[agent]}");
                }
            }
        }

        void Talk()
        {
            foreach (var agent in AliveAgentList)
            {
                gameData.RemainTalkMap[agent] = gameSetting.MaxTalk;
            }

            var skipCounter = new Dictionary<Agent, int>();
            for (var turn = 0; turn < gameSetting.MaxTalkTurn; turn++)
            {
                var talkList = new List<Talk>();
                foreach (var agent in AliveAgentList.Shuffle())
                {
                    var text = Utterance.OVER;
                    if (gameData.RemainTalkMap[agent] > 0)
                    {
                        text = gameServer.RequestTalk(agent);
                    }
                    if (text == null || text.Length == 0)
                    {
                        text = Utterance.SKIP;
                    }
                    else
                    {
                        text = StripText(text);
                    }
                    if (text == Utterance.SKIP)
                    {
                        if (skipCounter.ContainsKey(agent))
                        {
                            skipCounter[agent]++;
                        }
                        else
                        {
                            skipCounter[agent] = 1;
                        }
                        if (skipCounter[agent] > gameSetting.MaxSkip)
                        {
                            text = Utterance.OVER;
                        }
                    }
                    talkList.Add(new Talk(gameData.NextTalkIdx, Day, turn, agent, text));
                    if (text != Utterance.OVER && text != Utterance.SKIP)
                    {
                        skipCounter[agent] = 0;
                    }
                }
                var continueTalk = false;
                foreach (var talk in talkList)
                {
                    gameData.AddTalk(talk.Agent, talk);
                    if (GameLogger != null)
                    {
                        GameLogger.Log($"{Day},talk,{talk.Idx},{talk.Turn},{talk.Agent.AgentIdx},{talk.Text}");
                    }
                    if (talk.Text != Utterance.OVER)
                    {
                        continueTalk = true;
                    }
                }
                if (!continueTalk)
                {
                    break;
                }
            }
        }

        void Whisper()
        {
            // No whisper in case of solo werewolf.
            if (AliveWolfList.Count == 1)
            {
                return;
            }
            foreach (var agent in AliveWolfList)
            {
                gameData.RemainWhisperMap[agent] = gameSetting.MaxWhisper;
            }

            var skipCounter = new Dictionary<Agent, int>();
            for (var turn = 0; turn < gameSetting.MaxWhisperTurn; turn++)
            {
                var whisperList = new List<Whisper>();
                foreach (var agent in AliveWolfList.Shuffle())
                {
                    var text = Utterance.OVER;
                    if (gameData.RemainWhisperMap[agent] > 0)
                    {
                        text = gameServer.RequestWhisper(agent);
                    }
                    if (text == null || text.Length == 0)
                    {
                        text = Utterance.SKIP;
                    }
                    else
                    {
                        text = StripText(text);
                    }
                    if (text == Utterance.SKIP)
                    {
                        if (skipCounter.ContainsKey(agent))
                        {
                            skipCounter[agent]++;
                        }
                        else
                        {
                            skipCounter[agent] = 1;
                        }
                        if (skipCounter[agent] > gameSetting.MaxSkip)
                        {
                            text = Utterance.OVER;
                        }
                    }
                    whisperList.Add(new Whisper(gameData.NextWhisperIdx, Day, turn, agent, text));
                    if (text != Utterance.OVER && text != Utterance.SKIP)
                    {
                        skipCounter[agent] = 0;
                    }
                }
                var continueWhisper = false;
                foreach (var whisper in whisperList)
                {
                    gameData.AddWhisper(whisper.Agent, whisper);
                    if (GameLogger != null)
                    {
                        GameLogger.Log($"{Day},whisper,{whisper.Idx},{whisper.Turn},{ whisper.Agent.AgentIdx},{whisper.Text}");
                    }
                    if (whisper.Text != Utterance.OVER)
                    {
                        continueWhisper = true;
                    }
                }
                if (!continueWhisper)
                {
                    break;
                }
            }
        }

        void Vote()
        {
            gameData.VoteList.Clear();
            foreach (var agent in AliveAgentList)
            {
                var target = gameServer.RequestVote(agent);
                if (target == null || StatusOf(target) == Status.DEAD || agent == target)
                {
                    target = AliveAgentList.Where(a => a != agent).Shuffle().First();
                }
                var vote = new Vote(Day, agent, target);
                gameData.VoteList.Add(vote);
                if (GameLogger != null)
                {
                    GameLogger.Log($"{Day},vote,{agent.AgentIdx},{target.AgentIdx}");
                }
            }
            gameData.LatestVoteList = new List<Vote>(gameData.VoteList);
        }

        void Divine()
        {
            foreach (var agent in AliveAgentList.Where(a => RoleOf(a) == Role.SEER))
            {
                var target = gameServer.RequestDivineTarget(agent);
                var targetRole = RoleOf(target);
                if (target == null || StatusOf(target) == Status.DEAD || targetRole == Role.UNC)
                {
                    // Do nothing.
                }
                else
                {
                    var divine = new Judge(Day, agent, target, targetRole.GetSpecies());
                    gameData.Divine = divine;

                    if (targetRole == Role.FOX)
                    {
                        gameData.AddLastDeadAgent(target);
                        gameData.CursedFox = target;
                    }

                    if (GameLogger != null)
                    {
                        GameLogger.Log($"{Day},divine,{agent.AgentIdx},{target.AgentIdx},{divine.Result}");
                    }
                }
            }
        }

        void Guard()
        {
            foreach (var agent in AliveAgentList.Where(a => RoleOf(a) == Role.BODYGUARD))
            {
                if (agent == gameData.Executed)
                {
                    continue;
                }
                var target = gameServer.RequestGuardTarget(agent);
                if (target == null || agent == target)
                {
                    // Do nothing.
                }
                else
                {
                    var guard = new Guard(Day, agent, target);
                    gameData.Guard = guard;

                    if (GameLogger != null)
                    {
                        GameLogger.Log($"{Day},guard,{agent.AgentIdx},{target.AgentIdx},{RoleOf(target)}");
                    }
                }
            }
        }

        Role RoleOf(Agent agent) => agent == null ? Role.UNC : gameData.RoleMap[agent];
        Status StatusOf(Agent agent) => agent == null ? Status.UNC : gameData.StatusMap[agent];

        void AttackVote()
        {
            gameData.AttackVoteList.Clear();
            foreach (var agent in AliveWolfList)
            {
                var target = gameServer.RequestAttackTarget(agent);
                if (target == null || StatusOf(target) == Status.DEAD || RoleOf(target) == Role.WEREWOLF)
                {
                    // Do nothing.
                }
                else
                {
                    var attackVote = new Vote(Day, agent, target);
                    gameData.AttackVoteList.Add(attackVote);

                    if (GameLogger != null)
                    {
                        GameLogger.Log($"{Day},attackVote,{attackVote.Agent.AgentIdx},{attackVote.Target.AgentIdx}");
                    }
                }
            }
            gameData.LatestAttackVoteList = new List<Vote>(gameData.AttackVoteList);
        }

        static readonly Regex regexStripText = new Regex(@"^Agent\[.+?\] (.+)");

        string StripText(string text)
        {
            var m = regexStripText.Match(text);
            if (m.Success)
            {
                return m.Groups[1].ToString();
            }
            return text;
        }
    }
}
