using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrivateApi.Data.ObjectModels.Whisky;
using PrivateApi.Data.ResponseModels;
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

        [HttpPost("ScrapBottles")]
        public async Task<IActionResult> ScrapWhiskyBottlesFromLinks()
        {
            var whiskyLinks = await _helper.GetWhiskyDetailLinks();
            var scrappedBottles = new List<WhiskyBottleDetail>();

            var scrappedBottlesCount = 0;
            var savedBottlesCount = 0;

            var pagedLinks = new List<WhiskyDetailLink>();
            var linkPage = 0;

            do
            {
                pagedLinks = _helper.GetPage<WhiskyDetailLink>(whiskyLinks, linkPage).ToList();

                foreach (var link in pagedLinks)
                {
                    var uri = new Uri(link.OriginalLink);
                    var result = await _webScraper.IndexDetailForUri(uri);

                    if (result != null)
                    {
                        scrappedBottles.Add(result);
                        scrappedBottlesCount++;
                        Console.WriteLine($"{scrappedBottlesCount}. \t{uri} was successfully scrapped!");
                    }
                }

             
                foreach (var bottle in scrappedBottles)
                {
                    var uploaded = await _helper.UploadBottle(bottle);
                    if (uploaded)
                    {
                        savedBottlesCount++;
                        Console.WriteLine($"{savedBottlesCount}. \t{bottle.OriginalLink} was successfully saved to MongoDB!");
                    }
                }

                if(scrappedBottles.Count >= 1)
                {
                    Console.WriteLine("--------------------------------------------");
                    Console.WriteLine($"Set completeted, jumping to next Page. {scrappedBottles.Count} Whisky Bottles were scraped.");
                    Console.WriteLine("--------------------------------------------");
                }

                // Reset scrapped bottles for better memeory management and get next page
                scrappedBottles.Clear();
                linkPage++;       
            } while (pagedLinks.Count >= 1);

            Console.WriteLine($"{scrappedBottlesCount} Whisky Bottles were scraped and a total of {savedBottlesCount} Bottles were Saved to MongoDB!");
            Console.WriteLine("--------------------------------------------");

            var bottleScrappingResponse = new BottleScrappingResponse(scrappedBottlesCount, scrappedBottlesCount - savedBottlesCount, savedBottlesCount);
            await _helper.UploadWhiskyScrappingLog(bottleScrappingResponse);

            return Ok(bottleScrappingResponse);
        }
    
        [HttpGet("Bottles")]
        public async Task<IActionResult> GetAllBottles()
        {
            var result = await _helper.GetWhiskyBottles();

            if (result != null) return Ok(result);
            return BadRequest();
        }
    }
}