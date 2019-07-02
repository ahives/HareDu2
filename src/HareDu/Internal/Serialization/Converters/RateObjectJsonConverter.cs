// Copyright 2013-2019 Albert L. Hives
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
namespace HareDu.Internal.Serialization.Converters
{
    using System;
    using Model;
    using Newtonsoft.Json;

    class RateObjectJsonConverter :
        JsonConverter<Rate>
    {
        public override void WriteJson(JsonWriter writer, Rate value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override Rate ReadJson(JsonReader reader, Type objectType, Rate existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            return existingValue ?? DefaultValueCache.DefaultRate;
        }
    }
}