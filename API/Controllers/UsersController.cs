using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entites;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await _userRepository.GetUsersAsync();
            var userToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);
            return Ok(userToReturn);
       
        }

        [HttpGet("{data}")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersByData(string data)
        {
            var user = await _userRepository.GetUsersByDataAsync(data);
            var userToReturn = _mapper.Map<IEnumerable<MemberDto>>(user);
            return Ok(user);
        }
        [HttpGet("user/{hash}")]
        public async Task<ActionResult<MemberDto>> GetUser(string hash)
        {
            int hashInt = Int32.Parse(hash);
            var user = await _userRepository.GetUserByHash(hashInt);
            if(user != null)
            {
                return _mapper.Map<MemberDto>(user);
            }

            return BadRequest("User does not exist");
        }

    }
}