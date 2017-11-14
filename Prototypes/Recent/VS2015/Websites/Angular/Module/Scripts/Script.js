/// <reference path="angular.min.js" />

// Create module
var myApp = angular.module("myModule", []) // Method chaining to controller
                   .controller("myController", function ($scope) {

                       var employee = {
                           firstName: "Chris",
                           lastName: "Cornelius",
                           gender: "Unspecified"
                       }

                       $scope.message = "CC Angular Controller";
                       $scope.employee = employee;
                   });

// Create the controller and register it with the module in one line
//myApp.controller("myController", function ($scope) {
//    var employee = {
//        firstName : "Chris",
//        lastName : "Cornelius",
//        gender : "Unspecified"
//    }

//    $scope.message = "CC Angular Controller";
//    $scope.employee = employee;
//});
