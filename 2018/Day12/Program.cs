﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Day12
{
    class Program
    {
        static void Main(string[] args)
        {
            var sm = StateMachine.Parse(File.ReadAllText("Input.txt"));
            while (sm.Gen() < 21)
            {
                Console.WriteLine(sm.Get());
                sm.Next();
            }
            Console.Read();
        }
    }

    class StateMachine
    {
        readonly Dictionary<string, string> rules = new Dictionary<string, string>();
        string state;
        int gen;
        public int Gen() => gen;
        public string Get() => $"[{gen}] {state}";
        public void Next()
        {
            gen++;
            var newState = new StringBuilder();
            for (var i = 0; i < state.Length - 4; i++)
            {
                var seg = state.Substring(i, 5);
                if (rules.ContainsKey(seg))
                {
                    newState.Append(rules[seg]);
                }
                else
                {
                    newState.Append(".");
                }
            }
            state = $"...{newState.ToString().Trim('.')}...";
        }
        public static StateMachine Parse(string data)
        {
            var sm = new StateMachine();
            var reader = new StringReader(data);
            sm.state = $"...{reader.ReadLine().Split(':').Last().Trim()}...";
            reader.ReadLine();
            string rule;
            while (!string.IsNullOrWhiteSpace(rule = reader.ReadLine()))
            {
                var ruleSeg = rule.Split("=>").Select(seg => seg.Trim());
                sm.rules.Add(ruleSeg.First(), ruleSeg.Last());
            }
            return sm;
        }
    }
}
