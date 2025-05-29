// System namespaces básicos
global using System;
global using System.Collections.Generic;
global using System.ComponentModel.DataAnnotations;
global using System.IdentityModel.Tokens.Jwt;
global using System.Linq;
global using System.Net;
global using System.Security.Claims;
global using System.Text;
global using System.Text.Json;
global using System.Threading.Tasks;

// Microsoft ASP.NET Core y extensiones
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;

// Nombres del proyecto
global using Backend.GestorProyectos.Endpoints;
global using Backend.GestorProyectos.Extensions;
global using Backend.GestorProyectos.Middlewares;
global using Backend.GestorProyectos.Models;
global using Backend.GestorProyectos.Services;
