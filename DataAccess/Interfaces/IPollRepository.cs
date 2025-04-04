﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace DataAccess.Interfaces
{
    //Interface here is used for a rough definition for Repository classes
    public interface IPollRepository
    {
        void CreatePoll(Poll poll);
        IQueryable<Poll> GetPolls();
        Poll GetPoll(int id);

        bool Vote(int pollId, int selectedOption);
    }
}
