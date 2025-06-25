using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using NeofamilyParser.BLL.Models;
using NeofamilyParser.BLL.Models.Neofamily;
using RestSharp;
using System.Text.RegularExpressions;

namespace NeofamilyParser.BLL.ApiClient
{
    public class NeofamilyApiClient : INeofamilyApiClient
    {
        private const string tasksUrl = "https://backend.neofamily.ru/api/task";
        
        private IRestClient _restClient;
        private IHtmlParser _htmlParser;

        public NeofamilyApiClient(IRestClient restClient, IHtmlParser htmlParser)
        {
            this._restClient = restClient;
            this._htmlParser = htmlParser;
        }

        public IEnumerable<TaskModel> GetTasks()
        {
            var tasksRequest = new RestRequest(tasksUrl);
            tasksRequest.AddParameter("subject", "fizika");
            tasksRequest.AddParameter("sort[id]", "asc");
            tasksRequest.AddParameter("perPage", int.MaxValue);
            var tasksResponse = this._restClient.Get<TasksResponseModel>(tasksRequest);

            foreach(var task in tasksResponse.Data)
            {
                var solutionRequest = new RestRequest(tasksUrl + "/{id}/solution");
                solutionRequest.AddUrlSegment("id", task.Id.ToString());
                Console.WriteLine(task.Id.ToString());

                task.Solution = this._restClient.Get<TaskSolutionResponseModel>(solutionRequest);
                Thread.Sleep(400);//hack to avoid toomanyrequests
            }

            var parsedTasks = this.ParseTasks(tasksResponse.Data);
            return parsedTasks;
        }

        private IEnumerable<TaskModel> ParseTasks(IEnumerable<TaskResponseModel> tasks)
        {
            var parsedTasks = new List<TaskModel>();
            var parsingTime = DateTime.Now;
            var sourceRegex = new Regex(@"Источник: (.*)$");
            var answerRegex = new Regex(@"Ответ:\s*(.*)(Источник:|\n|\r)");
            var solutionRegex = new Regex(@"Ответ:(.|\s|\n|\r)*$");

            foreach (var item in tasks)
            {
                var parsedTask = new TaskModel();
                parsedTask.Id = item.Id;
                parsedTask.Section = string.Join(";", item.Themes.Select(x => x.Section.Name));
                parsedTask.Theme = string.Join(";", item.Themes.Select(x => x.Name));
                parsedTask.Part = item.TaskLine.Value;
                parsedTask.Number = item.TaskLine.Name;               
                parsedTask.QuestionText = this.RemoveHtmlTags(item.Question);
                parsedTask.QuestionImages = this.GetImages(item.Question);
                var solutionText = this.RemoveHtmlTags(item.Solution.Data.Solution);
                parsedTask.SolutionText = solutionRegex.Replace(solutionText,"");
                parsedTask.SolutionImages = this.GetImages(item.Solution.Data.Solution);
                parsedTask.Source = sourceRegex.Match(solutionText).Groups[1].Value;
                parsedTask.Answer = answerRegex.Match(solutionText).Groups[1].Value;
                parsedTask.CreatedAt = parsingTime;

                parsedTasks.Add(parsedTask);
            }

            return parsedTasks;
        }

        private string RemoveHtmlTags(string input)
        {
            var nodeList = this._htmlParser.ParseFragment(input, null);
            var result = string.Concat(nodeList.Select(x => x.Text()));

            return result;
        }

        private IEnumerable<TaskImageModel> GetImages(string input)
        {
            var images = new List<TaskImageModel>();
            var imageNameRegex = new Regex(@".*\/(.+)$");
            var nodes = this._htmlParser.ParseFragment(input, null).QuerySelectorAll("img");

            foreach(var item in nodes)
            {
                var imageSource = ((IHtmlImageElement)item).Source;
                var imageRequest = new RestRequest(imageSource);
                var image = this._restClient.DownloadData(imageRequest);
                var imageName = imageNameRegex.Match(imageSource).Groups[1].Value;

                images.Add(new TaskImageModel() { 
                    Name = imageName, Image = image 
                });
            }

            return images;
        }
    }
}
