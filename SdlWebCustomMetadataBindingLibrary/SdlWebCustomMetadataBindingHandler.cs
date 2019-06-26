using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Tridion.ContentManager.CoreService.Client;
using Trisoft.InfoShare.Plugins.SDK;
using Trisoft.InfoShare.Plugins.SDK.Extensions.MetadataBinding;
using SdlWebCustomMetadataBindingLibrary.Models;
using SdlWebCustomMetadataBindingLibrary.Factories;
using System.Linq;

namespace SdlWebCustomMetadataBindingLibrary
{
    // This attribute is used to make the class discoverable by ContentManager.
    [Export("SdlWebCustomMetadataBindingHandler", typeof(IHandler))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    /// <summary>
    /// Custom metadata binding handler that is used to connect the metadata fields to the external metadata source.
    /// </summary>
    public class SdlWebCustomMetadataBindingHandler : IHandler
    {

        #region Properties
        /// <summary>
        /// The handler configuration.
        /// </summary>
        private IHandlerConfiguration _configuration;
        /// <summary>
        /// The root node label that will be used for the 'tree mode'
        /// </summary>
        private string _rootNode;

        /// <summary>
        /// This method will be called by the business code after the instance of the handler is created.
        /// </summary>
        /// <param name="configuration">Configuration with the parameters provided in the XML Extension Settings.</param>

        /// <summary>
        /// The Category Id used to retrieve the keywords
        /// </summary>
        private string _CategoryId;

        /// <summary>
        /// The Endpoint address of the core service
        /// </summary>
        private string _EndpointAddress;

        /// <summary>
        /// The Username to connect the core service
        /// </summary>
        private string _Username;

        /// <summary>
        /// The Username to connect the core service
        /// </summary>
        private string _Password;

        /// <summary>
        /// Core service CLient Object
        /// </summary>
        private CoreServiceClient _client;

        /// <summary>
        /// List of keywords based on category
        /// </summary>
        private IdentifiableObjectData[] _KeywordList;
        #endregion


        public void Initialize(IHandlerConfiguration configuration)
        {
            // Cache the passed configuration for the future use
            _configuration = configuration;
            _configuration.LogService.Debug("Inside Initialize");
            // Access parameters
            if (!configuration.Parameters.TryGetValue("RootNode", out _rootNode))
            {
                _rootNode = "";
            }

            //Category Id
            if (!configuration.Parameters.TryGetValue("CategoryId", out _CategoryId))
            {
                _CategoryId = "";
            }

            //User Name
            if (!configuration.Parameters.TryGetValue("Username", out _Username))
            {
                _Username = "";
            }

            //Password
            if (!configuration.Parameters.TryGetValue("Password", out _Password))
            {
                _Password = "";
            }

            //Endpoint URL
            if (!configuration.Parameters.TryGetValue("EndpointAddress", out _EndpointAddress))
            {
                _EndpointAddress = "";
            }

            //Core Service Client 
            _client = TridionCoreServiceFactory.CreateCoreService(_EndpointAddress, _Username, _Password);

            // A list of keywords based on the category
            _KeywordList = GetKeywordsList();
        }

        /// <summary>
        /// Used to convert ids (stored in the database) into tags (including labels visible in UI)
        /// </summary>
        /// <param name="context">The arguments of the method.</param>
        /// <returns>The result with the resolved tags.</returns>
        public IResolveIdsResult ResolveIds(IResolveIdsContext context)
        {

            return new ResolveIdsResult()
            {
                Tags = GetKeywordsFromCategory()
            };
        }

        /// <summary>
        /// Returns the limited amound of tags matching the user input for showing the autosuggest options in UI.
        /// </summary>
        /// <param name="context">The arguments of the method.</param>
        /// <returns>The result with the tags matching the input filter.</returns>
        public IRetrieveTagsResult RetrieveTags(IRetrieveTagsContext context)
        {
            return new RetrieveTagsResult()
            {
                Tags = GetKeywordsFromCategory(),
                Messages = new List<IMessage>()
            };
        }

        /// <summary>
        /// This method creates a list of keywords based on the category value passed from XML in CM
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public List<Tag> GetKeywordsFromCategory()
        {
            _configuration.LogService.Debug("Inside GetKeywordsFromCategory");
            List<Tag> tagList = new List<Tag>();
            if (_KeywordList != null)
            {
                foreach (var keyword in _KeywordList)
                {
                    _configuration.LogService.Debug("keyword.Title " + keyword.Title);
                    tagList.Add(new Tag
                    {
                        Id = keyword.Id,
                        Label = keyword.Title,
                        AnnotatedLabel = keyword.Title,
                        Description = keyword.Title
                    });
                }
            }
            return tagList;

        }


        /// <summary>
        /// Returns the tags and their relations to allow to reconstruct the tree in UI.
        /// Since this method is called only once, the tree should be complete, meaning that it should contain
        /// all the subnodes you would like to be able to show in UI. There is no lazy load concept support, 
        /// so there will be no second call once you expand the node.
        /// </summary>
        /// <param name="context">The arguments of the method.</param>
        /// <returns>The result with the tags and the structure.</returns>
        public IRetrieveTagStructureResult RetrieveTagStructure(IRetrieveTagStructureContext context)
        {

            return new RetrieveTagStructureResult()
            {
                //Todo : Assign the keyword list properties values to the Structure Tagnproperties values and remove the below code

                Tags = GetStructuredKeywordsFromCategory(),

                Relations = GetStructuredRelationFromCategory(),
                Messages = new List<IMessage>()
            };
        }

        /// <summary>
        /// Validates the passed ids against the external taxonomy.
        /// For each id that is not valid, should return an error message.
        /// </summary>
        /// <param name="context">The arguments of the method.</param>
        /// <returns>The result with the validation messages.</returns>
        public IValidateResult Validate(IValidateContext context)
        {
            IDictionary<string, IMessage> messages = new Dictionary<string, IMessage>();
            foreach (string id in context.Ids)
            {
               
                if(_KeywordList!= null && !_KeywordList.ToList().Where(i=>i.Id!=id).Any())
                {
                    messages.Add(id, new Message()
                    {
                        ResouceLib = "SdlWebCustomMetadataBinding",
                        Level = MessageLevel.Error,
                        Number = -600000,
                        ResourceId = "TermNotFound",
                        Description = String.Format(@"The tag '{0}' is not valid.", id),
                        Parameters = new List<IMessageParam>()
                    });
                }
            }

            return new ValidateResult()
            {
                Messages = messages
            };
        }

        /// <summary>
        /// This Method returns a list structures keywords based on parent child relationship
        /// </summary>
        /// <returns></returns>
        public List<StructureTag> GetStructuredKeywordsFromCategory()
        {
            _configuration.LogService.Debug("Inside GetStructuredKeywordsFromCategory");
            List<StructureTag> tagList = new List<StructureTag>();
            var categoryData = (CategoryData)_client.Read(_CategoryId, new ReadOptions());
            if (_KeywordList != null)
            {
                tagList.Add(new StructureTag
                {
                    Id = _CategoryId,
                    Label = categoryData.Description,
                    AnnotatedLabel = categoryData.Description,
                    Description = categoryData.Description,
                    IsSelectable = false
                });
                foreach (var keyword in _KeywordList)
                {
                    _configuration.LogService.Debug("keyword.Title " + keyword.Title);
                    tagList.Add(new StructureTag
                    {
                        Id = keyword.Id,
                        Label = keyword.Title,
                        AnnotatedLabel = keyword.Title,
                        Description = keyword.Title,
                        IsSelectable = true
                    });
                }
            }
            return tagList;

        }
        /// <summary>
        /// This method returns the relation of parent child keywords node from Tridion site
        /// </summary>
        /// <returns>A list of parent child keyword Ids</returns>
        public List<TagRelation> GetStructuredRelationFromCategory()
        {
            List<TagRelation> tagList = new List<TagRelation>();
            var categoryData = (CategoryData)_client.Read(_CategoryId, new ReadOptions());
            if (_KeywordList != null)
            {
                tagList.Add(new TagRelation
                {
                    ToId = _CategoryId

                });
                foreach (var keyword in _KeywordList)
                {
                    _configuration.LogService.Debug("keyword.Title " + keyword.Title);
                    IdentifiableObjectData[] list = _client.GetList(keyword.Id, new ChildKeywordsFilterData());
                    if (list.Length >= 0)
                    {
                        if (!tagList.ToList().Where(i => i.ToId.Contains(keyword.Id)).Any())
                        {
                            tagList.Add(new TagRelation
                            {
                                FromId = _CategoryId,
                                ToId = keyword.Id
                            });
                        }
                        if (list.Length > 0)
                        {
                            foreach (IdentifiableObjectData keyworditem in list)
                            {
                                tagList.Add(new TagRelation
                                {
                                    FromId = keyword.Id,
                                    ToId = keyworditem.Id

                                });
                            }
                        }

                    }
                }
            }
            return tagList;

        }
        /// <summary>
        /// Explicitly release any disposable resources
        /// </summary>
        public void Dispose()
        {
            // In our example we don't need a cleanup
        }

        public IdentifiableObjectData[] GetKeywordsList()
        {
            var filter = new OrganizationalItemItemsFilterData();
            var categoryData = (CategoryData)_client.Read(_CategoryId, new ReadOptions());

           return _client.GetList(_CategoryId, filter);
        }

    }

}