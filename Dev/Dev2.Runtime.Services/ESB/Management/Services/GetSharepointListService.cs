#pragma warning disable
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Dev2.Common;
using Dev2.Common.Common;
using Dev2.Communication;
using Dev2.Data.ServiceModel;
using Dev2.DynamicServices;
using Dev2.Runtime.Hosting;
using Dev2.Runtime.ServiceModel.Data;
using Dev2.Workspaces;
using Warewolf.Resource.Errors;

namespace Dev2.Runtime.ESB.Management.Services
{

    public class GetSharepointListService : DefaultEsbManagementEndpoint
    {
        #region Implementation of DefaultEsbManagementEndpoint
        
        public override StringBuilder Execute(Dictionary<string, StringBuilder> values, IWorkspace theWorkspace)
        {
            var serializer = new Dev2JsonSerializer();

            if (values == null)
            {
                throw new InvalidDataContractException(ErrorResource.NoParameter);
            }
            string serializedSource = null;
            values.TryGetValue("SharepointServer", out StringBuilder tmp);
            if (tmp != null)
            {
                serializedSource = tmp.ToString();
            }

            if(string.IsNullOrEmpty(serializedSource))
            {
                var message = new ExecuteMessage();
                message.HasError = true;
                message.SetMessage(ErrorResource.NoSharepointServerSet);
                Dev2Logger.Debug(ErrorResource.NoSharepointServerSet, GlobalConstants.WarewolfDebug);
                return serializer.SerializeToBuilder(message);
            }

            SharepointSource source;
            SharepointSource runtimeSource = null;
            try
            {
                source = serializer.Deserialize<SharepointSource>(serializedSource);

                if(source.ResourceID != Guid.Empty)
                {
                    runtimeSource = ResourceCatalog.Instance.GetResource<SharepointSource>(theWorkspace.ID, source.ResourceID);
                    if(runtimeSource == null)
                    {
                        var contents = ResourceCatalog.Instance.GetResourceContents(theWorkspace.ID, source.ResourceID);
                        runtimeSource = new SharepointSource(contents.ToXElement());
                    }
                }
            }
            catch(Exception e)
            {
                Dev2Logger.Error(e, GlobalConstants.WarewolfError);
                var res = new DbTableList("Invalid JSON data for sharepoint server parameter. Exception: {0}", e.Message);
                return serializer.SerializeToBuilder(res);
            }
            if(runtimeSource == null)
            {
                var res = new DbTableList(ErrorResource.InvalidSharepointServerSource);
                Dev2Logger.Debug(ErrorResource.InvalidSharepointServerSource, GlobalConstants.WarewolfDebug);
                return serializer.SerializeToBuilder(res);
            }
            if(string.IsNullOrEmpty(runtimeSource.Server))
            {
                var res = new DbTableList(ErrorResource.InvalidSharepointServerSent, serializedSource);
                Dev2Logger.Debug(string.Format(ErrorResource.InvalidSharepointServerSent, serializedSource), GlobalConstants.WarewolfDebug);
                return serializer.SerializeToBuilder(res);
            }

            try
            {
                Dev2Logger.Info("Get Sharepoint Server Lists. " + source.Server, GlobalConstants.WarewolfDebug);
                var lists = runtimeSource.LoadLists();
                return serializer.SerializeToBuilder(lists);
            }
            catch(Exception ex)
            {
                var tables = new DbTableList(ex);
                return serializer.SerializeToBuilder(tables);
            }
        }

        #endregion

        public override DynamicService CreateServiceEntry() => EsbManagementServiceEntry.CreateESBManagementServiceEntry(HandlesType(), "<DataList><Database ColumnIODirection=\"Input\"/><Dev2System.ManagmentServicePayload ColumnIODirection=\"Both\"></Dev2System.ManagmentServicePayload></DataList>");

        public override string HandlesType() => "GetSharepointListService";
    }
}