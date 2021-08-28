using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CognizantChallenge.Compilers;
using CognizantChallenge.DAL.Models;
using CognizantChallenge.DAL.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CognizantChallenge.Models;
using CognizantChallenge.Models.Index;
using CognizantChallenge.Models.TopUsers;

namespace CognizantChallenge.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Home/TopUsers")]
        public async Task<IActionResult> TopUsers()
        {
            var usersRepo = _unitOfWork.UsersRepository;
            var users = await usersRepo.GetTop3();
            var model = new TopUsersModel();
            model.TopUsers = users.Select(user => new TopUser()
                { Name = user.Name, Scores = user.Scores, Tasks = user.Tasks });
            return View(model);
        }

        [Route("CheckTask")]
        [HttpPost]
        public async Task<IActionResult> CheckTask(
            CheckChallengeModel model,
            [FromServices] ICompiler compiler)
        {
            CompileResponse result;
            var challengeRepo = _unitOfWork.ChallengesRepository;
            var challenge = challengeRepo.GetEntity(model.ChallengeId);
            var @input = challenge.Input;
            var @output = challenge.Output;

            var script = model.Script;
            script = script.Replace("@input", @input.ToString()).Replace("@output", @output.ToString());
            try
            {
                _logger.LogInformation($"Running script by User = {model.UserId}.\nLanguage: {model.Language}\nScript:{model.Script}");
                result = compiler.Run(script, model.Language);
            }
            catch (WebException e)
            {
                _logger.LogError("Web exception appears: "+ e);
                return new JsonResult(new {Message = "Something wrong with a request: "+e, Error = true});
            }
            
            if (Math.Floor(result.StatusCode / 100.0) > 2 || (!result.Memory.HasValue && !result.CpuTime.HasValue))
            {
                _logger.LogError("Code contains errors: "+ result.Output);
                return new JsonResult(new { Message = "Please, rewrite the function. Exception: "+ result.Output, Error = true });   
            }

            var message = "";
            if (Convert.ToBoolean(result.Output))
            {
                var usersRepo = _unitOfWork.UsersRepository;
                var user = usersRepo.GetEntity(model.UserId);

                var challengeName = challengeRepo.GetEntity(model.ChallengeId).Name;

                user.Tasks += " " + challengeName;
                user.Scores += 1;
                _unitOfWork.Save();
                message = "Congratulation! The task has been completed successfully!";
            }
            else
            {
                message = "Sorry! You've got a wrong result!";
            }

            return new JsonResult(new {Message = message, Error = false});
        }

        [Route("CreateUser")]
        [HttpPost]
        public async Task<IActionResult> CreateUser(string name)
        {
            var user = new User(){ Name = name, Scores = 0};
            IEnumerable<LanguageModel> languages = null;
            IEnumerable<ChallengeModel> challenges = null;
            try
            {
                var userRepo = _unitOfWork.UsersRepository;
                userRepo.Insert(user);
                _unitOfWork.Save();
                _logger.LogInformation($"User {user.Name} has been created");
                
                var languagesRepo = _unitOfWork.LanguagesRepository;
                
                _logger.LogInformation($"Getting Languages");
                languages = (await languagesRepo.GetAll())
                    .Select(l => new LanguageModel {Id =l.Id, Name= l.Name, Template = l.Template, RequestedName = l.RequestedName})
                    .ToList();

                _logger.LogInformation($"Getting Challenges");
                var challengesRepo = _unitOfWork.ChallengesRepository;
                challenges = (await challengesRepo.GetAll())
                    .Select(ch => new ChallengeModel() {Id = ch.Id, Name = ch.Name, Description = ch.Description})
                    .ToList();
            }
            catch(Exception ex)
            {
                _logger.LogError($"There is something wrong: " + ex);
                return new JsonResult(new
                {
                    Languages = languages,
                    Challenges = challenges,
                    User = new UserModel{Id = user.Id, Name = user.Name},
                    Error= true,
                    Message= "There is something wrong: " + ex,
                });
            }
            
            return new JsonResult(new
            {
                Languages = languages,
                Challenges = challenges,
                User = new UserModel{Id = user.Id, Name = user.Name},
                Error= false,
                Message= "",
            });
        }
        
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}