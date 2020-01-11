// Copyright 2013-2020 Albert L. Hives
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
namespace HareDu.Extensions
{
    public static class TypeConverterExtensions
    {
        public static ulong ToLong(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return ulong.MaxValue;
            
            if (value.Equals("infinity"))
                return ulong.MaxValue;

            return ulong.TryParse(value, out ulong result) ? result : ulong.MaxValue;
        }
        
        public static string ToRequeueModeString(this RequeueMode mode)
        {
            switch (mode)
            {
                case RequeueMode.DoNotAckRequeue:
                    return "ack_requeue_false";
                
                case RequeueMode.RejectRequeue:
                    return "reject_requeue_true";
                
                case RequeueMode.DoNotRejectRequeue:
                    return "reject_requeue_false";

                default:
                    return "ack_requeue_true";
            }
        }
    }
}