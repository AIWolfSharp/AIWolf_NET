﻿//
// Contents.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//


namespace AIWolf.Lib
{
    /// <summary>
    /// Contents of talk/whisper.
    /// </summary>
    public class Contents
    {
        /// <summary>
        /// The topic of this talk/whisper.
        /// </summary>
        /// <remarks>DUMMY means invalid talk/whisper.</remarks>
        public Topic Topic { get; } = Topic.DUMMY;

        /// <summary>
        /// The target agent mentioned in this talk/whisper.
        /// </summary>
        /// <remarks>Required except AGREE and DISAGREE.</remarks>
        public Agent Target { get; }

        /// <summary>
        /// The role mentioned in this talk/whisper.
        /// </summary>
        /// <remarks>Required on ESTIMATE and COMINGOUT.</remarks>
        public Role Role { get; } = Role.UNC;

        /// <summary>
        /// The species mentioned in this talk/whisper.
        /// </summary>
        /// <remarks>Required on DIVINED and INQUESTED.</remarks>
        public Species Species { get; } = Species.UNC;

        /// <summary>
        /// The talk/whisper mentioned in this talk/whisper.
        /// </summary>
        /// <remarks>Required on AGREE and DISAGREE.</remarks>
        public Talk Talk { get; }

        /// <summary>
        /// Initializes a new instance of contents having topic of skip and over.
        /// </summary>
        /// <param name="topic"></param>
        public Contents(Topic topic)
        {
            if (topic == Topic.Skip || topic == Topic.Over)
            {
                Topic = topic;
            }
            else
            {
                Error.RuntimeError("Can not initialize by this constructor in case of " + topic + ".");
                Error.Warning("Force topic to be DUMMY.");
            }
        }

        /// <summary>
        /// Initializes a new instance of contents having topic of estimation and comingout.
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="target"></param>
        /// <param name="role"></param>
        public Contents(Topic topic, Agent target, Role role)
        {
            if (topic == Topic.ESTIMATE || topic == Topic.COMINGOUT)
            {
                Topic = topic;
                Target = target;
                Role = role;
            }
            else
            {
                Error.RuntimeError("Can not initialize by this constructor in case of " + topic + ".");
                Error.Warning("Force topic to be DUMMY.");
            }
        }

        /// <summary>
        /// Initializes a new instance of contents having topic of divination and inquest.
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="target"></param>
        /// <param name="species"></param>
        public Contents(Topic topic, Agent target, Species species)
        {
            if (topic == Topic.DIVINED || topic == Topic.INQUESTED)
            {
                Topic = topic;
                Target = target;
                Species = species;
            }
            else
            {
                Error.RuntimeError("Can not initialize by this constructor in case of " + topic + ".");
                Error.Warning("Force topic to be DUMMY.");
            }
        }

        /// <summary>
        /// Initializes a new instance of contents having topic of attack, guard and vote.
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="target"></param>
        public Contents(Topic topic, Agent target)
        {
            if (topic == Topic.ATTACK || topic == Topic.GUARDED || topic == Topic.VOTE)
            {
                Topic = topic;
                Target = target;
            }
            else
            {
                Error.RuntimeError("Can not initialize by this constructor in case of " + topic + ".");
                Error.Warning("Force topic to be DUMMY.");
            }
        }

        /// <summary>
        /// Initializes a new instance of contents having topic of agreement and disagreement.
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="talk"></param>
        public Contents(Topic topic, Talk talk)
        {
            if (topic == Topic.AGREE || topic == Topic.DISAGREE)
            {
                Topic = topic;
                Talk = talk;
            }
            else
            {
                Error.RuntimeError("Can not initialize by this constructor in case of " + topic + ".");
                Error.Warning("Force topic to be DUMMY.");
            }
        }

        public override string ToString()
        {
            switch (Topic)
            {
                case Topic.DUMMY:
                case Topic.Skip:
                case Topic.Over:
                    return Topic.ToString();
                case Topic.ESTIMATE:
                case Topic.COMINGOUT:
                    return Topic + ": target=" + Target + " role=" + Role;
                case Topic.DIVINED:
                case Topic.INQUESTED:
                    return Topic + ": target=" + Target + " species=" + Species;
                case Topic.GUARDED:
                case Topic.VOTE:
                case Topic.ATTACK:
                    return Topic + ": target=" + Target;
                case Topic.AGREE:
                case Topic.DISAGREE:
                    return Topic + ": talk/whisper=" + Talk;
                default:
                    return "";
            }
        }
    }
}
