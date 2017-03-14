using AIWolf.Common.Data;

namespace AIWolf.Client.Lib
{
    /// <summary>
    /// Factory class to create talk contents.
    /// </summary>
    /// <remarks></remarks>
    public class TemplateTalkFactory
    {
        TemplateTalkFactory() { }

        /// <summary>
        /// Returns talk about estimation.
        /// </summary>
        /// <param name="target">The agent estimated.</param>
        /// <param name="role">The estimated role.</param>
        /// <returns>Talk about estimation.</returns>
        /// <remarks></remarks>
        public static string Estimate(Agent target, Role role)
        {
            string[] split = { Topic.ESTIMATE.ToString(), (target != null) ? target.ToString() : "null", role.ToString() };
            return WordAttachment(split);
        }

        /// <summary>
        /// Returns talk about comingout.
        /// </summary>
        /// <param name="target">The agent who playing the role.</param>
        /// <param name="role">The role which the agent is playing.</param>
        /// <returns>Talk about comingout that which role someone is playing.</returns>
        /// <remarks></remarks>
        public static string Comingout(Agent target, Role role)
        {
            string[] split = { Topic.COMINGOUT.ToString(), (target != null) ? target.ToString() : "null", role.ToString() };
            return WordAttachment(split);
        }

        /// <summary>
        /// Returns talk about divination.
        /// </summary>
        /// <param name="target">The agent divined.</param>
        /// <param name="species">The species which the divined agent is found to be.</param>
        /// <returns>Talk about divination.</returns>
        /// <remarks></remarks>
        public static string Divined(Agent target, Species species)
        {
            string[] split = { Topic.DIVINED.ToString(), (target != null) ? target.ToString() : "null", species.ToString() };
            return WordAttachment(split);
        }

        /// <summary>
        /// Returns talk about inquest.
        /// </summary>
        /// <param name="target">The agent inquired.</param>
        /// <param name="species">The species which the inquired agent is found to be.</param>
        /// <returns>Talk about inquest.</returns>
        /// <remarks></remarks>
        public static string Inquested(Agent target, Species species)
        {
            string[] split = { Topic.INQUESTED.ToString(), (target != null) ? target.ToString() : "null", species.ToString() };
            return WordAttachment(split);
        }

        /// <summary>
        /// Returns talk about guard.
        /// </summary>
        /// <param name="target">The agent guarded.</param>
        /// <returns>Talk about guard.</returns>
        /// <remarks></remarks>
        public static string Guarded(Agent target)
        {
            string[] split = { Topic.GUARDED.ToString(), (target != null) ? target.ToString() : "null" };
            return WordAttachment(split);
        }

        /// <summary>
        /// Returns talk about vote for execution.
        /// </summary>
        /// <param name="target">The agent whom the talker wants to execute.</param>
        /// <returns>Talk about vote for execution.</returns>
        /// <remarks></remarks>
        public static string Vote(Agent target)
        {
            string[] split = { Topic.VOTE.ToString(), (target != null) ? target.ToString() : "null" };
            return WordAttachment(split);
        }

        /// <summary>
        /// Returns talk about agreement.
        /// </summary>
        /// <param name="talkType">TALK/WHISPER.</param>
        /// <param name="day">The day of talk/whisper.</param>
        /// <param name="id">Index number of talk/whisper.</param>
        /// <returns>Talk about agreement.</returns>
        /// <remarks></remarks>
        public static string Agree(TalkType talkType, int day, int id)
        {
            string[] split = { Topic.AGREE.ToString(), talkType.ToString(), "day" + day, "ID:" + id };
            return WordAttachment(split);
        }

        /// <summary>
        /// Returns talk about disagreement.
        /// </summary>
        /// <param name="talkType">TALK/WHISPER.</param>
        /// <param name="day">The day of talk/whisper.</param>
        /// <param name="id">Index number of talk/whisper.</param>
        /// <returns>Talk about disagreement.</returns>
        /// <remarks></remarks>
        public static string Disagree(TalkType talkType, int day, int id)
        {
            string[] split = { Topic.DISAGREE.ToString(), talkType.ToString(), "day" + day, "ID:" + id };
            return WordAttachment(split);
        }

        /// <summary>
        /// There is nothing to talk.
        /// </summary>
        /// <returns>String "Over".</returns>
        /// <remarks></remarks>
        public static string Over()
        {
            return Talk.OVER;
        }

        /// <summary>
        /// Skip this turn though there is something to talk.
        /// </summary>
        /// <returns>String "Skip".</returns>
        /// <remarks></remarks>
        public static string Skip()
        {
            return Talk.SKIP;
        }

        private static string WordAttachment(string[] split)
        {
            var answer = "";
            for (var i = 0; i < split.Length; i++)
            {
                answer += split[i] + " ";
            }
            return answer.Trim();
        }
    }
}
