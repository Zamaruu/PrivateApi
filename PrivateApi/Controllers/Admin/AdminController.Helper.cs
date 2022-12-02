using System;
using PrivateApi.Data.ResponseModels;
using PrivateApi.MongoDB;

namespace PrivateApi.Controllers.Admin
{
	public class AdminControllerHelper
	{
		private readonly MongoAdminService mongoAdminService;

		public AdminControllerHelper(MongoAdminService maService)
		{
			mongoAdminService = maService;
		}

		public async Task<bool> SubmitErrorLog(RuntimeErrorResponse error)
		{
			var wasSubmitted = await mongoAdminService.SaveDocument<RuntimeErrorResponse>(error, Data.MongoAdminCollections.AdminLogs);
			return wasSubmitted;
		}
	}
}

