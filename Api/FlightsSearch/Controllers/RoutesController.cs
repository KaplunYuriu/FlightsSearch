﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlightsSearch.Entities;
using FlightsSearch.Providers;
using FlightsSearch.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightsSearch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        private readonly IAirportsProvider _airportsProvider;
        private readonly IRoutesService _routesService;

        public RoutesController(IAirportsProvider airportsProvider, IRoutesService routesService)
        {
            _airportsProvider = airportsProvider;
            _routesService = routesService;
        }

        [HttpGet("{alias}")]
        public async Task<ActionResult<List<Route>>> Index(string alias)
        {
            var airport = await _airportsProvider.GetAirport(alias);

            if (airport == null)
                throw new ArgumentException($"Airport with alias '{alias}' doesn't exist.");

            return await airport.GetRoutes();
        }

        [HttpGet("between")]
        public async Task<ActionResult<List<Route>>> Between(string departure, string destination)
        {
            var departureAirport = await _airportsProvider.GetAirport(departure);
            var destinationAirport = await _airportsProvider.GetAirport(destination);

            if (departureAirport == null)
                throw new ArgumentException($"Airport with alias '{departure}' doesn't exist.");

            if (destinationAirport == null)
                throw new ArgumentException($"Airport with alias '{destination}' doesn't exist.");

            return await _routesService.GetRoutesBetween(departureAirport, destinationAirport).ConfigureAwait(false);
        }
    }
}