using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.Core
{
    public class BaseController : Controller
    {
        private TokenSession _session;

        protected T Resolve<T>()
        {
            T obj = HttpContext.RequestServices.GetService<T>();
            if (obj == null) throw new Exception($"Service of type {nameof(obj)} is not registered in container");
            return obj;

        }

        protected TokenSession GetSession()
        {
            if (_session != null) return _session;

            _session = new TokenSession(User);
            return _session;
        }
    }

    public class TokenSession
    {
        private readonly ClaimsPrincipal _claims;
        public TokenSession(ClaimsPrincipal claims)
        {
            _claims = claims;
        }
        private long? _id;

        public long Id
        {
            get
            {
                if (_id != null) return _id.Value;
                _id = long.Parse(_claims.FindFirst(ClaimTypes.NameIdentifier)?
                                     .Value ?? throw new KeyNotFoundException("User Id does not exist in CustomClaimsTypes"));
                return _id.Value;
            }
            set => _id = value;
        }

        private string _fullName;

        public string FullName
        {
            get
            {
                if (_fullName != null) return _fullName;
                _fullName = _claims.FindFirst(ClaimTypes.Name)?.Value;
                return _fullName;
            }
            set => _fullName = value;
        }

        private string _email;

        public string Email
        {
            get
            {
                if (_email != null) return _email;
                _email = _claims.FindFirst(ClaimTypes.Email)?.Value;
                return _email;
            }
            set => _email = value;
        }

        private List<string> _roles;

        public List<string> Roles
        {
            get
            {
                if (_roles != null) return _roles;
                var roles = _claims.FindAll(ClaimTypes.Role);
                _roles = new List<string>();
                foreach (var role in roles)
                {
                    _roles.Add(role?.Value.ToString());
                }
                return _roles;
            }
            set => _roles = value;
        }
    }

}
