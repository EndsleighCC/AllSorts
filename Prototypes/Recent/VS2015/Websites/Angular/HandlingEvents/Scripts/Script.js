/// <reference path="angular.js" />

// Create module
var myApp = angular.module("myModule", []) // Method chaining to controller
                   .controller("myController", function ($scope) {

                       $scope.message = "Any silly old message";

                       var employees = [
                           { firstName: "Chris", lastName: "Cornelius", gender: "Unspecified", salary: 10000 },
                           { firstName: "Fred", lastName: "Nurk", gender: "Neuter", salary: 6000 },
                           { firstName: "Jeff", lastName: "Jackson", gender: "Other", salary: 8000 },
                           { firstName: "Boris", lastName: "Johnson", gender: "Surprise", salary: 5000 },
                       ];

                       $scope.employees = employees;

                       var countries = [
                           {
                               name: "UK",
                               cities: [
                                   { name: "London"},
                                   { name: "Manchester" },
                                   { name: "Birmingham" },
                               ]
                           },
                           {
                               name: "USA",
                               cities: [
                                   { name: "Los Angeles" },
                                   { name: "Chicago" },
                                   { name: "Houston" },
                               ]
                           },
                           {
                               name: "Australia",
                               cities: [
                                   { name: "Melbourne" },
                                   { name: "Sydney" },
                                   { name: "Brisbane" },
                               ]
                           },
                       ];

                       $scope.countries = countries;

                       var technologies = [
                           { name: "C#", likes: 0, dislikes:0 },
                           { name: "ASP.NET", likes: 0, dislikes: 0 },
                           { name: "SQL Server", likes: 0, dislikes: 0 },
                           { name: "AngularJS", likes: 0, dislikes: 0 },
                       ];

                       $scope.technologies = technologies;

                       $scope.incrementLikes = function (technology) {
                           technology.likes += 1;
                       }

                       $scope.incrementDislikes = function (technology) {
                           technology.dislikes += 1;
                       }

                   });
