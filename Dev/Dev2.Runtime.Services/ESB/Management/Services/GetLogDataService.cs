using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Dev2.Common;
using Dev2.Common.Interfaces.Core.DynamicServices;
using Dev2.Common.Interfaces.Enums;
using Dev2.Common.Interfaces.Wrappers;
using Dev2.Common.Wrappers;
using Dev2.Communication;
using Dev2.DynamicServices;
using Dev2.DynamicServices.Objects;
using Dev2.Workspaces;

namespace Dev2.Runtime.ESB.Management.Services
{
    public class GetLogDataService : IEsbManagementEndpoint
    {
        public IFile FileWrapper { get; set; }
        private string _serverLogFilePath;

        public GetLogDataService()
        {
            FileWrapper = new FileWrapper();
        }

        public GetLogDataService(IFile file)
        {
            FileWrapper = file;
        }
        public Guid GetResourceID(Dictionary<string, StringBuilder> requestArgs)
        {
            return Guid.Empty;
        }

        public AuthorizationContext GetAuthorizationContextForService()
        {
            return AuthorizationContext.Administrator;
        }

        public string HandlesType()
        {
            return "GetLogDataService";
        }

        public StringBuilder Execute(Dictionary<string, StringBuilder> values, IWorkspace theWorkspace)
        {
            Dev2Logger.Info("Get Log Data Service", "Warewolf Info");

            var serializer = new Dev2JsonSerializer();
            try
            {
                List<dynamic> tmpObjects = new List<dynamic>();
                var buffor = new Queue<string>();
                Stream stream = File.Open(ServerLogFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var file = new StreamReader(stream);
                while (!file.EndOfStream)
                {
                    string line = file.ReadLine();

                    buffor.Enqueue(line);
                }

                {
                    var logData = buffor.AsQueryable();

                    //File.ReadAllLines(ServerLogFilePath);

                    foreach (var singleEntry in logData)
                    {
                        var matches = Regex.Split(singleEntry, @"(\d+[-.\/]\d+[-.\/]\d+ \d+[:]\d+[:]\d+,\d+)\s[[](\w+[-]\w+[-]\w+[-]\w+[-]\w+)[]]\s(\w+)\s+[-]\s+");
                        if (matches.Length > 1)
                        {
                            var match = matches;
                            var tmpObj = new
                            {
                                ExecutionId = match[2],
                                LogType = match[3],
                                DateTime = match[1],
                                Message = match[4]
                            };
                            tmpObjects.Add(tmpObj);
                        }
                    }

                    var logEntries = new List<LogEntry>();
                    var groupedEntries = tmpObjects.GroupBy(o => o.ExecutionId);
                    foreach (var groupedEntry in groupedEntries)
                    {
                        var logEntry = new LogEntry();
                        logEntry.ExecutionId = groupedEntry.Key;
                        logEntry.Status = "Success";
                        foreach (var s in groupedEntry)
                        {
                            if (s.Message.StartsWith("Started Execution"))
                            {
                                logEntry.StartDateTime = DateTime.Parse(s.DateTime);
                            }
                            if (s.LogType == "ERROR")
                            {
                                logEntry.Result = "ERROR";
                            }
                            if (s.Message.StartsWith("Completed Execution"))
                            {
                                logEntry.CompletedDateTime = DateTime.Parse(s.DateTime);
                            }
                            if (s.Message.StartsWith("About to execute"))
                            {
                                logEntry.User = GetUser(s.Message);
                                logEntry.Url = s.Message;
                            }
                        }
                        logEntry.ExecutionTime = (logEntry.CompletedDateTime - logEntry.StartDateTime).Milliseconds.ToString();
                        logEntries.Add(logEntry);
                    }
                    return serializer.SerializeToBuilder(logEntries);
                }
            }
            catch (Exception e)
            {
                Dev2Logger.Info("Get Log Data ServiceError", e, "Warewolf Info");
            }
            return serializer.SerializeToBuilder("");
        }

        private string GetUser(string message)
        {
            string toReturn= message.Split('[')[2].Split(':')[0];
            return toReturn;
        }

        public DynamicService CreateServiceEntry()
        {
            var findServices = new DynamicService { Name = HandlesType(), DataListSpecification = new StringBuilder("<DataList><ResourceType ColumnIODirection=\"Input\"/><Roles ColumnIODirection=\"Input\"/><ResourceName ColumnIODirection=\"Input\"/><Dev2System.ManagmentServicePayload ColumnIODirection=\"Both\"></Dev2System.ManagmentServicePayload></DataList>") };

            var fetchItemsAction = new ServiceAction { Name = HandlesType(), ActionType = enActionType.InvokeManagementDynamicService, SourceMethod = HandlesType() };

            findServices.Actions.Add(fetchItemsAction);

            return findServices;
        }

        public string ServerLogFilePath
        {
            get
            {
                return _serverLogFilePath ?? EnvironmentVariables.ServerLogFile;
            }
            set
            {
                _serverLogFilePath = value;
            }
        }
    }


    public class LogEntry
    {
        public DateTime StartDateTime { get; set; }
        public string Status { get; set; }
        public string Url { get; set; }
        public string Result { get; set; }
        public string User { get; set; }
        public DateTime CompletedDateTime { get; set; }
        public string ExecutionTime { get; set; }
        public string ExecutionId { get; set; }
    }
}