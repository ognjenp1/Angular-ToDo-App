using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ToDoCore.Models;
using ToDoInfrastructure;

namespace ToDoApi.Services
{
    public class ReminderService : IHostedService
    {
        protected IServiceProvider _serviceProvider;
        private readonly IOptions<ReminderConfig> _config;
        private readonly ILogger _logger;

        public ReminderService(IServiceProvider serviceProvider, IOptions<ReminderConfig> config, ILogger<ReminderService> logger)
        {
            _serviceProvider = serviceProvider;
            _config = config;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("ReminderService started!");
            _ = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(_config.Value.Interval));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var hostedServiceDbContext = (ToDoDbContext)scope.ServiceProvider.GetRequiredService(typeof(ToDoDbContext));

                var expiredLists = hostedServiceDbContext.Lists.Where(l => l.ReminderDate <= DateTime.Today && l.Reminded == false).ToList();
                _logger.LogDebug("ReminderService found " + expiredLists.Count +" ToDoLists!");
                var apiKey = _config.Value.SendGridKey;
                var client = new SendGridClient(apiKey);
                var emailFrom = new EmailAddress(_config.Value.EmailFrom, _config.Value.TestUser);
                var emailTo = new EmailAddress(_config.Value.EmailTo, "test");
                var subject = _config.Value.EmailSubject;
                var plainTextContent = _config.Value.EmailTextContent;
                foreach (ToDoList expiredList in expiredLists)
                {
                    var html = _config.Value.HtmlOpenTag + expiredList.Id + _config.Value.HtmlCloseTag;
                    var msg = MailHelper.CreateSingleEmail(emailFrom, emailTo, subject, plainTextContent, html);
                    var response = await client.SendEmailAsync(msg);
                    expiredList.Reminded = true;

                    expiredList.Update(expiredList);
                }

                hostedServiceDbContext.SaveChanges();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("ReminderService stopped!");
            return Task.CompletedTask;
        }
    }
}
