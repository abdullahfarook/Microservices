using System;
using Autofac;
using EventBus.Common;
using EventBus.Common.Abstractions;
using EventBus.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Payment.Api.Services;
using RabbitMQ.Client;

namespace Payment.Api.Core
{
    public static partial class Extensions
    {
        public static string ApplicationName { get; set; } = "Framework";
        public static string AdminMail { get; set; } = "framework@brickclay.com";
        public static string ApiUrl { get; set; } = "Url";

    }
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string User = "User";
        public const string Contributor = "Contributor";
    }

    public enum ChargeType
    {
        Recurring = 0,
        Manual = 1
    }
}
