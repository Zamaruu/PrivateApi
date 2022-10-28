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
        public async Task<IActionResult> LinkScraping(int scrappingLimit = 20)
        {
            var linkIndexingResponse = await _webScraper.IndexLinks(scrappingLimit);

            if(linkIndexingResponse.LinkCount >= 1)
            {
                linkIndexingResponse = await _helper.UploadLinks(linkIndexingResponse);

                if (linkIndexingResponse == null) return new ContentResult() { StatusCode = 500, Content = "Error while updateing Links to MongoDB!"};

                await _helper.UploadLinkIndexLog(linkIndexingResponse.HttpResponse());
                Console.WriteLine(linkIndexingResponse.ToString());

                return Ok(linkIndexingResponse.HttpResponse());
            }
            else
            {
                return BadRequest("No links could be Scrapped");
            }
        }

        [HttpPost("DetailScraping")]
        public async Task<IActionResult> DetailScraping(Uri uri)
        {
            if (uri == null) return BadRequest();

            var result = await _webScraper.IndexDetailForUri(uri);
            
            if(result == null) return new ContentResult() { StatusCode = 500, Content = "Error while scarpping html file" };

            result.OriginalLink = uri.ToString();

            var uploaded = await _helper.UploadBottle(result);

            if (uploaded)
            {
                return Ok(result);
            }

            return new ContentResult() { StatusCode = 500, Content = "Error while updateing Bottle to MongoDB!" };
        }
    }
}
