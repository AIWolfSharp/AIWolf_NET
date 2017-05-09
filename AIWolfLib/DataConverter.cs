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
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Collections.Generic;

namespace AIWolf.Lib
{
    /// <summary>
    /// Encodes object and decodes packet string.
    /// </summary>
    public static class DataConverter
    {
        static JsonSerializerSettings serializerSetting;

        /// <summary>
        /// Initializes this class.
        /// </summary>
        static DataConverter()
        {
            serializerSetting = new JsonSerializerSettings()
            {
                // Sort.
                ContractResolver = new OrderedContractResolver()
            };
            // Do not convert enum into integer.
            serializerSetting.Converters.Add(new StringEnumConverter());
        }

        /// <summary>
        /// Serializes the given object into the JSON string.
        /// </summary>
        /// <param name="obj">The object to be serialized.</param>
        /// <returns>The JSON string serialized from the given object.</returns>
        public static string Serialize(object obj) => JsonConvert.SerializeObject(obj, serializerSetting);

        /// <summary>
        /// Deserializes the given JSON string into the object of type T.
        /// </summary>
        /// <typeparam name="T">The type of object returned.</typeparam>
        /// <param name="json">The JSON string to be deserialized.</param>
        /// <returns>The object of type T deserialized from the JSON string.</returns>
        public static T Deserialize<T>(string json) => JsonConvert.DeserializeObject<T>(json, serializerSetting);

        class OrderedContractResolver : DefaultContractResolver
        {
            protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
                => base.CreateProperties(type, memberSerialization).OrderBy(p => p.PropertyName).ToList();
        }
    }
}
