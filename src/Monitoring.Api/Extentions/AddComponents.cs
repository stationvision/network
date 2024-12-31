using Monitoring.Api.Handlers.Boards;
using Monitoring.Api.Handlers.Pulses;
using Monitoring.Core.Commands;
using Monitoring.Core.Commands.Boards;
using Monitoring.Core.Commands.Puls;
using Monitoring.Core.Commands.TCPMessages;
using Monitoring.Core.Dispatchers;
using Monitoring.Core.Dispatchers.Interfaces;
using Monitoring.Core.Extentions;
using Monitoring.Core.Queries;
using Monitoring.Core.Queries.Boards;
using Monitoring.Core.Queries.Puls;

namespace Monitoring.Api.Extentions
{
    public static class CustomeComponents
    {
        public static IServiceCollection AddComponents(this IServiceCollection services)
        {
            RegisterCommandsAndQueries(services);
            CommandsAndQueriesExtensions.AddCommandQueryHandlers(services, typeof(IQueryHandler<>), typeof(CommandLineConfigurationExtensions));

            return services;
        }

        public static IServiceCollection RegisterCommandsAndQueries(this IServiceCollection services)
        {
            //Queries
            services.AddScoped<Query, GetAllBoardQuery>();
            services.AddScoped<Query, GetAlPulslQuery>();
            services.AddScoped<Query, GetByIdQuery>();
            services.AddScoped<Query, GetBoardByIdQuery>();

            //Commands
            services.AddScoped<Command, CreateBoardCommand>();
            services.AddScoped<Command, DeleteBoardCommand>();
            services.AddScoped<Command, EditBoardCommand>();
            services.AddScoped<Command, CreatePulsCommand>();
            services.AddScoped<Command, EditPulsCommand>();
            services.AddScoped<Command, DeletePulsCommand>();
            services.AddScoped<Command, SendMessageToConfigBoardCommand>();
            services.AddScoped<Command, BoardChangeStatusCommand>();
            services.AddScoped<Command, TimeOutCommand>();

            //Handlers
            services.AddScoped<ICommandHandler<CreateBoardCommand>, CreateBoardHandler>();
            services.AddScoped<ICommandHandler<DeleteBoardCommand>, DeleteBaordHandler>();
            services.AddScoped<ICommandHandler<EditBoardCommand>, EditBoardHandler>();
            services.AddScoped<ICommandHandler<BoardChangeStatusCommand>, BoardChangeStatusHandler>();
            services.AddScoped<ICommandHandler<TimeOutCommand>, BoardTimeOutHandler>();
            services.AddScoped<ICommandHandler<SendMessageToConfigBoardCommand>, SendMessageToConfigBoardHandler>();

            services.AddScoped<ICommandHandler<CreatePulsCommand>, CreatePulsHandler>();
            services.AddScoped<ICommandHandler<EditPulsCommand>, EditPulsHandler>();
            services.AddScoped<ICommandHandler<DeletePulsCommand>, DeletePulsHandler>();



            services.AddScoped<IQueryHandler<GetAllBoardQuery>, GetAllBoardsQueryHandler>();
            services.AddScoped<IQueryHandler<GetAlPulslQuery>, GetAllPulsQueryHandler>();
            services.AddScoped<IQueryHandler<GetByIdQuery>, GetByIdQueryHandler>();
            services.AddScoped<IQueryHandler<GetBoardByIdQuery>, GetBoardByIdQueryHandler>();

            //Dispatcher
            services.AddScoped<IQueryDispatcher, QueryDispatcher>();
            services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            services.AddScoped<IAsyncQueryDispatcher, AsyncQueryDispatcher>();

            return services;
        }
    }
}
