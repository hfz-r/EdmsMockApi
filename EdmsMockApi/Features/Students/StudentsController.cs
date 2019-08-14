using System.Net;
using System.Threading.Tasks;
using EdmsMockApi.Dtos.DataProfiles;
using EdmsMockApi.Dtos.Errors;
using EdmsMockApi.Dtos.ProfileFields;
using EdmsMockApi.Dtos.Profiles;
using EdmsMockApi.Infrastructure.Attributes;
using EdmsMockApi.Json.ActionResults;
using EdmsMockApi.Json.Serializer;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EdmsMockApi.Features.Students
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class StudentsController : BaseController
    {
        private readonly IMediator _mediator;

        public StudentsController(IJsonFieldsSerializer jsonFieldsSerializer, IMediator mediator) : base(
            jsonFieldsSerializer)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Receive a list of all Profiles
        /// </summary>
        /// <param name="query">Query parameters to contain on json</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Route("get-profiles", Name = nameof(GetProfiles))]
        [ProducesResponseType(typeof(ProfilesRootObject), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorsRootObject), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.Unauthorized)]
        [GetRequestsErrorInterceptorActionFilter]
        public async Task<IActionResult> GetProfiles([FromQuery] GetProfiles.Query query)
        {
            if (query.Limit < Configurations.MinLimit || query.Limit > Configurations.MaxLimit)
                return await Error(HttpStatusCode.BadRequest, "limit", "Invalid limit parameter");

            if (query.Page <= 0)
                return await Error(HttpStatusCode.BadRequest, "page", "Invalid request parameters");

            var profileDto = await _mediator.Send(query);

            var rootObject = new ProfilesRootObject()
            {
                Profiles = profileDto
            };

            var json = JsonFieldsSerializer.Serialize(rootObject, query.Fields);

            return new RawJsonActionResult(json);
        }

        /// <summary>
        /// Receive a list of profile fields
        /// </summary>
        /// <param name="id">Profile id</param>
        /// <param name="fields">Comma-separated list of fields to include in the response</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Route("get-profile-fields/{id}", Name = nameof(GetProfileFields))]
        [ProducesResponseType(typeof(ProfileFieldsRootObject), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ErrorsRootObject), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.NotFound)]
        [GetRequestsErrorInterceptorActionFilter]
        public async Task<IActionResult> GetProfileFields(int id, string fields = "")
        {
            if (id <= 0)
                return await Error(HttpStatusCode.BadRequest, "profile_id", "profile_id is required.");

            var profileFieldDto = await _mediator.Send(new GetProfileFields.Query {ProfileId = id, Fields = fields});
            if (profileFieldDto == null)
                return await Error(HttpStatusCode.NotFound, "profile_fields", "not found");

            var rootObject = new ProfileFieldsRootObject
            {
                ProfileFields = profileFieldDto
            };

            var json = JsonFieldsSerializer.Serialize(rootObject, fields);

            return new RawJsonActionResult(json);
        }

        /// <summary>
        /// Retrieve profile by specific profile id
        /// </summary>
        /// <param name="id">Profile id</param>
        /// <param name="fields">Comma-separated list of fields to include in the response</param>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Route("get-profile-by-id/{id}", Name = nameof(GetProfileById))]
        [ProducesResponseType(typeof(ProfilesRootObject), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ErrorsRootObject), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.NotFound)]
        [GetRequestsErrorInterceptorActionFilter]
        public async Task<IActionResult> GetProfileById(int id, string fields = "")
        {
            if (id <= 0)
                return await Error(HttpStatusCode.BadRequest, "id", "invalid profile id");

            var profileDto = await _mediator.Send(new GetProfileById.Query {Id = id, Fields = fields});
            if (profileDto == null)
                return await Error(HttpStatusCode.NotFound, "profile", "not found");

            var rootObject = new ProfilesRootObject();
            rootObject.Profiles.Add(profileDto);

            var json = JsonFieldsSerializer.Serialize(rootObject, fields);

            return new RawJsonActionResult(json);
        }

        /// <summary>
        /// Login to the E-DMS server
        /// </summary>
        /// <param name="query">Query parameters to contain on json</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Route("login", Name = nameof(Login))]
        [ProducesResponseType(typeof(LoginRootObject), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorsRootObject), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.Unauthorized)]
        [GetRequestsErrorInterceptorActionFilter]
        public async Task<IActionResult> Login([FromQuery] Login.Query query)
        {
            var result = await _mediator.Send(query);

            var rootObject = new LoginRootObject
            {
                Status = result
            };

            return Ok(rootObject);
        }

        #region Search

        /// <summary>
        /// Get an output by using general search.
        /// </summary>
        /// <param name="query">Query parameters to contain on json</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Route("search", Name = nameof(GetSearch))]
        [ProducesResponseType(typeof(DataProfilesRootObject), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorsRootObject), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.Unauthorized)]
        [GetRequestsErrorInterceptorActionFilter]
        public async Task<IActionResult> GetSearch([FromQuery] Search.Query query)
        {
            if (query.Limit < Configurations.MinLimit || query.Limit > Configurations.MaxLimit)
                return await Error(HttpStatusCode.BadRequest, "limit", "Invalid limit parameter");

            if (query.Page <= 0)
                return await Error(HttpStatusCode.BadRequest, "page", "Invalid request parameters");

            var dataProfileDto = await _mediator.Send(query);

            var rootObject = new DataProfilesRootObject()
            {
                DataProfiles = dataProfileDto
            };

            var json = JsonFieldsSerializer.Serialize(rootObject, query.Fields);

            return new RawJsonActionResult(json);
        }

        /// <summary>
        /// Get a profile by providing query parameters
        /// </summary>
        /// <param name="query">Query parameters to contain on json</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Route("search-by-doc", Name = nameof(GetSearchByDoc))]
        [ProducesResponseType(typeof(DataProfilesRootObject), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorsRootObject), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.Unauthorized)]
        [GetRequestsErrorInterceptorActionFilter]
        public async Task<IActionResult> GetSearchByDoc([FromQuery] SearchByDoc.Query query)
        {
            if (query.Limit < Configurations.MinLimit || query.Limit > Configurations.MaxLimit)
                return await Error(HttpStatusCode.BadRequest, "limit", "Invalid limit parameter");

            if (query.Page <= 0)
                return await Error(HttpStatusCode.BadRequest, "page", "Invalid request parameters");

            var dataProfileDto = await _mediator.Send(query);

            var rootObject = new DataProfilesRootObject()
            {
                DataProfiles = dataProfileDto
            };

            var json = JsonFieldsSerializer.Serialize(rootObject, query.Fields);

            return new RawJsonActionResult(json);
        }

        /// <summary>
        /// Get a profile by providing column name and value (example: Matric Number: MSU001)
        /// </summary>
        /// <param name="query">Query parameters to contain on json</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Route("profile-search", Name = nameof(GetProfileSearch))]
        [ProducesResponseType(typeof(DataProfilesRootObject), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorsRootObject), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.Unauthorized)]
        [GetRequestsErrorInterceptorActionFilter]
        public async Task<IActionResult> GetProfileSearch([FromQuery] ProfileSearch.Query query)
        {
            if (query.Limit < Configurations.MinLimit || query.Limit > Configurations.MaxLimit)
                return await Error(HttpStatusCode.BadRequest, "limit", "Invalid limit parameter");

            if (query.Page <= 0)
                return await Error(HttpStatusCode.BadRequest, "page", "Invalid request parameters");

            var dataProfileDto = await _mediator.Send(query);

            var rootObject = new DataProfilesRootObject()
            {
                DataProfiles = dataProfileDto
            };

            var json = JsonFieldsSerializer.Serialize(rootObject, query.Fields);

            return new RawJsonActionResult(json);
        }

        #endregion
    }
}