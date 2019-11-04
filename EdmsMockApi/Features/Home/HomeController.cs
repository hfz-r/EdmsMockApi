using System.IO;
using System.Threading.Tasks;
using EdmsMockApi.Data.Repositories;
using EdmsMockApi.Entities;
using EdmsMockApi.Models;
using EdmsMockApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceReference;

namespace EdmsMockApi.Features.Home
{
    public class HomeController : Controller
    {
        private readonly IDocufloSdkService _docufloSdkService;
        private readonly IRepository<Profile> _profileRepository;

        public HomeController(IDocufloSdkService docufloSdkService, IRepository<Profile> profile)
        {
            _docufloSdkService = docufloSdkService;
            _profileRepository = profile;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Test()
        {
            return await Task.FromResult(View(new ExportViewModel())) ;
        }

        [HttpPost]
        public async Task<IActionResult> Test(ExportViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await viewModel.FileContent.CopyToAsync(memoryStream);

                    var body = new ExportRequestBody
                    {
                        FileContent = memoryStream.ToArray(),
                        strFileName = viewModel.FileName,
                        strFolderName = "BEXCELLLE",
                        strProfile = (await _profileRepository.Table.FirstOrDefaultAsync(p => p.ProfileId == 9))?.ProfileName,
                        arrProfileValue = new ArrayOfString {"666", "Profile Pictures"},
                        userID = "garyfoongck",
                    };

                    var export = await _docufloSdkService.Export(body);

                    return Ok(export == "1" ? "Success" : export);
                }
            }

            return View(viewModel);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}