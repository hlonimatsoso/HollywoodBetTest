using HollywoodBetTest.Data;
using HollywoodBetTest.Data.Repositories;
using HollywoodBetTest.Interfaces.Services;
using HollywoodBetTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HollywoodBetTest.Api.Services
{
    public class TournamentService : BaseService<Tournament>,ITournamentService
    {
        public TournamentService(BaseRepository<Tournament> repo) : base(repo)
        { }

        public Tournament GetTournamentByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
