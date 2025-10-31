using Pathlock.Api.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pathlock.Api.Services
{
    // Simple scheduler: returns an order by resolving dependencies and due date/estimated hours heuristics.
    public class SchedulerService
    {
        public IEnumerable<string> GetRecommendedOrder(SchedulerInput input)
        {
            var tasks = input.tasks;
            // Build simple graph by title -> dependencies
            var dict = tasks.ToDictionary(t => t.title, t => t.dependencies ?? Array.Empty<string>());

            // Kahn's algorithm for topological sort with fallback to estimatedHours/dueDate ordering
            var incoming = new Dictionary<string,int>();
            foreach (var key in dict.Keys) incoming[key] = 0;
            foreach (var kv in dict)
            {
                foreach (var dep in kv.Value)
                {
                    if (incoming.ContainsKey(dep)) incoming[kv.Key]++;
                    // else ignore unknown dependency
                }
            }

            var q = new Queue<string>(incoming.Where(kv => kv.Value == 0).Select(kv => kv.Key));
            var result = new List<string>();
            while (q.Count > 0)
            {
                var cur = q.Dequeue();
                result.Add(cur);
                foreach (var kv in dict.Where(d=>d.Value.Contains(cur)))
                {
                    incoming[kv.Key]--;
                    if (incoming[kv.Key] == 0) q.Enqueue(kv.Key);
                }
            }

            // If cycle exists or incomplete, append remaining tasks sorted by estimatedHours ascending
            if (result.Count != dict.Count)
            {
                var remaining = dict.Keys.Except(result).OrderBy(k => tasks.First(t=>t.title==k).estimatedHours);
                result.AddRange(remaining);
            }

            return result;
        }
    }
}
