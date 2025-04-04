using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter.Xml;
using DataAccess.DataContext;
using DataAccess.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class PollFileRepository : IPollRepository
    {
        //Directory for json file and enabling indentation
        private readonly string _filePath = "polls.json";
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        public PollFileRepository()
        {
            //If file doesn't exist, new empty list is made
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]"); 
            }
        }

        public void CreatePoll(Poll poll)
        {
            //Reads json file, and converts it into a list of polls, else a new list is made
            var json = File.ReadAllText(_filePath);
            List<Poll> polls = JsonSerializer.Deserialize<List<Poll>>(json, _jsonOptions) ?? new List<Poll>();

            // Assign new ID
            poll.Id = polls.Any() ? polls.Max(p => p.Id) + 1 : 1;

            polls.Add(poll);


            var serialisedJson = JsonSerializer.Serialize(polls, _jsonOptions);
            File.WriteAllText(_filePath, serialisedJson);
        }

        public IQueryable<Poll> GetPolls()
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Poll>>(json, _jsonOptions).AsQueryable() ?? new List<Poll>().AsQueryable();
        }

        public Poll GetPoll(int id)
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Poll>>(json, _jsonOptions).AsQueryable().SingleOrDefault(p => p.Id == id);
        }

        public bool Vote(int pollId, int selectedOption)
        {
            //The polls are retrieved with the specific option vote count being iterated by 1 and then saved back to the json file
            var polls = GetPolls();
            //This refers to the specific part of the polls list and not a seperate option
            var poll = polls.SingleOrDefault(p => p.Id == pollId);

            if (poll == null)
                return false;

            switch (selectedOption)
            {
                case 1:
                    poll.Option1VotesCount++;
                    break;
                case 2:
                    poll.Option2VotesCount++;
                    break;
                case 3:
                    poll.Option3VotesCount++;
                    break;
                default:
                    return false;
            }

            var serialisedJson = JsonSerializer.Serialize(polls, _jsonOptions);
            File.WriteAllText(_filePath, serialisedJson);
            return true;
        }
    }
}
