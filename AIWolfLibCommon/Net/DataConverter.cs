using AIWolf.Common.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AIWolf.Common.Net
{
    /// <summary>
    /// Encodes object and decodes packet string.
    /// </summary>
    /// <remarks></remarks>
    public class DataConverter
    {
        static DataConverter converter;

        /// <summary>
        /// A unique instance of this class.
        /// </summary>
        /// <returns>A unique instance of DataConverter class.</returns>
        /// <remarks></remarks>
        public static DataConverter GetInstance()
        {
            if (converter == null)
            {
                converter = new DataConverter();
            }
            return converter;
        }

        JsonSerializerSettings serializerSetting;

        DataConverter()
        {
            serializerSetting = new JsonSerializerSettings();
            // Sort.
            serializerSetting.ContractResolver = new OrderedContractResolver();
            // Do not convert enum into integer.
            serializerSetting.Converters.Add(new StringEnumConverter());
        }

        /// <summary>
        /// Returns the JSON string converted from the object given.
        /// </summary>
        /// <param name="obj">The object to be converted.</param>
        /// <returns>The JSON string converted from the object.</returns>
        /// <remarks></remarks>
        public string Convert(object obj)
        {
            return JsonConvert.SerializeObject(obj, serializerSetting);
        }

        /// <summary>
        /// Returns the instance of Packet class converted from the JSON string given.
        /// </summary>
        /// <param name="line">The JSON string to be converted.</param>
        /// <returns>The instance of Packet class converted from the JSON string.</returns>
        /// <remarks></remarks>
        public Packet ToPacket(string line)
        {
            Dictionary<string, object> map = JsonConvert.DeserializeObject<Dictionary<string, object>>(line, serializerSetting);

            Request request = map["request"] != null ? (Request)Enum.Parse(typeof(Request), (string)map["request"]) : Request.DUMMY;
            GameInfoToSend gameInfoToSend = null;
            if (map["gameInfo"] != null)
            {
                gameInfoToSend = JsonConvert.DeserializeObject<GameInfoToSend>(JsonConvert.SerializeObject(map["gameInfo"], serializerSetting), serializerSetting);
                if (map["gameSetting"] != null)
                {
                    GameSetting gameSetting = JsonConvert.DeserializeObject<GameSetting>(JsonConvert.SerializeObject(map["gameSetting"], serializerSetting), serializerSetting);
                    return new Packet(request, gameInfoToSend, gameSetting);
                }
                else
                {
                    return new Packet(request, gameInfoToSend);
                }
            }
            else if (map["talkHistory"] != null)
            {
                List<TalkToSend> talkHistoryList = ToTalkList(JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(JsonConvert.SerializeObject(map["talkHistory"], serializerSetting), serializerSetting));
                List<TalkToSend> whisperHistoryList = ToTalkList(JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(JsonConvert.SerializeObject(map["whisperHistory"], serializerSetting), serializerSetting));
                return new Packet(request, talkHistoryList, whisperHistoryList);
            }
            else
            {
                return new Packet(request);
            }
        }

        private List<TalkToSend> ToTalkList(List<Dictionary<string, string>> mapList)
        {
            List<TalkToSend> list = new List<TalkToSend>();
            foreach (var value in mapList)
            {
                TalkToSend talk = JsonConvert.DeserializeObject<TalkToSend>(JsonConvert.SerializeObject(value, serializerSetting), serializerSetting);
                list.Add(talk);
            }
            return list;
        }

        /// <summary>
        /// Returns the instance of Agent class converted from the object given.
        /// </summary>
        /// <param name="obj">The object to be converted.</param>
        /// <returns>The agent converted from the object.</returns>
        /// <remarks></remarks>
        public Agent ToAgent(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            if (obj is string)
            {
                Match m = new Regex(@"{""agentIdx"":(\d+)}").Match((string)obj);
                if (m.Success)
                {
                    return Agent.GetAgent(int.Parse(m.Groups[1].Value));
                }
            }
            if (obj is Agent)
            {
                return (Agent)obj;
            }
            else if (obj is Dictionary<string, object>)
            {
                return Agent.GetAgent((int)((Dictionary<string, object>)obj)["agentIdx"]);
            }
            else
            {
                throw new AIWolfRuntimeException("Can not convert to agent " + obj.GetType() + "\t" + obj);
            }
        }
    }

    /// <summary>
    /// Resolves member mappings for a type, camel casing ordered property names. 
    /// </summary>
    /// <remarks></remarks>
    class OrderedCamelCasePropertyNamesContractResolver : CamelCasePropertyNamesContractResolver
    {
        protected override System.Collections.Generic.IList<JsonProperty> CreateProperties(System.Type type, MemberSerialization memberSerialization)
        {
            return base.CreateProperties(type, memberSerialization).OrderBy(p => p.PropertyName).ToList();
        }
    }

    /// <summary>
    /// Resolves a JsonContract for a given Type. 
    /// </summary>
    /// <remarks></remarks>
    class OrderedContractResolver : DefaultContractResolver
    {
        protected override System.Collections.Generic.IList<JsonProperty> CreateProperties(System.Type type, MemberSerialization memberSerialization)
        {
            return base.CreateProperties(type, memberSerialization).OrderBy(p => p.PropertyName).ToList();
        }
    }
}
