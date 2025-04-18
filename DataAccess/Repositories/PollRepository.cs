﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter.Xml;
using DataAccess.DataContext;
using DataAccess.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class PollRepository : IPollRepository
    {
        private PollDbContext _pollContext;

        //This is dependency injection
        public PollRepository(PollDbContext pollContext)
        {
            _pollContext = pollContext;
        }

        public void CreatePoll(Poll poll)
        {
            _pollContext.Polls.Add(poll);
            _pollContext.SaveChanges();
        }

        public IQueryable<Poll> GetPolls()
        {
            return _pollContext.Polls;
        }

        public Poll GetPoll(int id)
        {
            return _pollContext.Polls.SingleOrDefault(x => x.Id == id);
        }

        public void AddUsersVoteToPoll(Vote vote)
        {
            _pollContext.Votes.Add(vote);
            _pollContext.SaveChanges();
        }

        public bool UserVotedOnPoll(string voterId, int pollId)
        {
            var vote = _pollContext.Votes.SingleOrDefault(x => x.VoterId == voterId && x.PollId == pollId);

            return vote != null;
        }

        public bool Vote(int pollId, int selectedOption)
        {
            //retrieves poll and adds 1 to the vote count depending on the option selected
            var poll = GetPoll(pollId);
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

            _pollContext.Polls.Update(poll);
            _pollContext.SaveChanges();
            return true;
        }


    }
}
