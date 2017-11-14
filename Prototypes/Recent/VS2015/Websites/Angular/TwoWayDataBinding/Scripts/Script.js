/// <reference path="angular.min.js" />

// Create module
var myApp = angular.module("myModule", []) // Method chaining to controller
                   .controller("myController", function ($scope) {
                       var employee = {
                           firstName: "Chris",
                           lastName: "Cornelius",
                           gender: "Unspecified"
                       }

                       $scope.employee = employee;

                       $scope.message = "A silly old message";
                   });
