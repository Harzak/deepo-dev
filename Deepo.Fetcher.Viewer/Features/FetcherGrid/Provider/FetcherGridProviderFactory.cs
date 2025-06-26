using Deepo.DAL.EF.Models;
using Deepo.DAL.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace Deepo.Fetcher.WPF.Features.FetcherGrid.Provider
{
    internal class FetcherGridProviderFactory : IFetcherGridProviderFactory
    {
        private readonly ILogger _logger;

        private const string movieSubscriptionQuery = "SELECT [Release_Movie_ID] FROM [dbo].[Release_Movie]";
        private const string vinyleSubscriptionQuery = "SELECT [Release_Album_ID] FROM [dbo].[Release_Album]";

        private readonly IReleaseAlbumDBService _releaseAlbumDBService;
        private readonly DEEPOContext _db;
        private readonly string _connstring;

        public FetcherGridProviderFactory(DEEPOContext db, IReleaseAlbumDBService releaseAlbumDBService, ILogger<FetcherGridProviderFactory> logger)
        {
            _db = db;
            _releaseAlbumDBService = releaseAlbumDBService;
            _connstring = _db.Database.GetDbConnection().ConnectionString;
            _logger = logger;
        }

        public IFetcherGridProvider CreateFetcherGridProvider(string id)
        {
            switch (id)
            {
                //todo: removed this 
                case "0d958bae-c459-41fe-ac05-c997c35ec3ea":
                    return new FetcherVinylGridProvider(_connstring, vinyleSubscriptionQuery, _releaseAlbumDBService, _logger);
                case "74302626-f1fb-411d-80f1-586d4f1d74df":
                    return new FetcherMovieGridProvider(_connstring, movieSubscriptionQuery, _logger);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
