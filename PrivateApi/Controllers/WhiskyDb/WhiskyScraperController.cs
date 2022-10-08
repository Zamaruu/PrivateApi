using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrivateApi.MongoDB;
using PrivateApi.Sections.WhiskyDb;

namespace PrivateApi.Controllers.WhiskyDb
{
    [Route("[controller]")]
    [ApiController]
    public class WhiskyScraperController : ControllerBase
    {
        private readonly WebScraper _webScraper;
        private readonly WhiskyScraperControllerHelper _helper;

        public WhiskyScraperController(WebScraper webScraper, MongoWhiskyService mwService)
        {
            _webScraper = webScraper;
            _helper = new WhiskyScraperControllerHelper(mwService);
        }

        [HttpPost("LinkScraping")]
        public async Task<IActionResult> LinkScraping()
        {
            var links = await _webScraper.IndexLinks();

            if(links.Count >= 1)
            {
                await _helper.UploadLinks(links);
                return Ok(links);
            }
            else
            {
                return BadRequest("No links could be Scrapped");
            }
        }
    }
}
