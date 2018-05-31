﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fido2NetLib;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Fido2Demo
{
    [Route("api/[controller]")]
    public class MyController : Controller
    {
        // todo: Add proper config
        private Fido2NetLib _lib = new Fido2NetLib(new Fido2NetLib.Configuration {
            ServerDomain = "localhost",
            Origin = "https://localhost:44329"
        });

        // GET: api/<controller>
        [HttpPost]
        [Route("/attestation/options")]
        public fido2NetLib.OptionsResponse Get([FromBody] Createdto dto)
        {

            User user = new User
            {
                DisplayName = dto.DisplayName,
                Name = dto.Username,
                Id = "ABC"
            };

            var challenge = _lib.CreateCredentialChallenge(user);
            HttpContext.Session.SetString("fido2.challenge", JsonConvert.SerializeObject(challenge));

            return challenge;
        }

        [HttpPost]
        [Route("/attestation/result")]
        public Fido2NetLib.CreationResult HandleResult([FromBody] AuthenticatorAttestationRawResponse attestionResponse)
        {
            var json = HttpContext.Session.GetString("fido2.challenge");
            var origChallenge = JsonConvert.DeserializeObject<OptionsResponse>(json);

            var res = _lib.CreateCredentialResult(attestionResponse, origChallenge);
            return res;
            
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

    public class Createdto
    {
        [JsonProperty(PropertyName = "displayName")]
        public string DisplayName { get; set; }
        public string Username { get; set; }
    }
}