using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using YMM.Application.Abstract.Services;
using YMM.Application.Immplementation;

namespace YMM.Application
{
    public static class ModuleApplicationRegister
    {
        public static IServiceCollection ApplicationRegist(this  IServiceCollection service)
        {
            service.AddTransient<IAuthService, AuthService>();
            service.AddTransient<IEmailService, EmailService>();
            return service;
        }

    }
}
