using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entites;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class OpinionController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public OpinionController(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpPost("addOpinion")]
        public async Task<ActionResult<Opinion>> AddOpinion(OpinionDto opinionsDto)
        {
            var currentUser = HttpContext.User;
            var senderId = "";
            if(currentUser.HasClaim(c => c.Type == "userId"))
            {
                senderId = currentUser.Claims.FirstOrDefault(c => c.Type == "userId").Value.ToString(); 
            }
            DateTime date =DateTime.Now;

            var user = _context.Users.SingleOrDefault(u => u.ID == opinionsDto.UserId);
            if(user != null)
            {
                var opinion = new Opinion
                {
                    UserId = opinionsDto.UserId,
                    SenderId = Int32.Parse(senderId),
                    OpinionValue = opinionsDto.OpinionValue,
                    OpinionDescription = opinionsDto.OpinionDescription,
                    Date = date
                };
                _context.Opinions.Add(opinion);
                await _context.SaveChangesAsync();
                return Ok(opinion);
            }
            return BadRequest("User does not exist");
        }
    }
}