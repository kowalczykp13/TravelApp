using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entites;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class TripsController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public TripsController(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }


        [HttpGet]
        public async Task<IEnumerable<Trip>> getTrips()
        {
            return await _context.Trips.ToListAsync();
        }


        [HttpPost("addTrip")]
        public async Task<ActionResult<Trip>> addTrip(TripDto tripDto)
        {
            var currentUser = HttpContext.User;
            var userId = "";
            if(currentUser.HasClaim(c => c.Type == "userId"))
            {
                userId = currentUser.Claims.FirstOrDefault(c => c.Type == "userId").Value.ToString(); 
            }
            if(!String.IsNullOrEmpty(userId))
            {
                var user = await _context.Users.SingleOrDefaultAsync(x => x.ID.ToString() == userId);
                if(user != null)
                {
                    var trip = new Trip
                    {
                        CreatorId = user.ID,
                        Price = tripDto.Price,
                        StartTime = tripDto.StartTime,
                        StartFrom = tripDto.StartFrom,
                        EndIn = tripDto.EndIn
                    };
                
                _context.Trips.Add(trip);
                await _context.SaveChangesAsync();
                return Ok();
                }
                return BadRequest("User does not exist");
            }
            else
            {
                return BadRequest("User does not exist");
            }
        }


        [HttpDelete("deleteTrip/{tripId}")]
        public async Task<ActionResult<Trip>> deleteTrip(int tripId)
        {
            var trip = await _context.Trips.SingleOrDefaultAsync(x => x.Id == tripId);
            if(trip != null)
            {
                _context.Trips.Remove(trip);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest("Trip does not exist");
            }
        }


       [HttpPost("addPassenger")]
       public async Task<ActionResult<Trip>> addPassenger(PassengerDto passengerDto)
       {
            var currentUser = HttpContext.User;
            var userId = "";
            if(currentUser.HasClaim(c => c.Type == "userId"))
            {
                userId = currentUser.Claims.FirstOrDefault(c => c.Type == "userId").Value.ToString(); 
            }
           var user = await _context.Users.SingleOrDefaultAsync(x => x.ID.ToString() == userId);
           var trip = await _context.Trips.SingleOrDefaultAsync(x => x.Id == passengerDto.TripId);
           var tripCreator = await _context.Trips.SingleOrDefaultAsync(x => x.Id == passengerDto.UserId);
           if(user != null && trip != null && tripCreator == null)
           {
               var passenger = new Passenger(passengerDto.UserId, passengerDto.TripId);
               _context.Passenger.Add(passenger);
               await _context.SaveChangesAsync();
               return Ok(); 
           }
           return BadRequest();
       }


       [HttpDelete("deletePassenger")]
        public async Task<ActionResult<Trip>> deletePassenger(PassengerDto passengerDto)
       {
           var user = await _context.Users.SingleOrDefaultAsync(x => x.ID == passengerDto.UserId);
           var trip = await _context.Trips.SingleOrDefaultAsync(x => x.Id == passengerDto.TripId);
           
           if(user != null && trip != null)
           {
               var passenger = await _context.Passenger.SingleOrDefaultAsync(x => x.UserId == passengerDto.UserId && x.TripId == passengerDto.TripId);
               if(passenger != null)
               {
                    _context.Passenger.Remove(passenger);
                    await _context.SaveChangesAsync();
                    return Ok(); 
               }
           }
           return BadRequest();
       }

       [HttpGet("{tripId}")]
       public async Task<ActionResult<TripInfoDto>> GetTripById(int tripId)
       {
           var trip = await _context.Trips.Where(t => t.Id == tripId).FirstOrDefaultAsync();
           if(trip != null)
           {
               List<AppUser> users = new List<AppUser>();
               var passenger = _context.Passenger.Where(p => p.TripId == trip.Id).ToList();
               var owner = await _context.Users.Where(u => u.ID == trip.CreatorId).FirstOrDefaultAsync();
               foreach(var p in passenger)
               {
                    var user = await _context.Users.Where(u => p.UserId == u.ID).FirstOrDefaultAsync();
                    if(user != null)
                    {
                        users.Add(user);
                    }
               }
               var usersToReturn = _mapper.Map<List<MemberDto>>(users);
               var userToReturn = _mapper.Map<MemberDto>(owner);
               var tripInfo = new TripInfoDto()
               {
                   Creator = userToReturn,
                   Passenger = usersToReturn,
                   Price = trip.Price,
                   StartTime = trip.StartTime,
                   StartFrom = trip.StartFrom,
                   EndIn = trip.EndIn

               };
               return Ok(tripInfo);
           }
           return BadRequest("Trip does not exist");
       }


    }
}