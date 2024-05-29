using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace DistributionManager.Domain.Helper.JsonDiff
{
    public static class JsonCompare
    {
        /// <summary>
        /// Compares two JSON strings and returns the differences.
        /// </summary>
        /// <param name="beforeJson">The JSON string representing the initial state. Must be a valid JSON serialized.</param>
        /// <param name="afterJson">The JSON string representing the final state. Must be a valid JSON serialized.</param>
        /// <returns>A <see cref="DifferenceDetails"/> object containing all differences between the two JSON strings.</returns>
        public static DifferenceDetails Compare(string beforeJson, string afterJson, List<string>? ignoredKeys = null)
        {
            var differences = new DifferenceDetails();
            var beforeJObject = default(JObject);
            var afterJObject = default(JObject);

            if (string.IsNullOrEmpty(beforeJson)|| beforeJson == "null")
            {
                afterJObject = string.IsNullOrEmpty(afterJson) ? new JObject() : JObject.Parse(afterJson);

                foreach (var property in afterJObject.Properties())
                {
                    var afterProp = afterJObject[property.Name];
                    differences.Differences[property.Name] = new Changes { Inserted = new List<object> { afterProp.ToObject<object>() } };
                }
                return differences;
            }

            beforeJObject = JObject.Parse(beforeJson);
            afterJObject = string.IsNullOrEmpty(afterJson) ? new JObject() : JObject.Parse(afterJson);

            foreach (var property in beforeJObject.Properties().Union(afterJObject.Properties()).Select(p => p.Name).Distinct())
            {
                if (!(ignoredKeys ?? Enumerable.Empty<string>()).Contains(property))
                {
                    var beforeProp = beforeJObject[property];
                    var afterProp = afterJObject[property];

                    if (beforeProp == null)
                    {
                        differences.Differences[property] = new Changes { Inserted = new List<object> { afterProp.ToObject<object>() } };
                    }
                    else if (afterProp == null)
                    {
                        differences.Differences[property] = new Changes { Removed = new List<object> { beforeProp.ToObject<object>() } };
                    }
                    else if (!JToken.DeepEquals(beforeProp, afterProp))
                    {
                        if (beforeProp.Type == JTokenType.Array && afterProp.Type == JTokenType.Array)
                        {
                            var beforeItems = beforeProp.ToObject<List<JObject>>();
                            var afterItems = afterProp.ToObject<List<JObject>>();
                            var removedItems = beforeItems.Except(afterItems, new JObjectComparer()).ToList();
                            var addedItems = afterItems.Except(beforeItems, new JObjectComparer()).ToList();

                            if (removedItems.Count > 0)
                            {
                                differences.Differences[property] = new Changes { Removed = removedItems.Cast<object>().ToList() };
                            }
                            if (addedItems.Count > 0)
                            {
                                if (differences.Differences.ContainsKey(property))
                                    differences.Differences[property].Inserted = addedItems.Cast<object>().ToList();
                                else
                                    differences.Differences[property] = new Changes { Inserted = addedItems.Cast<object>().ToList() };
                            }
                        }
                        else
                        {
                            differences.Differences[property] = new Changes { Updated = new List<object> { afterProp.ToObject<object>() } };
                        }
                    }
                }
            }

            return differences;
        }

        public static bool HasDifference(string simpleBefore, string simpleAfter, List<string>? ignoredKeys = null)
        {
            var diff = default(DifferenceDetails);
            

            if (!(string.IsNullOrEmpty(simpleBefore)|| string.IsNullOrEmpty(simpleAfter)))
            {
                diff = JsonCompare.Compare(simpleBefore, simpleAfter, ignoredKeys);
            }

            return diff.Differences.Any();
        }

        public class Changes
        {
            public List<object> Inserted { get; set; }
            public List<object> Updated { get; set; }
            public List<object> Removed { get; set; }
        }

        public class DifferenceDetails
        {
            public Dictionary<string, Changes> Differences { get; set; }

            public DifferenceDetails()
            {
                Differences = new Dictionary<string, Changes>();
            }
        }

        public class JObjectComparer : IEqualityComparer<JObject>
        {
            public bool Equals(JObject x, JObject y)
            {
                return JToken.DeepEquals(x, y);
            }

            public int GetHashCode(JObject obj)
            {
                return obj.ToString().GetHashCode();
            }
        }
    }
}
