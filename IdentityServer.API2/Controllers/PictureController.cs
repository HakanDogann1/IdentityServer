using IdentityServer.API2.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace IdentityServer.API2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PictureController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IActionResult GetPictures()
        {
            var pictureList = new List<Picture>()
            {
                new Picture() {PictureId=1,Url="test"},
                new Picture() {PictureId=2,Url="deneme"},
                new Picture() {PictureId=3,Url="dene"},
                new Picture() {PictureId=4,Url="testtt"},
                new Picture() {PictureId=5,Url="test"},
            };

            return Ok(pictureList);
        }
    }
}
