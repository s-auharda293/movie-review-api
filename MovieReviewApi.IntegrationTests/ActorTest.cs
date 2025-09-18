using Azure;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MovieReviewApi.Application.Commands.Actor;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Queries.Actor;
using System.Net;
using System.Text.Json;
using Xunit.Abstractions;

namespace MovieReviewApi.IntegrationTests;

public static class ActorTestData {
    public static IEnumerable<object[]> CreateActors =>
        new List<object[]>
        {
            new object[]
            {
                new CreateActorDto
                {
                    Name = "Unisha Test Doeeee",
                    DateOfBirth = DateTime.Parse("1990-05-15T00:00:00Z"),
                    Bio = "An actor known for action movies.",
                    MovieIds = null
                },
                "Unisha Test Doeeee",
                DateTime.Parse("1990-05-15T00:00:00Z"),
                "An actor known for action movies."
            },
            new object[]
            {
                new CreateActorDto
                {
                    Name = "John Doe",
                    DateOfBirth = DateTime.Parse("1985-01-01T00:00:00Z"),
                    Bio = "A versatile actor.",
                    MovieIds = new List<Guid>()
                },
                "John Doe",
                DateTime.Parse("1985-01-01T00:00:00Z"),
                "A versatile actor."
            },
            new object[]
            {
                new CreateActorDto
                {
                    Name = "Jane Smith",
                    DateOfBirth = DateTime.Parse("1992-03-10T00:00:00Z"),
                    Bio = "Known for drama films.",
                    MovieIds = null
                },
                "Jane Smith",
                 DateTime.Parse("1992-03-10T00:00:00Z"),
                 "Known for drama films."
            }

        };


    public static IEnumerable<object[]> CreateInvalidActors =>
       new List<object[]>
       {
            new object[]
            {
                new CreateActorDto
                {
                    Name = "U",
                    DateOfBirth = DateTime.Parse("1990-05-15T00:00:00Z"),
                    Bio = "An actor known for action movies.",
                    MovieIds = null
                },
                 new Dictionary<string, string>
                {
                    { "dto.Name", "Name must be at least 2 characters long" }
                },
            },
            new object[]
            {
                new CreateActorDto
                {
                    Name = "John Doe",
                    DateOfBirth = DateTime.Parse("2090-01-01T00:00:00Z"),
                    Bio = "A v",
                    MovieIds = new List<Guid>()
                },
                 new Dictionary<string, string>
                {
                    { "dto.Bio", "Bio must be at least 10 characters long" },
                    { "dto.DateOfBirth", "Date of birth must be in the past" }
                }
            },
            new object[]
            {
                new CreateActorDto
                {
                    Name = "Jane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane Smith Jane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane SmithJane Smith",
                    DateOfBirth = DateTime.Parse("1992-03-10T00:00:00Z"),
                    Bio = "Known for drama films.",
                    MovieIds = null
                },
                new Dictionary<string,string>{
                    { "dto.Name", "Name can't exceed 100 characters"}
                }
            },
            new object[]
            {
                new CreateActorDto
                {
                    Name = "Clint Eastwood",
                    DateOfBirth = DateTime.Parse("1990-05-15T00:00:00Z"),
                    Bio = "An actor known for action and adventure movies.",
                    MovieIds = new List<Guid> { Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6") }
                },
                 new Dictionary<string, string>
                {
                    { "Actor.MoviesNotFound", "One or more movies with Ids 3fa85f64-5717-4562-b3fc-2c963f66afa6 do not exist." }
                },
            },

       };
}

public class ActorTests : IClassFixture<MovieReviewWebApplicationFactory>
{
    private readonly IMediator _mediator;
    private readonly ITestOutputHelper _output;
    private readonly IServiceScope _scope;


    public ActorTests(MovieReviewWebApplicationFactory factory, ITestOutputHelper output)
    {
        //factory.EnsureDatabase();

        _scope = factory.Services.CreateScope();
        _mediator = _scope.ServiceProvider.GetRequiredService<IMediator>();
        _output = output;
    }

    //happy path tests
    [Theory]
    [MemberData(nameof(ActorTestData.CreateActors),MemberType = typeof(ActorTestData))]
    public async Task CreateActor_WithValidData_ReturnsCreatedActor(CreateActorDto actorDto, string expectedName, DateTime expectedDob, string expectedBio)
    {
        // arrange
        var command = new CreateActorCommand(actorDto);

        // act
        var result = await _mediator.Send(command);

        // assert
        Assert.NotNull(result);
        Assert.NotNull(result.Value);
        Assert.False(result.IsFailure);
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);

        var actor = result.Value;

        Assert.Equal(expectedName, actor.Name);
        Assert.Equal(expectedDob, actor.DateOfBirth);
        Assert.Equal(expectedBio, actor.Bio);
        Assert.NotNull(actor?.Id);
        Assert.NotNull(actor.Movies);
        Assert.Empty(actor.Movies);

        _output.WriteLine($"Response: {JsonSerializer.Serialize(result)}");
    }


    [Fact]
    public async Task UpdateActor_WithValidIdAndData_UpdatesActorSuccessfully()
    {
        // arrange
        var createCommand = new CreateActorCommand(new CreateActorDto
        {
            Name = "Original Name",
            DateOfBirth = DateTime.Parse("1985-01-01"),
            Bio = "Original Bio",
            MovieIds = null
        });

        var createdResult = await _mediator.Send(createCommand);
        var actorId = createdResult.Value.Id!;

        // act
        var updateCommand = new UpdateActorCommand(actorId, new UpdateActorDto
        {
            Name = "Updated Name",
            DateOfBirth = DateTime.Parse("1990-05-05"),
            Bio = "Updated Bio",
            MovieIds = null
        });

        var result = await _mediator.Send(updateCommand);

        // assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        var actor = result.Value;
        Assert.Equal("Updated Name", actor?.Name);
        Assert.Equal(DateTime.Parse("1990-05-05"), actor?.DateOfBirth);
        Assert.Equal("Updated Bio", actor?.Bio);

        _output.WriteLine($"Updated Actor: {JsonSerializer.Serialize(result)}");
    }

    [Fact]
    public async Task PatchActor_WithValidIdAndData_UpdatesActorFieldSuccessfully()
    {
        // arrange
        var createCommand = new CreateActorCommand(new CreateActorDto
        {
            Name = "Patch Original",
            DateOfBirth = DateTime.Parse("1985-01-01"),
            Bio = "Original Bio",
            MovieIds = null
        });

        var createdResult = await _mediator.Send(createCommand);
        var actorId = createdResult.Value.Id!; // Guid, not nullable

        // act: patch only the Bio
        var patchCommand = new PatchActorCommand(actorId, new PatchActorDto
        {
            Bio = "Patched Bio" // Only updating the Bio
                                // Name and DateOfBirth left null to remain unchanged
        });

        var result = await _mediator.Send(patchCommand);

        // assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);

        var actor = result.Value;
        Assert.Equal("Patch Original", actor?.Name); // unchanged
        Assert.Equal(DateTime.Parse("1985-01-01"), actor?.DateOfBirth); // unchanged
        Assert.Equal("Patched Bio", actor?.Bio); // updated
        Assert.Empty(actor.Movies);

        _output.WriteLine($"Patched Actor: {JsonSerializer.Serialize(result)}");
    }

    [Fact]
    public async Task DeleteActor_WithValidId_RemovesActor()
    {
        // arrange
        var createCommand = new CreateActorCommand(new CreateActorDto
        {
            Name = "Actor To Delete",
            DateOfBirth = DateTime.Parse("1980-01-01"),
            Bio = "This actor will be deleted",
            MovieIds = null
        });

        var createdResult = await _mediator.Send(createCommand);
        var actorId = createdResult.Value.Id!; // Guid, non-nullable

        // act
        var deleteCommand = new DeleteActorCommand(actorId);
        var result = await _mediator.Send(deleteCommand);

        // assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);

        _output.WriteLine($"Actor {actorId} deleted successfully.");
    }

    [Fact]
    public async Task GetActors_WhenActorsExist_ReturnsListOfActors() {
        // arrange
        var getQuery = new GetActorsQuery();

        // act
        var getResult = await _mediator.Send(getQuery);
        _output.WriteLine($"Actors: {JsonSerializer.Serialize(getResult)}");

        // assert
        getResult.Should().NotBeNull();
        getResult.Value.Should().NotBeNull();
        //var actors = getResult.Value as List<ActorDto>;
        var actors = getResult.Value.ToList();

        actors.Should().BeOfType<List<ActorDto>>();
        actors.Should().NotBeEmpty();

        var valueType = getResult.Value.GetType();
        _output.WriteLine($"Value is List<ActorDto>: {valueType == typeof(List<ActorDto>)}");
        _output.WriteLine($"Value implements IEnumerable<ActorDto>: {typeof(IEnumerable<ActorDto>).IsAssignableFrom(valueType)}");

    }


    [Fact]
    public async Task GetActorById_ShouldReturnRequestedActor()
    {
        // arrange
        var createCommand = new CreateActorCommand(new CreateActorDto
        {
            Name = "Actor To Get",
            DateOfBirth = DateTime.Parse("1980-01-01"),
            Bio = "This actor will be requested",
            MovieIds = null
        });

        var createdResult = await _mediator.Send(createCommand);
        createdResult.Should().NotBeNull();
        createdResult.Value.Should().NotBeNull();


        var actorId = createdResult.Value.Id;
        actorId.Should().NotBe(Guid.Empty);

        // act
        var getActorByIdQuery = new GetActorByIdQuery(actorId);
        var getResult = await _mediator.Send(getActorByIdQuery);

        // assert
        getResult.Should().NotBeNull();
        getResult.Value.Should().NotBeNull();
        getResult.Value.Id.Should().Be(actorId);
        getResult.Value.Name.Should().Be("Actor To Get");
        getResult.Value.Bio.Should().Be("This actor will be requested");
        getResult.Value.DateOfBirth.Should().Be(DateTime.Parse("1980-01-01"));

        _output.WriteLine($"Actor: {JsonSerializer.Serialize(getResult)}");
    }

    [Theory]
    [MemberData(nameof(ActorTestData.CreateInvalidActors), MemberType = typeof(ActorTestData))]
    public async Task CreateActor_WhenDataIsInvalid_ReturnsValidationErrors(CreateActorDto actorDto,Dictionary<string,string> expectedErrors) {
        // arrange
        var command = new CreateActorCommand(actorDto);

        //act
        var result = await _mediator.Send(command);

        //assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
        foreach (var expected in expectedErrors) {
            result.Errors.Should().Contain(e => 
            e.Code == expected.Key &&
            e.Description == expected.Value
            );
        }

        _output.WriteLine($"Invalid Creation: {JsonSerializer.Serialize(result)}");
    }
}