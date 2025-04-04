using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter.Xml;
using DataAccess.DataContext;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class PollRepository
    {
        private PollDbContext _pollContext;

        //This is dependency injection
        public PollRepository(PollDbContext pollContext)
        {
            _pollContext = pollContext;
        }

        public void AddPoll(Poll poll)
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

        public bool Vote(int pollId, int selectedOption)
        {
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
