//
// DataConverter.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace AIWolf.Lib
{
#if JHELP
    /// <summary>
    /// オブジェクトとJSON文字列を相互に変換する
    /// </summary>
#else
    /// <summary>
    /// Converts object into JSON string and vice versa.
    /// </summary>
#endif
    public static class DataConverter
    {
        static JsonSerializerSettings serializerSetting = new JsonSerializerSettings
        {
            Converters = new List<JsonConverter>
            {
                // Do not convert enum into integer.
                new StringEnumConverter()
            }
        };

#if JHELP
        /// <summary>
        /// オブジェクトをJSON文字列に変換する
        /// </summary>
        /// <param name="obj">変換するオブジェクト</param>
        /// <returns>オブジェクトを変換したJSON文字列</returns>
#else
        /// <summary>
        /// Serializes the given object into the JSON string.
        /// </summary>
        /// <param name="obj">The object to be serialized.</param>
        /// <returns>The JSON string serialized from the given object.</returns>
#endif
        public static string Serialize(object obj) => JsonConvert.SerializeObject(obj, serializerSetting);

#if JHELP
        /// <summary>
        /// JSON文字列をT型のオブジェクトに変換する
        /// </summary>
        /// <typeparam name="T">返されるオブジェクトの型</typeparam>
        /// <param name="json">変換するJSON文字列</param>
        /// <returns>JSON文字列を変換したT型のオブジェクト</returns>
#else
        /// <summary>
        /// Deserializes the given JSON string into the object of type T.
        /// </summary>
        /// <typeparam name="T">The type of object returned.</typeparam>
        /// <param name="json">The JSON string to be deserialized.</param>
        /// <returns>The object of type T deserialized from the JSON string.</returns>
#endif
        public static T Deserialize<T>(string json) => JsonConvert.DeserializeObject<T>(json, serializerSetting);
    }
}
