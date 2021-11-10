﻿using Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tests
{
    public class FakePlayer : IPlayer
    {
        public List<string> Playlist { get; } = new();
        public Task Blimp()
        {
            Playlist.Add("blimp");
            return Task.CompletedTask;
        }

        public Task Say(string culture, string text)
        {
            Playlist.Add(culture + ": " + text);
            return Task.CompletedTask;
        }

        internal string[] EvictPlaylist()
        {
            var result = Playlist.ToArray();
            Playlist.Clear();
            return result;  
        }
    }
}