/*
 * home
 *
 * The API for the Home Starter project
 *
 * OpenAPI spec version: 1.0.2
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json;
using IO.Swagger.Attributes;
using IO.Swagger.Models;

namespace IO.Swagger.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    public class RemoteApiController : Controller
    { 
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>returns ir code from remote</remarks>
        /// <param name="remote">Lirc remote</param>
        /// <param name="code">ir code</param>
        /// <response code="200">All the codes</response>
        [HttpGet]
        [Route("/motta/home/1.0.1/remotes/{remote}/{code}")]
        [ValidateModelState]
        [SwaggerOperation("GetRemoteCode")]
        [SwaggerResponse(200, typeof(List<string>), "All the codes")]
        public virtual IActionResult GetRemoteCode([FromRoute]string remote, [FromRoute]string code)
        { 
            string exampleJson = null;
            
            var example = exampleJson != null
            ? JsonConvert.DeserializeObject<List<string>>(exampleJson)
            : default(List<string>);
            return new ObjectResult(example);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>returns all ir codes from remote</remarks>
        /// <param name="remote">Lirc remote</param>
        /// <response code="200">All the codes</response>
        [HttpGet]
        [Route("/motta/home/1.0.1/remotes/{remote}")]
        [ValidateModelState]
        [SwaggerOperation("GetRemoteCodes")]
        [SwaggerResponse(200, typeof(List<string>), "All the codes")]
        public virtual IActionResult GetRemoteCodes([FromRoute]string remote)
        { 
            string exampleJson = null;
            
            var example = exampleJson != null
            ? JsonConvert.DeserializeObject<List<string>>(exampleJson)
            : default(List<string>);
            return new ObjectResult(example);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>returns all installed remotes</remarks>
        /// <param name="skip">number of records to skip</param>
        /// <param name="limit">max number of records to return</param>
        /// <response code="200">All the installed remotes</response>
        [HttpGet]
        [Route("/motta/home/1.0.1/remotes")]
        [ValidateModelState]
        [SwaggerOperation("GetRemotes")]
        [SwaggerResponse(200, typeof(List<string>), "All the installed remotes")]
        public virtual IActionResult GetRemotes([FromQuery]int? skip, [FromQuery]int? limit)
        { 
            string exampleJson = null;
            
            var example = exampleJson != null
            ? JsonConvert.DeserializeObject<List<string>>(exampleJson)
            : default(List<string>);
            return new ObjectResult(example);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>flashes ir code simulating the remote control</remarks>
        /// <param name="remote">Lirc remote</param>
        /// <param name="code">ir code</param>
        /// <response code="200">response</response>
        [HttpPost]
        [Route("/motta/home/1.0.1/remotes/{remote}/{code}")]
        [ValidateModelState]
        [SwaggerOperation("SendRemoteCode")]
        [SwaggerResponse(200, typeof(ApiResponse), "response")]
        public virtual IActionResult SendRemoteCode([FromRoute]string remote, [FromRoute]string code)
        { 
            string exampleJson = null;
            
            var example = exampleJson != null
            ? JsonConvert.DeserializeObject<ApiResponse>(exampleJson)
            : default(ApiResponse);
            return new ObjectResult(example);
        }
    }
}
